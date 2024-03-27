namespace Engine;

public interface IVariableSolver
{
    public double? Resolve(string? variable);
    public void AddVariable(string variable);
}