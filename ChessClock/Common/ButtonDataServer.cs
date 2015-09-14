using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClock.Common
{
    class ButtonDataServer : INotifyPropertyChanged
    {
        private string _timeValue;
        private string _moveValue;

        public string TimeValue {
            get
            {
                return _timeValue;
            }
            set
            {
                _timeValue = value;
                OnPropertyChanged("TimeValue");
            }
        }
        public string MoveValue {
            get
            {
                return _moveValue;
            }
            set
            {
                _moveValue = value;
                OnPropertyChanged("MoveValue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
