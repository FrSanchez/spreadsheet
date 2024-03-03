namespace Engine;

public class VariableNode : Node
{
    private string _variable;

    public VariableNode(string variable)
    {
        _variable = variable;
    }

    public override double GetValue()
    {
        return base.GetValue();
    }
}