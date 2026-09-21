namespace StudioOMS.Exceptions;


public class StudioOMSException : Exception
{
    public StudioOMSException()
    {
    }

    public StudioOMSException(string? message) : base(message)
    {
    }

    public StudioOMSException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
