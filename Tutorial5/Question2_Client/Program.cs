using System.IO.Pipes;
using System.Text;

namespace Tutorial5_question2_client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- NAMED PIPE CLIENT ---");

            // 1. Kết nối tới Pipe Server có tên "MySystemPipe"
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "MySystemPipe", PipeDirection.InOut))
            {
                Console.WriteLine("[Client] Đang kết nối tới Server...");
                pipeClient.Connect(5000);

                Console.WriteLine("[Client] Đã kết nối thành công!");

                // 2. Gửi tin nhắn
                using (StreamWriter writer = new StreamWriter(pipeClient, Encoding.UTF8, leaveOpen: true))
                using (StreamReader reader = new StreamReader(pipeClient, Encoding.UTF8, leaveOpen: true))
                {
                    string msg = "Hello Server! Đây là dữ liệu từ bộ nhớ của Client.";
                    writer.WriteLine(msg);
                    writer.Flush();
                    Console.WriteLine("[Client] Đã gửi tin nhắn.");

                    // 3. Đọc phản hồi từ Server
                    string response = reader.ReadLine();
                    Console.WriteLine($"[Client] Server phản hồi: {response}");
                }
            } // Tự động dọn dẹp tài nguyên

            Console.WriteLine("\n[Client] Nhấn phím bất kỳ để thoát.");
            Console.ReadLine();
        }
    }
}
