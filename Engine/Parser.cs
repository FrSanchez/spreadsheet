using Engine.Tree;

namespace Engine;

public class Parser(IVariableSolver solver)
{
    private readonly NodeFactory _nodeFactory = new (solver);
    private readonly ShuntingYard _sy = new(solver);

    public Node? ParseWithShuntingYard(string expression)
    {
        var nodes = _sy.ConvertToPostfix(expression.Trim());
        var stack = new Stack<Node>();
        foreach (var node in nodes)
        {
            if (node is NumberNode || node is VariableNode)
            {
                stack.Push(node);
            }
            else
            {
                ((OperatorNode)node).Right = stack.Pop();
                ((OperatorNode)node).Left = stack.Pop();
                stack.Push(node);
            }
        }

        return stack.Pop();
    }
    
    public Node? Parse(string expression)
    {
        var root = _nodeFactory.CreateNode(expression.Trim());
        if (root != null) return root;
        
        foreach (var symbol in Symbols.Valid)
        {
            var idx = expression.LastIndexOf(symbol);
            if (idx == -1) continue;
                
            root = _nodeFactory.CreateNode(symbol.ToString());
            ((OperatorNode)root!).Left = Parse(expression.Remove(idx).Trim());
            ((OperatorNode)root).Right = Parse(expression[(idx + 1)..].Trim());
            return root;
        }

        return root;
    }
}

