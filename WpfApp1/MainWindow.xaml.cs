using System;
using System.Diagnostics;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Collections.Generic.List<Launchable> _launchables;

        public MainWindow()
        {
            InitializeComponent();

            _launchables = new System.Collections.Generic.List<Launchable>();
            
            LoadLaunchables();
            CreateButtons();
        }

        private void LoadLaunchables()
        {
            int maxCount = 0;
            if (LauncherSettings.Default.Apps.Count > maxCount) { maxCount = LauncherSettings.Default.Apps.Count; }
            if (LauncherSettings.Default.Args.Count > maxCount) { maxCount = LauncherSettings.Default.Args.Count; }
            if (LauncherSettings.Default.Labels.Count > maxCount) { maxCount = LauncherSettings.Default.Labels.Count; }
            if (LauncherSettings.Default.Icons.Count > maxCount) { maxCount = LauncherSettings.Default.Icons.Count; }

            for (int i = 0; i < maxCount; i++)
            {
                Launchable newButton = new Launchable();
                newButton.app = LauncherSettings.Default.Apps[i];
                newButton.args = LauncherSettings.Default.Args[i];
                newButton.label = LauncherSettings.Default.Labels[i];
                newButton.icon = LauncherSettings.Default.Icons[i];
                _launchables.Add(newButton);
            }
        }

        private void SaveLaunchables()
        {
            LauncherSettings.Default.Apps.Clear();
            LauncherSettings.Default.Args.Clear();
            LauncherSettings.Default.Labels.Clear();
            LauncherSettings.Default.Icons.Clear();

            foreach (Launchable curButton in _launchables)
            {
                LauncherSettings.Default.Apps.Add(curButton.app);
                LauncherSettings.Default.Args.Add(curButton.args);
                LauncherSettings.Default.Labels.Add(curButton.label);
                LauncherSettings.Default.Icons.Add(curButton.icon);
            }

            LauncherSettings.Default.Save();
        }

        private void CreateButtons()
        {
            for (int i = 0; i < _launchables.Count; i++)
            {
                Launchable curButton = _launchables[i];

                Button MyControl = new Button();
                MyControl.Content = curButton.label;
                MyControl.Click += new RoutedEventHandler(this.button_Click);
                MyControl.Tag = i;

                Grid.SetRow(MyControl, _launchables.IndexOf(curButton));
                Grid.SetColumn(MyControl, 0);
                gridMain.Children.Add(MyControl);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int i = (int) button.Tag;
            if (!String.IsNullOrEmpty(_launchables[i].app))
            {
                Process.Start(_launchables[i].app, _launchables[i].args);
            }
            else if (!String.IsNullOrEmpty(_launchables[i].args))
            {
                Process.Start(_launchables[i].args);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveLaunchables();
        }

        private void cmdSettings_Click(object sender, RoutedEventArgs e)
        {
            string notepadpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Notepad++", "notepad++.exe");
            Process.Start(notepadpath, ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        }
    }
}
