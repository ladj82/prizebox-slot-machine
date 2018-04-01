using System.Collections.Generic;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Model.Interfaces;
using SlotMachine.Utility;

namespace SlotMachine.Controller
{
    public abstract class BasePrinterController
    {
        public abstract IPrinter PrinterConfig { get; protected set; }

        protected abstract List<Command> Commands { get; set; }

        protected BasePrinterController(IPrinter oPrinter)
        {
            Commands = new List<Command>();
            PrinterConfig = oPrinter;
        }

        public virtual void ExecuteCommands(out List<KeyValuePair<string, object>> oCommandResult)
        {
            oCommandResult = new List<KeyValuePair<string, object>>();

            if (Commands != null)
            {
                foreach (var oCommand in Commands)
                {
                    var iResult = oCommand.Function.Invoke();
                    
                    oCommandResult.Add(new KeyValuePair<string, object>(oCommand.Name, iResult));

                    LogUtility.Log(LogUtility.LogType.Information,
                        string.Format("{0} - Return: {1}", oCommand.Name, iResult));
                }

                Commands = new List<Command>();
            }
        }
    }
}
