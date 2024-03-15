using System.Collections;
using System.Text;
using Engine;

namespace ExpressionDemo;

public class Menu
{
    private readonly List<MenuEntry> _entries;
    private ExpressionTree? _tree;
    private string? _expression;

    private bool NewExpression()
    {
        Console.WriteLine("Enter an expression:");
        _expression = Console.ReadLine();
        if (_expression != null) _tree = new ExpressionTree(_expression);
        return true;
    }

    private bool Quit()
    {
        return false;
    }

    private bool AddVariable()
    {
        if (_tree == null)
        {
            Console.WriteLine("Expression is empty");
            return true;
        }

        string? name;
        do
        {
            Console.Write("Enter variable name: ");
            name = Console.ReadLine();
        } while (string.IsNullOrEmpty(name));

        var value = double.NaN;
        var parsed = false;
        do
        {
            Console.Write("Enter value: ");
            var inputValue = Console.ReadLine();
            parsed = double.TryParse(inputValue, out value);
        } while (!parsed || double.IsNaN(value));
        _tree.AddVariable(name, value);
        return true;
    }

    private bool ListVariables()
    {
        if (_tree != null)
        {
            var output = new List<string>();
            foreach (var variable in _tree.GetVariableNames())
            {
                double value = _tree.GetVariable(variable);
                output.Add($"{variable}={value}");
            }
            Console.WriteLine(string.Join(',', output));
        }
        return true;
    }

    private bool Solve()
    {
        if (_tree != null)
        {
            double solution = _tree.Solve();
            Console.WriteLine($"Result: {solution}");
        }
        else
        {
            Console.WriteLine("No expression");
        }

        return true;
    }

    public Menu()
    {
        _entries = new List<MenuEntry>();
        _expression = string.Empty;
        Build();
    }

    private void Build()
    {
        _entries.Add(new MenuEntry() { Name = "New Expression", Function = NewExpression });
        _entries.Add(new MenuEntry() { Name = "Add Variable", Function = AddVariable });
        _entries.Add(new MenuEntry() { Name = "List Variables", Function = ListVariables });
        _entries.Add(new MenuEntry() { Name = "Solve", Function = Solve });
        _entries.Add(new MenuEntry() { Name = "Quit", Function = Quit });
    }
    
    public void Show()
    {
        Console.WriteLine("Select an option:\n");
        if (_expression != null)
        {
            Console.WriteLine($"Expression: {_expression}");
        }

        int cnt = 1;
        foreach (var entry in _entries)
        {
            Console.WriteLine($"{cnt++}: {entry.Name}");
        }
        Console.WriteLine("");
    }

    public bool Action()
    {
        var line = Console.ReadLine();
        if (int.TryParse(line, out var input))
        {
            if (input > 0 && input <= _entries.Count)
            {
                var value = true;
                try
                {
                    value = _entries[input - 1].Function();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return value;
            }
            else
            {
                Console.WriteLine($"not a valid option");
            }
        }
        else
        {
            {
                Console.WriteLine($"not a valid option");
            }

        }
        return true;
    }
}