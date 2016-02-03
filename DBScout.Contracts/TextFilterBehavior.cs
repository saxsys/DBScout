using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBScout.Contracts
{
    public static class TextFilterBehavior
    {
        public static void SetFilterText(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(FilterTextProperty,value);
        }

        public static string GetFilterText(DependencyObject dependencyObject)
        {
           return (string)dependencyObject.GetValue(FilterTextProperty);
        }

        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.RegisterAttached(
            "FilterText", typeof (string), typeof (TextFilterBehavior), new UIPropertyMetadata(default(string), OnAttached));

        private static void OnAttached(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var textBox = (TextBox) dependencyObject;
            textBox.PreviewTextInput += TextBoxOnPreviewTextInput;
            textBox.Unloaded += OnUnload;
        }

        private static void OnUnload(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.PreviewTextInput -= TextBoxOnPreviewTextInput;
                textBox.Unloaded -= OnUnload;
            }
        }

        private static void TextBoxOnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            var dependencyObject=(DependencyObject) sender;
            var filtertext = GetFilterText(dependencyObject);
            textCompositionEventArgs.Handled = textCompositionEventArgs.Text.Contains(filtertext);
        }
    }
}
