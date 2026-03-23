using System.IO.Pipes;
using System.Text;

namespace Tutorial5_question2_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- NAMED PIPE SERVER ---");

            // 1. Khởi tạo Pipe Server với tên "MySystemPipe"
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("MySystemPipe", PipeDirection.InOut))
            {
                Console.WriteLine("[Server] Đang đợi Client kết nối...");
                pipeServer.WaitForConnection(); // Chặn luồng cho đến khi có Client

                Console.WriteLine("[Server] Client đã kết nối!");

                try
                {
                    // 2. Đọc tin nhắn từ Client
                    using (StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8, leaveOpen: true))
                    using (StreamWriter writer = new StreamWriter(pipeServer, Encoding.UTF8, leaveOpen: true))
                    {
                        string clientMsg = reader.ReadLine();
                        Console.WriteLine($"[Server] Nhận được: {clientMsg}");

                        // 3. Gửi phản hồi
                        string response = "Chào Client! Tôi đã nhận được dữ liệu của bạn.";
                        writer.WriteLine(response);
                        writer.Flush();
                        Console.WriteLine("[Server] Đã gửi phản hồi.");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"[Server] Lỗi: {ex.Message}");
                }
            } // Tự động đóng pipe và dọn dẹp tài nguyên nhờ using

            Console.WriteLine("\n[Server] Đã đóng kết nối. Nhấn phím bất kỳ để thoát.");
            Console.ReadLine();
        }
    }
}
