// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using HocTiengAnh.Views.Pages;
using HocTiengAnh.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui;

namespace HocTiengAnh.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _serviceProvider.GetService<LogService>()!.Info("App closed!");
            var configService = _serviceProvider.GetService<ConfigureService>();
            configService!.Save();
            if (!string.IsNullOrEmpty(configService.AppConfig.CurrentDataFile))
            {
                string path = configService.AppConfig.CurrentDataFile;
                var wordsService = _serviceProvider.GetService<WordsService>();
                wordsService!.SaveToFile(path);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            _serviceProvider.GetService<LogService>()!.Info("App Started!");
            var configService = _serviceProvider.GetService<ConfigureService>();
            configService!.Load();
            if(!string.IsNullOrEmpty(configService.AppConfig.CurrentDataFile))
            {
                string path = configService.AppConfig.CurrentDataFile;
                var wordsService = _serviceProvider.GetService<WordsService>();
                wordsService!.LoadFromFile(path);
            }
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _navigationWindow = (
                    _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow
                )!;
                _navigationWindow!.ShowWindow();
                _navigationWindow.Navigate(typeof(Views.Pages.HomePage));
            }
            await Task.CompletedTask;
        }
    }
}
