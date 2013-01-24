using System;
using System.Collections.Generic;

namespace ReasonFramework.Common
{
    /// <summary>
    /// Задание
    /// </summary>
    public class GameTask
    {
        private string _taskText;//текст таска
        public string Text { get { return _taskText; } }

        private double _taskRanking;//общий рейтинг таска
        public string Ranking { get { return _taskRanking.ToString(); } }

        private byte _userRanking;//оценка пользователя
        public string UserRanking { get { return _userRanking.ToString(); } }

        private List<Comment> _taskComments; //коменты таска
        public List<Comment> Comments { get { return _taskComments; } }

        public GameTask(string text, double rank, List<Comment> comments = null, byte userRank = 0)
        {
            _taskText = text;
            _taskRanking = rank;
            _taskComments = comments == null ? new List<Comment>() : comments;
            _userRanking = userRank;
        }

        public void SetUserRank(byte rank)
        {
            _userRanking = rank;
        }
    }
}
