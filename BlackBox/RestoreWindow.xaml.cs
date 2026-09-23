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
            return;
        }

        FileSnapshot selectedSnapshot = snapshots[selectedIndex];

        File.WriteAllBytes(
            selectedSnapshot.OriginalPath,
            selectedSnapshot.Data
        );
    }

    public RestoreWindow(List<FileSnapshot> snapshots)
    {
        InitializeComponent();

        this.snapshots = snapshots;

        DisplaySnapshots();
    }
}