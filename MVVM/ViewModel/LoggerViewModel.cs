using RTC_LoggerServer.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace RTC_LoggerServer.MVM.ViewModel
{
    class LoggerViewModel : ObservableObject
    {
        private ObservableCollection<Net.Server.LogData> _logs;
        public ObservableCollection<Net.Server.LogData> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
            }
        }

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

        public RelayCommand ClearBtnCommand { get; set; }

        public LoggerViewModel()
        {
            ClearBtnCommand = new RelayCommand(o => _logs.Clear());
            _logs = new ObservableCollection<Net.Server.LogData>();
            Net.Server.OnLog += LogHandler;
        }

        private void LogHandler(Net.Server.LogData logData)
        {
            Trace.WriteLine($"logdata: {logData.DateTime}, {logData.Message}");
            Logs.Add(logData);
        }
    }
}