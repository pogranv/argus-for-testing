namespace StatusesApi.Exceptions;

public class InvalidGraphException : Exception
{
    public InvalidGraphException(string message) : base(message) { }
}
