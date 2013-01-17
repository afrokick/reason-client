using System;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
    /// <summary>
    /// This class storage all game data
    /// </summary>
    class Storage
    {
        #region Fields
        private Dictionary<string, string> _dicUids;
        private int _complatedTasks;
        private int _skippedTasks;
        #endregion

        public Storage()
        {
            _dicUids = new Dictionary<string, string>();
        }

        private void Initialize()
        {

        }
    }
}
