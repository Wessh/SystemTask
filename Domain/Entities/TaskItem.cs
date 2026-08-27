using Domain.Enums;

namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; init; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public StatusTask Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected TaskItem() { } // EF Core

    public TaskItem(string title, string description, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Status = StatusTask.Pending;
        CreatedAt = DateTime.UtcNow;
        DueDate = ValidateDueDate(dueDate);
    }

    #region Domain Rules

    public void ChangeStatus(StatusTask status)
    {
        if (Status == StatusTask.Completed || Status == StatusTask.Cancelled)
            throw new InvalidOperationException("Task cannot change status");

        Status = status;
    }

    public void StartTask()
    {
        if (Status != StatusTask.Pending && Status != StatusTask.OnHold)
            throw new InvalidOperationException("Task cannot be started");
        Status = StatusTask.InProgress;
    }

    public void OnHoldTask()
    {
        if (Status != StatusTask.InProgress)
            throw new InvalidOperationException("Task cannot on hold"); 
        Status = StatusTask.OnHold;
    }

    public void CompleteTask()
    {
        if (Status != StatusTask.InProgress)
            throw new InvalidOperationException("Task status cannot be completed");

        Status = StatusTask.Completed;
    }

    public void CancelTask()
    {
        if (Status == StatusTask.Completed)
            throw new InvalidOperationException("Completed task cannot be cancelled");

        Status = StatusTask.Cancelled;
    }

    private DateTime ValidateDueDate(DateTime dueDate)
    {
        if (dueDate < CreatedAt)
            throw new ArgumentException("Due date cannot be in the past");

        return dueDate;

    }

    #endregion


}
