using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
    public enum LoginTypeEnum{
        stand_alone,
        vk,
        fb,
        tw
    }
    public delegate void ResponseEventHandler(NetResponse sender);
    /// <summary>
    /// Class for work with net
    /// </summary>
    public partial class Network
    {
        private const string SERVER_PATCH = "http://devby.ru/server/engine.php";
        //private List<WebClient> _webClients;
        private Storage _storage;
        public event ResponseEventHandler OnAuthCompleted;
        public event ResponseEventHandler OnSendTaskCompleted;

        public Network()
        {
            //_webClients = new List<WebClient>();
        }

        public void InitStorage(Storage storage)
        {
            _storage = storage;
        }

        #region Down level work
        /// <summary>
        /// Отправляет запрос на сервер.
        /// Прикручивает валидацию
        /// </summary>
        /// <param name="request"></param>
        private void SendRequest(NetRequest request)
        {
            try
            {
                if (_storage.Id != 0)
                    request.AddParam("userdbid", _storage.Id);
                Logger.Log("[Network] Send packet: {0} into {1}", request.GetMethod, string.Concat(SERVER_PATCH, "?", request.GetParamsString()));
                var webClient = new WebClient();
                //webClient.DownloadDataAsync(new Uri(string.Concat(SERVER_PATCH, "?", request.GetParamsString())));
                //webClient.DownloadDataCompleted += OnDownloadedData;
                ParseInputData(Encoding.ASCII.GetString(webClient.DownloadData(new Uri(string.Concat(SERVER_PATCH, "?", request.GetParamsString())))));
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
                if (e == null)
                    Logger.Log("e null");
                else if (e.Result == null)
                {
                    Logger.Log("e.Result null");
                }
                else
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
            var response = new NetResponse(data);
            var method = response.GetMethod;
            //Logger.Log("input data method:{0}", method.ToString());

            if (response.IsError)
            {
                Logger.Log("[Server Error]{0}", response.GetError);
            }

            switch(method)
            {
                case PacketTypes.auth:
                    if (OnAuthCompleted != null)
                    {
                        OnAuthCompleted(response);
                    }
                    break;
                case PacketTypes.gettask:
                case PacketTypes.sendtask:
                    if (OnSendTaskCompleted != null)
                    {
                        OnSendTaskCompleted(response);
                    }
                    break;
                default:
                    Logger.Log("Error parse: Packet haven't method!");
                    break;
            }
        }

        #endregion
        
    }
}