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
    private string storagePath = "";

    public MainWindow()
    {   
        InitializeComponent();
        storagePath = CreateStorageFolder();
        GreetMessage();
    }

    public void GreetMessage()
    {
        SnapshotDisplay.Text = "Welcome to Black Box Save and Restore Files as you please!\n";
        SnapshotDisplay.Text += $"Storage: {storagePath}\n\n";
    }
    
    private string CreateStorageFolder()
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string path = System.IO.Path.Combine(appData, "BlackBox", "Snapshots");

        Directory.CreateDirectory(path);

        return path;
    }
    
    private string WriteChunksToDisk(List<byte[]> chunks)
    {
        string snapshotId = Guid.NewGuid().ToString();

        string snapshotFolder = System.IO.Path.Combine(storagePath, snapshotId);
        Directory.CreateDirectory(snapshotFolder);

        for (int i = 0; i < chunks.Count; i++)
        {
            string chunkPath = System.IO.Path.Combine(snapshotFolder, $"chunk_{i:D4}.bin");
            File.WriteAllBytes(chunkPath, chunks[i]);
        }

        return snapshotFolder;
    }


    private void OpenRestoreWindow(object sender, RoutedEventArgs e)
        {
            RestoreWindow window = new RestoreWindow(snapshots);

            window.ShowDialog();
        }

    private const int ChunkSize = 1024 * 1024;

   private List<byte[]> SplitIntoChunks(byte[] data)
    {
        List<byte[]> chunks = new();

        for (int offset = 0; offset < data.Length; offset += ChunkSize)
        {
            int length = Math.Min(ChunkSize, data.Length - offset);

            byte[] chunk = new byte[length];
            Array.Copy(data, offset, chunk, 0, length);

            chunks.Add(chunk);
        }

        return chunks;
    }

    private void SelectSnapshot(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            string selectedFile = dialog.FileName;

            byte[] fileData = File.ReadAllBytes(selectedFile);

            List<byte[]> chunks = SplitIntoChunks(fileData);

            string snapshotFolder = WriteChunksToDisk(chunks);

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
                $"Size: {fileData.Length} bytes\n" +
                $"Chunks: {chunks.Count}\n"+
                $"Saved to: {snapshotFolder}\n\n";
        }
    }

        private void UpdateFileCount()
    {
        FileCountDisplay.Text = $"File Count: {snapshots.Count}";
    }

}