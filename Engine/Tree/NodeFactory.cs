using System.Text.RegularExpressions;

namespace Engine.Tree;

public class NodeFactory(IVariableSolver solver)
{
    private readonly IVariableSolver _solver = solver;
    private readonly Dictionary<char, Type> _nodes = new()
    {
        { '+', typeof(AddOperatorNode) },
        { '-', typeof(SubtractOperatorNode) },
        { '*', typeof(MultiplyOperatorNode) },
        { '/', typeof(DivisionOperatorNode) },
    };

    public Node? CreateNode(string contents)
    {
        if (!string.IsNullOrEmpty(contents))
        {
            var operation = contents[0];
            if (_nodes.ContainsKey(operation))
            {
                return (OperatorNode)Activator.CreateInstance(_nodes[operation])!;
            }
        }

        double value;
        if (double.TryParse(contents, out value))
        {
            return new NumberNode(value);
        }

        var match = Regex.Match(contents, "^[a-zA-Z]+[a-zA-Z0-9]*");
        if (match.Success)
        {
            return new VariableNode(contents, _solver);
        }

        return null;
    }
}