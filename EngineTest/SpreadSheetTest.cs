using Engine;

namespace EngineTest;

public class SpreadSheetTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestContructorError1()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new SpreadSheet(0, 1);
        });
    }    
    
    [Test]
    public void TestContructorError2()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new SpreadSheet(1, 0);
        });
    }

    [Test]
    public void TestSetCell()
    {
        var ss = new SpreadSheet(3, 3);
        ss.SetCell(0, 0, "hello");
        var cell = ss[0, 0];
        Assert.NotNull(cell);
        Assert.That("hello", Is.EqualTo(cell.Text));
        Assert.That("hello", Is.EqualTo(cell.Value));
    }

    [Test]
    public void TestRange()
    {
        var ss = new SpreadSheet(4, 4);
        ss.SetCell(1,1,"hello");
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                string expected = string.Empty;
                if (r == 1 && c == 1)
                {
                    expected = "hello";
                }
                Assert.That(expected, Is.EqualTo(ss.GetCell(r,c).Value));
                Assert.That(expected, Is.EqualTo(ss.GetCell(r,c).Text));
            }
        }
    }

    [Test]
    public void TestFormulaBasic1()
    {
        var ss = new SpreadSheet(2, 2);
        ss.SetCell(0,0, "hello");
        ss.SetCell(0,1,"=A1");
        Assert.Multiple(() =>
        {
            Assert.That(ss.GetCell(0, 1).Text, Is.EqualTo("=A1"));
            Assert.That(ss.GetCell(0, 1).Value, Is.EqualTo("hello"));
            Assert.That(ss.GetCell(0, 0).Value, Is.EqualTo("hello"));
        });
    }

    [Test]
    public void TestFormulaBasic2()
    {
        var ss = new SpreadSheet(2, 2);
        ss.SetCell(0,0, "hello");
        ss.SetCell(0,1,"=A1");
        ss.SetCell(0,0,"world");
        Assert.That(ss.GetCell(0,1).Value, Is.EqualTo("world"));
    }

    [Test]
    public void TestFormulaBasic3()
    {
        var ss = new SpreadSheet(5, 5);
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                if (r > 0 || c > 0)
                {
                    ss.SetCell(r, c, "=A1");
                }
            }
        }

        ss.SetCell(0, 0, "hello");
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                Assert.That(ss.GetCell(r, c).Value, Is.EqualTo("hello"));
            }
        }
    }

    [Test]
    public void TestFormulaCascade()
    {
        int rows = 2,
        cols = 10;
        var ss = new SpreadSheet(rows, cols);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (r > 0 || c > 0)
                {
                    int prevC = c - 1;
                    int prevR = r + 1;
                    if (prevC < 0)
                    {
                        prevR--;
                        prevC = cols - 1;
                    }

                    ss.SetCell(r, c, $"={((char)(prevC + 'A')).ToString()}{prevR}");
                }
            }
        }

        ss.SetCell(0,0,"hello");
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Assert.That(ss[r,c].Value, Is.EqualTo("hello"));
            }
        }
    }
    
    [Test, TestCaseSource(nameof(GetTestData))]
    public void TestReadCellInvalid(int row, int col)
    {
        var ss = new SpreadSheet(3, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() => { ss.SetCell(row, col, "hello"); });
    }

    private static IEnumerable<int[]> GetTestData()
    {
        yield return new int[] { -1, 0 };
        yield return new int[] { 0, -1 };
        yield return new int[] { 3, 0 };
        yield return new int[] { 0, 3 };
        yield return new int[] { 1000, 0 };
        yield return new int[] { 0, 1000 };
    }
}