namespace Engine;

public abstract class Node
{
    public Node? Left { get; set; }
    public Node? Rigth { get; set; }

    public Node()
    {
        Left = null;
        Rigth = null;
    }
    public virtual double GetValue()
    {
        return 0;
    }
}