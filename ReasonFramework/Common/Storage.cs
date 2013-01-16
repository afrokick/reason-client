using System;

namespace ReasonFramework.Common
{
    /// <summary>
    /// This class storage all game data
    /// </summary>
    class Storage
    {
        #region Fields
        private int _userId;
        public int UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion

        public Storage()
        {

        }

        private void Initialize()
        {

        }
    }
}
