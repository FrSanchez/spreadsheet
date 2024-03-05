// See https://aka.ms/new-console-template for more information

namespace ExpressionDemo;

public abstract class Program
{
    public static void Main(string[] args)
    {
        Console.Clear();

        var menu = new Menu();
        do
        {
            menu.Show();
        } while (menu.Action());
    }
}