﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ReasonFramework.Common;

namespace ReasonApplication
{
    [Activity(Label = "AndroidApplication", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReasonApplication : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var net = new Network();
            net.SendRequest();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}
