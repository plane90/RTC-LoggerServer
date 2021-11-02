using RTC_LoggerServer.Core;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        public RelayCommand LoggerViewCommand { get; set; }
        public RelayCommand FrameViewCommand { get; set; }
        public FrameViewModel FrameVM { get; set; }
        public LoggerViewModel LoggerVM { get; set; }

        public MainWindowViewModel()
        {
            LoggerVM = new LoggerViewModel();
            FrameVM = new FrameViewModel();
            LoggerViewCommand = new RelayCommand(o => UpdateVisibility(LoggerVM));
            FrameViewCommand = new RelayCommand(o => UpdateVisibility(FrameVM));
            LoggerVM.IsLoggerVMVisible = true;
        }

        private void UpdateVisibility(object selected)
        {
            LoggerVM.IsLoggerVMVisible = LoggerVM.Equals(selected);
            FrameVM.IsFrameVMViewVisible = FrameVM.Equals(selected);
        }
    }
}
