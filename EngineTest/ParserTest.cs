using System.Text.RegularExpressions;
using Engine;
using Engine.Tree;

namespace EngineTest;

public class ParserTest : IVariableSolver
{
    private Parser _parser;
    private readonly Dictionary<string?, double> _vars = new Dictionary<string?, double>();
    
    [SetUp]
    public void Setup()
    {
        _vars.Clear();
        _parser = new Parser(this);
    }

    [Test]
    public void ParseSingleElement()
    {
        var root = _parser.Parse("987654321.01") as NumberNode;
        Assert.NotNull(root);
    }
    
    [Test]
    public void ParseMultipleElement()
    {
        var root = _parser.Parse("a*b+c/d");
        var opNode = root as AdditionOperatorNode;
        Assert.NotNull(opNode);
        Assert.True(opNode.Left is MultiplyOperatorNode);
        Assert.True(opNode.Right is DivisionOperatorNode);
    }

    [Test]
    public void ParseSameLevel()
    {
        var root = _parser.Parse("1+2+3+4+5");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(1 + 2 + 3 + 4 + 5));
    }

    [Test]
    public void ParseMultipleOperations()
    {
        var root = _parser.Parse(("1+5*4/2-1-2"));
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(1 + 5 * 4 / 2 - 1 - 2));
    }
    
    [Test]
    public void ParseWithSpaces()
    {
        var root = _parser.Parse(("1 + 5 * 4 / 2 - 1 - 2"));
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(1 + 5 * 4 / 2 - 1 - 2));
    }

    [Test]
    public void ExponentTest()
    {
        var root = _parser.Parse(("1 + 5 * 4 / 2 - 1 - 2.0 ^3.3"));
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(1 + 5.0 * 4.0 / 2.0 - 1 - Math.Pow(2.0, 3.3)));
    }

    [Test]
    public void VariableSolverTest()
    {
        _vars.Add("a", 2.25);
        _vars.Add("b", 3.3);
        _vars.Add("c", 1.5);
        _vars.Add("d", 21.0);
        var root = _parser.Parse("a + b - c -d / 2");
        Assert.That(root, Is.Not.Null);
        double delta = Math.Abs(root!.GetValue() - (2.25 + 3.3 - 1.5 - 21.0 / 2));
        Assert.That(delta, Is.LessThanOrEqualTo(0.00000001f));
    }

    [Test]
    public void ParseSingleAllAddition()
    {
        var root = _parser.Parse("a+2+b+5");
        _vars.Add("a", 2.25);
        _vars.Add("b", 3.3);
        Assert.That(root, Is.Not.Null);
        Assert.That(root is AdditionOperatorNode, Is.True);
        Assert.That(root.GetValue(), Is.EqualTo(2.25 + 2 + 3.3 + 5));
    }

    [Test]
    public void ParseSingleMultiplication()
    {
        var root = _parser.Parse("3 * 5");
        Assert.That(root, Is.Not.Null);
        Assert.That(root is MultiplyOperatorNode, Is.True);
        Assert.That(root.GetValue(), Is.EqualTo(3 * 5));
    }

    [Test]
    public void ParseSingleAddMult()
    {
        var root = _parser.Parse(" 3 * 5 - 9 * 8");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(3 * 5 - 9 * 8));
    }

    [Test]
    public void ParseSingleWithOrder()
    {
        var root = _parser.Parse(" 3 * 5 - 9 * 2 ^ 3");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(3 * 5 - 9 * 8));
    }
    
    [Test]
    public void ParseSingleWithOrderSY()
    {
        var root = _parser.ParseWithShuntingYard(" 3 * 5 - 9 * 2 ^ 3");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(3 * 5 - 9 * 8));
    }

    [Test]
    public void ParseWithParentheses()
    {
        var root = _parser.ParseWithShuntingYard("(( 3 + 5) * ( 10 - 8)) ^2");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(Math.Pow(( 3 + 5) * ( 10 - 8), 2)));
    }

    [Test]
    public void ParseSingleMultDiv()
    {
        var root = _parser.Parse("4 / 2 * 5");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(4 / 2 * 5));
    }

    [Test]
    public void ParseSingleDivMult()
    {
        var root = _parser.Parse("4 * 3 / 2");
        Assert.That(root, Is.Not.Null);
        Assert.That(root.GetValue(), Is.EqualTo(4 * 3 / 2));
    }

    public double? Resolve(string? variable)
    {
        return _vars[variable];
    }

    public void AddVariable(string variable)
    {
        
    }
}