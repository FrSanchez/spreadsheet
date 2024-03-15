namespace ExpressionDemo;

public class MenuEntry
{
    public string Name { get; set; }
    public Func<bool> Function { get; set; }
}