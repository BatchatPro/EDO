using EDO.Database.Models.AccessReferences;
using EDO.Domain;

namespace EDO.Database.Models;

public class DocumentUser: BaseEntity
{
    public string UserId { get; set; }
    public int DocumentId { get; set; }
    public ApplicationUserReference User { get; set; }
    public Document Document { get; set; }
}
