using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaMisterDoctor.ViewModels;
using EngineDoctor;
using EngineDoctor.Helpers;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Input;
using MessageBox.Avalonia.Enums;

namespace AvaloniaMisterDoctor.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();

        var appSettings = DbHelper.ReadConnectionSettings();
        Engine.Setup(appSettings);
        Engine.ErrorMessages.ListChanged += ErrorMessagesOnListChanged;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void BtnBotStart_OnClick(object? sender, RoutedEventArgs e)
    {
        // Button Bot Start Click event handler

        Engine.Connect();
    }
    
    private void ErrorMessagesOnListChanged(object? sender, ListChangedEventArgs e)
    {
        if (sender is not BindingList<string> errorList) return;
        if (e.ListChangedType == ListChangedType.ItemAdded)
        {
            // If message is added show a message box
            var errorMessage = errorList[e.NewIndex];
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Error", errorMessage, ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error);
            messageBoxStandardWindow.Show();    
        }
    }

    private async void BtnConfigureCredentials_OnClick(object? sender, RoutedEventArgs e)
    {
        // Throw login box
        var loginDialog = new ContentDialog
        {
            Title = "Configure your Twitch Credentials",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
        };

        var viewModel = new CredentialsViewViewModel(loginDialog);
        var connectionSettings = DbHelper.ReadConnectionSettings();
        
        viewModel.ChannelName = connectionSettings.ChannelName;
        viewModel.Username = connectionSettings.BotUsername;
        viewModel.OAuthToken = connectionSettings.BotOAuthKey;
        viewModel.BotClientId = connectionSettings.BotClientId;
       
        loginDialog.Content = new ConfigureCredentialsView
        {
            DataContext = viewModel
        };

        loginDialog.PrimaryButtonClick += ConfigureConnection;
        
        var result = await loginDialog.ShowAsync();

        loginDialog.Closed += (dialog, args) => loginDialog.PrimaryButtonClick -= ConfigureConnection;
    }
    
    private async void ConfigureConnection(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        var def = args.GetDeferral();
        //Engine.Setup();
        if (sender.Content is not ConfigureCredentialsView credentialsView) return;
        if (credentialsView.DataContext is not CredentialsViewViewModel viewModel)
        {
            return;
        }
        var channel = viewModel.ChannelName;
        if (string.IsNullOrEmpty(channel))
        {
            ShowError("Channel Name can not be blank");
            args.Cancel = true;
            return;
        }

        var userName = viewModel.Username;
        if (string.IsNullOrEmpty(userName))
        {
            ShowError("No Username Set");
            args.Cancel = true;
            return;
        }

        var oAuth = viewModel.OAuthToken;
        if (string.IsNullOrEmpty(oAuth))
        {
            ShowError("No OAuth Set");
            args.Cancel = true;
            return;
        }

        if (!oAuth.StartsWith("oauth:", StringComparison.CurrentCultureIgnoreCase))
        {
            ShowError("OAuth Key must start with 'oauth:'");
            args.Cancel = true;
            return;
        }

        var clientId = viewModel.BotClientId;
        if (string.IsNullOrEmpty(clientId))
        {
            ShowError("No Client ID");
            args.Cancel = true;
        
            return;
        }

        var connectionSettings = DbHelper.ReadConnectionSettings();
        connectionSettings.BotUsername = userName;
        connectionSettings.ChannelName = channel;
        connectionSettings.BotOAuthKey = oAuth;
        connectionSettings.BotClientId = clientId;
        
        Engine.UpdateSettings(connectionSettings);
        Engine.Connect();
        var errors = Engine.ErrorMessages;
        if (errors.Any())
        {
            ShowError(errors.First());
            args.Cancel = true;
            return;
        }
        
        def.Complete();
    }

    private void ShowError(string message)
    {
        var errorDialog = new ContentDialog()
        {
            Content = $"\"{message}\"",
            Title = "Error",
            PrimaryButtonText = "Ok"
        };
    
        _ = errorDialog.ShowAsync();
    }
}