using System;
using AvaloniaMisterDoctor.Classes;
using AvaloniaMisterDoctor.Services;

namespace AvaloniaMisterDoctor.ViewModels;

public class PluginsViewModel : ViewModelBase
{
    public PluginsViewModel()
    {
        InvokeCommand = new FACommand(OnInvokeCommandExecute);
    }
    
    public string Header { get; init; }

    public string Description { get; init; }

    public bool Navigates { get; init; }

    public string PageType { get; init; }

    public FACommand InvokeCommand { get; }

    private void OnInvokeCommandExecute(object parameter)
    {
        var type = Type.GetType($"MisterDocterNET.Views.{PageType}");            
        NavigationService.Instance.Navigate(type);
    }
}