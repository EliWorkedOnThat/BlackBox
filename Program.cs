// Modules
using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        string path = FileOrDirectoryPath();

        Console.WriteLine($"You selected: {path}");

        ByteCode(path);
    }

    static string FileOrDirectoryPath()
    {
        Console.WriteLine(
            "Please enter the path of the folder or file you want to store in the BlackBox:"
        );

        string path = Console.ReadLine().Trim('"');

        return path;
    }

    static void ByteCode(string path)
    {
        if (File.Exists(path))
        {
            Console.WriteLine("Path is a file.");

            byte[] fileBytes = File.ReadAllBytes(path);

            Console.WriteLine(
                $"Successfully read {fileBytes.Length} bytes from the file."
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
                Console.WriteLine($"Reading: {file}");

                byte[] fileBytes = File.ReadAllBytes(file);

                Console.WriteLine(
                    $"Successfully read {fileBytes.Length} bytes."
                );
            }
        }
        else
        {
            Console.WriteLine("[ERROR] Path does not exist.");
        }
    }
}