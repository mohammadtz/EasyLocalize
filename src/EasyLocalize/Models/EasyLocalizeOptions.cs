using System.Collections.Generic;

namespace EasyLocalize.Models
{
    public class EasyLocalizeOptions
    {
        public IEnumerable<RegisterMessage> RegisterMessages { get; set; } = new List<RegisterMessage>();
        public string DefaultLanguage { get; set; } = "en-US";
        public string DefaultHeaderName { get; set; } = "Accept-Language";
    }
}