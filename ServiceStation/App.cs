﻿using System.Windows;
using ServiceStation.ViewModels.Abstraction;
using ServiceStation.ViewModels.Implementation;

namespace ServiceStation;

public class App(MainViewModel viewModel) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = new MainWindow()
        {
            DataContext = viewModel
        };
        mainWindow.Show();
        
        base.OnStartup(e);
    }
}