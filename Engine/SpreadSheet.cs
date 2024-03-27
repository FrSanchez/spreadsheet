using System.ComponentModel;
using System.Globalization;

namespace Engine;

public class SpreadSheet : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly int _rowCount;
    private readonly int _colCount;
    private readonly List<List<SpreadSheetCell>>_cells;
    
    public SpreadSheet(int rowCount, int colCount)
    {
        if (rowCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(rowCount));
        }

        if (colCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(colCount));
        }
        _rowCount = rowCount;
        _colCount = colCount;
        _cells = new();
        InitializeData();
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

    public void SetCell(int row, int col, string value)
    {
        ValidateRowAndCol(row, col);
        _cells[row][col].Text = value;
    }

    public IEnumerable<IEnumerable<Cell>> GetRows()
    {
        return _cells;
    }

    public IEnumerable<Cell> this[int row] => GetRow(row);

    public Cell this[int row, int col] => GetCell(row, col);

    private void InitializeData()
    {
        for (int r = 0; r < _rowCount; r++)
        {
            var row = new List<SpreadSheetCell>();
            for (int c = 0; c < _colCount; c++)
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
        if (text.Length <= 2 || text[0] != '=') return;
        var col = char.ToUpper(text[1]) - 'A';
        if (!int.TryParse(text.AsSpan(2), out var row)) return;
        if (row <= 0 || row >= _rowCount || col < 0 || col >= _colCount) return;
        var otherCell = GetCell(row - 1, col);
        cell.UnBind(otherCell);
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
        if (text == cell.Value) return;
        if (text != string.Empty && text.Length >= 2 && text[0] == '=')
        {
            var expression = new ExpressionTree(text[1..]);
            foreach(var name in expression.GetVariableNames())
            {
                if (name == null) continue;
                var location = GetLocation(name);
                if (location != null)
                {
                    var row = location.Row;
                    var col = location.Col;
                    if (row < 0 || row > _rowCount || col < 0 || col > _colCount) continue;
                    var otherCell = GetCell(row, col);
                    cell.Bind(otherCell);
                    if (double.TryParse(otherCell.Value, out var cellValue))
                    {
                        expression.AddVariable(name, cellValue);
                    }
                    else
                    {
                        expression.AddVariable(name, null);
                    }
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
}