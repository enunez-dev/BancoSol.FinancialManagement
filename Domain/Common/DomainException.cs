namespace Domain.Common;

// Sirve como la excepción base para las validaciones del dominio y las violaciones de reglas de negocio.
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}