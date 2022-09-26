using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using MessageBox.Avalonia.Enums;


namespace AvaloniaMisterDoctor.Views
{
    public partial class MainWindow : CoreWindow
    {
        private NavigationViewItem _lastItem;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
           
        }

        private void ContentFrame_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            // Error for navigation failed
            var errorMessage = e.Exception.Message;
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Navigation Failed", errorMessage, ButtonEnum.Ok, MessageBox.Avalonia.Enums.Icon.Error);
            messageBoxStandardWindow.Show();    
        }
        
        private void NavView_OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
        {
            // Nav item is invoked/clicked
            var item = e.InvokedItemContainer as NavigationViewItem;
            if (item == null || item == _lastItem) return;
        
            var viewName = item.Tag as string ?? "";
            
            if (!NavigateToView(viewName)) return;
        
            _lastItem = item;
        }
        
        private bool NavigateToView(string clickedView)
        {
            Type view;
            switch (clickedView)
            {
                case "HomeView":
                case "Home":
                {
                    view = typeof(HomeView);
                    break;
                }
                case "PluginsView":
                case "Plugins":
                {
                    view = typeof(PluginsView);
                    break;
                }
                case "ChannelView":
                case "Channel":
                {
                    view = typeof(ChannelView);
                    break;
                }
                case "SettingsView":
                case "Settings":
                {
                    view = typeof(SettingsView);
                    break;
                }
                default:
                {
                    return false;
                }
            }
        
            ContentFrame.Navigate(view, null, new EntranceNavigationTransitionInfo());
        
            return true;
        }
        
        private void NavView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
        {
            // Nav view selection changed
        }
        
        private void NavView_OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item.Tag != null && item is NavigationViewItem && item.Tag is "HomeView")
                {
                    item.IsSelected = true;
                }
            }
            
            ContentFrame.Navigate(typeof(HomeView));
        }
    }
}