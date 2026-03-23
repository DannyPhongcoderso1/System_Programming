namespace Tutorial3_question1
{
    public class PlayerClass
    {
        public int Score; // Nằm trên Heap như một phần của đối tượng PlayerClass
    }

    // 2. Value Type: Dữ liệu sẽ nằm tại nơi nó được khai báo (thường là STACK)
    public struct PointStruct
    {
        public int X;
        public int Y;
    }

    class Program
    {
        // Biến static: Nằm trên Heap (vùng High Frequency Heap)
        // Tồn tại suốt vòng đời ứng dụng
        static PlayerClass globalPlayer = new PlayerClass();

        static void Main(string[] args)
        {
            Console.WriteLine("--- Bắt đầu phương thức Main ---");

            // Gọi phương thức để quan sát cấp phát bộ nhớ
            ProcessData();

            Console.WriteLine("--- Phương thức ProcessData đã kết thúc ---");

            // Ép buộc GC chạy để minh họa việc thu hồi bộ nhớ Heap
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.ReadLine();
        }

        static void ProcessData()
        {
            // [STACK]: Biến localInt nằm trực tiếp trên Stack
            int localInt = 100;

            // [STACK]: localPoint là struct, toàn bộ X và Y nằm trên Stack
            PointStruct localPoint = new PointStruct { X = 10, Y = 20 };

            // [STACK] & [HEAP]: 
            // - p1 (biến tham chiếu/địa chỉ) nằm trên STACK.
            // - Đối tượng thực sự được tạo bởi 'new' nằm trên HEAP.
            PlayerClass p1 = new PlayerClass { Score = 50 };

            Console.WriteLine($"Score: {p1.Score}, Point: ({localPoint.X},{localPoint.Y})");

        }
    }
}
