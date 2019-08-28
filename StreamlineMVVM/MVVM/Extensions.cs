using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StreamlineMVVM
{
    // Using these requires a reference in the XAML:
    // xmlns:ext="clr-namespace:MVVMFramework;assembly=MVVMFramework"

    public class Extensions : DependencyObject
    {
        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty ComboBoxHighlightProperty = DependencyProperty.RegisterAttached(
            "ComboBoxHighlight",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetComboBoxHighlight(UIElement element, SolidColorBrush value) { element.SetValue(ComboBoxHighlightProperty, value); }
        public static SolidColorBrush GetComboBoxHighlight(UIElement element) { return (SolidColorBrush)element.GetValue(ComboBoxHighlightProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCornerRadius(UIElement element, double value) { element.SetValue(CornerRadiusProperty, value); }
        public static double GetCornerRadius(UIElement element) { return (double)element.GetValue(CornerRadiusProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty FocusThicknessProperty = DependencyProperty.RegisterAttached(
            "FocusThickness",
            typeof(double),
            typeof(Extensions),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetFocusThickness(UIElement element, double value) { element.SetValue(FocusThicknessProperty, value); }
        public static double GetFocusThickness(UIElement element) { return (double)element.GetValue(FocusThicknessProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseOverBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseOverBackground(UIElement element, SolidColorBrush value) { element.SetValue(MouseOverBackgroundProperty, value); }
        public static SolidColorBrush GetMouseOverBackground(UIElement element) { return (SolidColorBrush)element.GetValue(MouseOverBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseOverBorderProperty = DependencyProperty.RegisterAttached(
            "MouseOverBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseOverBorder(UIElement element, SolidColorBrush value) { element.SetValue(MouseOverBorderProperty, value); }
        public static SolidColorBrush GetMouseOverBorder(UIElement element) { return (SolidColorBrush)element.GetValue(MouseOverBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached(
            "MouseDownBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseDownBackground(UIElement element, SolidColorBrush value) { element.SetValue(MouseDownBackgroundProperty, value); }
        public static SolidColorBrush GetMouseDownBackground(UIElement element) { return (SolidColorBrush)element.GetValue(MouseDownBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty MouseDownBorderProperty = DependencyProperty.RegisterAttached(
            "MouseDownBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseDownBorder(UIElement element, SolidColorBrush value) { element.SetValue(MouseDownBorderProperty, value); }
        public static SolidColorBrush GetMouseDownBorder(UIElement element) { return (SolidColorBrush)element.GetValue(MouseDownBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionActiveBackgroundProperty = DependencyProperty.RegisterAttached(
            "SelectionActiveBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionActiveBackground(UIElement element, SolidColorBrush value) { element.SetValue(SelectionActiveBackgroundProperty, value); }
        public static SolidColorBrush GetSelectionActiveBackground(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionActiveBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionActiveBorderProperty = DependencyProperty.RegisterAttached(
            "SelectionActiveBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionActiveBorder(UIElement element, SolidColorBrush value) { element.SetValue(SelectionActiveBorderProperty, value); }
        public static SolidColorBrush GetSelectionActiveBorder(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionActiveBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionInactiveBackgroundProperty = DependencyProperty.RegisterAttached(
            "SelectionInactiveBackground",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionInactiveBackground(UIElement element, SolidColorBrush value) { element.SetValue(SelectionInactiveBackgroundProperty, value); }
        public static SolidColorBrush GetSelectionInactiveBackground(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionInactiveBackgroundProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty SelectionInactiveBorderProperty = DependencyProperty.RegisterAttached(
            "SelectionInactiveBorder",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));
        [AttachedPropertyBrowsableForChildren]
        public static void SetSelectionInactiveBorder(UIElement element, SolidColorBrush value) { element.SetValue(SelectionInactiveBorderProperty, value); }
        public static SolidColorBrush GetSelectionInactiveBorder(UIElement element) { return (SolidColorBrush)element.GetValue(SelectionInactiveBorderProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
            "CommandParameter",
            typeof(object),
            typeof(Extensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCommandParameter(DependencyObject depObject, object value) { depObject.SetValue(CommandProperty, value); }
        public static object GetCommandParameter(DependencyObject depObject) { return depObject.GetValue(CommandProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(Extensions),
            new FrameworkPropertyMetadata(defaultICommand, FrameworkPropertyMetadataOptions.None));
        [AttachedPropertyBrowsableForChildren]
        public static void SetCommand(DependencyObject depObject, ICommand value) { depObject.SetValue(CommandProperty, value); }
        public static ICommand GetCommand(DependencyObject depObject) { return (ICommand)depObject.GetValue(CommandProperty); }
        private static ICommand defaultICommand { get; set; } = new RelayCommand(defaultICommandCommand);
        private static void defaultICommandCommand(object parameter)
        {
            // Does nothing
        }
    }
}
