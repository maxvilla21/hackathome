using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Newtonsoft.Json;

namespace HackAtHomeClient
{

    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/CustomIcon", Theme = "@android:style/Theme.Holo")]
    public class ListEvidenciasActivity : Activity
    {
        private Complex _data;
        private ListView _listViewEvidences;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListEvidencias);

            _listViewEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);
            var textViewParticipant = FindViewById<TextView>(Resource.Id.tvParticipante);

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
                GetEvidences();
            }
            else
            {
                _listViewEvidences.Adapter = _data.EvidencesAdapter;
            }

            textViewParticipant.Text = _data.ResultInfo.FullName;

            _listViewEvidences.ItemClick += _listViewEvidences_ItemClick;
        }

        private void _listViewEvidences_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var adapter = (EvidencesAdapter)_listViewEvidences.Adapter; 
            var evidence = adapter[e.Position];

            Intent intent = new Intent(this, typeof(DetailItemActivity));

            intent.PutExtra("Participant", _data.ResultInfo.FullName);
            intent.PutExtra("Token", _data.ResultInfo.Token);

            intent.PutExtra("EvidenceID", evidence.EvidenceID);
            intent.PutExtra("EvidenceStatus", evidence.Status);
            intent.PutExtra("EvidenceTitle", evidence.Title);

            StartActivity(intent);
        }

        private async void GetEvidences()
        {
            var serviceClient = new ServiceClient();
            var evidences=await serviceClient.GetEvidencesAsync(_data.ResultInfo.Token);
            _data.EvidencesAdapter = new EvidencesAdapter(this, evidences, Resource.Layout.ListItem, Resource.Id.tvTitle, Resource.Id.tvStatus);
            _listViewEvidences.Adapter = _data.EvidencesAdapter;
        }      
    }
}