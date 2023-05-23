﻿using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace WpfApp;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
//public partial class App : Application
//{
//}

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host
            .CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Modern>();
                

                Resources.Add("services", services.BuildServiceProvider());
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<Modern>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}