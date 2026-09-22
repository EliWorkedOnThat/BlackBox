using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static byte[] encryptionKey;

    public static readonly List<FileSnapshot> snapshots = new();

    static void Main()
    {
        using (Aes aes = Aes.Create())
        {
            encryptionKey = aes.Key;
        }

        while (true)
        {
            int choice = Menu();

            if (choice == 1)
            {
                string path = FileOrDirectoryPath();

                Console.WriteLine($"You selected: {path}");

                int previousCount = snapshots.Count;

                ByteCode(path);

                for (int i = previousCount; i < snapshots.Count; i++)
                {
                    Encryption(snapshots[i]);
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine("Recall selected.");

                int selected = RecallMeznu();

                FileSnapshot snapshot = snapshots[selected - 1];

                Console.WriteLine($"You selected: {snapshot.OriginalPath}");

                Decryption(snapshot);
                Reconstruction(snapshot);
            }
            else if (choice == 3)
            {
                Console.WriteLine("Closing BlackBox...");
                break;
            }
        }
    }

    static string FileOrDirectoryPath()
    {
        Console.WriteLine(
            "Please enter the path of the folder or file you want to store in the BlackBox:"
        );

        string path = Console.ReadLine().Trim('"');

        return path;
    }

    public class FileSnapshot
    {
        public string OriginalPath { get; set; }
        public byte[] Data { get; set; }

        public byte[] EncryptedData { get; set; }
        public byte[] IV { get; set; }

        public byte[] DecryptedData { get; set; }
    }

    static void ByteCode(string path)
    {
        if (File.Exists(path))
        {
            Console.WriteLine("Path is a file.");

            FileSnapshot snapshot = new FileSnapshot
            {
                OriginalPath = path,
                Data = File.ReadAllBytes(path)
            };

            snapshots.Add(snapshot);

            Console.WriteLine($"Reading: {snapshot.OriginalPath}");
            Console.WriteLine(
                $"Successfully read {snapshot.Data.Length} bytes."
            );
        }
        else if (Directory.Exists(path))
        {
            Console.WriteLine(
                "Path is a directory. Starting file enumeration..."
            );

            string[] files = Directory.GetFiles(
                path,
                "*",
                SearchOption.AllDirectories
            );

            foreach (string file in files)
            {
                FileSnapshot snapshot = new FileSnapshot
                {
                    OriginalPath = file,
                    Data = File.ReadAllBytes(file)
                };

                snapshots.Add(snapshot);

                Console.WriteLine($"Reading: {snapshot.OriginalPath}");
                Console.WriteLine(
                    $"Successfully read {snapshot.Data.Length} bytes."
                );
            }
        }
        else
        {
            Console.WriteLine("[ERROR] Path does not exist.");
        }
    }

    static void Encryption(FileSnapshot snapshot)
    {
        using Aes aes = Aes.Create();

        aes.Key = encryptionKey;

        aes.GenerateIV();

        ICryptoTransform encryptor = aes.CreateEncryptor();

        byte[] encrypted = encryptor.TransformFinalBlock(
            snapshot.Data,
            0,
            snapshot.Data.Length
        );

        snapshot.EncryptedData = encrypted;
        snapshot.IV = aes.IV;

        Console.WriteLine($"Encrypted: {snapshot.OriginalPath}");
        Console.WriteLine($"Original bytes: {snapshot.Data.Length}");
        Console.WriteLine($"Encrypted bytes: {snapshot.EncryptedData.Length}");
        Console.WriteLine($"IV bytes: {snapshot.IV.Length}");
        Console.WriteLine();
    }

    static void Decryption(FileSnapshot snapshot)
    {
        using Aes aes = Aes.Create();

        aes.Key = encryptionKey;
        aes.IV = snapshot.IV;

        ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] decrypted = decryptor.TransformFinalBlock(
            snapshot.EncryptedData,
            0,
            snapshot.EncryptedData.Length
        );

        snapshot.DecryptedData = decrypted;

        Console.WriteLine($"Decrypted: {snapshot.OriginalPath}");
        Console.WriteLine($"Decrypted bytes: {snapshot.DecryptedData.Length}");
    }

    static void Reconstruction(FileSnapshot snapshot)
    {
        string recoveryDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "BlackBox_Recovered"
        );

        Directory.CreateDirectory(recoveryDirectory);

        string fileName = Path.GetFileName(snapshot.OriginalPath);

        string recoveryPath = Path.Combine(
            recoveryDirectory,
            fileName
        );

        File.WriteAllBytes(
            recoveryPath,
            snapshot.DecryptedData
        );

        Console.WriteLine($"Reconstructed: {recoveryPath}");
        Console.WriteLine(
            $"Successfully wrote {snapshot.DecryptedData.Length} bytes."
        );
    }

    static int RecallMeznu()
    {
        int count = 1;

        Console.WriteLine("==== Recall Menu ====");

        foreach (FileSnapshot snapshot in snapshots)
        {
            Console.WriteLine($"{count}. {snapshot.OriginalPath}");
            count++;
        }

        Console.Write("Choose a snapshot: ");

        return int.Parse(Console.ReadLine());
    }

    static int Menu()
    {
        Console.WriteLine();
        Console.WriteLine("=====BLACKBOX====");
        Console.WriteLine("1. Store file/directory");
        Console.WriteLine("2. Recall file/directory");
        Console.WriteLine("3. Exit");
        Console.WriteLine("Choose an option:");

        return int.Parse(Console.ReadLine());
    }
}

