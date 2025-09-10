using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Sms
{
    public class SpeedSmsSettings
    {
        public string AccessToken { get; set; } = "";
        public string ApiUrl { get; set; } = "https://api.speedsms.vn/index.php";
        public int SmsType { get; set; } = 5; // 4: brandname mặc định (Verify hoặc Notify)
        public string Sender { get; set; } = "deviceID"; // brandname mặc định
        public string MessageTemplate { get; set; } = "Mã xác thực của bạn là: {0}";
    }
}
