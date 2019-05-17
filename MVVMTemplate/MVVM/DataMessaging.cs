using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    public static class DataMessaging
    {
        public static void Transmit(MessageData data)
        {
            if (OnDataTransmitted != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnDataTransmitted(data);
                });
            }
        }

        public static Action<MessageData> OnDataTransmitted;
    }

    public class MessageData
    {
        public string message = "";
        public string title = "";
        public string hyperlink = "";
        public MessageBoxImage type { get; set; }
    }
}
