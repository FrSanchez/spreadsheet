namespace Engine;

public class ValueNode : Node
{

    private double _value;
    
    public ValueNode(double value)
    {
        _value = value;
    }

    public override double GetValue()
    {
        return _value;
    }
}