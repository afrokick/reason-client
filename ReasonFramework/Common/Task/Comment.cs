using System;

namespace ReasonFramework.Common
{
    public enum CommentMark
    {
        NONE = 0,
        SPAM,
        DISLIKE,
        LIKE
    }

    public class Comment
    {
        private string _text;
        public string Text { get { return _text; } }
        private string _authName;
        public string AuthName { get { return _authName; } }
        private DateTime _date;
        public DateTime Date { get { return _date; } }
        private byte _ranking;
        public byte Ranking { get { return _ranking; } }
        private int _like;
        private int _dislike;
        private int _spam;
        private CommentMark _mark;
        public CommentMark Mark { get { return _mark; } }

        public bool CanVoted
        {
            get { return _mark != CommentMark.NONE; }
        }

        public Comment(string text, string authName, DateTime date, byte ranking, int like, int dislike, int spam, CommentMark mark)
        {
            _text = text;
            _authName = authName;
            _date = date;
            _ranking = ranking;
            _like = like;
            _dislike = dislike;
            _spam = spam;
            _mark = mark;
        }

        /// <summary>
        /// Проголосовать за комментарий
        /// </summary>
        /// <param name="mark">Оценка</param>
        public void AddMark(CommentMark mark)
        {
            if (!CanVoted)
            {
                //уже проголосовал
            }
            else
            {
                switch (mark)
                {
                    case CommentMark.LIKE:
                        _like++;
                        break;
                    case CommentMark.DISLIKE:
                        _dislike++;
                        break;
                    case CommentMark.SPAM:
                        _spam++;
                        break;
                }

                _mark = mark;
            }
        }
    }
}
