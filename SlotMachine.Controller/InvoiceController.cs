using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using SlotMachine.Controller.Exceptions;
using SlotMachine.Controller.Interfaces;
using SlotMachine.Model;
using SlotMachine.Utility;

namespace SlotMachine.Controller
{
    public class InvoiceController : IController
    {
        private readonly IInvoiceView _view;
        private readonly Wizard _model;
        private WebClient _oWebClient;

        public InvoiceController(IInvoiceView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            _view.InitInstruction = AppConfigUtility.sMessageReadQRCode;
            _view.SetFocus();
            _view.SetEnabled(true);
        }

        public async void HandleInvoiceInput(string sQrCode)
        {
            try
            {
                if (OnStepStartTimeoutEventHandler != null)
                {
                    TimeoutHandler = new Timer { Interval = AppConfigUtility.iWebRequestTimeOut };

                    OnStepStartTimeoutEventHandler();
                }

                _view.SetEnabled(false);

                if (OnStepWarningEventHandler != null) OnStepWaitEventHandler(AppConfigUtility.sMessageWaitCheckInvoiceAndStoreRequest);

                var oInvoice = GetInvoiceFromQrCode(sQrCode);

                if (oInvoice == null)
                {
                    throw new AlertMessageException(AppConfigUtility.sMessageInvalidQRCode);
                }

                if (await IsInvoiceExisting(oInvoice.HashQrCode))
                {
                    throw new AlertMessageException(AppConfigUtility.sMessageInvoiceExisting);
                }

                if (!await IsStoreExisting(oInvoice.Cnpj))
                {
                    throw new AlertMessageException(AppConfigUtility.sMessageStoreNotExisting);
                }

                _model.Invoice = oInvoice;

                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
            }
            catch (AlertMessageException ex)
            {
                if (OnStepWarningEventHandler != null) OnStepWarningEventHandler(ex.Message);
            }
            catch (WebRequestException ex)
            {
                LogUtility.Log(LogUtility.LogType.Warning, MethodBase.GetCurrentMethod().Name, ex.Message);

                if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(ex.Message);
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.SystemError, MethodBase.GetCurrentMethod().Name, ex.Message);

                if (OnStepErrorEventHandler != null) OnStepErrorEventHandler(AppConfigUtility.sMessageGenericInvoiceError);
            }
        }

        private Invoice GetInvoiceFromQrCode(string sQrCode)
        {
            var oInvoice = new Invoice();
            var oKeys = ParseQrCodeInput(sQrCode);

            if (oKeys == null) return null;
            if (!oKeys.ContainsKey("chNFe")) return null;
            if (!oKeys.ContainsKey("nVersao")) return null;
            if (!oKeys.ContainsKey("tpAmb")) return null;
            if (!oKeys.ContainsKey("dhEmi")) return null;
            if (!oKeys.ContainsKey("vNF")) return null;
            if (!oKeys.ContainsKey("vICMS")) return null;
            if (!oKeys.ContainsKey("digVal")) return null;
            if (!oKeys.ContainsKey("cIdToken")) return null;
            if (!oKeys.ContainsKey("cHashQRCode")) return null;

            oInvoice.QrCode = oKeys["QrCode"];
            oInvoice.HashQrCode = oKeys["cHashQRCode"];
            oInvoice.Cpf = oKeys.ContainsKey("cDest") ? DocumentUtility.IsCpfValid(oKeys["cDest"]) ? oKeys["cDest"] : string.Empty : string.Empty;
            oInvoice.Cnpj = oKeys["chNFe"].Substring(6, 14);
            oInvoice.Value = Convert.ToDecimal(oKeys["vNF"].Replace('.', ','));
            oInvoice.Icms = Convert.ToDecimal(oKeys["vICMS"].Replace('.', ','));

            return oInvoice;
        }

        private async Task<bool> IsInvoiceExisting(string sHashQrCode)
        {
            try
            {
                var oRequest = new CheckExistingInvoiceRequest
                {
                    HashQRCode = sHashQrCode
                };

                using (_oWebClient = new WebClient())
                {
                    var oResponseResult = await _oWebClient.UploadStringTaskAsync(AppConfigUtility.sGetExistingInvoiceRequestUrl, "POST", oRequest.ToJSON(true));

                    if (string.IsNullOrEmpty(oResponseResult)) throw new Exception();

                    var oResponse = oResponseResult.FromJSON<CheckExistingInvoiceResponse>();

                    return oResponse.Result;
                }
            }
            catch (Exception)
            {
                throw new WebRequestException(AppConfigUtility.sMessageWebRequestError);
            }
        }

        private async Task<bool> IsStoreExisting(string sCnpj)
        {
            try
            {
                var oRequest = new CheckExistingStoreRequest
                {
                    Cnpj = sCnpj
                };

                using (_oWebClient = new WebClient())
                {
                    var oResponseResult = await _oWebClient.UploadStringTaskAsync(AppConfigUtility.sGetExistingStoreRequestUrl, "POST", oRequest.ToJSON(true));

                    if (string.IsNullOrEmpty(oResponseResult)) throw new Exception("");

                    var oResponse = oResponseResult.FromJSON<CheckExistingStoreResponse>();

                    return oResponse.Result;
                }
            }
            catch (Exception)
            {
                throw new WebRequestException(AppConfigUtility.sMessageWebRequestError);
            }
        }

        private Dictionary<string, string> ParseQrCodeInput(string sQrCode)
        {
            if (string.IsNullOrEmpty(sQrCode)) throw new ArgumentNullException("sQrCode");

            var dicInvoiceQrCodeKeys = GeneralUtility.ParseUrlParameters(sQrCode);

            if (dicInvoiceQrCodeKeys != null)
                dicInvoiceQrCodeKeys.Add("QrCode", sQrCode);

            return dicInvoiceQrCodeKeys;
        }

        public UserControl UserControl { get { return _view.UserControl; } }
        
        public void OnTimeoutTriggered()
        {
            if (_oWebClient != null && _oWebClient.IsBusy)
            {
                _oWebClient.CancelAsync();
            }
        }

        public void SetFocus()
        {
            _view.SetFocus();
        }

        public void SetEnabled(bool enabled)
        {
            _view.SetEnabled(enabled);
        }

        public Timer TimeoutHandler { get; set; }

        public event StepController.StepStartTimeoutEventHandler OnStepStartTimeoutEventHandler;
        public event StepController.StepCompletedEventHandler OnStepCompletedEventHandler;
        public event StepController.StepBackEventHandler OnStepBackEventHandler;
        public event StepController.StepCanceledEventHandler OnStepCanceledEventHandler;
        public event StepController.StepErrorEventHandler OnStepErrorEventHandler;
        public event StepController.StepWarningEventHandler OnStepWarningEventHandler;
        public event StepController.StepWaitEventHandler OnStepWaitEventHandler;
    }
}
