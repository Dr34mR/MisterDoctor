using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EngineDoctor;

namespace AvaloniaMisterDoctor.Views;

public partial class PluginsView : UserControl
{
    public PluginsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

   

    private void PluginsList_OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // Bind list of plugins
        if (sender is not ItemsRepeater repeatingList) return;
        var plugins = Engine.LoadedPlugins;
        repeatingList.Items = plugins;
    }
}