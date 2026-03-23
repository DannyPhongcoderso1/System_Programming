using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tutorial5_question3_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- TCP SOCKET SERVER ---");

            // Lắng nghe tại địa chỉ IP cục bộ, cổng 5000
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            server.Start();

            Console.WriteLine("[Server] Đang lắng nghe tại cổng 5000...");

            try
            {
                // Chấp nhận kết nối từ Client
                using (TcpClient client = server.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("[Server] Một Client đã kết nối!");

                    // 1. Đọc yêu cầu từ Client
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[Server] Yêu cầu nhận được: {request}");

                    // 2. Xử lý và gửi phản hồi
                    string response = "TCP Server xác nhận: " + request.ToUpper();
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                    Console.WriteLine("[Server] Đã gửi phản hồi.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server] Lỗi: {ex.Message}");
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\n[Server] Nhấn phím bất kỳ để thoát.");
            Console.ReadLine();
        }
    }
}
