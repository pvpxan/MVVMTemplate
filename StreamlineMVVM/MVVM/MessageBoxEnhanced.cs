using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StreamlineMVVM
{
    public static class MessageBoxEnhanced
    {
        // Sets Window Title and Message Content.
        public static WindowMessageResult Show(string windowTitle, string contentHeader)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader));
        }

        // Sets Window Title and Message Content on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader));
        }
        // ------------------------------------------

        // Sets Window Title, Message Content, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader, "", "", "", windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Content, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader, "", "", "", windowMessageButtons, windowMessageIcon));
        }
        // ------------------------------------------

        // Sets Window Title, Message Header, Message Body, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader, contentBody, "", "", windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader, string contentBody, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader, contentBody, "", "", windowMessageButtons, windowMessageIcon));
        }
        // ------------------------------------------

        // Sets Window Title, Message Header, Message Body, Message HyperLink, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLink, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader, contentBody, "", hyperLink, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader, string contentBody, string hyperLink, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader, contentBody, "", hyperLink, windowMessageButtons, windowMessageIcon));
        }
        // ------------------------------------------

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, and Message Icon on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, Message Icon, and Color Set.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon, WindowMessageColorSet windowMessageColorSet)
        {
            return displayDialog(dialogDataBuilder(null, windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, Message Icon, and Color Set on target window.
        public static WindowMessageResult Show(Window parentWindow, string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon, WindowMessageColorSet windowMessageColorSet)
        {
            return displayDialog(dialogDataBuilder(parentWindow, windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon));
        }
        // ------------------------------------------

        private static DialogData dialogDataBuilder(
            Window parentWindow = null,
            string windowTitle = "",
            string contentHeader = "",
            string contentBody = "",
            string hyperLinkText = "",
            string hyperLinkUri = "",
            WindowMessageButtons windowMessageButtons = WindowMessageButtons.Ok,
            WindowMessageIcon windowMessageIcon = WindowMessageIcon.Information,
            WindowMessageColorSet windowMessageColorSet = null)
        {
            DialogData data = new DialogData()
            {
                ParentWindow = parentWindow,
                WindowTitle = windowTitle,
                ContentBody = contentBody,
                ContentHeader = contentHeader,
                HyperLinkText = hyperLinkText,
                HyperLinkUri = hyperLinkUri,
                MessageButtons = windowMessageButtons,
                MessageIcon = windowMessageIcon,
            };

            if (windowMessageColorSet == null)
            {
                data.Background = Brushes.White;
                data.ContentHeaderColor = Brushes.DarkBlue;
                data.ContentBodyColor = Brushes.Black;
                data.HyperLinkColor = Brushes.Blue;
                data.HyperLinkMouseOverColor = Brushes.Red;
                data.HyperLinkMouseDisabledColor = Brushes.Gray;
            }
            else
            {
                if (windowMessageColorSet.Background == null)
                {
                    data.Background = Brushes.White;
                }

                if (windowMessageColorSet.ContentHeaderColor == null)
                {
                    data.ContentHeaderColor = Brushes.DarkBlue;
                }

                if (windowMessageColorSet.ContentBodyColor == null)
                {
                    data.ContentBodyColor = Brushes.Black;
                }

                if (windowMessageColorSet.HyperLinkColor == null)
                {
                    data.HyperLinkColor = Brushes.Blue;
                }

                if (windowMessageColorSet.HyperLinkMouseOverColor == null)
                {
                    data.HyperLinkMouseOverColor = Brushes.Red;
                }

                if (windowMessageColorSet.HyperLinkMouseDisabledColor == null)
                {
                    data.HyperLinkMouseDisabledColor = Brushes.Gray;
                }
            }

            return data;
        }

        private static WindowMessageResult displayDialog(DialogData data)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);

            Window window = data.ParentWindow;

            if (window == null)
            {
                try
                {
                    window = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                }
                catch
                {
                    if (Application.Current.Windows.Count > 0)
                    {
                        window = Application.Current.Windows[0];
                    }
                }
            }

            return DialogService.OpenDialog(viewmodel, window, ShutdownMode.OnLastWindowClose);
        }
    }
}
