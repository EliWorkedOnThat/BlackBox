// Modules
using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

class Program
{
    public static readonly List<FileSnapshot> snapshots = new();

    static void Main()
    {
        string path = FileOrDirectoryPath();

        Console.WriteLine($"You selected: {path}");

        ByteCode(path);

        Console.WriteLine(
            $"BlackBox currently contains {snapshots.Count} file snapshot(s)."
        );
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

        public byte[] EncryptedData {get; set;}
        public byte[] IV {get; set;}
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

        static void Encryption()
    {
        foreach (FileSnapshot snapshot in snapshots)
        {
            
        }
    }

    static void Decryption()
        {
            
        }

    }
}