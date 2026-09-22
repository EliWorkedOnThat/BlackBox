using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.IO;
using Microsoft.Win32;

namespace BlackBox;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GreetMessage();
    }

    public void GreetMessage()
    {
        SnapshotDisplay.Text = "Welcome to Black Box Save and Restore Files as you please!";
    }

    private void SelectSnapshot(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            string selectedFile = dialog.FileName;

            SnapshotDisplay.Text = selectedFile;
        }
    }
}