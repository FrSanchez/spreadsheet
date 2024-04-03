using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using Engine.Utils;
using Microsoft.VisualBasic.FileIO;

namespace Engine;

public partial class SpreadSheet : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly int _rowCount;
    private readonly int _colCount;
    private readonly List<List<SpreadSheetCell>>_cells;
    private readonly Stack<IOperation> _undoStack;
    private readonly Stack<IOperation> _redoStack;
    
    public SpreadSheet(int rowCount, int colCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(rowCount, 1);

        ArgumentOutOfRangeException.ThrowIfLessThan(colCount, 1);
        _rowCount = rowCount;
        _colCount = colCount;
        _cells = new();
        InitializeData();
        _undoStack = new Stack<IOperation>();
        _redoStack = new Stack<IOperation>();
    }

    private void ValidateRowAndCol(int row, int col)
    {
        if (row < 0 || row >= _rowCount)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        if (col < 0 || col >= _colCount)
        {
            throw new ArgumentOutOfRangeException(nameof(col));
        }
    }

    private IEnumerable<Cell> GetRow(int row)
    {
        return _cells[row];
    }
    
    public Cell GetCell(int row, int col)
    {
        ValidateRowAndCol(row, col);
        return _cells[row][col];
    }

    public void SetCellText(int row, int col, string value)
    {
        ValidateRowAndCol(row, col);
        _undoStack.Push(new ChangeTextOperation(_cells[row][col]));
        _cells[row][col].Text = value;
    }

    public int GetUndoSize()
    {
        return _undoStack.Count;
    }

    public int GetRedoSize()
    {
        return _redoStack.Count;
    }

    public void Undo()
    {
        if (_undoStack.Count <= 0) return;
        var operation = _undoStack.Pop();
        Console.WriteLine(">>UNDO: [" + operation + "]");
        if (operation is ChangeTextOperation textOperation)
        {
            _redoStack.Push(new ChangeTextOperation(textOperation.GetCell));
        }
        operation.Apply();
    }

    public void Redo()
    {
        if (_redoStack.Count <= 0) return;
        var operation = _redoStack.Pop();
        Console.WriteLine("<<REDO: [" + operation + "]");
        if (operation is ChangeTextOperation textOperation)
        {
            _undoStack.Push(new ChangeTextOperation(textOperation.GetCell));
        }

        operation.Apply();
    }

    public IEnumerable<IEnumerable<Cell>> GetRows()
    {
        return _cells;
    }

    private void InitializeData()
    {
        for (var r = 0; r < _rowCount; r++)
        {
            var row = new List<SpreadSheetCell>();
            for (var c = 0; c < _colCount; c++)
            {
                var cell = new SpreadSheetCell(r,c);
                cell.PropertyChanged += OnCellChanged;
                cell.PropertyChanging += OnCellChanging;
                row.Add(cell);
            }
            _cells.Add(row);
        }
    }

    private void PreUpdateValue(SpreadSheetCell cell)
    {
        var text = cell.Text;
        if (text == cell.Value || string.IsNullOrEmpty(text)) return;
        if (text is ['=', _, ..])
        {
            var expression = new ExpressionTree(text[1..]);
            foreach (var name in expression.GetVariableNames())
            {
                if (name == null) continue;
                var loc = GetLocation(name);
                if (loc == null) continue;

                if (loc.Row < 0 || loc.Row > _rowCount || loc.Col < 0 || loc.Col > _colCount) continue;
                var otherCell = GetCell(loc.Row, loc.Col);
                cell.UnBind(otherCell);
            }
        }
    }

    private static Pair? GetLocation(string variable)
    {
        if (string.IsNullOrEmpty(variable) || variable.Length < 2)
        {
            return null;
        }

        var col = char.ToUpper(variable[0]) - 'A';
        if (int.TryParse(variable[1..], out var row))
        {
            var rc = new Pair()
            {
                Col = col,
                Row = row - 1,
            };
            return rc;
        }
        return null;
    }


    private void UpdateValue(SpreadSheetCell cell)
    {
        var text = cell.Text;
        var nextValue = text;
        if (text == cell.Value || string.IsNullOrEmpty(text)) return;

        var match = SingleCellFormula().Match(text);
        if (match.Success)
        {
            // a single variable is set, no need to use the expression tree
            var loc = GetLocation(text[1..]);
            if (loc == null)
            {
                return;
            }

            if (loc.Row < 0 || loc.Row > _rowCount || loc.Col < 0 || loc.Col > _colCount) return;
            var otherCell = GetCell(loc.Row, loc.Col);
            cell.Bind(otherCell);
            cell.SetValue(otherCell.Value);
            return;
        }

        if (text is ['=', _, ..])
        {
            var expression = new ExpressionTree(text[1..]);
            foreach (var name in expression.GetVariableNames())
            {
                if (name == null) continue;
                expression.AddVariable(name, null);
                var loc = GetLocation(name);
                if (loc == null) continue;

                if (loc.Row < 0 || loc.Row > _rowCount || loc.Col < 0 || loc.Col > _colCount) continue;
                var otherCell = GetCell(loc.Row, loc.Col);
                cell.Bind(otherCell);
                if (double.TryParse(otherCell.Value, out var cellValue))
                {
                    expression.AddVariable(name, cellValue);
                }
            }

            try
            {
                nextValue = expression.Solve().ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                nextValue = "#error!";
            }
        }
        cell.SetValue(nextValue);
    }

    private void OnCellChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (sender is SpreadSheetCell cell && args.PropertyName == "Text")
        {
            UpdateValue(cell);
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }

    private void OnCellChanging(object? sender, PropertyChangingEventArgs args)
    {
        if (sender is SpreadSheetCell cell && args.PropertyName == "Text")
        {
            PreUpdateValue(cell);
        }
    }

    public void SaveFile(Stream stream)
    {
        using var streamWriter = new StreamWriter(stream);
        foreach (var cell in _cells)
        {
            var line = string.Join(",", cell.Select(c => c.Text).ToArray());
            streamWriter.WriteLineAsync(line);
        }
    }

    public void ReadFile(Stream stream)
    {
        using var parser = new TextFieldParser(stream);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        var row = 0;
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields == null) continue;
            var col = 0;
            foreach (var field in fields)
            {
                _cells[row][col++].Text = field;
            }

            row++;
        }

    }

    [GeneratedRegex("^=[A-Z][0-9]{1,2}$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex SingleCellFormula();
}