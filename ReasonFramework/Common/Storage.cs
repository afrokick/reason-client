using System;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
    public delegate void UpdateDataEventHandler();
    /// <summary>
    /// This class storage all game data
    /// </summary>
    public class Storage
    {
        #region Fields
        private int _id;
        public int Id { get { return _id; } }
        private string _name;
        public string Name { get { return _name; } }

        private Dictionary<string, string> _dicUids;

        private int _complatedTasks;
        public int ComplatedTasks { 
            get { return _complatedTasks; }
        }

        private int _skippedTasks;
        public int SkippedTasks { get { return _skippedTasks; } }

        private Task _currentTask;
        public Task CurrentTask { get { return _currentTask; } }

        public event UpdateDataEventHandler OnDataUpdate;

        private Network _net;
        #endregion

        public Storage(Network net)
        {
            _net = net;
            _dicUids = new Dictionary<string, string>();

            _net.OnAuthComplated += OnAuthComplatedCallback;
            _net.OnSendTaskComplated += OnSendTaskComplatedCallback;
        }
        #region Callbacks
        private void OnAuthComplatedCallback(NetResponse response)
        {
            if (!response.IsError)
            {
                _id = int.Parse(response.GetValue("id"));
                _name = response.GetValue("name");
                _complatedTasks = int.Parse(response.GetValue("completed"));
                _skippedTasks = int.Parse(response.GetValue("skipped"));
                if (response.IsContainsKey("task"))
                {
                    var taskText = response.GetValue("task");
                    var taskRank = double.Parse(response.GetValue("rank"));
                    byte taskUserRank = 0;//load from localStorage
                    List<Comment> taskComments = null;
                    //if (response.IsContainsKey("comments"))
                    //{
                    //    taskComments = new List<Comment>();
                    //    var comments = response.GetValue("comments").Split(new char[]{'|'});
                    //    foreach(var comm in comments)
                    //    {
                    //        var commParam = comm.Split(new string[]{";1;"},StringSplitOptions.RemoveEmptyEntries);
                    //    }
                    //}
                    _currentTask = new Task(taskText, taskRank, taskComments, taskUserRank);
                }
                CallDataUpdate();
            }
        }
        private void OnSendTaskComplatedCallback(NetResponse response)
        {
            if (!response.IsError)
            {

            }
        }
        #endregion
        private void CallDataUpdate()
        {
            if (OnDataUpdate != null)
                OnDataUpdate();
        }

        private void SetComplatedTasks()
        {
            _complatedTasks++;
            CallDataUpdate();
        }

        private void SetSkippedTasks()
        {
            _skippedTasks++;
            CallDataUpdate();
        }

        private void LoadLocal()
        {

        }

        private void SaveLocal()
        {

        }
    }
}
