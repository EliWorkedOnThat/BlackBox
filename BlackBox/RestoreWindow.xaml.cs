using System.Collections.Generic;
using System.Windows;
using System.IO;
namespace BlackBox;

public partial class RestoreWindow : Window
{
    private List<FileSnapshot> snapshots;

    private void DisplaySnapshots()
    {
        foreach (FileSnapshot snapshot in snapshots)
        {
            SnapshotList.Items.Add(snapshot.OriginalPath);
        }
    }

   public void RestoreBytes(object sender, RoutedEventArgs e)
{
    int selectedIndex = SnapshotList.SelectedIndex;

    if (selectedIndex == -1)
    {
        MessageBox.Show(
            "Select a snapshot first.",
            "Nothing selected",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        return;
    }

    FileSnapshot selectedSnapshot = snapshots[selectedIndex];

    try
    {
        File.WriteAllBytes(
            selectedSnapshot.OriginalPath,
            selectedSnapshot.Data
        );

        MessageBox.Show(
            $"Restored:\n{selectedSnapshot.OriginalPath}",
            "Resurrected",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"Couldn't restore the file:\n{ex.Message}",
            "Restore failed",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}

    public RestoreWindow(List<FileSnapshot> snapshots)
    {
        InitializeComponent();

        this.snapshots = snapshots;

        DisplaySnapshots();
    }
}