using System.Text.RegularExpressions;

namespace Engine.Tree;

public class NodeFactory
{
    private readonly IVariableSolver _solver;

    public NodeFactory(IVariableSolver solver)
    {
        _solver = solver;
    }
    // private readonly Dictionary<char, Type> _nodes = new()
    // {
    //     { '+', typeof(AddOperatorNode) },
    //     { '-', typeof(SubtractOperatorNode) },
    //     { '*', typeof(MultiplyOperatorNode) },
    //     { '/', typeof(DivisionOperatorNode) },
    //     { '^', typeof(ExponentOperatorNode) },
    // };

    public Node? CreateNode(string contents)
    {
        if (!string.IsNullOrEmpty(contents))
        {
            var operation = contents[0];
            if (contents.Length == 1 && Symbols.Contains(operation))
            {
                switch (operation)
                {
                    case '+': return new AdditionOperatorNode();
                    case '-': return new SubtractOperatorNode();
                    case '*': return new MultiplyOperatorNode();
                    case '/': return new DivisionOperatorNode();
                    case '^': return new ExponentOperatorNode();
                }
            }
        }

        if (double.TryParse(contents, out var number))
        {
            return new NumberNode(number);
        }

        var match = Regex.Match(contents, "^[a-zA-Z]+[a-zA-Z0-9]*$");
        return match.Success ? new VariableNode(contents, _solver) : null;
    }
}