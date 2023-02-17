using EasyLocalize.Models;

namespace EasyLocalize.Contracts
{
    public interface IMessage
    {
        string DefaultHeaderName { get; }
        string DefaultLanguage { get; }
        EasyLocalizeResponse Get(string key, params object[] parameters);
        void SetLanguage(string? acceptedLanguage);
    }
}