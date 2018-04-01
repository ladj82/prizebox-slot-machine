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
    public class UserGenderController : IController
    {
        private readonly IUserGenderView _view;
        private readonly Wizard _model;

        public UserGenderController(IUserGenderView view, Wizard model)
        {
            _view = view;
            _model = model;
            view.SetController(this);
        }

        public void InitView()
        {
            _view.InitInstruction = AppConfigUtility.sMessageInformGender;
            _view.SetFocus();
            _view.SetEnabled(true);

            if (OnStepStartTimeoutEventHandler != null)
            {
                TimeoutHandler = new Timer { Interval = AppConfigUtility.iNoInteractionTimeOut };

                OnStepStartTimeoutEventHandler();
            }
        }

        public void HandleGenderInput(string sGender)
        {
            try
            {
                _view.SetEnabled(false);

                if (!string.IsNullOrEmpty(sGender))
                {
                    _model.User.Gender = sGender;

                    if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
                }
                else
                {
                    throw new AlertMessageException(AppConfigUtility.sMessageInvalidGender);
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

        public void BackStep()
        {
            if (OnStepBackEventHandler != null) OnStepBackEventHandler();
        }

        public UserControl UserControl { get { return _view.UserControl; } }
        
        public void SetFocus()
        {
            _view.SetFocus();
            _view.ShowBackButton(string.IsNullOrEmpty(_model.Invoice.Cpf));
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
