using Engine;
using Engine.Tree;

namespace EngineTest;

public class OperatorsTest : IVariableSolver
{
    public class ClassifierData(string op, Type type)
    {
        public string Op { get;  } = op;
        public Type? Type { get;  } = type;
    }
    [SetUp]
    public void Setup()
    {
        
    }

    [Test, TestCaseSource("ClassifierTestData")]
    public void ClassifyAddTest(ClassifierData data)
    {
        var classifier = new NodeFactory(this);
        var op = classifier.CreateNode(data.Op);
        if (op?.GetType() == data.Type)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

    private static IEnumerable<ClassifierData> ClassifierTestData()
    {
        yield return new ClassifierData( "+", typeof(AddOperatorNode) );
        yield return new ClassifierData( "-", typeof(SubtractOperatorNode) );
        yield return new ClassifierData( "/", typeof(DivisionOperatorNode) );
        yield return new ClassifierData( "*", typeof(MultiplyOperatorNode) );
    }

    public double Resolve(string variable)
    {
        throw new NotImplementedException();
    }
}