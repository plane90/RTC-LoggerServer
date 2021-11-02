using System;
using System.Collections.Generic;
using System.Text;

namespace RTC_LoggerServer.MVVM.Model
{
    class LoggerData
    {
        [Flags]
        enum SelectedFilter
        {
            All = 0,
            Debug = 1,
            Error = 2,
            Warning =4,
        }

        SelectedFilter selectedFilter = SelectedFilter.All;
        public List<string> _logs = new List<string>();
        public List<string> Logs { get => _logs; }
    }
}
