namespace Engine.Tree;

public class VariableNode : Node, ILeafNode
{
    private readonly string _variable;
    private readonly IVariableSolver _solver;
    
    public VariableNode(string variable, IVariableSolver solver) 
    {
        _variable = variable;
        _solver = solver;
    }

    public override double GetValue()
    {
        return _solver.Resolve(_variable);
    }

    public override string ToString()
    {
        return $"var:{_variable}";
    }
}