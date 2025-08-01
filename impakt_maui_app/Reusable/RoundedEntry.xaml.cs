namespace impakt_maui_app.Reusable;

public partial class RoundedEntry : ContentView
{
	public RoundedEntry()
	{
		InitializeComponent();
        // BindingContext = this;
	}

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(RoundedEntry),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: OnTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RoundedEntry)bindable;
        control.Text = (string)newValue; // assuming you have a Label named TitleLabel
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(RoundedEntry),
            string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(
            nameof(Keyboard),
            typeof(Keyboard),
            typeof(RoundedEntry),
            Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(RoundedEntry),
            false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
}