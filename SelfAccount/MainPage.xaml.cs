namespace SelfAccount;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        Permissions.RequestAsync<Permissions.StorageRead>();
        Permissions.RequestAsync<Permissions.StorageWrite>();

        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }


    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //	count++;

    //	if (count == 1)
    //		CounterBtn.Text = $"Clicked {count} time";
    //	else
    //		CounterBtn.Text = $"Clicked {count} times";

    //	SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}

