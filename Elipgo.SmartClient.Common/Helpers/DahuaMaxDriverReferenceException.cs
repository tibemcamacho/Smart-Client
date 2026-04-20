using System;

namespace Elipgo.SmartClient.Common.Helpers
{
    public class DahuaMaxDriverReferenceException : Exception
    {
        public DahuaMaxDriverReferenceException()
        {
        }

        public DahuaMaxDriverReferenceException(string message)
            : base(message)
        {
        }

        public DahuaMaxDriverReferenceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
