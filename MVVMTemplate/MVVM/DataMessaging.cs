using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    public class TextAggregator
    {
        public void Transmit(string data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<string> OnDataTransmittedEvent;
    }

    public class IntAggregator
    {
        public void Transmit(int data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<int> OnDataTransmittedEvent;
    }

    public class BoolAggregator
    {
        public void Transmit(bool data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<bool> OnDataTransmittedEvent;
    }

    public class IInputElementAggregator
    {
        public void Transmit(IInputElement data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<IInputElement> OnDataTransmittedEvent;
    }

    public class ObjectAggregator
    {
        public void Transmit(object data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<object> OnDataTransmittedEvent;
    }

    public class DataMessaging
    {
        public void Transmit(DialogData data)
        {
            if (OnDataTransmittedEvent != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmittedEvent(data);
                });
            }
        }

        public Action<DialogData> OnDataTransmittedEvent;
    }
}
