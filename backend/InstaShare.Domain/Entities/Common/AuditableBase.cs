namespace InstaShare.Domain.Entities.Common;

public abstract class AuditableBase
{
  public DateTime Created { get; set; }

  public DateTime? LastModified { get; set; }
}