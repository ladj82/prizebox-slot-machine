using System;

namespace SlotMachine.Controller.Exceptions
{
    public class WebRequestException : Exception
    {
        public WebRequestException()
        {

        }

        public WebRequestException(string message) : base(message)
        {

        }

        public WebRequestException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
