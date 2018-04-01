namespace SlotMachine.Model
{
    public class Campaign
    {
        public int TemplateId { get; set; }

        public string HeaderTitle { get; set; }

        public string HeaderSubTitle { get; set; }

        public string PrizeCode { get; set; }

        public string Rules { get; set; }

        public string DueDate { get; set; }

        public string PrizeType { get; set; }

        public string StoreName { get; set; }

        public string StoreInfo { get; set; }
    }
}
