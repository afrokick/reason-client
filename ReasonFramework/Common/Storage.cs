using System;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
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
        private int _skippedTasks;

        private Task _currentTask;
        public Task CurrentTask { get { return _currentTask; } }

        private Network _net;
        #endregion

        public Storage(Network net)
        {
            _net = net;
            _dicUids = new Dictionary<string, string>();

            _net.OnAuthComplated += (k) =>
            {
                if (!k.IsError)
                {
                    _id = int.Parse(k.GetValue("id"));
                    _name = k.GetValue("name");
                    //laod statistics
                }
            };
        }
    }
}
