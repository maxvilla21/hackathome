using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    public class Complex : Fragment
    {
        public override void OnCreate(Bundle saveInstanteState)
        {
            base.OnCreate(saveInstanteState);
            RetainInstance = true;

        }

        public ResultInfo ResultInfo { get; set; }
        public Evidence Evidence { get; set; }

        public EvidencesAdapter EvidencesAdapter { get; set; }
        public EvidenceDetail EvidenceDetail { get; set; }

    }
}