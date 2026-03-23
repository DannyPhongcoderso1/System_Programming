using System.Diagnostics;

namespace Tutorial5_question1_processB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int processId = Process.GetCurrentProcess().Id;

            long addressFromA = 0x12345678; // Nhập số từ màn hình A
            unsafe
            {
                char* p = (char*)addressFromA;
                Console.WriteLine(*p);
            }
        }
    }
}
