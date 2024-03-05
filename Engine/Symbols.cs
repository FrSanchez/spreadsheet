namespace Engine;

public static class Symbols
{
    public static readonly List<char> Valid = new() { '+', '-', '*', '/', '^'};
    public static bool Contains(char c)
    {
        return Symbols.Valid.Contains(c);
    }
}