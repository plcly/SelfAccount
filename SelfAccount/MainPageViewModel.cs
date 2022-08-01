using LiteDBHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SelfAccount
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private AccountService _accountService;
        public event PropertyChangedEventHandler PropertyChanged;
        public MainPageViewModel()
        {
            _accountService = new AccountService(Config.Key, Config.IV, Config.DBPwd, Config.DBName);
            AccountCategoies = new ObservableCollection<string>();
            Accounts = new ObservableCollection<LiteDBHelper.Account>();
        }

        private string _inputCategory;

        public string InputCategory
        {
            get { return _inputCategory; }
            set
            {
                _inputCategory = value;
                OnPropertyChanged();
            }
        }
        private string _inputName;

        public string InputName
        {
            get { return _inputName; }
            set
            {
                _inputName = value;
                OnPropertyChanged();
            }
        }
        private string _inputValue;

        public string InputValue
        {
            get { return _inputValue; }
            set
            {
                _inputValue = value;
                OnPropertyChanged();
            }
        }

        private ICommand _addCommand;
        public ICommand AddCommand =>
            _addCommand ?? (_addCommand = new Command(ExecuteAddCommand));

        void ExecuteAddCommand()
        {
            if (!string.IsNullOrEmpty(InputCategory)&& !string.IsNullOrEmpty(InputName) && !string.IsNullOrEmpty(InputValue) )
            {
                var ac = new LiteDBHelper.Account
                {
                    AccountCategory = InputCategory,
                    AccountName = InputName,
                    AccountValue = InputValue,
                };
                _accountService.Insert(ac);
                Refresh();
            }
            
        }

        private void Refresh()
        {
            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                var ac = _accountService.GetAccounts(SelectedCategory);
                Accounts = new ObservableCollection<LiteDBHelper.Account>(ac);
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        private string _selectedCategory;

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                Refresh();
                
                OnPropertyChanged();
            }
        }


        private ObservableCollection<string> _accountCategoies;

        public ObservableCollection<string> AccountCategoies
        {
            get => _accountCategoies;
            set
            {
                _accountCategoies = value;
                OnPropertyChanged();

            }
        }
        private ObservableCollection<LiteDBHelper.Account> _accounts;

        public ObservableCollection<LiteDBHelper.Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new Command(ExecuteSearchCommand));

        void ExecuteSearchCommand()
        {

            if (!string.IsNullOrEmpty(SearchText))
            {
                AccountCategoies = new ObservableCollection<string>(_accountService.GetCategories());
            }

        }

        private ICommand _copyCommad;
        public ICommand CopyCommad =>
            _copyCommad ?? (_copyCommad = new Command<LiteDBHelper.Account>(ExecuteCopyCommad));

        void ExecuteCopyCommad(LiteDBHelper.Account account)
        {

        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
