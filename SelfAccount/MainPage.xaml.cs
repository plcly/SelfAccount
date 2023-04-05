
namespace SelfAccount;

public partial class MainPage : ContentPage
{
    public Action<string> MessageAction;

    public MainPage()
    {
        var s = Permissions.RequestAsync<Permissions.StorageRead>();
        var r = Permissions.RequestAsync<Permissions.StorageWrite>();
        Task.WaitAll(s, r);
        InitializeComponent();
        MessageAction = ShowMessage;
        DbPath.Text = Config.DBName;
        keyEntry.Focus();
    }

    private void ShowMessage(string msg)
    {
        DisplayAlert("Error:", msg, "OK");
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        ShowContent();
    }

    private void keyEntry_Completed(object sender, EventArgs e)
    {
        ShowContent();
    }

    private void ShowContent()
    {
        if (!string.IsNullOrEmpty(keyEntry.Text))
        {
            BindingContext = new MainPageViewModel(keyEntry.Text, ivEntry.Text, MessageAction);
            loadGrid.IsVisible = false;
            contentStack.IsVisible = true;
            searchEntry.Focus();
        }
    }

    private void searchEntry_Completed(object sender, EventArgs e)
    {
        var vm = BindingContext as MainPageViewModel;
        vm.SearchCommand.Execute(null);
    }
}

