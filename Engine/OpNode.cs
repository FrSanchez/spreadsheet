namespace Engine;

public class OpNode : Node
{
    private char _operation;
    public OpNode(char operation) 
    {
        _operation = operation;
    }

    public override double GetValue()
    {
        double lv = 0, rv = 0;
        if (Left != null)
        {
            lv = Left.GetValue();
        }

        if (Rigth != null)
        {
            rv = Rigth.GetValue();
        }

        switch (_operation)
        {
            case '+': return lv + rv;
            case '-': return lv - rv;
            case '*': return lv * rv;
            case '/': return lv * rv;
        }

        return 0;
    }
}