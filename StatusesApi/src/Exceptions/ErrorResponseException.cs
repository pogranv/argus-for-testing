namespace StatusesApi.Exceptions;

public class ErrorResponseException : Exception
{
    public ErrorResponseException(string message) : base(message) { }
}   
