namespace SharedLibrary.Email
{
    public class EmailSettings
    {
        public string SMTP_SERVER { get; set; } = "";
        public int SMTP_PORT { get; set; }
        public string SENDER_NAME { get; set; } = "";
        public string SENDER_EMAIL { get; set; } = "";
        public string SENDER_PASSWORD { get; set; } = "";
    }
}
