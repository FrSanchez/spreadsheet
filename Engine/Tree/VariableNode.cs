namespace Engine.Tree;

public class VariableNode(string variable, IVariableSolver solver) : Node, ILeafNode
{
    private readonly string _variable = variable;
    private readonly IVariableSolver _solver = solver;

    public override double GetValue()
    {
        return _solver.Resolve(_variable);
    }

    public override string ToString()
    {
        return $"var:{_variable}";
    }
}