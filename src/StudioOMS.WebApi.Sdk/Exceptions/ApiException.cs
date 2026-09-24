namespace StudioOMS.Exceptions;


#pragma warning disable RCS1194 // Implement exception constructors
public sealed class ApiException(string message) : Exception(message);
#pragma warning restore RCS1194 // Implement exception constructors