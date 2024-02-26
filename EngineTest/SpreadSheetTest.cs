using Engine;

namespace EngineTest;

public class SpreadSheetTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new SpreadSheet(0, 1);
        });
    }    
    
    [Test]
    public void Test2()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new SpreadSheet(1, 0);
        });
    }

    [Test]
    public void Test3()
    {
        var ss = new SpreadSheet(3, 3);
        ss.SetCell(0, 0, "hello");
        var cell = ss[0, 0];
        Assert.NotNull(cell);
        Assert.That("hello", Is.EqualTo(cell.Text));
        Assert.That("hello", Is.EqualTo(cell.Value));
    }
}