using System;

namespace SlotMachine.Controller.Exceptions
{
    public class AlertMessageException : Exception
    {
        public AlertMessageException()
        {

        }

        public AlertMessageException(string sMessage) : base(sMessage)
        {

        }

        public AlertMessageException(string sMessage, Exception oInnerException)
            : base(sMessage, oInnerException)
        {

        }
    }
}
