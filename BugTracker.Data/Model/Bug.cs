namespace BugTracker.Data.Model
{
    public class Bug : BugHeader
    {
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
