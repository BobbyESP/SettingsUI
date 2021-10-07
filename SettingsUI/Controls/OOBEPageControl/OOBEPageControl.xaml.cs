using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SettingsUI.Controls
{
    public sealed partial class OOBEPageControl : UserControl
    {
        public OOBEPageControl()
        {
            this.InitializeComponent();
            if (string.IsNullOrEmpty(ModuleImageSource))
            {
                ModuleImageSource = "ms-appx:///nothing.png";
            }
        }

        public string ModuleTitle
        {
            get { return (string)GetValue(ModuleTitleProperty); }
            set { SetValue(ModuleTitleProperty, value); }
        }

        public string ModuleDescription
        {
            get => (string)GetValue(ModuleDescriptionProperty);
            set => SetValue(ModuleDescriptionProperty, value);
        }

        public string ModuleImageSource
        {
            get => (string)GetValue(ModuleImageSourceProperty);
            set => SetValue(ModuleImageSourceProperty, value);
        }

        public object ModuleContent
        {
            get { return (object)GetValue(ModuleContentProperty); }
            set { SetValue(ModuleContentProperty, value); }
        }

        public static readonly DependencyProperty ModuleTitleProperty = DependencyProperty.Register("ModuleTitle", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ModuleDescriptionProperty = DependencyProperty.Register("ModuleDescription", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ModuleImageSourceProperty = DependencyProperty.Register("ModuleImageSource", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ModuleContentProperty = DependencyProperty.Register("ModuleContent", typeof(object), typeof(SettingsPageControl), new PropertyMetadata(new Grid()));
    }
}
