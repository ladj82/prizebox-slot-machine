using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Utility;
using Timer = System.Timers.Timer;

namespace SlotMachine.Controller
{
    public class TryAgainController : IController
    {
        private readonly ITryAgainView _view;
        private readonly Wizard _model;

        public TryAgainController(ITryAgainView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            if (_model.Campaign.PrizeType.ToEnum<Enumerations.PrizeType>() != Enumerations.PrizeType.None)
            {
                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            }
            else
            {
                _view.SetFocus();
                _view.SetEnabled(true);

                _view.HeaderMessage = AppConfigUtility.sMessageNonePrizeHeader;
                _view.TryAgainMessage = AppConfigUtility.sMessageNonePrizeSubHeader;

                WaitAndMoveNext();
            }
        }

        private void WaitAndMoveNext()
        {
            var oBackgroundWorker = new BackgroundWorker();

            oBackgroundWorker.DoWork += (sender, args) =>
            {
                Thread.Sleep(AppConfigUtility.iAlertTimeOut);
            };

            oBackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            };

            oBackgroundWorker.RunWorkerAsync();
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
