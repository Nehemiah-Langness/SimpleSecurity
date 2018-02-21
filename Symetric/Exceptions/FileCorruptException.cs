using System;

namespace Security.Exceptions
{
    public class FileCorruptException : Exception
    {
        public FileCorruptException() : base(){}
        public FileCorruptException(string message) : base(message){ }
        public FileCorruptException(string message, Exception innerException) : base(message, innerException){ }
    }
}