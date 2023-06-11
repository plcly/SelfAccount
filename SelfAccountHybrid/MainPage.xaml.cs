namespace SelfAccountHybrid
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var s = Permissions.RequestAsync<Permissions.StorageRead>();
            var r = Permissions.RequestAsync<Permissions.StorageWrite>();
            Task.WaitAll(s, r);
            InitializeComponent();
        }
    }
}