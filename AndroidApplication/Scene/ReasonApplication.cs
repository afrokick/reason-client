using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ReasonFramework.Common;
using System.Threading.Tasks;

namespace ReasonApplication
{
    [Activity(Label = "AndroidApplication", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReasonApplication : Activity
    {
        private Storage _storage;
        private Network _net;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _net = new Network();
            _storage = new Storage(_net);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btStandalone);
            button.Click += delegate
            {
                button.Enabled = false;
                SetLoginAuthScreen();
            };
        }

        private void SetLoginAuthScreen()
        {
            SetContentView(Resource.Layout.LoginAuth);
            var btLogin = FindViewById<Button>(Resource.Id.btAuth);
            btLogin.Click += delegate
            {
                btLogin.Enabled = false;
                var lbError = FindViewById<TextView>(Resource.Id.lbError);
                lbError.Text = "";
                System.Threading.Tasks.Task.Factory.StartNew(() => _net.SendAuth(LoginTypeEnum.stand_alone,
                                                          FindViewById<EditText>(Resource.Id.tbLogin).Text,
                                                          FindViewById<EditText>(Resource.Id.tbPass).Text))
                    .ContinueWith((t) =>
                    {
                        if (_storage.Id == 0)
                        {
                            lbError.Text = "Invalid username or password!";
                            btLogin.Enabled = true;
                        }
                        else
                        {
                            SetHomeScreen();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            };
        }
        private void SetHomeScreen()
        {
            SetContentView(Resource.Layout.HomeScreen);
            try
            {
                var tabs = (TabHost)FindViewById<TabHost>(Resource.Id.tabHost1);

                tabs.Setup();

                TabHost.TabSpec spec = tabs.NewTabSpec("tag1");
                spec.SetContent(Resource.Id.tab1);
                spec.SetIndicator("Домой");
                tabs.AddTab(spec);

                spec = tabs.NewTabSpec("tag2");
                spec.SetContent(Resource.Id.tab2);
                spec.SetIndicator("Задание");
                tabs.AddTab(spec);

                tabs.SetCurrentTabByTag("tag1");
            }
            catch (Exception ex)
            {
                Logger.Log("error23:{0}", ex);
            }

            var btnSendComment = FindViewById<Button>(Resource.Id.btSendComment);
            UpdateUserDataView();
            _storage.OnDataUpdate += () =>
            {
                UpdateUserDataView();
            };
        }

        private void UpdateUserDataView()
        {
            var lbName = FindViewById<TextView>(Resource.Id.lbName);
            lbName.Text = string.Format("{0}(id:{1})", _storage.Name, _storage.Id);

            var lbComplated = FindViewById<TextView>(Resource.Id.lbComplated);
            lbComplated.Text = string.Format("Complated: {0}", _storage.ComplatedTasks);

            var lbSkipped = FindViewById<TextView>(Resource.Id.lbSkipped);
            lbSkipped.Text = string.Format("Skipped: {0}", _storage.SkippedTasks);

            if (_storage.CurrentTask != null)
            {
                var lbTaskText = FindViewById<TextView>(Resource.Id.lbTaskText);
                lbTaskText.Text = _storage.CurrentTask.Text;

                var lbTaskRank = FindViewById<TextView>(Resource.Id.lbRank);
                lbTaskRank.Text = _storage.CurrentTask.Ranking;
            }
        }
    }
}

