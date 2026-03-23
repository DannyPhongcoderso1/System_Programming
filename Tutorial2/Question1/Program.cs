namespace Tutorial2_question1
{
    using System;

    public struct PointStruct { public int X; public int Y; }
    public class PointClass { public int X; public int Y; }

    class Program
    {
        static void Main()
        {
            // Thử nghiệm với Struct (Value Type)
            PointStruct s1 = new PointStruct { X = 10, Y = 10 };
            PointStruct s2 = s1;
            s2.X = 99;
            Console.WriteLine($"Struct: s1.X = {s1.X}, s2.X = {s2.X}");

            // Thử nghiệm với Class (Reference Type)
            PointClass c1 = new PointClass { X = 10, Y = 10 };
            PointClass c2 = c1;
            c2.X = 99;
            Console.WriteLine($"Class:  c1.X = {c1.X}, c2.X = {c2.X}");
        }
    }
}
