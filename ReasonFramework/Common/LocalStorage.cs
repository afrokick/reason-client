using System;
using System.IO;

namespace ReasonFramework.Common
{
    class LocalStorage
    {
        private static LocalStorage _instance;
        public static LocalStorage Instance

        {
            get
            {
                if (_instance == null)
                    _instance = new LocalStorage();
                return _instance;
            }
        }

        private static string _patch = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        static LocalStorage()
        {

        }

        public LocalStorage()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(
        Path.Combine(
            _patch,
            "filename.jpg"));
        }

        public void Load()
        {

        }

        public void Save()
        {

        }
    }
}
