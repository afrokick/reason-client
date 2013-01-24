using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ReasonFramework.Common;
using System.Threading.Tasks;
using System.Timers;

namespace ReasonApplication
{
    [Activity(Label = "AndroidApplication", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReasonApplication : Activity
    {
        #region Fields
        private Storage _storage;
        private Network _net;

        private TextView lbSkipped;
        private TextView lbCompleted;
        private TextView lbWait;
        private TextView lbName;
        private TextView lbTaskText;
        private TextView lbTaskRank;
        private EditText tbComment;

        private ScrollView layoutTask;
        private LinearLayout layoutHome;

        private Timer timer;
        #endregion
        #region Impl
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _net = new Network();
            _storage = new Storage(_net);
            _net.InitStorage(_storage);

            SetMainScreen();
        }

        protected override void OnDestroy()
        {
            if (timer != null)
                timer.Stop();
            base.OnDestroy();
        }
        #endregion

        private void SetMainScreen()
        {
            SetContentView(Resource.Layout.Main);

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
                Task.Factory.StartNew(() => _net.SendAuth(LoginTypeEnum.stand_alone,
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
            lbName = FindViewById<TextView>(Resource.Id.lbName);
            lbCompleted = FindViewById<TextView>(Resource.Id.lbCompleted);
            lbSkipped = FindViewById<TextView>(Resource.Id.lbSkipped);
            tbComment = FindViewById<EditText>(Resource.Id.tbTaskComment);
            lbTaskText = FindViewById<TextView>(Resource.Id.lbTaskText);
            lbWait = FindViewById<TextView>(Resource.Id.lbWait);
            layoutTask = FindViewById<ScrollView>(Resource.Id.scroll);
            layoutHome = FindViewById<LinearLayout>(Resource.Id.tab1);
            lbTaskRank = FindViewById<TextView>(Resource.Id.lbRank);

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
            var layout = FindViewById<ScrollView>(Resource.Id.scroll);
            var btCloseTask = FindViewById<Button>(Resource.Id.btCloseTask);
            btCloseTask.Click += (o, k) =>
            {
                if (_storage.CurrentTask == null)
                    return;
                HideTask();
                Task.Factory.StartNew(() => _net.SendTaskDone(false)).
                    ContinueWith((t) =>
                    {
                        UpdateUserDataView();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            };

            var btSendTask = FindViewById<Button>(Resource.Id.btSendComment);
            btSendTask.Click += (o, k) =>
            {
                //var btn = new Button(this);
                //btn.SetText("New Button", TextView.BufferType.Normal);
                //layout.AddView(btn);

                if (_storage.CurrentTask == null)
                    return;
                HideTask();//->Close()
                Task.Factory.StartNew(() => _net.SendTaskDone(true, (byte)0, tbComment.Text.Length > 0 ? tbComment.Text : "")).
                    ContinueWith((t) =>
                    {
                        UpdateUserDataView();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            };
            UpdateUserDataView();
            _storage.OnDataUpdate += () =>
            {
                RunOnUiThread(() => UpdateUserDataView());
            };

            timer = new Timer();
            timer.Interval = 10000;
            timer.Elapsed += (o,k) =>
            {
                if (_storage.CurrentTask == null)
                {
                    Task.Factory.StartNew(() => _net.GetTask());
                    Logger.Log("try get new tasks...");
                }
                timer.Enabled = true;
            };
            timer.Start();
        }

        private void UpdateUserDataView()
        {
            Logger.Log("Game: UpdateUserDataView()");
            
            lbName.Text = string.Format("{0}(id:{1})", _storage.Name, _storage.Id);
            lbCompleted.Text = string.Format("Completed: {0}", _storage.CompletedTasks);
            lbSkipped.Text = string.Format("Skipped: {0}", _storage.SkippedTasks);

            tbComment.Text = "";
            if (_storage.CurrentTask != null)
            {                
                lbTaskText.Text = _storage.CurrentTask.Text;
                lbTaskRank.Text = _storage.CurrentTask.Ranking;
                ShowTask();
            }
            else
            {
                HideTask();
            }

            
            lbWait.Visibility = _storage.CurrentTask != null ? ViewStates.Invisible : ViewStates.Visible;
        }
        //убирает таск во временную карзину
        private void HideTask()
        {
            layoutTask.Visibility = ViewStates.Invisible;
            lbWait.Visibility = ViewStates.Visible;
        }
        //показывает новый таск, или тот, который скрыли
        private void ShowTask()
        {
            layoutTask.Visibility = ViewStates.Visible;
            lbWait.Visibility = ViewStates.Invisible;
        }
        //убирает полностью таск
        private void DeleteTask()
        {

        }
    }
}

