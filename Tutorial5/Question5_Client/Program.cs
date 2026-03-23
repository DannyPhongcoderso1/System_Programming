using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial5_question5_client
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            // Thử gọi hàm hợp lệ
            CallRemoteMethod("MoneyExchange", new object[] { "USD", 50.0 }, 1);

            // Thử gọi hàm khác
            CallRemoteMethod("GetServerTime", null, 2);

            // Thử gọi hàm SAI để test lỗi
            CallRemoteMethod("InvalidFunc", null, 3);

            Console.ReadLine();
        }

        static void CallRemoteMethod(string method, object[] parameters, int id)
        {
            try
            {
                using var client = new TcpClient("127.0.0.1", 6000);
                using var stream = client.GetStream();

                var req = new JsonRpcRequest { Method = method, Params = parameters, Id = id };
                string jsonReq = JsonSerializer.Serialize(req);
                stream.Write(Encoding.UTF8.GetBytes(jsonReq));

                byte[] buffer = new byte[2048];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonRes = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                var res = JsonSerializer.Deserialize<JsonRpcResponse>(jsonRes,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (res.Error != null)
                    Console.WriteLine($"[ID {res.Id}] LỖI: {res.Error.Message}");
                else
                    Console.WriteLine($"[ID {res.Id}] KẾT QUẢ: {res.Result}");
            }
            catch (Exception ex) { Console.WriteLine("Lỗi kết nối: " + ex.Message); }
        }
    }
    public class JsonRpcRequest
    {
        public string Jsonrpc { get; set; } = "2.0";
        public string Method { get; set; }
        public object[] Params { get; set; }
        public int Id { get; set; } // Dùng để khớp phản hồi với yêu cầu
    }

    public class JsonRpcResponse
    {
        public string Jsonrpc { get; set; } = "2.0";
        public object Result { get; set; }
        public RpcError Error { get; set; }
        public int Id { get; set; }
    }

    public class RpcError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
