﻿using Microsoft.Extensions.Logging;
using ExpenseTracker.Services; // Add this line

namespace ExpenseTracker
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
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ExpenseService>();
            builder.Services.AddSingleton<TagService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<DebtService>();
            return builder.Build();
        }
    }
}