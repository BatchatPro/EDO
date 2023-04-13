using NullGuard;

namespace EDO.Domain;

public class BaseEntity
{
    public int Id { get; set; }
    [AllowNull]
    public DateTime? CreateDate { get; set; } = DateTime.Now;
    [AllowNull]
    public DateTime? UpdateDate { get; set; }
    [AllowNull]
    public string? CreatedBy { get; set; }
    [AllowNull]
    public string? UpdatedBy { get; set;}
}