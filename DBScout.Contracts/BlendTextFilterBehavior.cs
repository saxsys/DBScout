using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DBScout.Contracts
{
    public class BlendTextFilterBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "FilterText", typeof (string), typeof (BlendTextFilterBehavior), new PropertyMetadata(default(string)));

        public string FilterText
        {
            get { return (string) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObjectOnPreviewTextInput;
        }

        private void AssociatedObjectOnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            textCompositionEventArgs.Handled = textCompositionEventArgs.Text.Contains(FilterText);
        }
    }
}