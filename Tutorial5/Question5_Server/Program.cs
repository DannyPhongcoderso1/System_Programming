using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial5_question5_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            TcpListener server = new TcpListener(IPAddress.Any, 6000);
            server.Start();
            Console.WriteLine("[JSON-RPC Server] Đang chạy tại cổng 6000...");

            while (true)
            {
                using var client = server.AcceptTcpClient();
                using var stream = client.GetStream();

                byte[] buffer = new byte[2048];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonRequest = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                JsonRpcResponse response = new JsonRpcResponse();
                int requestId = 0;

                try
                {
                    var request = JsonSerializer.Deserialize<JsonRpcRequest>(jsonRequest,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    requestId = request.Id;
                    response.Id = requestId;

                    // Xử lý các phương thức khác nhau
                    switch (request.Method)
                    {
                        case "MoneyExchange":
                            string curr = request.Params[0].ToString();
                            double amt = double.Parse(request.Params[1].ToString());
                            response.Result = (curr == "USD") ? amt * 25000 : amt * 27000;
                            break;

                        case "GetServerTime":
                            response.Result = DateTime.Now.ToString("F");
                            break;

                        default:
                            response.Error = new RpcError { Code = -32601, Message = "Method not found" };
                            break;
                    }
                }
                catch (Exception ex)
                {
                    response.Error = new RpcError { Code = -32700, Message = "Parse error or Invalid params" };
                    response.Id = requestId;
                }

                string jsonResponse = JsonSerializer.Serialize(response);
                stream.Write(Encoding.UTF8.GetBytes(jsonResponse));
            }
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
