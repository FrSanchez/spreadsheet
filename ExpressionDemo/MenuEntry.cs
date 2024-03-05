namespace ExpressionDemo;

public class MenuEntry
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public Func<bool> Function { get; set; }
}