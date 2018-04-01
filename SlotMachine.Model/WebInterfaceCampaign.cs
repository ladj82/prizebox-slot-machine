using System;

namespace SlotMachine.Model
{
    public class GetCampaignTeamplateRequest
    {
        public string InvoiceCnpj { get; set; }

        public string UserCpf { get; set; }

        public string UserGender { get; set; }

        public decimal InvoiceValue { get; set; }
    }

    public class GetCampaignTeamplateReponse
    {
        public int CampaignTemplateId { get; set; }

        public string HeaderTitle { get; set; }

        public string HeaderSubTitle { get; set; }

        public string PrizeCode { get; set; }

        public string Rules { get; set; }

        public DateTime DueDate { get; set; }

        public string PrizeType { get; set; }

        public string StoreName { get; set; }

        public string StoreInfo { get; set; }
    }
}
