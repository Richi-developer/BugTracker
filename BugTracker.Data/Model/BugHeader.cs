﻿namespace BugTracker.Data.Model;

public class BugHeader
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
    /// Автор
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Статус бага
    /// </summary>
    public string Status { get; set; } = BugStatuses.New;
}