using System.ComponentModel;

namespace Engine;

public class SpreadSheet : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _rowCount;
    private int _colCount;
    private SpreadSheetCell[,] _cells;
    
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
        _cells = new SpreadSheetCell[_rowCount, _colCount];
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

    public Cell GetCell(int row, int col)
    {
        ValidateRowAndCol(row, col); 
        return _cells[row, col];
    }

    public void SetCell(int row, int col, string value)
    {
        ValidateRowAndCol(row, col);
        _cells[row, col].Text = value;
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
            for (int c = 0; c < _colCount; c++)
            {
                _cells[r, c] = new SpreadSheetCell(r,c);
                _cells[r, c].PropertyChanged += OnCellChanged;
            }
        }
    }

    private void UpdateValue(SpreadSheetCell cell)
    {
        string text = cell.Text;
        string nextValue = text;
        if (text != cell.Value)
        {
            if (text[0] == '=')
            {
                int row = -1, col = -1;
                if (int.TryParse(text[1].ToString(), out row) && int.TryParse(text.Substring(2), out col))
                {
                    nextValue = GetCell(row, col).Value;
                }
            } 
            cell.SetValue(nextValue);
        } 
    }
    private void OnCellChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (sender != null && sender is SpreadSheetCell)
        {
            UpdateValue((SpreadSheetCell)sender);
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}