using System;

namespace ReasonFramework.Common
{
    public partial class Network
    {
        #region up level
        /// <summary>
        /// Пользователь выполнил или закрыл таск
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="rank"></param>
        /// <param name="comment"></param>
        public void SendTaskDone(bool completed, byte rank = 0, string comment = "")
        {
            try
            {
                var request = new NetRequest(PacketTypes.sendtask);
                request.AddParam("completed", completed);
                if (completed)
                {
                    if (rank != 0)
                        request.AddParam("rank", rank);
                    if (!string.IsNullOrEmpty(comment.Trim()) && comment.Length > 0)
                        request.AddParam("comment", comment);
                    _storage.SetCompletedTasks();
                }
                else
                {
                    _storage.SetSkippedTasks();
                }
                SendRequest(request);
            }
            catch (Exception ex)
            {
                Logger.Log("error sendTaskDone:" + ex);
            }

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
            request.AddParam("auth_type", loginType.ToString());
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
        /// <summary>
        /// Получить таск
        /// </summary>
        public void GetTask()
        {
            var request = new NetRequest(PacketTypes.gettask);
            SendRequest(request);
        }
        #endregion
    }
}
