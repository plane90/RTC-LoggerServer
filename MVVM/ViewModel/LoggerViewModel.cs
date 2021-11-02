using RTC_LoggerServer.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class LoggerViewModel : ObservableObject
    {
        private List<string> _logs = new List<string>() { "1", "2" };
        public List<string> Logs { get => _logs; }
        private bool _isLoggerVMVisible;
        public bool IsLoggerVMVisible
        {
            get => _isLoggerVMVisible;
            set
            {
                _isLoggerVMVisible = value;
                OnPropertyChanged();
            }
        }

        public LoggerViewModel()
        {
            Trace.WriteLine($"LoggerViewModel id:{this.GetHashCode()}");
        }
    }
}