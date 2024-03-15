namespace Engine;

public class VariableSolver : IVariableSolver
{
    private readonly Dictionary<string?, double> _variables = new Dictionary<string?, double>();

    public void Clear()
    {
        _variables.Clear();
    }

    public void AddVariable(string? variable, double value)
    {
        _variables[variable] = value;
    }

    public double GetVariable(string? variable)
    {
        return Resolve(variable);
    }
    
    public double Resolve(string? variable)
    {
        return _variables.GetValueOrDefault(variable, 0);
    }

    public IEnumerable<string?> GetVariableNames()
    {
        return _variables.Keys;
    }
}