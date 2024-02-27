namespace EngineTest;

using Engine;
public class CellTest
{
    [Test]
    public void Test1()
    {
        var cell = new SpreadSheetCell(1, 1);
        cell.PropertyChanged += (sender, args) =>
        {
            if (sender != null)
            {
                Console.WriteLine(nameof(sender));
            }
            Assert.NotNull(args);
            Assert.That("Text", Is.EqualTo(args.PropertyName));
        };
        cell.Text = "hello";
        Assert.That("hello", Is.EqualTo(cell.Text));
    }
}