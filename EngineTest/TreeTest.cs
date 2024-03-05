using System.Diagnostics;
using Engine;
using Engine.Tree;

namespace EngineTest;

public class TreeTest : IVariableSolver
{
    private ExpressionTree _tree;
    private readonly NodeFactory _factory;

    public TreeTest()
    {
        _factory = new NodeFactory(this);
    }

    [SetUp]
    public void Setup()
    {
        _tree = new ExpressionTree("");
    }

    [Test]
    public void SolveSingleNum()
    {
        _tree.Root = new NumberNode(10);
        Assert.That(_tree.Solve(), Is.EqualTo(10.0));
    }
    
    [Test]
    public void SolveOneLevel()
    {
        // 10 + 20
        OperatorNode? root = _factory.CreateNode("+") as OperatorNode;
        if (root == null) throw new ArgumentNullException(nameof(root));
        NumberNode lhs = new NumberNode(10);
        NumberNode rhs = new NumberNode(20);
        root.Left = lhs;
        root.Right = rhs;
        _tree.Root = root;
        Assert.That(_tree.Solve(), Is.EqualTo(30));
    }

    [Test]
    public void SolveTwoLevels()
    {
        // (11 + 22) * 0.5
        _tree.Root = _factory.CreateNode("*");
        (((OperatorNode)_tree.Root!)!).Left = (OperatorNode)_factory.CreateNode("+")!;
        ((OperatorNode)_tree.Root).Right = new NumberNode(0.5f);
        var lhs = ((OperatorNode)_tree.Root).Left as OperatorNode;
        Debug.Assert(lhs != null, nameof(lhs) + " != null");
        lhs.Left = new NumberNode(22);
        lhs.Right = new NumberNode(11);
        Assert.That(_tree.Solve(), Is.EqualTo(16.5) );
    }
    
    public double Resolve(string variable)
    {
        throw new NotImplementedException();
    }
}