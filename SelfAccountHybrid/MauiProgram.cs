using Microsoft.AspNetCore.Components.WebView.Maui;
using MudBlazor.Services;

namespace SelfAccountHybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
			var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddMudServices();

            var httpclientHandler = new HttpClientHandler();
            httpclientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
            var httpClient = new HttpClient(httpclientHandler);
            builder.Services.AddSingleton<HttpClient>(httpClient);

            return builder.Build();
        }
    }
}