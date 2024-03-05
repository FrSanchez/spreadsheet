using System.Data;

namespace Engine.Tree;

public abstract class OperatorNode : Node
{
    public Node? Left { get; set; } = null;
    public Node? Right { get; set; } = null;

    public override double GetValue()
    {
        throw new EvaluateException("A binary operator needs to values");
    }
}