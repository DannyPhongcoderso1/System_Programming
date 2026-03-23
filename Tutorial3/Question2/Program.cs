namespace Tutorial3_question2
{
    class LargeObject
    {
        // Mỗi đối tượng chiếm khoảng 100KB để nhanh chóng tạo áp lực bộ nhớ
        private byte[] data = new byte[1024 * 100];
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Demo Garbage Collection & Memory Pressure ---");

            // 1. Lấy lượng bộ nhớ ban đầu
            long initialMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Bo nho ban dau: {initialMemory / 1024} KB");

            // 2. Cấp phát một lượng lớn đối tượng trong vòng lặp
            Console.WriteLine("Dang cap phat 5000 doi tuong...");
            for (int i = 0; i < 5000; i++)
            {
                LargeObject obj = new LargeObject();
                // obj se bi mat tham chieu ngay sau moi vong lap
            }

            long memoryAfterAllocation = GC.GetTotalMemory(false);
            Console.WriteLine($"Bo nho sau khi cap phat: {memoryAfterAllocation / 1024} KB");
            Console.WriteLine($"Chenh lech: {(memoryAfterAllocation - initialMemory) / 1024} KB");

            // 3. Cuong che thu gom rac (Force GC)
            Console.WriteLine("\nDang goi GC.Collect()...");
            GC.Collect();
            GC.WaitForPendingFinalizers(); // Doi cac ham huy chay xong
            GC.Collect(); // Goi lan 2 de dam bao don dep ca cac doi tuong o Gen cao hon

            long memoryAfterGC = GC.GetTotalMemory(true);
            Console.WriteLine($"Bo nho sau khi GC: {memoryAfterGC / 1024} KB");
            Console.WriteLine($"So luong bo nho duoc giai phong: {(memoryAfterAllocation - memoryAfterGC) / 1024} KB");

            Console.ReadLine();
        }
    }
}
