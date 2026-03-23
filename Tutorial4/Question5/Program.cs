using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Tutorial4_question5
{
    class Program
    {
        // Khóa AES (32 bytes) và Vector khởi tạo (16 bytes)
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("abcdef0123456789");

        static async Task Main(string[] args)
        {
            string rawFile = "sensitive_data.txt";
            string finalFile = "encrypted_compressed.dat";

            // 1. Chuẩn bị dữ liệu (Read text data from a file)
            string content = "Thông tin nhạy cảm";
            await File.WriteAllTextAsync(rawFile, content);
            Console.WriteLine($"[1] Đã đọc dữ liệu từ file: {rawFile}");

            // QUY TRÌNH XUÔI: Mã hóa -> Nén -> Lưu
            Console.WriteLine("[2 & 3] Đang mã hóa sau đó nén...");
            byte[] processedData = EncryptThenCompress(content);

            // 4. Save the final output to disk
            await File.WriteAllBytesAsync(finalFile, processedData);
            Console.WriteLine($"[4] Đã lưu file cuối cùng: {finalFile}");

            // 5. Reverse the process: Decompress -> Decrypt
            Console.WriteLine("[5] Đang đảo ngược quy trình: Giải nén -> Giải mã...");
            string recovered = DecompressThenDecrypt(processedData);
            Console.WriteLine($"\nKết quả khôi phục: {recovered}");
        }

        // Thực hiện đúng: Encrypt -> Compress
        static byte[] EncryptThenCompress(string text)
        {
            byte[] encryptedBytes;

            // Bước 2: Encrypt (Sử dụng AES)
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                using var encryptor = aes.CreateEncryptor();
                using var msEncrypt = new MemoryStream();
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] rawBytes = Encoding.UTF8.GetBytes(text);
                    csEncrypt.Write(rawBytes, 0, rawBytes.Length);
                }
                encryptedBytes = msEncrypt.ToArray();
            }

            // Bước 3: Compress 
            using var msCompress = new MemoryStream();
            using (var gZip = new GZipStream(msCompress, CompressionMode.Compress))
            {
                gZip.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            return msCompress.ToArray();
        }

        // Thực hiện đúng: Decompress -> Decrypt
        static string DecompressThenDecrypt(byte[] compressedData)
        {
            byte[] decompressedBytes;

            // Bước 5.1: Decompress
            using (var msInput = new MemoryStream(compressedData))
            using (var msOutput = new MemoryStream())
            {
                using (var gZip = new GZipStream(msInput, CompressionMode.Decompress))
                {
                    gZip.CopyTo(msOutput);
                }
                decompressedBytes = msOutput.ToArray();
            }

            // Bước 5.2: Decrypt
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                using var decryptor = aes.CreateDecryptor();
                using var msDecrypt = new MemoryStream(decompressedBytes);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var msResult = new MemoryStream();

                csDecrypt.CopyTo(msResult);
                return Encoding.UTF8.GetString(msResult.ToArray());
            }
        }
    }
}
