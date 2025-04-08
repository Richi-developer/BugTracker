namespace BugTracker.Dto
{
    /// <summary>
    /// Данные для создания бага
    /// </summary>
    public class BugDto
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }


    }
}