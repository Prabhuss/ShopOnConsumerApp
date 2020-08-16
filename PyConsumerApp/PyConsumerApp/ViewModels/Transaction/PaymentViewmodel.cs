using Plugin.Connectivity;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Transaction
{
    /// <summary>
    /// ViewModel for Payment page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class PaymentViewModel : BaseViewModel
    {

        public string orderConfirmCaption;
        public string orderConfirmMessage;
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="PaymentViewModel" /> class.
        /// </summary>
        public PaymentViewModel()
        {
            PaymentSuccessIcon = "PaymentSuccess.svg";
            PaymentFailureIcon = "PaymentFailure.svg";
            ContinueShoppingCommand = new Command(ContinueShoppingClickedAsync);
            SetMessage();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when track order button is clicked.
        /// </summary>
        public Command ContinueShoppingCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when track order button is clicked.
        /// </summary>
        private void ContinueShoppingClickedAsync(object obj)
        {
            Application.Current.MainPage = new NavigationPage(new BottomNavigationPage());
            BaseViewModel.Navigation = Application.Current.MainPage.Navigation;

        }
        private async void SetMessage()
        {
            Merchantdata MerchantSettingDetails = await CategoryDataService.Instance.GetMerchantSettings("OrderCaption");
            if (MerchantSettingDetails != null)
            {
                if (MerchantSettingDetails.SettingIsActive.ToLower() == "yes")
                {
                    OrderConfirmCaption = MerchantSettingDetails.SettingMessage;
                }

            }

            MerchantSettingDetails = await CategoryDataService.Instance.GetMerchantSettings("OrderMessage");
            if (MerchantSettingDetails != null)
            {
                if (MerchantSettingDetails.SettingIsActive.ToLower() == "yes")
                {
                    OrderConfirmMessage = MerchantSettingDetails.SettingMessage;
                }

            }

        }

        private Task DisplayAlert(string v1, string settingMessage, string v2)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the payment success icon.
        /// </summary>
        public string PaymentSuccessIcon { get; set; }

        /// <summary>
        /// Gets or sets the payment failure icon.
        /// </summary>
        public string PaymentFailureIcon { get; set; }
        public string OrderConfirmCaption
        {
            get { return this.orderConfirmCaption; }
            set
            {
                if (this.orderConfirmCaption == value)
                {
                    return;
                }

                this.orderConfirmCaption = value;
                this.NotifyPropertyChanged();
            }
        }
        public string OrderConfirmMessage
        {
            get { return this.orderConfirmMessage; }
            set
            {
                if (this.orderConfirmMessage == value)
                {
                    return;
                }

                this.orderConfirmMessage = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
