using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial5_question4_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            TcpListener server = new TcpListener(IPAddress.Any, 6000);
            server.Start();
            Console.WriteLine("[RPC Server] Đang đợi lệnh gọi hàm tại cổng 6000...");

            while (true)
            {
                using var client = server.AcceptTcpClient();
                using var stream = client.GetStream();

                // 1. Nhận và Deserialization JSON
                byte[] buffer = new byte[2048];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonRequest = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var request = JsonSerializer.Deserialize<RpcRequest>(jsonRequest);

                RpcResponse response = new RpcResponse();

                // 2. Định tuyến hàm (Method Dispatching)
                if (request.MethodName == "MoneyExchange")
                {
                    string currency = request.Parameters[0].ToString();
                    double amount = double.Parse(request.Parameters[1].ToString());

                    // Thực thi logic hàm
                    double result = ExecuteMoneyExchange(currency, amount);
                    response.Result = result;
                }
                else { response.Error = "Hàm không tồn tại!"; }

                // 3. Gửi kết quả trả về
                string jsonResponse = JsonSerializer.Serialize(response);
                byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
        }

        static double ExecuteMoneyExchange(string currency, double amount)
        {
            Console.WriteLine($"[RPC] Đang xử lý đổi tiền: {amount} {currency}...");
            if (currency == "USD") return amount * 25000; // Giả sử tỷ giá
            if (currency == "EUR") return amount * 27000;
            return amount;
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
