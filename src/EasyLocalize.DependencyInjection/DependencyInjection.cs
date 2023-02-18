using EasyLocalize.Contracts;
using EasyLocalize.Implementation;
using EasyLocalize.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EasyLocalize.DependencyInjection;

public static class DependencyInjection
{
    public static void AddEasyLocalizer(this IServiceCollection service, Action<EasyLocalizeOptions> action)
    {
        var easyLocalizeOptions = new EasyLocalizeOptions();
        action.Invoke(easyLocalizeOptions);
        service.AddSingleton<IMessage>(_ => new Message(easyLocalizeOptions));
    }
}