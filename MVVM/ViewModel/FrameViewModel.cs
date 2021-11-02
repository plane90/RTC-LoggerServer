using RTC_LoggerServer.Core;

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
    }
}