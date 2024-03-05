using System.Globalization;

namespace Engine.Tree;

public class NumberNode(double number) : Node, ILeafNode
{
    public override double GetValue()
    {
        return number;
    }

    public override string ToString()
    {
        return number.ToString(CultureInfo.InvariantCulture);
    }
}