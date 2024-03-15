using Engine;
using Microsoft.VisualBasic;

namespace EngineTest;

public class ShuntingYardTest : IVariableSolver
{

    private ShuntingYard sy;
    [SetUp]
    public void Setup()
    {
        sy = new ShuntingYard(this);
    }

    [Test]
    public void TetConvertToPostfix()
    {
        var rpn = sy.ConvertToPostfix("(33 + 0.454) * 22.99 / (0.00011 - 345465) ^ 2 ^ 3");
        rpn.ForEach(x=>Console.Write($"[{x}] "));
        Console.WriteLine();
    }
    
    [Test]
    public void TestConvertWithVariables()
    {
        var rpn = sy.ConvertToPostfix("(a + 0.454) * b / (0.00011 - 345465) ^ 2 ^ 3");
        rpn.ForEach(x=>Console.Write($"[{x}] "));
        Console.WriteLine();
    }

    public double Resolve(string? variable)
    {
        throw new NotImplementedException();
    }
}