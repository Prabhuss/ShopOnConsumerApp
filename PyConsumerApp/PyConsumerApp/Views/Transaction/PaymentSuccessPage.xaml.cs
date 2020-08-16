
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Transaction
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentSuccessPage : ContentPage
    {
        public PaymentSuccessPage()
        {
            InitializeComponent();
        }
    }
}