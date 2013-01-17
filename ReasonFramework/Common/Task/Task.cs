using System;

namespace ReasonFramework.Common
{
    /// <summary>
    /// Типы тасков
    /// </summary>
    public enum TaskTypes
    {
        NONE = 0,
        PHYSIC,
        MENTAL,
        SEX
    }
    /// <summary>
    /// Задание
    /// </summary>
    public class Task
    {
        private string _taskText;
        private TaskTypes _taskType;
        private byte _taskRanking;
        private object _taskComments;
    }
}
