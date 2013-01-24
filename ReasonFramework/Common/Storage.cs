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

        private int _completedTasks;
        public int CompletedTasks { 
            get { return _completedTasks; }
        }

        private int _skippedTasks;
        public int SkippedTasks { get { return _skippedTasks; } }

        private GameTask _currentTask;
        public GameTask CurrentTask { get { return _currentTask; } }

        public event UpdateDataEventHandler OnDataUpdate;

        private Network _net;
        #endregion

        public Storage(Network net)
        {
            _net = net;
            _dicUids = new Dictionary<string, string>();

            _net.OnAuthCompleted += OnAuthCompletedCallback;
            _net.OnSendTaskCompleted += OnSendTaskCompletedCallback;
        }
        #region Callbacks
        private void OnAuthCompletedCallback(NetResponse response)
        {
            if (!response.IsError)
            {
                _id = int.Parse(response.GetValue("id"));
                _name = response.GetValue("name");
                _completedTasks = int.Parse(response.GetValue("completed"));
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
                    _currentTask = new GameTask(taskText, taskRank, taskComments, taskUserRank);
                }
                CallDataUpdate();
            }
        }
        private void OnSendTaskCompletedCallback(NetResponse response)
        {
            if (!response.IsError)
            {
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
                    _currentTask = new GameTask(taskText, taskRank, taskComments, taskUserRank);
                }
                else
                {
                    _currentTask = null;
                }
                //CallDataUpdate();
            }
        }
        #endregion
        private void CallDataUpdate()
        {
            if (OnDataUpdate != null)
                OnDataUpdate();
            else
            {
                Logger.Log("[ERROR] Storage.OnDataUpdate() null!");
            }
        }

        public void SetCompletedTasks()
        {
            _completedTasks++;
        }

        public void SetSkippedTasks()
        {
            _skippedTasks++;
        }

        private void LoadLocal()
        {

        }

        private void SaveLocal()
        {

        }
    }
}
