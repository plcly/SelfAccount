﻿using LiteDBHelper;
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
        private bool _isSearching;


        public event PropertyChangedEventHandler PropertyChanged;
        public MainPageViewModel(string key, string iv, Action<string> action)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iv))
                {
                    iv = Config.IV;
                }
                var status = Permissions.CheckStatusAsync<Permissions.StorageRead>().Result;
                var statusWrite = Permissions.CheckStatusAsync<Permissions.StorageWrite>().Result;
                _accountService = new AccountService(key, iv, Config.DBPwd, Config.DBName);
                AccountCategoies = new ObservableCollection<string>(_accountService.GetCategories());
                SelectedCategory = AccountCategoies.FirstOrDefault();
                Accounts = new ObservableCollection<Account>();
            }
            catch (Exception ex)
            {
                action(ex.Message);
            }


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

        private string _inputKey;

        public string InputKey
        {
            get { return _inputKey; }
            set
            {
                _inputKey = value;
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
            if (!string.IsNullOrEmpty(InputCategory) 
                && !string.IsNullOrEmpty(InputName) 
                && !string.IsNullOrEmpty(InputKey) 
                && !string.IsNullOrEmpty(InputValue))
            {
                var ac = new Account
                {
                    AccountCategory = InputCategory,
                    AccountName = InputName,
                    AccountKey = InputKey,
                    AccountValue = InputValue,
                };
                _accountService.Insert(ac);
                Refresh();
                InputCategory = String.Empty;
                InputName = String.Empty;
                InputKey = String.Empty;
                InputValue = String.Empty;
            }

        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new Command<Account>(ExecuteDeleteCommand));

        void ExecuteDeleteCommand(Account account)
        {
            _accountService.Delete(account);
            Refresh();
        }

        private void Refresh()
        {
            if (!_isSearching && !string.IsNullOrEmpty(SelectedCategory))
            {
                var ac = _accountService.GetAccounts(SelectedCategory);
                Accounts = new ObservableCollection<Account>(ac);
            }
            _isSearching = false;
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
                OnPropertyChanged();
                Refresh();
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
        private ObservableCollection<Account> _accounts;

        public ObservableCollection<Account> Accounts
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

            if (string.IsNullOrEmpty(SearchText))
            {
                AccountCategoies = new ObservableCollection<string>(_accountService.GetCategories());
                SelectedCategory = AccountCategoies.FirstOrDefault();
            }
            else
            {
                var acs = _accountService.SearchAccounts(SearchText);
                Accounts = new ObservableCollection<Account>(acs);
                _isSearching = true;
                AccountCategoies = new ObservableCollection<string>(new[] { "搜索结果" });
                //SelectedCategory = "搜索结果";
            }

        }

        private ICommand _copyCommad;
        public ICommand CopyCommad =>
            _copyCommad ?? (_copyCommad = new Command<Account>(ExecuteCopyCommad));

        void ExecuteCopyCommad(Account account)
        {

        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
