namespace SharedLibrary.Email
{
    public class EmailSettings
    {
        public bool EnableSsl { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 30;
        public string SMTP_SERVER { get; set; } = "";
        public int SMTP_PORT { get; set; }
        public string SENDER_NAME { get; set; } = "";
        public string SENDER_EMAIL { get; set; } = "";
        public string SENDER_PASSWORD { get; set; } = "";
    }
}
