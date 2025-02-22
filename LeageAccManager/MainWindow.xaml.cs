using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Windows;
using Newtonsoft.Json;
using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput.Native;
using WindowsInput;
using MessageBox = System.Windows.MessageBox;
using Window = System.Windows.Window;

namespace LeageAccManager
{
    public partial class MainWindow : Window
    {
        private const string CredentialsFilePath = "credentials.json";
        private List<Account> _accounts;

        public MainWindow()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            if (File.Exists(CredentialsFilePath))
            {
                var json = File.ReadAllText(CredentialsFilePath);
                _accounts = JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
            }
            else
            {
                _accounts = new List<Account>();
            }
            AccountsComboBox.ItemsSource = _accounts;
        }

        private void SaveAccounts()
        {
            var json = JsonConvert.SerializeObject(_accounts, Formatting.Indented);
            File.WriteAllText(CredentialsFilePath, json);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;
            

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            _accounts.Add(new Account { Username = username, Password = password });
            SaveAccounts();
            
            AccountsComboBox.ItemsSource = null;
            AccountsComboBox.ItemsSource = _accounts;
            AccountsComboBox.SelectedIndex = AccountsComboBox.Items.Count - 1;
        }
        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountsComboBox.SelectedItem as Account;
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account.");
                return;
            }

            AccLogin.Login(selectedAccount);
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountsComboBox.SelectedItem as Account;
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account.");
                return;
            }

            _accounts.Remove(selectedAccount);
            SaveAccounts();
            
            AccountsComboBox.ItemsSource = null;
            AccountsComboBox.ItemsSource = _accounts;
        }

        private void CheckAccount_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}    