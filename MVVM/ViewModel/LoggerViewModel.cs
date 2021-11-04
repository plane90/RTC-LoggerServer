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

        public LoggerViewModel()
        {
            _logs = new ObservableCollection<Net.Server.LogData>();
            Logs.Add(new Net.Server.LogData() { Type = Net.Server.LogType.Log, Message = "a", DateTime = "100" });
            Logs.Add(new Net.Server.LogData() { Type = Net.Server.LogType.Log, Message = "a", DateTime = "100" });
            Net.Server.OnLog += LogHandler;
        }

        private void LogHandler(Net.Server.LogData logData)
        {
            Trace.WriteLine($"logdata: {logData.DateTime}, {logData.Message}");
            Logs.Add(logData);
        }
    }
}