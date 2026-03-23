namespace Tutorial2_question2
{
    using System;
    using System.Diagnostics;

    public struct PointStruct { public int X; public int Y; }
    public class PointClass { public int X; public int Y; }

    class Program
    {
        static void Main()
        {
            const int size = 1_000_000;

            // ĐO LƯỜNG ARRAY OF STRUCTS
            GC.Collect();
            long memBeforeStruct = GC.GetTotalMemory(true);
            Stopwatch swStruct = Stopwatch.StartNew();

            PointStruct[] structArray = new PointStruct[size];
            for (int i = 0; i < size; i++)
            {
                structArray[i].X = i; structArray[i].Y = i;
            }

            swStruct.Stop();
            long memAfterStruct = GC.GetTotalMemory(true);

            // ĐO LƯỜNG ARRAY OF CLASSES
            GC.Collect();
            long memBeforeClass = GC.GetTotalMemory(true);
            Stopwatch swClass = Stopwatch.StartNew();

            PointClass[] classArray = new PointClass[size];
            for (int i = 0; i < size; i++)
            {
                classArray[i] = new PointClass { X = i, Y = i };
            }

            swClass.Stop();
            long memAfterClass = GC.GetTotalMemory(true);

            // --- IN KẾT QUẢ ---
            Console.WriteLine($"[Struct Array] Memory: {(memAfterStruct - memBeforeStruct) / 1024} KB | Time: {swStruct.ElapsedMilliseconds} ms");
            Console.WriteLine($"[Class Array]  Memory: {(memAfterClass - memBeforeClass) / 1024} KB | Time: {swClass.ElapsedMilliseconds} ms");
        }
    }
}
