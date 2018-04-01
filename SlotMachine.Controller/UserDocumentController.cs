using System;
using System.Reflection;
using System.Timers;
using System.Windows.Controls;
using SlotMachine.Controller.Exceptions;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Utility;

namespace SlotMachine.Controller
{
    public class UserDocumentController : IController
    {
        private readonly IUserDocumentView _view;
        private readonly Wizard _model;

        public UserDocumentController(IUserDocumentView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            if (!string.IsNullOrEmpty(_model.Invoice.Cpf) && DocumentUtility.IsCpfValid(_model.Invoice.Cpf))
            {
                _model.User = new User { Cpf = _model.Invoice.Cpf };

                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            }

            _view.InitInstruction = AppConfigUtility.sMessageInformCpf;
            _view.SetFocus();
            _view.SetEnabled(true);

            if (OnStepStartTimeoutEventHandler != null)
            {
                TimeoutHandler = new Timer { Interval = AppConfigUtility.iNoInteractionTimeOut };

                OnStepStartTimeoutEventHandler();
            }
        }

        public void HandleCpfInput(string sCpf)
        {
            try
            {
                if (!sCpf.Length.Equals(11)) return;

                _view.SetEnabled(false);

                if (DocumentUtility.IsCpfValid(sCpf))
                {
                    _model.User = new User { Cpf = sCpf };

                    if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
                }
                else
                {
                    throw new AlertMessageException(AppConfigUtility.sMessageInvalidCpf);
                }
            }
            catch (AlertMessageException ex)
            {
                if (OnStepWarningEventHandler != null) OnStepWarningEventHandler(ex.Message);
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);

                if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageGenericInvoiceError);
            }
        }

        public void CancelStep()
        {
            if (OnStepCanceledEventHandler != null) OnStepCanceledEventHandler();
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
            if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageTimeoutTriggered);
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
