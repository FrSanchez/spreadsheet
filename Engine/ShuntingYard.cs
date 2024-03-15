using System.Text.RegularExpressions;
using Engine.Tree;

namespace Engine;

public partial class ShuntingYard(IVariableSolver solver)
{
    private readonly NodeFactory _factory = new(solver);
    private readonly Dictionary<char, int> _precedence = new()
    {
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 },
        { '^', 3 }
    };

    public List<Node> ConvertToPostfix(string expression)
    {
        var outputQueue = new List<Node>();
        var operatorStack = new Stack<string?>();

        foreach(var match in MyRegex().Matches(expression))
        {
            var token = match.ToString();
            if (string.IsNullOrEmpty(token))
            {
                continue;
            }
            var node = _factory.CreateNode(match.ToString());
            if (node is NumberNode or VariableNode)
            {
                outputQueue.Add(node);
            }
            else switch (token[0])
            {
                case '(':
                    // Left parenthesis: Push to stack
                    operatorStack.Push(token);
                    break;
                case ')':
                {
                    // Right parenthesis: Pop operators from stack to output queue
                    while (operatorStack.Count > 0 && operatorStack.Peek()![0] != '(')
                    {
                        var opNode = _factory.CreateNode(operatorStack.Pop());
                        if (opNode != null) outputQueue.Add(opNode);
                    }
                    operatorStack.Pop(); // Discard '('
                    break;
                }
                default:
                {
                    if (_precedence.ContainsKey(token[0]))
                    {
                        // Operator: Handle precedence
                        while (operatorStack.Count > 0 && _precedence.ContainsKey(operatorStack.Peek()![0]) &&
                               _precedence[operatorStack.Peek()![0]] >= _precedence[token[0]])
                        {
                            var opNode = _factory.CreateNode(operatorStack.Pop());
                            if (opNode != null) outputQueue.Add(opNode);
                        }
                        operatorStack.Push(token);
                    }

                    break;
                }
            }
        }

        // Pop remaining operators from stack to output queue
        while (operatorStack.Count > 0)
        {
            var opNode = _factory.CreateNode(operatorStack.Pop());
            if (opNode != null) outputQueue.Add(opNode);
        }

        return outputQueue;
    }

    [GeneratedRegex(@"([*+/\-\^)(])|([0-9\.]+|[a-zA-Z]+[a-zA-Z0-9]*)")]
    private static partial Regex MyRegex();
}
