using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace ReasonFramework.Common
{
    /// <summary>
    /// Class for work with net
    /// </summary>
    public class Network
    {
        private const string SERVER_PATCH = "http://somehost.ru/engine.php";

        public Network()
        {
        }

        public void SendRequest()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadDataAsync(new Uri("http://www.google.com/search?&num=5&q=devby"));
            webClient.DownloadDataCompleted += OnDownload;
            
        }

        private void OnDownload(object sender, DownloadDataCompletedEventArgs e)
        {
            Logger.Log("resp:" + Encoding.ASCII.GetString(e.Result));
        }
    }
}