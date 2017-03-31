using System;

namespace RESTful_server.Services
{
    [Serializable]
    internal class RESTful_serverException : Exception
    {
        public RESTful_serverException()
        {
        }

        public RESTful_serverException(string message) : base(message)
        {
        }

        public RESTful_serverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}