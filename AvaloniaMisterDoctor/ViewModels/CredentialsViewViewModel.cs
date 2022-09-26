using System;
using EngineDoctor;
using FluentAvalonia.UI.Controls;

namespace AvaloniaMisterDoctor.ViewModels;

public class CredentialsViewViewModel : ViewModelBase
{
    private readonly ContentDialog dialog;

    public CredentialsViewViewModel(ContentDialog dialog)
    {
        if (dialog is null)
        {
            throw new ArgumentNullException(nameof(dialog));
        }

        this.dialog = dialog;
        dialog.Closed += DialogOnClosed;
    }
    private void DialogOnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        dialog.Closed -= DialogOnClosed;

        if (!Engine.IsConnected) return;
        
        var resultHint = new ContentDialog
        {
            Content = $"Successfully connected the Twitch Bot",
            Title = "Connected",
            PrimaryButtonText = "Awesome"
        };
    
        _ = resultHint.ShowAsync();

    }

    private string _username;

    /// <summary>
    /// Gets or sets the user input to check 
    /// </summary>
    public string Username
    {
        get => _username;
        set
        {
            if (RaiseAndSetIfChanged(ref _username, value))
            {
                //HandleUserInput();
            }
        }
    }

    private string _oauthToken;

    /// <summary>
    /// Gets or sets the user input to check 
    /// </summary>
    public string OAuthToken
    {
        get => _oauthToken;
        set
        {
            if (RaiseAndSetIfChanged(ref _oauthToken, value))
            {
                //HandleUserInput();
            }
        }
    }

    private string _channelName;

    /// <summary>
    /// Gets or sets the user input to check 
    /// </summary>
    public string ChannelName
    {
        get => _channelName;
        set
        {
            if (RaiseAndSetIfChanged(ref _channelName, value))
            {
                //HandleUserInput();
            }
        }
    }
    
    private string _botClientId;

    /// <summary>
    /// Gets or sets the user input to check 
    /// </summary>
    public string BotClientId
    {
        get => _botClientId;
        set
        {
            if (RaiseAndSetIfChanged(ref _botClientId, value))
            {
                //HandleUserInput();
            }
        }
    }
}