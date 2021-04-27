namespace MPData.Settings
{
    public class UserSettings : BaseNotificationClass
    {
        public string ExportPath { get; set; }
        public string TemplatePath { get; set; }
        public bool SaveAsPdf { get; set; }
    }
}
