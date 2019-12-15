using System;

namespace Lab1
{
    public class UiException : Exception
    {
        public UiException()
        {
        }

        public UiException(string message) : base(message)
        {
        }
    }
}