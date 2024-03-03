using Engine;

namespace EngineTest;

public class TreeTest
{
    private ExpressionTree tree;
    [SetUp]
    public void Setup()
    {
        tree = new();
    }

    [Test]
    public void SolveSingleNum()
    {
        tree.Root = new ValueNode(10);
        Assert.That(tree.Solve(), Is.EqualTo(10.0));
    }
    
    [Test]
    public void SolveOneLevel()
    {
        // 10 + 20
        OpNode root = new OpNode('+');
        ValueNode lhs = new ValueNode(10);
        ValueNode rhs = new ValueNode(20);
        root.Left = lhs;
        root.Rigth = rhs;
        tree.Root = root;
        Assert.That(tree.Solve(), Is.EqualTo(30));
    }

    [Test]
    public void SolveTwoLevels()
    {
        // (11 + 22) * 0.5
        tree.Root = new OpNode('*');
        tree.Root.Left = new OpNode('+');
        tree.Root.Rigth = new ValueNode(0.5f);
        var lhs = tree.Root.Left;
        lhs.Left = new ValueNode(22);
        lhs.Rigth = new ValueNode(11);
        Assert.That(tree.Solve(), Is.EqualTo(16.5) );
    }
}