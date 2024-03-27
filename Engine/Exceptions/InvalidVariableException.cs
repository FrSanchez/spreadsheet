using System.Transactions;

namespace Engine.Exceptions;

public class InvalidVariableException : Exception
{
    public InvalidVariableException()
    {
    }

    public InvalidVariableException(string msg) : base(msg)
    {
    }
}