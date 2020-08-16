using Plugin.Connectivity;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Transaction
{

    [Preserve(AllMembers = true)]
    [DataContract]
    class AddressEditViewodel : BaseViewModel
    {
        public Command ChangeAddressInfo { get; set; }
        public Command UseMyLocationCommand { get; set; }
        
        public Address customerAddress;

        public ObservableCollection<string> addressTypesList;
        public ObservableCollection<string> AddressTypesList
        {
            get { return addressTypesList; }
            set
            {
                if (addressTypesList != value)
                {
                    addressTypesList = value;
                    OnPropertyChanged();
                }
            }
        }

        string selectedAddressType;
        [DataMember(Name = "selectedAddressType")]
        public string SelectedAddressType
        {
            get { return selectedAddressType; }
            set
            {
                if (selectedAddressType != value)
                {
                    selectedAddressType = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "customerAddress")]
        public Address CustomerAddress
        {
            get => customerAddress;

            set
            {
                if (customerAddress == value) return;
                customerAddress = value;
                NotifyPropertyChanged();
            }
        }

        public AddressEditViewodel()
        {
            customerAddress = new Address();
            addressTypesList = new ObservableCollection<string>();
            addressTypesList.Add("Office");
            addressTypesList.Add("Home");
            addressTypesList.Add("Other");
            selectedAddressType = "";
            ChangeAddressInfo = new Command(this.ChangeAddress_Clicked);
            UseMyLocationCommand = new Command(this.UseMyLocation_Clicked); 
        }

        private Command backButtonCommand;
        [DataMember(Name = "backButtonCommand")]
        public Command BackButtonCommand => backButtonCommand ?? (backButtonCommand = new Command(BackButtonClicked));

        private async void BackButtonClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        private async void UseMyLocation_Clicked(object obj)
        {
            if (CustomerAddress.FirstName== null || CustomerAddress.Address2 == null || CustomerAddress.TagName == null)
            {
                await Application.Current.MainPage.DisplayAlert("To use location", "Please fill all the above fields", "Ok");
                return;
            }
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    CustomerAddress.Address1 = "Location is given as Delivery Address";
                    CustomerAddress.SocietyBuildingNo = "";
                    CustomerAddress.FlatNoDoorNo = "";
                    CustomerAddress.City = "";
                    CustomerAddress.State = "";

                    var app = App.Current as App;
                    try
                    {
                        var location = await Geolocation.GetLastKnownLocationAsync();
                        if (location != null)
                        {
                            CustomerAddress.Latitude = location.Latitude.ToString();
                            CustomerAddress.Longitude = location.Longitude.ToString();
                        }
                        else
                        {
                            DependencyService.Get<IToastMessage>().LongTime("Error L01: Unable to fetch Current location");
                        }
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Error L02: Unable to fetch Current location");
                    }
                    catch (PermissionException pEx)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Error L03: Unable to fetch Current location");
                    }
                    catch (System.Exception ex)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Error L04: Unable to fetch Current location");
                    }

                    bool resonse = await CartDataService.Instance.SaveAddressInfo(CustomerAddress);
                    if (resonse == true)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Address changed successfully");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Error L05: Connection Problem. Something went wrong.");
                        //await Application.Current.MainPage.DisplayAlert("Error Message", "Somthing went Wrong", "Ok");
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error101", e.Message, "OK");
                }
            }
            else
            {
                DependencyService.Get<IToastMessage>().LongTime("No Internet Connection");
            }
        }
        private async void ChangeAddress_Clicked(object obj)
        {
            if (CustomerAddress.FirstName == null || CustomerAddress.Address2 == null || CustomerAddress.TagName == null
                || CustomerAddress.Address1 == null || CustomerAddress.PostalCodeZipCode == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fields Empty Error", "Please fill all the Mandatory fields", "Ok");
                return;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    bool resonse = await CartDataService.Instance.SaveAddressInfo(CustomerAddress);
                    if (resonse == true)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Address changed successfully");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Error AD01: Unable to fetch Current location");
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error102", e.Message, "OK");
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

    }
}
