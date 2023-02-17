namespace EasyLocalize.Models
{
    public class RegisterMessage
    {
        public string? LocalizeKey { get; set; }
        public string? JsonFilePath { get; set; }
        public bool IsExternal { get; set; } = false;
    }
}