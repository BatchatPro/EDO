using EDO.Domain;

namespace EDO.Database.Models;

public class DocumentUser: BaseEntity
{
    public Guid UserId { get; set; }
    public int DocumentId { get; set; }
}
