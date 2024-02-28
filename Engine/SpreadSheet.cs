using System.ComponentModel;

namespace Engine;

public class SpreadSheet : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _rowCount;
    private int _colCount;
    private List<List<SpreadSheetCell>>_cells;
    
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
            throw new ArgumentOutOfRangeException("row");
        }

        if (col < 0 || col >= _colCount)
        {
            throw new ArgumentOutOfRangeException("col");
        }
    }

    public IEnumerable<Cell> GetRow(int row)
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

    public IEnumerable<Cell> this[int row]
    {
        get { return GetRow(row); }
    }

    public Cell this[int row, int col]
    {
        get
        {
            return GetCell(row, col);
        }
    } 

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
        string text = cell.Text;
        if (text.Length > 2 && text[0] == '=')
        {
            int row;
            int col = Char.ToUpper(text[1]) - 'A';
            int.TryParse(text.Substring(2), out row);
            Cell otherCell = GetCell(row - 1, col);
            cell.UnBind(otherCell);
        }
    }

    private void UpdateValue(SpreadSheetCell cell)
    {
        string text = cell.Text;
        string nextValue = text;
        if (text != cell.Value)
        {
            if (text != string.Empty && text.Length > 2 && text[0] == '=')
            {
                int row;
                int col = Char.ToUpper(text[1]) - 'A';
                int.TryParse(text.Substring(2), out row);
                Cell otherCell = GetCell(row - 1, col);
                nextValue = otherCell.Value;
                cell.Bind(otherCell);
            }
            cell.SetValue(nextValue);
        } 
    }
    private void OnCellChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (sender != null && sender is SpreadSheetCell && args.PropertyName == "Text")
        {
            UpdateValue((SpreadSheetCell)sender);
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }

    private void OnCellChanging(object? sender, PropertyChangingEventArgs args)
    {
        if (sender != null && sender is SpreadSheetCell && args.PropertyName == "Text")
        {
            PreUpdateValue((SpreadSheetCell)sender);
        }
    }
}