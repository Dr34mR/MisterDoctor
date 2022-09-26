using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AvaloniaMisterDoctor.Services;
using AvaloniaMisterDoctor.ViewModels;
using FluentAvalonia.Core;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using MessageBox.Avalonia.Enums;

namespace AvaloniaMisterDoctor.Views;

public partial class MainView : UserControl
{
    private NavigationViewItem _lastItem;


    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // Changed for SplashScreens:
        // -- If using a SplashScreen, the window will be available when this is attached
        //    and we can just call OnParentWindowOpened
        // -- If not using a SplashScreen (like before), the window won't be initialized
        //    yet and setting our custom titlebar won't work... so wait for the 
        //    WindowOpened event first
        if (e.Root is Window b)
        {
            if (!b.IsActive)
                b.Opened += OnParentWindowOpened;
            else
                OnParentWindowOpened(b, null);
        }

        _windowIconControl = this.FindControl<IControl>("WindowIcon");
        _frameView = this.FindControl<Frame>("FrameView");
        _navView = this.FindControl<NavigationView>("NavView");
        _navView.MenuItems = GetNavigationViewItems();
        _navView.FooterMenuItems = GetFooterNavigationViewItems();

        _frameView.Navigated += OnFrameViewNavigated;
        _navView.ItemInvoked += OnNavigationViewItemInvoked;
        _navView.BackRequested += OnNavigationViewBackRequested;

        _frameView.Navigate(typeof(HomeView));

        NavigationService.Instance.SetFrame(_frameView);
        NavigationService.Instance.SetOverlayHost(this.FindControl<Panel>("OverlayHost"));
    }
    
    private void FrameView_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        // Error for navigation failed
        var errorMessage = e.Exception.Message;
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow("Navigation Failed", errorMessage, ButtonEnum.Ok,
                MessageBox.Avalonia.Enums.Icon.Error);
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

        FrameView.Navigate(view, null, new EntranceNavigationTransitionInfo());

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

        FrameView.Navigate(typeof(HomeView));
    }
    
    private List<NavigationViewItem> GetNavigationViewItems()
        {
            return new List<NavigationViewItem>
            {
                new NavigationViewItem
                {
                    Content = "Home",
                    Tag = typeof(HomeView),
                    Icon = new FluentAvalonia.UI.Controls.IconElement{  },
                    Classes =
                    {
                        "SampleAppNav"
                    }
                },
                new NavigationViewItem
                {
                    Content = "Channel",
                    Tag = typeof(ChannelView),
                    Icon = new IconSourceElement { IconSource = (IconSource)this.FindResource("ResourcesIcon") },
                    Classes =
                    {
                        "SampleAppNav"
                    }
                },
                new NavigationViewItem
                {
                    Content = "Plugins",
                    Tag = typeof(PluginsView),
                    Icon = new IconSourceElement { IconSource = (IconSource)this.FindResource("CoreCtrlsIcon") },
                    Classes =
                    {
                        "SampleAppNav"
                    }
                },
                new NavigationViewItem
                {
                    Content = "Settings",
                    Tag = typeof(SettingsView),
                    Icon = new IconSourceElement { IconSource = (IconSource)this.FindResource("CtrlsIcon") },
                    Classes =
                    {
                        "SampleAppNav"
                    }
                },
               
            };
        }

    private List<NavigationViewItem> GetFooterNavigationViewItems()
        {
            return new List<NavigationViewItem>
            {
                new NavigationViewItem
                {
                    Content = "Settings",
                    Tag = typeof(SettingsView),
                    Icon = new IconSourceElement { IconSource = (IconSource)this.FindResource("SettingsIcon") },
                    Classes =
                    {
                        "SampleAppNav"
                    }
                }
            };
        }
        
    private void OnParentWindowOpened(object sender, EventArgs e)
    {
        if (e != null)
            (sender as Window).Opened -= OnParentWindowOpened;

        if (sender is CoreWindow cw)
        {
            var titleBar = cw.TitleBar;
            if (titleBar != null)
            {
                titleBar.ExtendViewIntoTitleBar = true;

                titleBar.LayoutMetricsChanged += OnApplicationTitleBarLayoutMetricsChanged;

                if (this.FindControl<Grid>("TitleBarHost") is Grid g)
                {
                    cw.SetTitleBar(g);
                    g.Margin = new Thickness(0, 0, titleBar.SystemOverlayRightInset, 0);
                }
            }                
        }
    }

    private void OnApplicationTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
    {
        if (this.FindControl<Grid>("TitleBarHost") is Grid g)
        {
            g.Margin = new Thickness(0, 0, sender.SystemOverlayRightInset, 0);
        }
    }
    
    private void OnNavigationViewBackRequested(object sender, NavigationViewBackRequestedEventArgs e)
    {
        _frameView.GoBack();
    }

    private void OnNavigationViewItemInvoked(object sender, NavigationViewItemInvokedEventArgs e)
    {
        // Change the current selected item back to normal
        SetNVIIcon(_navView.SelectedItem as NavigationViewItem, false);

        if (e.InvokedItemContainer is NavigationViewItem nvi && nvi.Tag is Type typ)
        {
            _frameView.Navigate(typ, null, e.RecommendedNavigationTransitionInfo);
        }
    }
    
    private void SetNVIIcon(NavigationViewItem item, bool selected)
    {
        // Technically, yes you could set up binding and converters and whatnot to let the icon change
        // between filled and unfilled based on selection, but this is so much simpler 

        if (item == null)
            return;

        Type t = item.Tag as Type;

        if (t == typeof(HomeView))
        {
            (item.Icon as IconSourceElement).IconSource = this.TryFindResource(selected ? "HomeIconFilled" : "HomeIcon", out var value) ? 
                (IconSource)value : null;
        }
        else if (t == typeof(ChannelView))
        {
            (item.Icon as IconSourceElement).IconSource = this.TryFindResource(selected ? "CoreCtrlsIconFilled" : "CoreCtrlsIcon", out var value) ?
                (IconSource)value : null;
        }
        // Skip NewControlsPage as its icon is the same for both
        else if (t == typeof(PluginsView))
        {
            (item.Icon as IconSourceElement).IconSource = this.TryFindResource(selected ? "ResourcesIconFilled" : "ResourcesIcon", out var value) ?
                (IconSource)value : null;
        }
        else if (t == typeof(SettingsView))
        {
            (item.Icon as IconSourceElement).IconSource = this.TryFindResource(selected ? "SettingsIconFilled" : "SettingsIcon", out var value) ?
                (IconSource)value : null;
        }
    }
    
    private void OnFrameViewNavigated(object sender, NavigationEventArgs e)
    {
        bool found = false;
        foreach (NavigationViewItem nvi in _navView.MenuItems)
        {
            if (nvi.Tag is Type tag && tag == e.SourcePageType)
            {
                found = true;
                _navView.SelectedItem = nvi;
                SetNVIIcon(nvi, true);
                break;
            }
        }

        if (!found)
        {
            if (e.SourcePageType == typeof(SettingsView))
            {
                _navView.SelectedItem = _navView.FooterMenuItems.ElementAt(0);
            }
            else
            {
                // only remaining page type is core controls pages
                _navView.SelectedItem = _navView.MenuItems.ElementAt(1);
            }
        }         

        if (_frameView.BackStackDepth > 0 && !_navView.IsBackButtonVisible)
        {
            AnimateContentForBackButton(true);
        }
        else if (_frameView.BackStackDepth == 0 && _navView.IsBackButtonVisible)
        {
            AnimateContentForBackButton(false);
        }
    }

       private async void AnimateContentForBackButton(bool show)
        {
            if (show)
            {
                var ani = new Animation
                {
                    Duration = TimeSpan.FromMilliseconds(250),
                    FillMode = FillMode.Forward,
                    Children =
                    {
                        new KeyFrame
                        {
                            Cue = new Cue(0d),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(12, 4, 12, 4))
                            }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(1d),
                            KeySpline = new KeySpline(0,0,0,1),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(48,4,12,4))
                            }
                        }
                    }
                };

                await ani.RunAsync((Animatable)_windowIconControl, null);

                _navView.IsBackButtonVisible = true;
            }
            else
            {
                _navView.IsBackButtonVisible = false;

                var ani = new Animation
                {
                    Duration = TimeSpan.FromMilliseconds(250),
                    FillMode = FillMode.Forward,
                    Children =
                    {
                        new KeyFrame
                        {
                            Cue = new Cue(0d),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(48, 4, 12, 4))
                            }
                        },
                        new KeyFrame
                        {
                            Cue = new Cue(1d),
                            KeySpline = new KeySpline(0,0,0,1),
                            Setters =
                            {
                                new Setter(MarginProperty, new Thickness(12,4,12,4))
                            }
                        }
                    }
                };

                await ani.RunAsync((Animatable)_windowIconControl, null);              
            }
        }

        private IControl _windowIconControl;
        private Frame _frameView;
        private NavigationView _navView;
}