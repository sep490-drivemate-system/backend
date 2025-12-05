using SharedLibrary.SharedKernel.Http.Implementation;

namespace PaymentService.Application.Common.Constants
{
    public static class Messages
    {
        public static class Wallet
        {
            public const string UnsupportedPaymentMethod = "Không tồn tại phương thức thanh toán";
            public const string UnHandlePayment = "Thanh toán có lỗi. Vui lòng thử lại";
        }     

        public static class Transaction
        {
           
        }       
    }
}
