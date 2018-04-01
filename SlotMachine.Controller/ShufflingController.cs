using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Utility;

namespace SlotMachine.Controller
{
    public class ShufflingController : IController
    {
        private readonly IShufflingView _view;
        private readonly Wizard _model;

        public ShufflingController(IShufflingView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            _view.SetFocus();
            _view.SetEnabled(true);

            var dicShufflingItems = GetShufflingItems();
            var lstResult = GetShufflingResult();

            HandleShuffling(dicShufflingItems, lstResult);
        }

        private async void HandleShuffling(Dictionary<string, string> dicShufflingItems, List<string> lstResult)
        {
            var result = await StartShuffling(dicShufflingItems, lstResult);

            if (result)
            {
                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            }
        }

        private Task<bool> StartShuffling(IDictionary<string, string> dicShufflingItems, List<string> lstResult)
        {
            var oTaskCompletion = new TaskCompletionSource<bool>();
            var oShufflingTimer = new Timer();
            var iShufflingCount = 0;

            oShufflingTimer.Interval = AppConfigUtility.iShufflingInterval;
            oShufflingTimer.Elapsed += async (sender, e) =>
            {
                try
                {
                    if (iShufflingCount == AppConfigUtility.iShuffleInteractions)
                    {
                        oShufflingTimer.Stop();

                        if (_model.Campaign.PrizeType.ToEnum<Enumerations.PrizeType>() != Enumerations.PrizeType.None)
                        {
                            await BlinkSlotContainer();
                        }

                        oTaskCompletion.SetResult(true);
                    }
                    else
                    {
                        var lstTempResult = GeneralUtility.GetDictionaryRandomItems(dicShufflingItems).Take(3).ToList();

                        var oImage1 = iShufflingCount <= AppConfigUtility.iShuffleInteractions - 30 ? lstTempResult.ElementAt(0) : lstResult.ElementAt(0);
                        var oImage2 = iShufflingCount <= AppConfigUtility.iShuffleInteractions - 20 ? lstTempResult.ElementAt(1) : lstResult.ElementAt(1);
                        var oImage3 = iShufflingCount <= AppConfigUtility.iShuffleInteractions - 10 ? lstTempResult.ElementAt(2) : lstResult.ElementAt(2);

                        _view.SetSlotImage1(oImage1);
                        _view.SetSlotImage2(oImage2);
                        _view.SetSlotImage3(oImage3);
                    }

                    iShufflingCount++;
                }
                catch (Exception ex)
                {
                    oTaskCompletion.SetResult(false);

                    LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);

                    if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageGenericInvoiceError);
                }
            };

            oShufflingTimer.Start();

            return oTaskCompletion.Task;
        }

        private List<string> GetShufflingResult()
        {
            try
            {
                int iPrizeSequence;
                var iShufflingColumns = 3;
                var oPrizeType = _model.Campaign.PrizeType.ToEnum<Enumerations.PrizeType>();
                var sStoreName = _model.Campaign.StoreName;
                var lstShufflingResult = new List<string>();
                var dicShufflingItems = GetShufflingItems();

                switch (oPrizeType)
                {
                    case Enumerations.PrizeType.None:
                        iPrizeSequence = 0;
                        break;
                    case Enumerations.PrizeType.Double:
                        iPrizeSequence = 2;
                        break;
                    case Enumerations.PrizeType.Triple:
                        iPrizeSequence = 3;
                        break;
                    default:
                        iPrizeSequence = 0;
                        break;
                }

                for (var iCount = 1; iCount <= iPrizeSequence; iCount++)
                    lstShufflingResult.Add(dicShufflingItems[sStoreName]);

                while (true)
                {
                    if (lstShufflingResult.Count.Equals(iShufflingColumns))
                        break;

                    var sRandomImage = GeneralUtility.GetDictionaryRandomItems(dicShufflingItems).Take(1).ToList().ElementAt(0);

                    if (!lstShufflingResult.Contains(sRandomImage))
                        lstShufflingResult.Add(sRandomImage);
                }

                if (iPrizeSequence > 0 && iPrizeSequence < iShufflingColumns)
                    GeneralUtility.Shuffle(lstShufflingResult);

                return lstShufflingResult;
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);

                if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageGenericInvoiceError);
            }

            return null;
        }

        private Task<bool> BlinkSlotContainer()
        {
            var oTaskCompletion = new TaskCompletionSource<bool>();
            var oBlinkTimer = new Timer();

            var iBlinkFlag = 0;

            oBlinkTimer.Elapsed += (sender, args) =>
            {
                if (iBlinkFlag == 50)
                {
                    oBlinkTimer.Stop();
                    oTaskCompletion.SetResult(true);
                }

                if (iBlinkFlag % 2 == 0)
                    _view.SetSlotWinnerContainer();
                else
                    _view.SetSlotStandardContainer();

                iBlinkFlag++;
            };

            oBlinkTimer.Start();

            return oTaskCompletion.Task;
        }

        private Dictionary<string, string> _dicShufflingItems { get; set; }

        private Dictionary<string, string> GetShufflingItems()
        {
            if (_dicShufflingItems != null)
            {
                return _dicShufflingItems;
            }

            var dicShufflingItems = new Dictionary<string, string>();

            Directory.GetFiles(AppConfigUtility.sShufflingImagesPath)
                .ToList()
                .ForEach(sItem =>
                {
                    if (string.IsNullOrEmpty(sItem)) return;

                    var sFileName = Path.GetFileNameWithoutExtension(sItem);

                    if (!string.IsNullOrEmpty(sFileName) && !dicShufflingItems.ContainsKey(sFileName))
                    {
                        dicShufflingItems.Add(Path.GetFileNameWithoutExtension(sItem), sItem);
                    }
                });

            _dicShufflingItems = dicShufflingItems;

            return _dicShufflingItems;
        }

        public UserControl UserControl { get { return _view.UserControl; } }
        
        public void SetFocus()
        {
            
        }

        public void SetEnabled(bool enabled)
        {
            
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
