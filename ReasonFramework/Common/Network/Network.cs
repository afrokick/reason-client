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
    public class Network
    {
        private const string SERVER_PATCH = "http://devby.ru/server/engine.php";
        //private List<WebClient> _webClients;
        //private Storage _storage;
        public event ResponseEventHandler OnAuthComplated;
        public event ResponseEventHandler OnSendTaskComplated;

        public Network()
        {
            //_webClients = new List<WebClient>();
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

            if (response.IsError)
            {
                Logger.Log("[Server Error]{0}", response.GetError);
            }

            switch(method)
            {
                case PacketTypes.auth:
                    if (OnAuthComplated != null)
                        OnAuthComplated(response);
                    break;
                case PacketTypes.sendtask:
                    if (OnSendTaskComplated != null)
                        OnSendTaskComplated(response);
                    break;
                default:
                    Logger.Log("Error parse: Packet haven't method!");
                    break;
            }
        }

        #endregion
        #region up level
        public void SendComplateTask(string comment)
        {

        }
        /// <summary>
        /// Пользователь выполнил или закрыл таск
        /// </summary>
        /// <param name="complated"></param>
        /// <param name="rank"></param>
        /// <param name="comment"></param>
        public void SendTaskDone(bool complated, byte rank = 0, string comment = "")
        {
            var request = new NetRequest(PacketTypes.sendtask);
            request.AddParam("complated", complated);
            if (complated)
            {
                if (rank != 0)
                    request.AddParam("rank", rank);
                if (!string.IsNullOrEmpty(comment.Trim()))
                    request.AddParam("comment", comment);
            }
            SendRequest(request);
        }

        public void SendLike()
        {

        }

        /// <summary>
        /// Посылаем при нажатии кнопки "Вход через..." или "Вход" для стендэлон
        /// </summary>
        /// <param name="loginType"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        public void SendAuth(LoginTypeEnum loginType, string login = "", string pass = "")
        {
            var request = new NetRequest(PacketTypes.auth);
            request.AddParam("auth_type",loginType.ToString());
            switch (loginType)
            {
                case LoginTypeEnum.stand_alone:
                    request.AddParam("login", login);
                    request.AddParam("pass", pass);
                    break;
                case LoginTypeEnum.vk:
                    break;
                default:
                    break;
            }

            SendRequest(request);
        }
        #endregion

        private void ResetAllCallback()
        {
            OnAuthComplated = null;
        }
    }
}