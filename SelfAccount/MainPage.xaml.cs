
namespace SelfAccount;

public partial class MainPage : ContentPage
{
    public Action<string> MessageAction;

    public MainPage()
    {
        var s = Permissions.RequestAsync<Permissions.StorageRead>();
        var r = Permissions.RequestAsync<Permissions.StorageWrite>();
        Task.WaitAll(s,r);
        InitializeComponent();
        MessageAction = ShowMessage;


    }

    private void ShowMessage(string msg)
    {
        DisplayAlert("Error:",msg,"OK");
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(keyEntry.Text))
        {
            BindingContext = new MainPageViewModel(keyEntry.Text, ivEntry.Text, MessageAction);
            loadGrid.IsVisible = false;
            contentStack.IsVisible = true;
        }
        
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

