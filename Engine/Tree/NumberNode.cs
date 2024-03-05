using System.Globalization;

namespace Engine.Tree;

public class NumberNode : Node, ILeafNode
{
    private readonly double _number;

    public NumberNode(double number)
    {
        _number = number;
    }
    public override double GetValue()
    {
        return _number;
    }

    public override string ToString()
    {
        return _number.ToString(CultureInfo.InvariantCulture);
    }
}