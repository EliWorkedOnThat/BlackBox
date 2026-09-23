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
using System.Collections.Generic;

namespace BlackBox;

public partial class MainWindow : Window
{

    public List<FileSnapshot> snapshots = new();

    public MainWindow()
    {   
        InitializeComponent();
        GreetMessage();
    }

    public void GreetMessage()
    {
        SnapshotDisplay.Text = "Welcome to Black Box Save and Restore Files as you please!";
    }
    
    private void OpenRestoreWindow(object sender, RoutedEventArgs e)
        {
            RestoreWindow window = new RestoreWindow(snapshots);

            window.ShowDialog();
        }

    private void SelectSnapshot(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            string selectedFile = dialog.FileName;

            byte[] fileData = File.ReadAllBytes(selectedFile);

            FileSnapshot snapshot = new FileSnapshot
                {
                    OriginalPath = selectedFile,
                    Data = fileData
                };

                snapshots.Add(snapshot);
                UpdateFileCount();

            SnapshotDisplay.Text +=
                $"File: {System.IO.Path.GetFileName(selectedFile)}\n" +
                $"Path: {selectedFile}\n" +
                $"Size: {fileData.Length} bytes\n\n";
        }
    }

        private void UpdateFileCount()
    {
        FileCountDisplay.Text = $"File Count: {snapshots.Count}";
    }

}