using System.Collections.Generic;
using System.Drawing;
using SlotMachine.Model;
using SlotMachine.Model.Interfaces;
using SlotMachine.Utility;

namespace SlotMachine.Controller.Interfaces
{
    public interface IPrinterHandler
    {
        IPrinter PrinterConfig { get; }

        void InitCommunication();

        void SetHeader(bool bPrintDateTime);

        void SetImage(string sImagePath);

        void SetLine(char sCharacter);

        void SetText(string sText, Enumerations.FontSize oFontSize = Enumerations.FontSize.Medium, FontStyle oFontStyle = FontStyle.Regular, bool bFontExpanded = false, Enumerations.TextAlignmentType oTextAlignmentType = Enumerations.TextAlignmentType.Left, bool bBreakLine = true);

        void SetVoucher(Enumerations.BarcodeType oBarcodeType, string sCode);

        void SetFooter();

        void BreakLine(int iNumBreaks);

        void SetCutPaper(Enumerations.CutType oPrintCutType);

        void CloseCommunication();

        void CheckStatus();

        void CheckErrors(List<KeyValuePair<string, object>> lstCommandResult);

        void ExecuteCommands(out List<KeyValuePair<string, object>> lstCommandResult);
    }
}
