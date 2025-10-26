using System;

namespace MyClassLibrary
{
    /// <summary>
    /// attribute для добавления комментария к типам
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommentAttribute : Attribute
    {
        /// <summary>
        /// текст комментария
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// создаёт атрибут с комментарием
        /// </summary>
        /// <param name="comment">текст комментария</param>
        public CommentAttribute(string comment) => Comment = comment;
    }
}
