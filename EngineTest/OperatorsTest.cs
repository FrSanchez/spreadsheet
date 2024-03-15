using Engine;
using Engine.Tree;

namespace EngineTest;

public class OperatorsTest : IVariableSolver
{
    public class ClassifierData
    {
        public ClassifierData(string? op, Type type)
        {
            Op = op;
            Type = type;
        }
        public string? Op { get;  }
        public Type? Type { get;  }
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test, TestCaseSource(nameof(ClassifierTestData))]
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
        yield return new ClassifierData( "+", typeof(AdditionOperatorNode) );
        yield return new ClassifierData( "-", typeof(SubtractOperatorNode) );
        yield return new ClassifierData( "/", typeof(DivisionOperatorNode) );
        yield return new ClassifierData( "*", typeof(MultiplyOperatorNode) );
    }

    public double Resolve(string? variable)
    {
        throw new NotImplementedException();
    }
}