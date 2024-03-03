namespace Engine;

public class ExpressionTree
{
    public Node? Root { get; set; }

    public ExpressionTree()
    {
        Root = null;
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