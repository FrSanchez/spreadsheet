namespace Engine.Tree;

public class ExponentOperatorNode : OperatorNode
{
    public override double GetValue()
    {
        if (Left == null) return 0;
        if (Right != null)
        {
            var value = Math.Pow(Left.GetValue(), Right.GetValue());
            return value;
        }

        return base.GetValue();

    }

    public override string ToString()
    {
        return "^";
    }
}