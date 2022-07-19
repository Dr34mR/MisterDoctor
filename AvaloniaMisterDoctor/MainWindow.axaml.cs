using Avalonia.Controls;
using Avalonia.Interactivity;
using EngineDoctor;
using EngineDoctor.Helpers;

namespace AvaloniaMisterDoctor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var appSettings = DbHelper.ReadConnectionSettings();
            Engine.Setup(appSettings);
        }

        private void BtnBotStart_OnClick(object? sender, RoutedEventArgs e)
        {
            // Button Bot Start Click event handler

            Engine.Connect();
        }
    }
}