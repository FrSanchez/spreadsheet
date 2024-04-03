namespace Engine.Utils;

internal class ChangeTextOperation(Cell cell) : IOperation
{
    private string OldValue { get;  } = cell.Text;
    public Cell GetCell { get; } = cell;
    public bool Apply()
    {
        GetCell.Text = OldValue;
        return true;
    }

    public override string ToString()
    {
        return $"{GetCell.Row},{GetCell.Col} -> {OldValue}";
    }
}