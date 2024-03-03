namespace Engine;

public class ExpressionTree
{
    private string _expression;
    public Node? Root { get; set; }
    public string Expression
    {
        get { return _expression; }
        set
        {
            _expression = value;
            Parse();
        }
    }

    private static readonly char[] Symbols = new[] { '+', '-', '*', '/' };
    private void Parse()
    {
        string [] parts = _expression.Split(ExpressionTree.Symbols);
        foreach(string part in parts)
        {
            Console.WriteLine(part);
        }
    }

    public ExpressionTree()
    {
        Root = null;
        Expression = string.Empty;
    }

    public double Solve()
    {
        if (Root == null)
        {
            return 0.0;
        }

        return Root.GetValue();
    }
}