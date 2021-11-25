using RTC_LoggerServer.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class FrameViewModel : ObservableObject
    {
        private int frameCnt = 0;
        private bool _isFrameVMViewVisible;
        public bool IsFrameVMViewVisible
        {
            get => _isFrameVMViewVisible;
            set { _isFrameVMViewVisible = value; OnPropertyChanged(); }
        }

        private BitmapImage _frame;
        public BitmapImage Frame
        {
            get => _frame;
            set { _frame = value; OnPropertyChanged(); }
        }

        public FrameViewModel()
        {
            Net.Server.OnFrame += FrameHandler;
            byte[] byteArrayIn = new byte[] { 255, 128, 0, 200 };
            BitmapSource bitmapSource = BitmapSource.Create(2, 2, 300, 300, PixelFormats.Indexed8, BitmapPalettes.Gray256, byteArrayIn, 2);

            Trace.WriteLine($"thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //Frame = new BitmapImage(new Uri("D:\\_Dev_WebRTC\\제목 없음.jpg"));
        }

        private void FrameHandler(byte[] encodedFrame)
        {
            try
            {
                Frame = LoadImage(encodedFrame);
                //Save(Frame, $"D:\\_Dev_WebRTC\\incoming\\frames_{frameCnt}.jpg");
                frameCnt++;
                //Trace.WriteLine($"thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            }
            catch (System.Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
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
        public void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);

            }
        }
    }
}