using RTC_LoggerServer.Core;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class FrameViewModel : ObservableObject
    {
        private bool _isFrameVMViewVisible;
        public bool IsFrameVMViewVisible
        {
            get => _isFrameVMViewVisible;
            set
            {
                _isFrameVMViewVisible = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _frame;
        public BitmapImage Frame { get => _frame; }

        public FrameViewModel()
        {
            Net.Server.OnFrame += FrameHandler;
        }

        private void FrameHandler(byte[] encodedFrame)
        {
            try
            {
                _frame = LoadImage(encodedFrame);
            }
            catch (System.Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
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