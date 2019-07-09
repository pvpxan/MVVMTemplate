using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MVVMTemplate
{
    // Using these requires a reference in the XAML:
    // xmlns:ext="clr-namespace:MVVM;assembly=MVVM"

    public class Extensions
    {
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
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverBrush",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));

        [AttachedPropertyBrowsableForChildren]
        public static void SetMouseOverBrush(UIElement element, SolidColorBrush value) { element.SetValue(MouseOverBrushProperty, value); }
        public static SolidColorBrush GetMouseOverBrush(UIElement element) { return (SolidColorBrush)element.GetValue(MouseOverBrushProperty); }

        // ----------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.RegisterAttached(
            "PressedBrush",
            typeof(SolidColorBrush),
            typeof(Extensions),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.AffectsRender));

        [AttachedPropertyBrowsableForChildren]
        public static void SetPressedBrush(UIElement element, SolidColorBrush value) { element.SetValue(PressedBrushProperty, value); }
        public static SolidColorBrush GetPressedBrush(UIElement element) { return (SolidColorBrush)element.GetValue(PressedBrushProperty); }
    }
}
