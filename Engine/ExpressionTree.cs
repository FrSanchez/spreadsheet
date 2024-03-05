using Engine.Tree;

namespace Engine;

public class ExpressionTree 
{
    private readonly string _expression;
    private readonly Parser _parser;
    public Node? Root { get; set; }
    public string Expression
    {
        get => _expression;
    }

    private static readonly char[] Symbols = new[] { '+', '-', '*', '/' };
    private readonly VariableSolver _solver;

    private void Parse()
    {
        Root = _parser.Parse(_expression);
    }

    public ExpressionTree(string expression)
    {
        _solver = new VariableSolver();
        _expression = expression;
        Root = null;
        _parser = new Parser(_solver);
        Parse();
    }

    public void AddVariable(string variable, double value)
    {
        _solver.AddVariable(variable, value);
    }

    public double GetVariable(string variable)
    {
        return _solver.GetVariable(variable);
    }

    public IEnumerable<string> GetVariableNames()
    {
        return _solver.GetVariableNames();
    }

    public double Solve()
    {
        if (Root == null)
        {
            return 0.0;
        }

        return Root.GetValue();
    }
}