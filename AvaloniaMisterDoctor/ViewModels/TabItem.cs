using System;
using AvaloniaMisterDoctor.Classes;
using AvaloniaMisterDoctor.Services;

namespace AvaloniaMisterDoctor.ViewModels;

public class TabItem : ViewModelBase
{
    public TabItem()
    {
        InvokeCommand = new FACommand(OnInvokeCommandExecute);
    }

    public string Header { get; set; }

    public string Description { get; set; }

    public string PreviewImageSource { get; set; }

    public string PageType { get; init; }

    public FACommand InvokeCommand { get; }

    private void OnInvokeCommandExecute(object parameter)
    {
        var type = Type.GetType($"MisterDocterNET.Views.{PageType}");
        NavigationService.Instance.Navigate(type);
    }
}