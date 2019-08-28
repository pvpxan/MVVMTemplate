using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StreamlineMVVM
{
    public class DataMessaging
    {
        public void Transmit(DialogData data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<DialogData> OnDataTransmittedEvent { get; set; }
    }

    public class TextAggregator
    {
        public void Transmit(string data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<string> OnDataTransmittedEvent { get; set; }
    }

    public class IntAggregator
    {
        public void Transmit(int data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<int> OnDataTransmittedEvent { get; set; }
    }

    public class BoolAggregator
    {
        public void Transmit(bool data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<bool> OnDataTransmittedEvent { get; set; }
    }

    public class IInputElementAggregator
    {
        public void Transmit(IInputElement data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<IInputElement> OnDataTransmittedEvent { get; set; }
    }

    public class ObjectAggregator
    {
        public void Transmit(object data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }

        public Action<object> OnDataTransmittedEvent { get; set; }
    }
}
