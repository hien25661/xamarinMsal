using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ActiveDirectoryXamMSAL
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        IEnumerable<IAccount> accounts;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        public static bool IsPageFirtsLoad = true;
        protected override async void OnAppearing()
        {
            if (IsPageFirtsLoad)
            {
                IsPageFirtsLoad = false;
                try
                {
                    await CheckUserLogin();
                }catch{
                    IsLoader = false;
                    OnSignInSignOutTxt = "Sign in";
                }
                BindingContext = this;
                //OnSignInSignOutTxt = "Sign in";
            }

            base.OnAppearing();
        }

        async Task CheckUserLogin(){
            IsLoader = true;
            AuthenticationResult authResult = null;
            accounts = await App.PCA.GetAccountsAsync();
            try
            {
                IAccount firstAccount = accounts.FirstOrDefault();
                authResult = await App.PCA.AcquireTokenSilentAsync(App.Scopes, firstAccount);
                if (authResult != null)
                {
                    await RefreshUserDataAsync(authResult.AccessToken).ConfigureAwait(false);
                    OnSignInSignOutTxt = "Sign out";
                }
            }
            catch (MsalUiRequiredException ex)
            {
                authResult = await App.PCA.AcquireTokenAsync(App.Scopes, App.UiParent);
                await RefreshUserDataAsync(authResult.AccessToken);
                OnSignInSignOutTxt = "Sign out";
            }
            IsLoader = false;
        }

        async void OnSignInSignOut(object sender, EventArgs e)
        {
            accounts = await App.PCA.GetAccountsAsync();
            try
            {
                if (btnSignInSignOut.Text == "Sign in")
                {
                    await CheckUserLogin();
                }
                else
                {
                    while (accounts.Any())
                    {
                        await App.PCA.RemoveAsync(accounts.FirstOrDefault());
                        accounts = await App.PCA.GetAccountsAsync();
                    }

                    slUser.IsVisible = false;
                    OnSignInSignOutTxt = "Sign in";
                }
            }
            catch (Exception)
            {
            }
            IsLoader = false;
        }

        public async Task RefreshUserDataAsync(string token)
        {
            //get data from API
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject user = JObject.Parse(responseString);

                slUser.IsVisible = true;

                Device.BeginInvokeOnMainThread(() =>
                {

                    lblDisplayName.Text = user["displayName"].ToString();
                    lblGivenName.Text = user["givenName"].ToString();
                    lblId.Text = user["id"].ToString();
                    lblSurname.Text = user["surname"].ToString();
                    lblUserPrincipalName.Text = user["userPrincipalName"].ToString();

                    // just in case
                    OnSignInSignOutTxt = "Sign out";
                });
            }
            else
            {
                await DisplayAlert("Something went wrong with the API call", responseString, "Dismiss");
            }
        }

        string _signInSignOut;
        public string OnSignInSignOutTxt
        {
            get => _signInSignOut;
            set
            {
                _signInSignOut = value;
                NotifyPropertyChanged("OnSignInSignOutTxt");
            }
        }

        bool _isLoader;
        public bool IsLoader
        {
            get => _isLoader;
            set
            {
                _isLoader = value;
                NotifyPropertyChanged("IsLoader");
            }
        }

        #region INotifyPropertyChanged      
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
