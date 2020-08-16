using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Forms;
using PyConsumerApp.Views.History;
using PyConsumerApp.Views.Profile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Profile
{
    /// <summary>
    /// ViewModel for health profile page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ProfileViewModel : BaseViewModel
    {

        //private ObservableCollection<CustomerProfile> profile;
        private CustomerProfile profile;
        public Command LogOutCommand { get; set; }
        public Command MyOrdersCommand { get; set; }
        public Command ChangeCustomerInfo { get; set; }
        App app;

        private Command backButtonCommand;
        public Command  AskEditProfileCommand;
        public Command BackButtonCommand//{ get; set; }
        {
            get { return backButtonCommand ?? (this.backButtonCommand = new Command(this.BackButtonClicked)); }
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ProfileViewModel" /> class.
        /// </summary>
        public ProfileViewModel()
        {
            app = App.Current as App;
            this.EditProfileCommand = new Command(this.EditProfileClicked);
            // this.ProfileImage = App.BaseImageUrl + "ProfileImage16.png";
            this.ProfileImage = App.profileImage;
            LogOutCommand = new Command(this.Logout_Clicked);
            MyOrdersCommand = new Command(this.MyOrders_Clicked);
            AskEditProfileCommand = new Command(this.ChangeInfoDialogBox_Clicked);
            ChangeCustomerInfo = new Command(this.ChangeInfo_Clicked);
            GetUserProfile();

        }
        #endregion


        private async void Logout_Clicked(object obj)
        {
            var app = Application.Current as App;
            bool answer = await Application.Current.MainPage.DisplayAlert("Log Out", "Do you really want to log out?", "Yes", "no");
            if (answer)
            {
                app.IsLoggedIn = false;
                app.UserPhoneNumber = null;
                app.SecurityAccessKey = null;
                app.MinimumOrderAmount = 0;
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                BaseViewModel.Navigation = Application.Current.MainPage.Navigation;
            }
        }

        private async void ChangeInfo_Clicked(object obj)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                Analytics.TrackEvent("ModifyUserInfo_Clicked", new Dictionary<string, string> {
                            { "MerchantBranchId", app.Merchantid},
                            { "UserPhoneNumber", app.UserPhoneNumber},
                            });
                bool resonse = await ProfileDataService.Instance.SaveCustomerInfo(Profile);
                GetUserProfile();
                if (resonse == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Profile changed successfully", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error Message", "Something went Wrong", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            else
            {
                try
                {
                    DependencyService.Get<IToastMessage>().LongTime("No Internet Connection");
                }
                catch { }
            }
        }
        public async void GetUserProfile()
        {
            try
            {
                CustomerProfile customerProfile = await ProfileDataService.Instance.GetUserInfo();
                if (customerProfile != null)
                {
                    Profile = customerProfile;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"error "+ex.Message);
            }
            
        }
        [DataMember(Name = "profile")]
        public CustomerProfile Profile
        {
            get => profile;

            set
            {
                if (profile == value) return;

                profile = value;
                NotifyPropertyChanged();
            }
        }
        /* public ObservableCollection<CustomerProfile> Profile
         {
             get { return this.profile; }
             set
             {
                 if (this.profile == value)
                 {
                     return;
                 }

                 this.profile = value;
                 this.NotifyPropertyChanged();
             }
         }*/

        
        private async void ChangeInfoDialogBox_Clicked()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Message", "Click on 'Edit Profile' to edit your information.", "Edit Profile", "Cancel");
            if (answer)
            {
                ProfileEditPage view = new ProfileEditPage();
                await Navigation.PushAsync(view);
                await Task.Delay(10);
            }
        }
        private async void EditProfileClicked()
        {
            ProfileEditPage view = new ProfileEditPage();
            await Navigation.PushAsync(view);
            await Task.Delay(10);
        }
        private async void MyOrders_Clicked()
        {
            MyOrdersPage view = new MyOrdersPage();
            await Navigation.PushAsync(view);
            await Task.Delay(10);
        }

        public Command EditProfileCommand { get; set; }

        public string ProfileImage { get; set; }


        private async void BackButtonClicked(object obj)
        {
            // Do something
            //await Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}