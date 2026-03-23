using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial5_question4_client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- RPC CLIENT ---");

            // 1. Chuẩn bị yêu cầu gọi hàm
            var request = new
            {
                MethodName = "MoneyExchange",
                Parameters = new object[] { "USD", 100.0 }
            };

            // 2. Serialization thành JSON
            string jsonRequest = JsonSerializer.Serialize(request);

            using var client = new TcpClient("127.0.0.1", 6000);
            using var stream = client.GetStream();

            // 3. Gửi yêu cầu
            byte[] buffer = Encoding.UTF8.GetBytes(jsonRequest);
            stream.Write(buffer, 0, buffer.Length);

            // 4. Nhận kết quả và hiển thị
            byte[] receiveBuffer = new byte[2048];
            int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            string jsonResponse = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

            Console.WriteLine($"[Client] Kết quả nhận được từ Server: {jsonResponse} VND");
            Console.ReadLine();
        }
    }
    public class RpcRequest
    {
        public string MethodName { get; set; }
        public object[] Parameters { get; set; }
    }

    public class RpcResponse
    {
        public object Result { get; set; }
        public string Error { get; set; }
    }
}
