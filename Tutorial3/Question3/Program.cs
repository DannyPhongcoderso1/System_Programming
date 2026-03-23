namespace Tutorial3_question3
{
    class Question3
    {
        static void Main(string[] args)
        {
            int[] data = Enumerable.Range(1, 10000).ToArray();

            Console.WriteLine("--- So sánh hiệu năng và bộ nhớ ---");

            // 1. Chạy bản chưa tối ưu
            long memBefore1 = GC.GetTotalMemory(true);
            int result1 = ProcessDataOriginal(data);
            long memAfter1 = GC.GetTotalMemory(false);
            Console.WriteLine($"[Original] Kết quả: {result1} | Bộ nhớ cấp phát: {memAfter1 - memBefore1} bytes");

            // 2. Chạy bản đã tối ưu
            long memBefore2 = GC.GetTotalMemory(true);
            int result2 = ProcessDataOptimized(data);
            long memAfter2 = GC.GetTotalMemory(false);
            Console.WriteLine($"[Optimized] Kết quả: {result2} | Bộ nhớ cấp phát: {memAfter2 - memBefore2} bytes");
        }

        #region ORIGINAL CODE (Lãng phí bộ nhớ)
        // Kỹ thuật chưa tốt: Dùng LINQ và tạo List mới liên tục trên Heap
        static int ProcessDataOriginal(int[] data)
        {
            // LINQ .Where tạo ra một Enumerator, .ToList tạo ra một vùng nhớ Heap mới
            var evens = data.Where(x => x % 2 == 0).ToList();
            return evens.Sum();
        }
        #endregion

        #region REFACTORED CODE (Tối ưu bộ nhớ)
        // Kỹ thuật tối ưu: Dùng Span<T> để thao tác trực tiếp trên vùng nhớ có sẵn
        static int ProcessDataOptimized(ReadOnlySpan<int> dataSpan)
        {
            // Zero-allocation: Không tạo mảng mới, không tạo List
            int sum = 0;
            for (int i = 0; i < dataSpan.Length; i++)
            {
                if (dataSpan[i] % 2 == 0)
                {
                    sum += dataSpan[i];
                }
            }
            return sum;
        }
        #endregion
    }
}

