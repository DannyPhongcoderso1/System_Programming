using System.Diagnostics;

namespace Tutorial5_question1_processA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int processId = Process.GetCurrentProcess().Id;
            string secretData = "Data-From-A-12345";

            // Lưu ý: Cần bật "Allow unsafe code" trong Project Properties
            unsafe
            {
                fixed (char* p = secretData)
                {
                    Console.WriteLine($"[Process A] Dia chi cua chuoi trong RAM: {(long)p:X}");
                }
            }
        }
    }
}
