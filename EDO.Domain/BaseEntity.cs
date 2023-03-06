using NullGuard;

namespace EDO.Domain;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime? CreateDate { get; set; } = DateTime.Now;
    [AllowNull]
    public DateTime? UpdateDate { get; set; }
}