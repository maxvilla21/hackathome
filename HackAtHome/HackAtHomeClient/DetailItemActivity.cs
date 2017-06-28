using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Koush;
using Newtonsoft.Json;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/CustomIcon", Theme = "@android:style/Theme.Black")]
    public class DetailItemActivity : Activity
    {
        private Complex _data;
        private ImageView _imageViewEvidence;
        private WebView _webView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DetailItem);
            var tvarticipant = FindViewById<TextView>(Resource.Id.tvParticipante);
            var tvTitle = FindViewById<TextView>(Resource.Id.tvTitle);
            var tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            
             _imageViewEvidence= FindViewById<ImageView>(Resource.Id.imageViewEvidence);
            _webView= FindViewById<WebView>(Resource.Id.webView1);

            _data = (Complex)this.FragmentManager.FindFragmentByTag("Data");
            if (_data == null)
            {
                _data = new Complex();
                var fragmentTransaction = this.FragmentManager.BeginTransaction();
                fragmentTransaction.Add(_data, "Data");
                fragmentTransaction.Commit();


                _data.ResultInfo = new ResultInfo()
                {
                    FullName = Intent.GetStringExtra("Participant"),
                    Token = Intent.GetStringExtra("Token")
                };

                _data.Evidence = new Evidence()
                {
                    EvidenceID = Intent.GetIntExtra("EvidenceID", 0),
                    Status = Intent.GetStringExtra("EvidenceStatus"),
                    Title = Intent.GetStringExtra("EvidenceTitle")
                };

                GetDetail();
            }
            else
            {
                FillData(_data.EvidenceDetail);
            }
            tvarticipant.Text = _data.ResultInfo.FullName;
            tvTitle.Text = _data.Evidence.Title;
            tvStatus.Text = _data.Evidence.Status;
    
        }

        private async void GetDetail()
        {
            var serviceClient = new ServiceClient();
            var evidenceDetail=await serviceClient.GetEvidenceByIDAsync(_data.ResultInfo.Token,_data.Evidence.EvidenceID);
            _data.EvidenceDetail = evidenceDetail;
            FillData(evidenceDetail);
        }

        private void FillData(EvidenceDetail evidenceDetail)
        {
            var description = $"<div style='color:#ecf0f1'>{evidenceDetail.Description}</div>";
            _webView.LoadDataWithBaseURL(null, description, "text/html", "uft-8", null);
            _webView.SetBackgroundColor(Color.Transparent);
            Koush.UrlImageViewHelper.SetUrlDrawable(_imageViewEvidence, evidenceDetail.Url);

        }
    }
}