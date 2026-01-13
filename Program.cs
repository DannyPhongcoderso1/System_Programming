namespace Tutorial_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("THÔNG TIN MÔI TRƯỜNG HỆ THỐNG");

            string osVersion = Environment.OSVersion.ToString();

            string currentDir = Environment.CurrentDirectory;

            DateTime currentTime = DateTime.Now;

            Console.WriteLine($"1. Phiên bản HĐH: {osVersion}");
            Console.WriteLine($"2. Thư mục hiện tại: {currentDir}");
            Console.WriteLine($"3. Thời gian hệ thống: {currentTime:dd/MM/yyyy HH:mm:ss}");
            Console.ReadLine();
        }
    }
}
