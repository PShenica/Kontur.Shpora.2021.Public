using System.Drawing;
using System.Linq;
using System.Threading;

namespace DataParallelism
{
    public class Draw
    {
        public static void DrawBitmap()
        {
            var bitmap = new Bitmap(1024, 1024);

            var a = Enumerable.Range(0, 1024 * 1024)
                .ToArray()
                .AsParallel()
                .Select((i => Thread.CurrentThread.ManagedThreadId*32));
            var pos = 0;
            foreach (var i in a)
            {
                bitmap.SetPixel(pos % 1024, pos/1024, Color.FromArgb(i,i,i));
                pos++;
            }

            bitmap.Save("sssss.jpg");
        }
    }
}