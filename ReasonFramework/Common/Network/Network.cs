using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
    /// <summary>
    /// Class for work with net
    /// </summary>
    public class Network
    {
        private const string SERVER_PATCH = "http://somehost.ru/engine.php";
        //private List<WebClient> _webClients;


        public Network()
        {
            //_webClients = new List<WebClient>();
        }

        #region Down level work
        /// <summary>
        /// Отправляет запрос на сервер
        /// </summary>
        /// <param name="request"></param>
        private void SendRequest(string request)
        {
            try
            {
                var webClient = new WebClient();
                webClient.DownloadDataAsync(new Uri(string.Concat(SERVER_PATCH, "?", request)));
                webClient.DownloadDataCompleted += OnDownloadedData;
            }
            catch (Exception ex)
            {
                Logger.Log("SendRequest ERROR:" + ex);
            }
            
        }
        /// <summary>
        /// Вызывается при получении данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownloadedData(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                ParseInputData(Encoding.ASCII.GetString(e.Result));
            }
            catch (Exception ex)
            {
                Logger.Log("OnDownloadedData ERROR:" + ex);
            }
        }
        /// <summary>
        /// Парсит данные от сервера
        /// </summary>
        /// <param name="data"></param>
        private void ParseInputData(string data)
        {
            Logger.Log("input data:{0}", data);
        }
        #endregion
        #region up level

        #endregion
    }
}