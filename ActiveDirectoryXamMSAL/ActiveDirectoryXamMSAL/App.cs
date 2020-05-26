using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

//Reference URL https://www.kompozure.com/blog/azure-ad-v2-and-msal-from-developers-point-of-view/
namespace ActiveDirectoryXamMSAL
{
    public class App : Application
    {
        //https://zure.com/blog/azure-ad-v2-and-msal-from-developers-point-of-view/

        public static PublicClientApplication PCA = null;

        /// <summary>
        /// The ClientID is the Application ID found in the portal (https://apps.dev.microsoft.com). 
        /// You can use the below id however if you create an app of your own you should replace the value here.
        /// </summary>
        public static string ClientID = "21148939-0966-4fbb-8408-86d287ea3403"; //XamarinAuthV2 personal
        public static string[] Scopes = { "openid", "profile", "email", "offline_access" };
        public static string Username = string.Empty;

        public static UIParent UiParent { get; set; }

        public App()
		{
            PCA = new PublicClientApplication(ClientID)//, "https://login.microsoftonline.com/918079db-c902-4e29-b22c-9764410d0375/v2.0")
            {
                RedirectUri = $"msal{ClientID}://auth"
            };

            MainPage = new ActiveDirectoryXamMSAL.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
