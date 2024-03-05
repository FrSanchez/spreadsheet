using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Engine.Tree;

namespace Engine;

public class Parser(IVariableSolver solver)
{
    private readonly IVariableSolver _solver = solver;
    private readonly NodeFactory _nodeFactory = new (solver);
    private readonly HashSet<char> _symbols = ['+', '-', '*', '/'];

    private static bool IsExpression(string expression)
    {
        var parts = Regex.Split(expression, @"[\+\-\/\*]");
        return expression.Length > 1 && parts.Length > 1;
    }

    public Node? Parse(string expression)
    {
        Node? root = null;
        var parts = Regex.Split(expression, @"([\+\-])");
        if (parts.Length == 1)
        {
            parts = Regex.Split(expression, @"([\*\/])");
        }

        foreach (var part in parts)
        {
            var current = IsExpression(part) ? Parse(part) : _nodeFactory.CreateNode(part);

            if (root == null)
            {
                root = current;
            }
            else
            {
                if (root is ILeafNode || (root as OperatorNode)?.Left == null)
                {
                    if (current is not OperatorNode node)
                    {
                        throw new InvalidExpressionException();
                    }

                    node.Left = root;
                    root = node;
                }
                else
                {
                    if ((root as OperatorNode)?.Right == null)
                    {
                        (root as OperatorNode)!.Right = current;
                    }
                    else
                    {
                        (current as OperatorNode)!.Left = root;
                        root = current;
                    }
                }
            }
        }

        return root;
    }
}

