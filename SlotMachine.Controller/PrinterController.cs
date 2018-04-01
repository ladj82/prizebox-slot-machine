using System;
using System.Collections.Generic;
using System.Reflection;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Controller.Interfaces.Bematech;
using SlotMachine.Model;
using SlotMachine.Utility;

namespace SlotMachine.Controller
{
    public class PrinterController
    {
        public static IPrinterHandler GetPrinter()
        {
            try
            {
                IPrinterHandler oPrinter = null;
                var sManufacturer = AppConfigUtility.sPrinterManufacturer;
                var sName = AppConfigUtility.sPrinterName;
                var sModel = AppConfigUtility.sPrinterModel;
                var oConnectionType = AppConfigUtility.sPrinterConnectionType.ToEnum<Enumerations.ConnectionType>();

                switch (sManufacturer)
                {
                    case "Bematech":
                        oPrinter = new BematechController(new Printer
                            {
                                Manufacturer = sManufacturer,
                                Name = sName,
                                Model = sModel,
                                ConnectionType = oConnectionType
                            }
                        );
                        break;
                }

                if (oPrinter == null)
                    throw new ArgumentNullException("PrinterHandler");

                return oPrinter;
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw;
            }
        }

        public static bool IsPrinterOk
        {
            get
            {
                try
                {
                    return true;

                    var oPrinter = GetPrinter();

                    List<KeyValuePair<string, object>> lstCommandResult;

                    oPrinter.InitCommunication();
                    oPrinter.CheckStatus();
                    oPrinter.CloseCommunication();
                    oPrinter.ExecuteCommands(out lstCommandResult);
                    oPrinter.CheckErrors(lstCommandResult);

                    return true;
                }
                catch (Exception ex)
                {
                    LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);
                    return false;
                }
            }
        }
    }
}
