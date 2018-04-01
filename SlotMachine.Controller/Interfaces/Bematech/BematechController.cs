using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SlotMachine.Model;
using SlotMachine.Model.Interfaces;
using SlotMachine.Utility;

namespace SlotMachine.Controller.Interfaces.Bematech
{
    public class BematechController : BasePrinterController, IPrinterHandler
    {
        public sealed override IPrinter PrinterConfig { get; protected set; }
        
        protected override List<Command> Commands { get; set; }

        private const int TotalCharactersPerLine = 48;

        public BematechController(IPrinter printer)
            : base(printer)
        {
            Commands.Add(new Command("BematechInterface.ConfiguraModeloImpressora", () => BematechInterface.ConfiguraModeloImpressora(Convert.ToInt32(PrinterConfig.Model))));
        }

        public void InitCommunication()
        {
            var sCommunicationPort = GetPrinterComPort;

            Commands.Add(new Command("BematechInterface.IniciaPorta", () => BematechInterface.IniciaPorta(sCommunicationPort)));
        }

        public void SetHeader(bool bPrintDateTime)
        {
            if (bPrintDateTime)
            {
                var dateTime = GeneralUtility.DateTime ?? DateTime.Now;
                var sText = dateTime.ToString("dd/MM/yy H:mm:ss");

                SetText(sText);

                BreakLine(1);
            }
        }

        public void SetImage(string sImagePath)
        {
            Commands.Add(new Command("BematechInterface.ImprimeBmpEspecial", () => BematechInterface.ImprimeBmpEspecial(sImagePath, -1, 0, 0)));
        }

        public void SetLine(char sCharacter)
        {
            SetText(new string(sCharacter, TotalCharactersPerLine));
        }

        public void SetText(string sText, Enumerations.FontSize oFontSize = Enumerations.FontSize.Medium, FontStyle oFontStyle = FontStyle.Regular, bool bFontExpanded = false, Enumerations.TextAlignmentType oTextAlignmentType = Enumerations.TextAlignmentType.Center, bool bBreakLine = true)
        {
            string sSpaces;

            switch (oTextAlignmentType)
            {
                case Enumerations.TextAlignmentType.Center:
                    sSpaces = PrintUtility.GetCenterAlignmentText(sText, TotalCharactersPerLine, oFontSize);
                    SetText(sSpaces, oFontSize, FontStyle.Regular, bFontExpanded, Enumerations.TextAlignmentType.Left, false);
                    break;

                case Enumerations.TextAlignmentType.Right:
                    sSpaces = PrintUtility.GetRightAlignmentText(sText, TotalCharactersPerLine, oFontSize);
                    SetText(sSpaces, oFontSize, FontStyle.Regular, bFontExpanded, Enumerations.TextAlignmentType.Left, false);
                    break;

                case Enumerations.TextAlignmentType.Left:
                default:
                    break;
            }

            int iItalic = oFontStyle.ToString().Contains(FontStyle.Italic.ToString()) ? 1 : 0;
            int iUnderline = oFontStyle.ToString().Contains(FontStyle.Underline.ToString()) ? 1 : 0;
            int iExpanded = bFontExpanded ? 1 : 0;
            int iBold = oFontStyle.ToString().Contains(FontStyle.Bold.ToString()) ? 1 : 0;

            Commands.Add(new Command("BematechInterface.FormataTX", () => BematechInterface.FormataTX(sText, (int)oFontSize, iItalic, iUnderline, iExpanded, iBold)));

            if (bBreakLine)
                BreakLine(1);
        }

        public void SetVoucher(Enumerations.BarcodeType oBarcodeType, string sCode)
        {
            if (string.IsNullOrEmpty(sCode)) return;

            Commands.Add(new Command("BematechInterface.ConfiguraCodigoBarras", () => BematechInterface.ConfiguraCodigoBarras(50, 0, 2, 0, 90)));

            BreakLine(1);

            //TODO: Implement all Barcode Types
            switch (oBarcodeType)
            {
                case Enumerations.BarcodeType.CODE128:
                    Commands.Add(new Command("BematechInterface.ImprimeCodigoBarrasCODE128", () => BematechInterface.ImprimeCodigoBarrasCODE128(sCode)));
                    break;
            }
        }

        public void SetFooter()
        {
            //BreakLine(1);

            //SetText("", -1, FontStyle.Bold, -1, Enumerations.TextAlignmentType.Center);

            //BreakLine(1);
        }

        public void BreakLine(int iNumBreaks)
        {
            if (iNumBreaks <= 0) return;

            const string sBreakLineCommand = "\r\n";

            while (iNumBreaks > 0)
            {
                Commands.Add(new Command("BematechInterface.ComandoTX", () => BematechInterface.ComandoTX(sBreakLineCommand, sBreakLineCommand.Length)));
                iNumBreaks--;
            }
        }

        public void SetCutPaper(Enumerations.CutType oPrintCutType)
        {
            switch (oPrintCutType)
            {
                case Enumerations.CutType.None:
                    // No cut.
                    break;

                case Enumerations.CutType.Partial:
                    Commands.Add(new Command("BematechInterface.AcionaGuilhotina", () => BematechInterface.AcionaGuilhotina(0)));
                    break;

                case Enumerations.CutType.Full:
                    Commands.Add(new Command("BematechInterface.AcionaGuilhotina", () => BematechInterface.AcionaGuilhotina(1)));
                    break;
            }
        }

        public void CloseCommunication()
        {
            Commands.Add(new Command("BematechInterface.FechaPorta", () => BematechInterface.FechaPorta()));
        }

        public void CheckStatus()
        {
            Commands.Add(new Command("BematechInterface.Le_Status", () => BematechInterface.Le_Status()));
        }

        public void CheckErrors(List<KeyValuePair<string, object>> lstCommandResult)
        {
            if (lstCommandResult == null)
                throw new ArgumentNullException("CommandResult");

            lstCommandResult.ToList().ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case "BematechInterface.IniciaPorta":
                            if (item.Value.Equals(0))
                                throw new Exception("Print Error: Erro ao iniciar a porta");

                            break;

                        case "BematechInterface.Le_Status":
                            if (item.Value.Equals(0))
                                throw new Exception("Print Error: Erro de comunicação");

                            if (item.Value.Equals(9))
                                throw new Exception("Print Error: Tampa aberta");

                            if (item.Value.Equals(32))
                                throw new Exception("Print Error: Impressora sem papel");

                            break;

                        case "BematechInterface.FechaPorta":
                            if (item.Value.Equals(0))
                                throw new Exception("Print Error: Erro ao fechar a porta");

                            break;
                    }
                }
            );
        }

        private string GetPrinterComPort
        {
            get
            {
                return !string.IsNullOrEmpty(PrinterConfig.IpAddress) ? PrinterConfig.IpAddress : PrinterConfig.ConnectionType.ToString();
            }
        }
    }
}
