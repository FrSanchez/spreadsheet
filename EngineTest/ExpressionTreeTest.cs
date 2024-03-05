using Engine;

namespace EngineTest;

public class ExpressionTreeTest
{
    [Test]
    public void LevelExpressionTest()
    {
        var expressionTree = new ExpressionTree("a*b+c/d");
        expressionTree.AddVariable("a", 5.0);
        expressionTree.AddVariable("b", 7.0);
        expressionTree.AddVariable("c", 2.0);
        expressionTree.AddVariable("d", 3.0);
        var actual = expressionTree.Solve();
        Assert.That(actual, Is.EqualTo((5.0 * 7.0) + (2.0 / 3.0)));
    }
}