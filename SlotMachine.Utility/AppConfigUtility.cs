using System;
using System.Configuration;

namespace SlotMachine.Utility
{
    public static class AppConfigUtility
    {
        public static readonly string sLogFileName = GeneralUtility.AssemblyDirectory + @"\log.xml";
        public static readonly string sAppImagesPath = GeneralUtility.AssemblyDirectory + ConfigurationManager.AppSettings["AppImagesPath"];
        public static readonly string sShufflingImagesPath = GeneralUtility.AssemblyDirectory + ConfigurationManager.AppSettings["ShufflingImagesPath"];

        public static readonly string sPrinterManufacturer = ConfigurationManager.AppSettings["PrinterManufacturer"];
        public static readonly string sPrinterName = ConfigurationManager.AppSettings["PrinterName"];
        public static readonly string sPrinterModel = ConfigurationManager.AppSettings["PrinterModel"];
        public static readonly string sPrinterConnectionType = ConfigurationManager.AppSettings["PrinterConnectionType"];

        public static readonly string sMessageGenericAppError = ConfigurationManager.AppSettings["MessageGenericAppError"];
        public static readonly string sMessageGenericInvoiceError = ConfigurationManager.AppSettings["MessageGenericInvoiceError"];
        public static readonly string sMessageTimeoutTriggered = ConfigurationManager.AppSettings["MessageTimeoutTriggered"];
        public static readonly string sMessageReadQRCode = ConfigurationManager.AppSettings["MessageReadQRCode"];
        public static readonly string sMessageInvalidQRCode = ConfigurationManager.AppSettings["MessageInvalidQRCode"];
        public static readonly string sMessageStoreNotExisting = ConfigurationManager.AppSettings["MessageStoreNotExisting"];
        public static readonly string sMessageInvoiceExisting = ConfigurationManager.AppSettings["MessageInvoiceAlreadyUsed"];
        public static readonly string sMessageInformCpf = ConfigurationManager.AppSettings["MessageInformCpf"];
        public static readonly string sMessageInformGender = ConfigurationManager.AppSettings["MessageInformGender"];
        public static readonly string sMessageInvalidCpf = ConfigurationManager.AppSettings["MessageInvalidCpf"];
        public static readonly string sMessageInvalidGender = ConfigurationManager.AppSettings["MessageInvalidGender"];
        public static readonly string sMessageWaitCheckInvoiceAndStoreRequest = ConfigurationManager.AppSettings["MessageWaitCheckInvoiceAndStoreRequest"];
        public static readonly string sMessageWaitGetCampaignTemplateRequest = ConfigurationManager.AppSettings["MessageWaitGetCampaignTemplateRequest"];
        public static readonly string sMessageWebRequestError = ConfigurationManager.AppSettings["MessageWebRequestError"];
        public static readonly string sMessageInitShuffling = ConfigurationManager.AppSettings["MessageInitShuffling"];
        public static readonly string sMessageNonePrize = ConfigurationManager.AppSettings["MessageNonePrize"];
        public static readonly string sMessageWinPrizeHeader = ConfigurationManager.AppSettings["MessageWinPrizeHeader"];
        public static readonly string sMessageWinPrizeSubHeader = ConfigurationManager.AppSettings["MessageWinPrizeSubHeader"];
        public static readonly string sMessageWinPrizeTakeCupom = ConfigurationManager.AppSettings["MessageWinPrizeTakeCupom"];
        public static readonly string sMessageNonePrizeHeader = ConfigurationManager.AppSettings["MessageNonePrizeHeader"];
        public static readonly string sMessageNonePrizeSubHeader = ConfigurationManager.AppSettings["MessageNonePrizeSubHeader"];

        public static readonly string sGetExistingInvoiceRequestUrl = ConfigurationManager.AppSettings["GetExistingInvoiceRequestUrl"];
        public static readonly string sGetExistingStoreRequestUrl = ConfigurationManager.AppSettings["GetExistingStoreRequestUrl"];
        public static readonly string sGetCampaignTemplateRequestUrl = ConfigurationManager.AppSettings["GetCampaignTemplateRequestUrl"];

        public static readonly int iShufflingInterval = Convert.ToInt32(ConfigurationManager.AppSettings["ShufflingInterval"]);
        public static readonly int iShuffleInteractions = Convert.ToInt32(ConfigurationManager.AppSettings["ShuffleInteractions"]);
        public static readonly int iShuffleResultDelay = Convert.ToInt32(ConfigurationManager.AppSettings["ShuffleResultDelay"]);
        public static readonly int iAlertTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["AlertTimeOut"]);
        public static readonly int iNoInteractionTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["NoInteractionTimeOut"]);
        public static readonly int iWebRequestTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["WebRequestTimeOut"]);
        public static readonly bool bLogSystemError = Convert.ToBoolean(ConfigurationManager.AppSettings["LogInformationFlag"]);
        public static readonly bool bLogInformation = Convert.ToBoolean(ConfigurationManager.AppSettings["LogSystemErrorFlag"]);
    }
}
