using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.UI;
using SlotMachine.Utility;

namespace SlotMachine.Main
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private Wizard _model;

        public StepController StepContainer { get; set; }

        public Main()
        {
            InitializeComponent();
            Cursor = Cursors.None;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            _model = new Wizard();
            StepContainer = new StepController();

            var lstControllers = new List<IController>
            {
                new InvoiceController(new InvoiceView(), _model),
                new UserDocumentController(new UserDocumentView(), _model),
                new UserGenderController(new UserGenderView(), _model),
                new CampaignController(new CampaignView(), _model),
                new ShufflingController(new ShufflingView(), _model),
                new PrintController(new PrintView(), _model),
                new TryAgainController(new TryAgainView(), _model)
            };

            var iStep = 0;
            lstControllers.ForEach(item =>
            {
                BindStepEvents(item);
                StepContainer.Add(++iStep, item);
            });

            StepContainer.OnStepMovedEventHandler += OnStepMovedEventHandler;
            StepContainer.MoveFirstStep();
        }

        private void BindStepEvents(IController controller)
        {
            controller.OnStepStartTimeoutEventHandler += () =>
            {
                //StartTimeout(controller);
            };

            controller.OnStepCompletedEventHandler += () =>
            {
                //StopTimeout(controller);
                GoToNextStep();
            };

            controller.OnStepBackEventHandler += () =>
            {
                GoToPreviousStep();
            };

            controller.OnStepCanceledEventHandler += () =>
            {
                Init();
            };

            controller.OnStepErrorEventHandler += (sErrorMessage) =>
            {
                AlertMessageHandler(Enumerations.AlertType.Error, sErrorMessage);
            };

            controller.OnStepWarningEventHandler += (sWarningMessage) =>
            {
                AlertMessageHandler(Enumerations.AlertType.Warning, sWarningMessage);
            };

            controller.OnStepWaitEventHandler += (sWaitMessage) =>
            {
                AlertMessageHandler(Enumerations.AlertType.Wait, sWaitMessage);
            };
        }

        private void OnStepMovedEventHandler(StepMovedEventHandlerArgs e)
        {
            ShowStep(e.StepIndex);
        }

        private void ShowStep(int stepIndex)
        {
            if (stepIndex == -1) return;

            var step = StepContainer[stepIndex];

            grdMainContainer.Children.Clear();
            grdMainContainer.Children.Add(step.UserControl);
            step.SetFocus();
            step.SetEnabled(true);
        }

        private void GoToNextStep()
        {
            if (StepContainer.CanMoveNext)
            {
                StepContainer.MoveNextStep();
            }
            else
            {
                Init();
            }
        }

        private void GoToPreviousStep()
        {
            if (StepContainer.CanMovePrevious)
            {
                StepContainer.MovePreviousStep();
            }
            else
            {
                Init();
            }
        }

        private void ReloadCurrentStep()
        {
            StepContainer.ReloadCurrentStep();
        }

        private void StartTimeout(IController oController)
        {
            var oTimeoutTimer = oController.TimeoutHandler;

            if (oTimeoutTimer == null) return;

            oTimeoutTimer.Elapsed += (sender, args) =>
            {
                oTimeoutTimer.Stop();

                Dispatcher.Invoke((Action)oController.OnTimeoutTriggered);
            };

            oTimeoutTimer.Start();
        }

        private void StopTimeout(IController controller)
        {
            var oTimeoutTimer = controller.TimeoutHandler;

            if (oTimeoutTimer == null) return;

            oTimeoutTimer.Stop();
        }

        private void AlertMessageHandler(Enumerations.AlertType oAlertType, string sMessage)
        {
            grdMainContainer.Children.Clear();
            grdMainContainer.Children.Add(new AlertView(sMessage));

            switch (oAlertType)
            {
                case Enumerations.AlertType.Warning:
                    GeneralUtility.Wait(AppConfigUtility.iAlertTimeOut);
                    ReloadCurrentStep();
                    break;

                case Enumerations.AlertType.Error:
                    GeneralUtility.Wait(AppConfigUtility.iAlertTimeOut);
                    Init();
                    break;

                case Enumerations.AlertType.Wait:
                    break;
            }
        }
    }
}