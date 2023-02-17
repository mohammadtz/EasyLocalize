using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasyLocalize.Contracts;
using EasyLocalize.Models;
using Newtonsoft.Json;

namespace EasyLocalize.Implementation
{
    public class Message : IMessage
    {
        private readonly IEnumerable<RegisterMessage> _registerMessages;
        private readonly string _defaultLanguage;
        private string _language;
        private readonly Dictionary<string, Dictionary<string, string>> _messages;
        public string DefaultHeaderName { get; }

        public string DefaultLanguage => _language;

        public Message(EasyLocalizeOptions options)
        {
            _registerMessages = options.RegisterMessages;
            _language = options.DefaultLanguage;
            _defaultLanguage = options.DefaultLanguage;
            _messages = new Dictionary<string, Dictionary<string, string>>();
            DefaultHeaderName = options.DefaultHeaderName;
            FillData();
        }
        
        private void FillData()
        {
            foreach (var registerMessage in _registerMessages)
            {
                if (registerMessage == null)
                    throw new Exception("EasyLocalize: register Messages Is Not Valid");
        
                if(registerMessage.JsonFilePath != null && !registerMessage.JsonFilePath.EndsWith(".json"))
                    throw new Exception("EasyLocalize: json file path is invalid");

                var path = registerMessage.IsExternal 
                    ? registerMessage.JsonFilePath 
                    : AppDomain.CurrentDomain.BaseDirectory + registerMessage.JsonFilePath;
                
                var readFile = File.ReadAllText(path ?? string.Empty);

                if (readFile.Length == 0)
                    throw new Exception("EasyLocalize: " + "json file is invalid");

                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(readFile);
            
                if(registerMessage.LocalizeKey != null && data != null)
                    _messages.Add(registerMessage.LocalizeKey.ToLower(), data);
            }
        }
        
        private Dictionary<string, string> GetMessagesByDefault
        {
            get
            {
                _messages.TryGetValue(_language.ToLower(), out var values);
                return values ?? new Dictionary<string, string>();
            }
        }
        
        public EasyLocalizeResponse Get(string key, params object[] parameters)
        {
            var defaultMessage = GetMessagesByDefault;
            defaultMessage.TryGetValue(key, out var value);
            return new EasyLocalizeResponse { Value = value ?? key, IsValid = value != null };
        }
        
        public void SetLanguage(string? acceptedLanguage)
        {
            if(acceptedLanguage != null && _registerMessages.Any(registerMessage=> registerMessage.LocalizeKey?.ToLower() == acceptedLanguage.ToLower()))
                _language =  acceptedLanguage; 
            else _language = _defaultLanguage;
        }
    }
}