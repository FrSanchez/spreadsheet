namespace Engine;

internal class SpreadSheetCell : Cell
{
    public SpreadSheetCell(int row, int col) : this(row, col, string.Empty)
    {
        
    }

    public SpreadSheetCell(int row, int col, string value) : base(row, col)
    {
        Text = value;
        _value = value;
    }

    public void SetValue(string value)
    {
        _value = value;
    }
}