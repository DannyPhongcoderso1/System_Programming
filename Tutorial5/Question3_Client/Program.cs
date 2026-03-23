using System.Net.Sockets;
using System.Text;

namespace Tutorial5_question3_client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- TCP SOCKET CLIENT ---");

            try
            {
                // Kết nối tới Server tại 127.0.0.1 cổng 5000
                using (TcpClient client = new TcpClient("127.0.0.1", 5000))
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("[Client] Đã kết nối tới Server!");

                    // 1. Gửi yêu cầu
                    string message = "xin chào từ socket client";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("[Client] Đã gửi: " + message);

                    // 2. Nhận phản hồi
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[Client] Phản hồi từ Server: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client] Lỗi: {ex.Message}");
            }

            Console.WriteLine("\n[Client] Nhấn phím bất kỳ để thoát.");
            Console.ReadLine();
        }
    }
}
