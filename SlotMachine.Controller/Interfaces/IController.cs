using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using SlotMachine.Model;

namespace SlotMachine.Controller.Interfaces
{
    public interface IController : IView
    {
        void InitView();

        Timer TimeoutHandler { get; set; }

        void OnTimeoutTriggered();

        event StepController.StepStartTimeoutEventHandler OnStepStartTimeoutEventHandler;

        event StepController.StepCompletedEventHandler OnStepCompletedEventHandler;

        event StepController.StepBackEventHandler OnStepBackEventHandler;

        event StepController.StepCanceledEventHandler OnStepCanceledEventHandler;

        event StepController.StepErrorEventHandler OnStepErrorEventHandler;

        event StepController.StepWarningEventHandler OnStepWarningEventHandler;

        event StepController.StepWaitEventHandler OnStepWaitEventHandler;
    }
}
