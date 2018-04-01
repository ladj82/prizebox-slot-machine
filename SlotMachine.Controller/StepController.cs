using System.Collections.Generic;
using System.Linq;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.Controller
{
    public class StepController : Dictionary<int, IController>
    {
        public StepController()
        {
            StepLocation = StepLocation.Start;
        }

        public delegate void StepStartTimeoutEventHandler();
        public delegate void StepCompletedEventHandler();
        public delegate void StepBackEventHandler();
        public delegate void StepCanceledEventHandler();
        public delegate void StepErrorEventHandler(string sErrorMessage);
        public delegate void StepWarningEventHandler(string sWarningMessage);
        public delegate void StepWaitEventHandler(string sWaitMessage);
        public delegate void StepMovedEventHandler(StepMovedEventHandlerArgs e);
        
        public event StepMovedEventHandler OnStepMovedEventHandler;

        public IController CurrentController { get; private set; }

        public IController FirstPage
        {
            get { return this[this.Min(x => x.Key)]; }
        }

        public IController LastPage
        {
            get { return this[this.Max(x => x.Key)]; }
        }

        public StepLocation StepLocation { get; private set; }

        public bool CanMoveNext
        {
            get
            {
                if (Count == 1)
                {
                    return false;
                }

                if (Count > 0 && StepLocation != StepLocation.End)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanMovePrevious
        {
            get
            {
                if (Count == 1)
                {
                    return false;
                }

                if (Count > 0 && StepLocation != StepLocation.Start)
                {
                    return true;
                }

                return false;
            }
        }

        public IController MoveFirstStep()
        {
            StepLocation = StepLocation.Start;
            
            var firstStepIndex = (from x in this select x.Key).Min();

            CurrentController = this[firstStepIndex];

            OnStepMoved();

            return CurrentController;
        }

        public IController MoveNextStep()
        {
            if (StepLocation != StepLocation.End && CurrentController != null)
            {
                var nextStepIndex = (from x in this where x.Key > IndexOf(CurrentController) select x.Key).Min();

                var lastStepIndex = (from x in this select x.Key).Max();

                StepLocation = nextStepIndex == lastStepIndex ? StepLocation.End : StepLocation.Middle;

                CurrentController = this[nextStepIndex];

                OnStepMoved();

                return CurrentController;
            }

            return null;
        }

        public IController MovePreviousStep()
        {
            if (StepLocation != StepLocation.Start && CurrentController != null)
            {
                var previousPageIndex = (from x in this where x.Key < IndexOf(CurrentController) select x.Key).Max();

                var firstPageIndex = (from x in this select x.Key).Min();

                StepLocation = previousPageIndex == firstPageIndex ? StepLocation.Start : StepLocation.Middle;

                CurrentController = this[previousPageIndex];

                OnStepMoved();

                return CurrentController;
            }

            return null;
        }

        public IController MoveToStep(int iStepIndex)
        {
            if (StepLocation != StepLocation.End && CurrentController != null)
            {
                var firstStepIndex = (from x in this select x.Key).Min();

                var lastStepIndex = (from x in this select x.Key).Max();

                if (iStepIndex == firstStepIndex)
                {
                    StepLocation = StepLocation.Start;
                }
                else if (iStepIndex == lastStepIndex)
                {
                    StepLocation = StepLocation.End;
                }
                else
                {
                    StepLocation = StepLocation.Middle;
                }

                CurrentController = this[iStepIndex];

                OnStepMoved();

                return CurrentController;
            }

            return null;
        }

        public void ReloadCurrentStep()
        {
            OnStepMoved();
        }

        public int IndexOf(IController controller)
        {
            foreach (var kv in this.Where(kv => kv.Value.Equals(controller)))
            {
                return kv.Key;
            }

            return -1;
        }

        private void OnStepMoved()
        {
            if (OnStepMovedEventHandler == null) return;

            var e = new StepMovedEventHandlerArgs
            {
                StepLocation = StepLocation,
                StepIndex = IndexOf(CurrentController),
            };

            OnStepMovedEventHandler(e);
        }

        public void Reset()
        {
            CurrentController = null;
            StepLocation = StepLocation.Start;
        }
    }

    public enum StepLocation
    {
        Start,
        Middle,
        End
    }

    public class StepMovedEventHandlerArgs
    {
        public StepLocation StepLocation { get; set; }

        public int StepIndex { get; set; }
    }
}
