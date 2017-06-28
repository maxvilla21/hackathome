using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Newtonsoft.Json;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/CustomIcon", Theme = "@android:style/Theme.Holo")]
    public class MainActivity : Activity
    {
        private EditText _editTextMail;
        private EditText _editTextPassword;

        private UserInfo _userInfo;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _userInfo = new UserInfo();
            _editTextMail = FindViewById<EditText>(Resource.Id.editTextEmail);
            _editTextPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            var buttonValidate = FindViewById<Button>(Resource.Id.buttonValidate);
            buttonValidate.Click += ButtonValidate_Click;  
           
        }

        private async void ButtonValidate_Click(object sender, System.EventArgs e)
        {
           _userInfo.Email = _editTextMail.Text;
           _userInfo.Password = _editTextPassword.Text;
            var serviceClient = new ServiceClient();

            ResultInfo result = await serviceClient.AutenticateAsync(_userInfo.Email, _userInfo.Password);
            if (result.Status==Status.Success)
            {
               // SendEvidence();

                Intent intent = new Intent(this, typeof(ListEvidenciasActivity));
                intent.PutExtra("Participant",result.FullName);
                intent.PutExtra("Token", result.Token);
                StartActivity(intent);
            }
            else
            {
                Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alert = builder.Create();
                alert.SetTitle("Resultado de la Autenticación");
                alert.SetIcon(Resource.Drawable.Icon);
                alert.SetMessage($"{result.Status}");
                alert.SetButton("OK", (s, ev) => { });
                alert.Show();
            }
        }

        private async void SendEvidence()
        {
            var microsoftClient = new MicrosoftServiceClient();
            LabItem microsoftEvidence = new LabItem()
            {
                Email =_userInfo.Email, 
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(
                    ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };

            await microsoftClient.SendEvidence(microsoftEvidence);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }
    }
}

