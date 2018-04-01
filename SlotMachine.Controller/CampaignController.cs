using System;
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
    public class CampaignController : IController
    {
        private readonly ICampaignView _view;
        private readonly Wizard _model;
        private WebClient _oWebClient;

        public CampaignController(ICampaignView view, Wizard model)
        {
            _view = view;
            _view.SetController(this);
            _model = model;
        }

        public void InitView()
        {
            _view.InitInstruction = AppConfigUtility.sMessageWaitGetCampaignTemplateRequest;
            _view.SetFocus();
            _view.SetEnabled(true);

            if (OnStepStartTimeoutEventHandler != null)
            {
                TimeoutHandler = new Timer { Interval = AppConfigUtility.iWebRequestTimeOut };

                OnStepStartTimeoutEventHandler();
            }

            HandleCampaignTemplateRequest();
        }

        private async void HandleCampaignTemplateRequest()
        {
            try
            {
                var oCampaignContent = await GetCampaignTemplateRequest(_model.Invoice.Cnpj, _model.Invoice.Cpf, _model.User.Gender, _model.Invoice.Value);

                _model.Campaign = new Campaign
                {
                    TemplateId = oCampaignContent.CampaignTemplateId,
                    HeaderTitle = oCampaignContent.HeaderTitle,
                    HeaderSubTitle = oCampaignContent.HeaderSubTitle,
                    PrizeCode = oCampaignContent.PrizeCode,
                    Rules = oCampaignContent.Rules,
                    DueDate = oCampaignContent.DueDate.ToShortDateString(),
                    PrizeType = oCampaignContent.PrizeType,
                    StoreName = oCampaignContent.StoreName,
                    StoreInfo = oCampaignContent.StoreInfo
                };

                if (OnStepCompletedEventHandler != null) OnStepCompletedEventHandler();
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

        private async Task<GetCampaignTeamplateReponse> GetCampaignTemplateRequest(string sCnpj, string sCpf, string sGender, decimal dcmValue)
        {
            try
            {
                var oRequest = new GetCampaignTeamplateRequest
                {
                    InvoiceCnpj = sCnpj,
                    UserCpf = sCpf,
                    UserGender = sGender,
                    InvoiceValue = dcmValue
                };

                using (_oWebClient = new WebClient())
                {
                    var oResponseResult = await _oWebClient.UploadStringTaskAsync(AppConfigUtility.sGetCampaignTemplateRequestUrl, "POST", oRequest.ToJSON(true));

                    return ValidateGetCampaignTeamplateRequest(oResponseResult);
                }
            }
            catch (Exception)
            {
                throw new WebRequestException(AppConfigUtility.sMessageWebRequestError);
            }
        }

        private GetCampaignTeamplateReponse ValidateGetCampaignTeamplateRequest(string sReponse)
        {
            try
            {
                if (string.IsNullOrEmpty(sReponse)) throw new Exception("");

                var oResponse = sReponse.FromJSON<GetCampaignTeamplateReponse>();

                if (oResponse.CampaignTemplateId <= 0)
                    throw new Exception(string.Format("Wrong data: CampaignTemplateId {0}", oResponse.CampaignTemplateId));

                if (string.IsNullOrEmpty(oResponse.HeaderTitle))
                    throw new Exception("Missing data: HeaderTitle");

                if (string.IsNullOrEmpty(oResponse.HeaderSubTitle))
                    throw new Exception("Missing data: HeaderSubTitle");

                if (string.IsNullOrEmpty(oResponse.PrizeCode))
                    throw new Exception("Missing data: PrizeCode");

                if (string.IsNullOrEmpty(oResponse.Rules))
                    throw new Exception("Missing data: Rules");

                if (oResponse.DueDate < DateTime.Now)
                    throw new Exception(string.Format("Wrong data: DueDate {0} < Today {1}", oResponse.CampaignTemplateId, DateTime.Now.ToShortDateString()));

                if (string.IsNullOrEmpty(oResponse.PrizeType))
                    throw new Exception("Missing data: PrizeType");

                try
                {
                    oResponse.PrizeType.ToEnum<Enumerations.PrizeType>();
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Wrong data: PrizeType {0}", oResponse.PrizeType));
                }

                if (string.IsNullOrEmpty(oResponse.StoreName))
                    throw new Exception("Missing data: StoreName");

                if (string.IsNullOrEmpty(oResponse.StoreInfo))
                    throw new Exception("Missing data: StoreInfo");

                return oResponse;
            }
            catch (Exception ex)
            {
                LogUtility.Log(LogUtility.LogType.Warning, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw;
            }
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
            if (_oWebClient != null && _oWebClient.IsBusy)
            {
                _oWebClient.CancelAsync();
            }
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
