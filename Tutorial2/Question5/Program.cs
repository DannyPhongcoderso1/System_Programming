namespace Tutorial2_question5
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Runtime.InteropServices;

    class MembershipCardPrinter
    {
        [DllImport("gdi32.dll")]
        private static extern int AddFontResource(string lpFileName);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_FONTCHANGE = 0x001D;
        private const IntPtr HWND_BROADCAST = (IntPtr)0xffff;

        static void Main()
        {
            string fontName = "CustomFont.ttf"; // File font nếu muốn font theo bản thân
            string fontPath = Path.Combine(Environment.CurrentDirectory, fontName);

            // Kiểm tra và in thẻ hội viên
            InstallFont(fontPath);
            PrintCard("NGÔ CHÁNH PHONG", "ID: 123456", "LEVEL: GOLD");
        }

        // Tải font
        static void InstallFont(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("File font không tồn tại!");
                return;
            }

            string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), Path.GetFileName(sourcePath));

            if (!File.Exists(destinationPath))
            {
                Console.WriteLine("Đang cài đặt font hệ thống...");
                File.Copy(sourcePath, destinationPath, true);

                AddFontResource(destinationPath);
                SendMessage(HWND_BROADCAST, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);

                Console.WriteLine("Cài đặt font thành công.");
            }
        }

        // Vẽ thẻ
        static void PrintCard(string name, string id, string level)
        {
            using (Bitmap bitmap = new Bitmap(500, 300))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.DarkBlue);

                Font fontTitle = new Font("Arial", 20, FontStyle.Bold);
                Font fontBody = new Font("Arial", 14);

                g.DrawString("MEMBERSHIP CARD", fontTitle, Brushes.Gold, new PointF(120, 30));
                g.DrawString(name, fontBody, Brushes.White, new PointF(50, 120));
                g.DrawString(id, fontBody, Brushes.White, new PointF(50, 160));
                g.DrawString(level, fontTitle, Brushes.Orange, new PointF(50, 220));

                string savePath = "MembershipCard.png";
                bitmap.Save(savePath);
                Console.WriteLine($"Đã xuất thẻ hội viên tại: {Path.GetFullPath(savePath)}");
            }
        }
    }
}
