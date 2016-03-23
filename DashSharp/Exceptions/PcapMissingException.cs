using System;

namespace DashSharp.Exceptions
{
    public class PcapMissingException : Exception
    {
        public PcapMissingException()
        {
        }

        public PcapMissingException(string message)
            : base(message)
        {
        }

        public PcapMissingException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public PcapMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PcapMissingException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }
    }
}