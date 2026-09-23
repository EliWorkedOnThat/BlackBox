namespace BlackBox;

public class FileSnapshot
{
    public string OriginalPath { get; set; }
    public byte[] Data { get; set; }

    public byte[] EncryptedData { get; set; }
    public byte[] IV { get; set; }

    public byte[] DecryptedData { get; set; }
}