using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Utility;
using Timer = System.Timers.Timer;

namespace SlotMachine.Controller
{
    public class PrintController : IController
    {
        private readonly IPrintView _view;
        private readonly Wizard _model;

        public PrintController(IPrintView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            if (_model.Campaign.PrizeType.ToEnum<Enumerations.PrizeType>() == Enumerations.PrizeType.None)
            {
                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            }
            else
            {
                _view.SetFocus();
                _view.SetEnabled(true);

                _view.HeaderMessage = AppConfigUtility.sMessageWinPrizeHeader;
                _view.SubHeaderMessage = AppConfigUtility.sMessageWinPrizeSubHeader;
                _view.TakeCupomMessage = AppConfigUtility.sMessageWinPrizeTakeCupom;
                _view.StoreName = _model.Campaign.StoreName;

                HandlePrizeResult();
            }
        }

        private void HandlePrizeResult()
        {
            var oBackgroundWorker = new BackgroundWorker();
            
            oBackgroundWorker.DoWork += (sender, args) => {
                PrintCampaign();
                Thread.Sleep(5000);
            };

            oBackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            };

            oBackgroundWorker.RunWorkerAsync();
        }

        private void PrintCampaign()
        {
            try
            {
                var oPrinter = PrinterController.GetPrinter();

                oPrinter.InitCommunication();

                oPrinter.SetHeader(true);

                oPrinter.SetText(_model.Campaign.HeaderTitle, Enumerations.FontSize.Large, FontStyle.Bold, true, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(1);

                oPrinter.SetText(_model.Campaign.HeaderSubTitle, Enumerations.FontSize.Medium, FontStyle.Bold, false, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(1);

                oPrinter.SetLine('-');

                oPrinter.SetText("Código Promocional", Enumerations.FontSize.Medium, FontStyle.Bold | FontStyle.Underline, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetVoucher(Enumerations.BarcodeType.CODE128, _model.Campaign.PrizeCode);

                oPrinter.BreakLine(1);

                oPrinter.SetText("CPF", Enumerations.FontSize.Medium, FontStyle.Bold | FontStyle.Underline, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText(_model.User.Cpf, Enumerations.FontSize.Medium, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetLine('-');

                oPrinter.BreakLine(1);

                oPrinter.SetText("Validade", Enumerations.FontSize.Large, FontStyle.Bold | FontStyle.Underline, true, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText(_model.Campaign.DueDate, Enumerations.FontSize.Medium, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(1);

                oPrinter.SetText("Regras", Enumerations.FontSize.Medium, FontStyle.Bold | FontStyle.Underline, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText(_model.Campaign.Rules, Enumerations.FontSize.Small, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(1);

                oPrinter.SetText("Sobre a loja", Enumerations.FontSize.Medium, FontStyle.Bold | FontStyle.Underline, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText(_model.Campaign.StoreName, Enumerations.FontSize.Small, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText(_model.Campaign.StoreInfo, Enumerations.FontSize.Small, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(1);

                oPrinter.SetImage(AppConfigUtility.sAppImagesPath + "imgLogoShoppingMueller.bmp");

                oPrinter.BreakLine(2);

                oPrinter.SetText("www.prizebox.com.br", Enumerations.FontSize.Small, FontStyle.Regular, false, Enumerations.TextAlignmentType.Center);

                oPrinter.SetText("C U P O N S   I N T E L I G E N T E S", Enumerations.FontSize.Small, FontStyle.Bold, false, Enumerations.TextAlignmentType.Center);

                oPrinter.BreakLine(2);

                oPrinter.SetCutPaper(Enumerations.CutType.Full);

                oPrinter.CloseCommunication();

                List<KeyValuePair<string, object>> commandResult;

                oPrinter.ExecuteCommands(out commandResult);

                oPrinter.CheckErrors(commandResult);
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);

                if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageGenericInvoiceError);
            }
        }

        public UserControl UserControl { get { return _view.UserControl; } }
        
        public void SetFocus()
        {
            _view.SetFocus();
        }

        public void SetEnabled(bool enabled)
        {
            _view.SetEnabled(enabled);
        }

        public Timer TimeoutHandler { get; set; }
        
        public void OnTimeoutTriggered()
        {
            
        }

        public event StepController.StepStartTimeoutEventHandler OnStepStartTimeoutEventHandler;
        public event StepController.StepCompletedEventHandler OnStepCompletedEventHandler;
        public event StepController.StepBackEventHandler OnStepBackEventHandler;
        public event StepController.StepCanceledEventHandler OnStepCanceledEventHandler;
        public event StepController.StepErrorEventHandler OnStepErrorEventHandler;
        public event StepController.StepWarningEventHandler OnStepWarningEventHandler;
        public event StepController.StepWaitEventHandler OnStepWaitEventHandler;
    }
}
