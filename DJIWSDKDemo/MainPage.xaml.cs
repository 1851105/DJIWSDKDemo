using DJI.WindowsSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DJI.WindowsSDK.UserAccount;
using Windows.UI.Popups;
using static System.Windows.Forms;
using System.Threading.Tasks;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace DJIWSDKDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
       

        public MainPage()
        {
            this.InitializeComponent();
            DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationEvent;
            DJISDKManager.Instance.RegisterApp("77ea0a7bb9ac97b7f2812ffa");
          
        }

        public object MessageBox { get; private set; }

        private async void Instance_SDKRegistrationEvent(SDKRegistrationState state, SDKError resultCode)
        {
            if (resultCode == SDKError.NO_ERROR)
            {
                System.Diagnostics.Debug.WriteLine("Register app successfully.");

                //The product connection state will be updated when it changes here.
                DJISDKManager.Instance.ComponentManager.GetProductHandler(0).ProductTypeChanged += async delegate (object sender, ProductTypeMsg? value)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        if (value != null && value?.value != ProductType.UNRECOGNIZED)
                        {
                            System.Diagnostics.Debug.WriteLine("The Aircraft is connected now.");
                            //You can load/display your pages according to the aircraft connection state here.
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("The Aircraft is disconnected now.");
                            //You can hide your pages according to the aircraft connection state here, or show the connection tips to the users.
                        }
                    });
                };

                //If you want to get the latest product connection state manually, you can use the following code
                var productType = (await DJISDKManager.Instance.ComponentManager.GetProductHandler(0).GetProductTypeAsync()).value;
                if (productType != null && productType?.value != ProductType.UNRECOGNIZED)
                {
                    System.Diagnostics.Debug.WriteLine("The Aircraft is connected now.");
                    //You can load/display your pages according to the aircraft connection state here.
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Register SDK failed, the error is: ");
                System.Diagnostics.Debug.WriteLine(resultCode.ToString());
            }
            
        }
        private async void loginButton_Click(object sender, RoutedEventArgs e  )
        {
            string userName = this.userName.Text;
            string passWord = this.passWord.Password;
            if (userName == "123" && passWord == "123")
            {
                var messageDialog = new MessageDialog("登陆成功");
                await messageDialog.ShowAsync();
            }
            else
            {
                var messageDialog = new MessageDialog("用户名或密码错误，请重新登录");
                await messageDialog.ShowAsync();
            }
        }
        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }


}
