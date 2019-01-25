
namespace Storage.Infrastructure.Exception
{
    public class LocationDomainException : System.Exception
    {
        public LocationDomainException()
        { }

        public LocationDomainException(string message)
            : base(message)
        { }

        public LocationDomainException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
