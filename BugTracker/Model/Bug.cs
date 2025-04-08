namespace BugTracker.Model
{
    public class Bug
    {
        /// <summary>
        /// Идентификатор бага
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string? Author { get; set; }

    }
}
