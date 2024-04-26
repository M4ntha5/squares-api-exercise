namespace Validation.Exceptions;

public class ValidatorsAdapterException : Exception
{
    private ValidatorsAdapterException(string message) : base(message)
    {
    }

    public static ValidatorsAdapterException ClassNotRegisteredException(Type type) =>
        new($"{type} is not registered in IoC container");
}