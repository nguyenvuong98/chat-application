using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ButtonActivity.Template
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XButtonActivity
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand),
            typeof(XButtonActivity));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
            typeof(object),
            typeof(XButtonActivity));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
            typeof(XButtonActivity), null,
            propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnTextChange((string)newVal));
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool),
            typeof(XButtonActivity), false,
            propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnIsBusy((bool)newVal));
        public static readonly BindableProperty BGColorProperty = BindableProperty.Create(nameof(BGColor), typeof(Color),
            typeof(XButtonActivity), Color.White,
            propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnBackgroundChange((Color)newVal));
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color),
           typeof(XButtonActivity), Color.Black,
           propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnBorderColorChange((Color)newVal));
        //public static readonly BindableProperty PaddingContentProperty = BindableProperty.Create(nameof(PaddingContent), typeof(int),
        //   typeof(XButtonActivity), 5,
        //   propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnPaddingContentChange((int)newVal));
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color),
          typeof(XButtonActivity), Color.Black,
          propertyChanged: (bindable, oldVal, newVal) => ((XButtonActivity)bindable).OnColorChange((Color)newVal));
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }
        public Color BGColor
        {
            get => (Color)GetValue(BGColorProperty);
            set => SetValue(BGColorProperty, value);
        }
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        //public int PaddingContent
        //{
        //    get => (int)GetValue(PaddingContentProperty);
        //    set => SetValue(PaddingContentProperty,value);
        //}
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        public XButtonActivity()
        {
            InitializeComponent();
        }
        private void OnTextChange(string value)
        {
            label.Text = value;
        }
        private void OnBackgroundChange(Color value)
        {
            frame.BackgroundColor = value;
            stack.BackgroundColor = value;
        }
        private void OnBorderColorChange(Color value)
        {
            frame.BorderColor = value;
        }
        private  void OnIsBusy(bool value)
        {
            activity.IsVisible = value;
            activity.IsRunning = value;
        }
        //private void OnPaddingContentChange(int value)
        //{
        //    frame.Padding = value;
        //}
        private void OnColorChange(Color value)
        {
            label.TextColor = value;
            activity.Color = value;
        }
        private void btn_Clicked(object sender, EventArgs e)
        {
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }
        private void GetStyle()
        {

        }
    }
}