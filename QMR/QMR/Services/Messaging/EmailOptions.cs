namespace QMR.Services.Messaging
{
    public class EmailOptions
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string MailUserId { get; set; }
        public string MailPassword { get; set; }
        public string AdminEmail { get; set; }
        public string SupportEmail { get; set; }
    }
}
