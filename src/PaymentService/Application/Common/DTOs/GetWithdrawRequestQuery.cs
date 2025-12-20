namespace PaymentService.Application.Common.DTOs
{
    public class GetWithdrawRequestQuery
    {
        public int Bin { get; set; }
        public string AccountNumber { get; set; }
    }
    public class VietQrLookupResponse
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public VietQrLookupData Data { get; set; }
    }

    public class VietQrLookupData
    {
        public string AccountName { get; set; }
    }

}
