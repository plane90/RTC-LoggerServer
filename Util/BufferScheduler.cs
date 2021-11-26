using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RTC_LoggerServer.Util
{
    class BufferScheduler
    {
        class FrameBuffer
        {
            public bool isReady = false;
            public bool isFail = false;
            public BitmapImage bmp = null;
        }
        private Queue<FrameBuffer> fq = new Queue<FrameBuffer>();
        private Action<BitmapImage> OnBitmapReady;

        public BufferScheduler(Action<BitmapImage> onBitmapReady)
        {
            this.OnBitmapReady = onBitmapReady;
            System.Threading.Tasks.Task.Run(() => Buffering());
        }

        public void AddIntoBuffer(byte[] imageData)
        {
            Trace.WriteLine($"Added image data");
            var fb = new FrameBuffer();
            fq.Enqueue(fb);
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    fb.bmp = ConvertToBitmapImage(imageData);
                    fb.isReady = true;
                }
                catch (Exception e)
                {
                    fb.isFail = true;
                }
            });
        }

        private void Buffering()
        {
            while (true)
            {
                if (fq.Count == 0)
                {
                    continue;
                }
                var imgData = fq.Dequeue();
                while (true)
                {
                    if (imgData.isReady)
                    {
                        Application.Current.Dispatcher.Invoke(() => OnBitmapReady(imgData.bmp));
                        Trace.WriteLine($"Rendered image data");
                        break;
                    }
                    else if (imgData.isFail)
                    {
                        break;
                    }
                }
            }
        }

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;


            var image = new BitmapImage();
            using (var ms = new MemoryStream(imageData))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
