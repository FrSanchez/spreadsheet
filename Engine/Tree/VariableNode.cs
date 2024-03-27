using System.Data;
using Engine.Exceptions;

namespace Engine.Tree;

public class VariableNode(string? variable, IVariableSolver solver) : Node, ILeafNode
{
    public override double GetValue()
    {
        var value = solver.Resolve(variable);
        if (null == value)
        {
            throw new InvalidVariableException("A null value is not permitted");
        }
        return (double)value;
    }

    public override string ToString()
    {
        return $"var:{variable}";
    }
}