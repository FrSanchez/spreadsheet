using System.Text.RegularExpressions;
using Engine;
using Engine.Tree;

namespace EngineTest;

public class ParserTest : IVariableSolver
{
    private Parser _parser;
    [SetUp]
    public void Setup()
    {
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
        var opNode = root as AddOperatorNode;
        Assert.NotNull(opNode);
        Assert.True(opNode.Left is MultiplyOperatorNode);
        Assert.True(opNode.Right is DivisionOperatorNode);
    }

    public double Resolve(string variable)
    {
        throw new NotImplementedException();
    }
}