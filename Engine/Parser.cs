using System.Data;
using System.Text.RegularExpressions;
using Engine.Tree;

namespace Engine;

public class Parser(IVariableSolver solver)
{
    private readonly NodeFactory _nodeFactory = new (solver);

    public Node? Parse(string expression)
    {
        var root = _nodeFactory.CreateNode(expression);
        if (root != null) return root;
        
        foreach (var symbol in Symbols.Valid)
        {
            var idx = expression.LastIndexOf(symbol);
            if (idx == -1) continue;
                
            root = _nodeFactory.CreateNode(symbol.ToString());
            ((OperatorNode)root!).Left = Parse(expression.Remove(idx));
            ((OperatorNode)root).Right = Parse(expression[(idx + 1)..]);
            return root;
        }

        return root;
    }
}

