using RTC_LoggerServer.Core;
using System;
using System.Collections.Generic;
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
        private int prevSec = 0;
        public int FPS
        {
            get => frameCnt;
            set { frameCnt = value; OnPropertyChanged(); }
        }
        private bool _isFrameVMViewVisible;
        public bool IsFrameVMViewVisible
        {
            get => _isFrameVMViewVisible;
            set { _isFrameVMViewVisible = value; OnPropertyChanged(); }
        }

        private Util.BufferScheduler bs;

        private BitmapImage _frame;
        public BitmapImage Frame
        {
            get => _frame;
            set { _frame = value; OnPropertyChanged(); }
        }

        public FrameViewModel()
        {
            Net.Server.OnFrame += OnFrameArrived;
            //byte[] byteArrayIn = new byte[] { 255, 128, 0, 200 };
            //BitmapSource bitmapSource = BitmapSource.Create(2, 2, 300, 300, PixelFormats.Indexed8, BitmapPalettes.Gray256, byteArrayIn, 2);
            bs = new Util.BufferScheduler(OnBitmapReady);
            Trace.WriteLine($"thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //Frame = new BitmapImage(new Uri("D:\\_Dev_WebRTC\\제목 없음.jpg"));
        }

        private void OnFrameArrived(byte[] encodedFrame)
        {
            try
            {
                bs?.AddIntoBuffer(encodedFrame);
                CalcFPS();
                //Frame = ConvertToBitmapImage(encodedFrame);
                //Save(Frame, $"D:\\_Dev_WebRTC\\incoming\\frames_{frameCnt}.jpg");
                //Trace.WriteLine($"thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private void CalcFPS()
        {
            if (prevSec == DateTime.Now.Second)
            {
                frameCnt++;
            }
            else
            {
                Trace.WriteLine($"Frame Per Second: {frameCnt}");
                prevSec = DateTime.Now.Second;
                FPS = frameCnt;
                frameCnt = 0;
            }
        }

        private void OnBitmapReady(BitmapImage source)
        {
            Frame = source;
        }

        private void Save(BitmapImage image, string filePath)
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