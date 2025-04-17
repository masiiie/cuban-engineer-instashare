namespace InstaShare.Application.CustomExceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("The specified resource was not found.")
    {
    }
    public NotFoundException(string message) : base(message)
    {
    }
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}