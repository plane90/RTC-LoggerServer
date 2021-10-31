using RTC_LoggerServer.Core;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        public RelayCommand LoggerViewCommand { get; set; }
        public RelayCommand FrameViewCommand { get; set; }
        public FrameViewModel FrameVM { get; set; }
        public LoggerViewModel LoggerVM { get; set; }
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private object _currentView;

        public MainWindowViewModel()
        {
            LoggerVM = new LoggerViewModel();
            FrameVM = new FrameViewModel();

            CurrentView = LoggerVM;

            LoggerViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoggerVM;
            });
            FrameViewCommand = new RelayCommand(o =>
            {
                CurrentView = FrameVM;
            });
        }
    }
}
