﻿using EDO.Domain;
using NullGuard;

namespace EDO.Database.Models;

public class Document:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string Status { get; set; }
    public int DocumentTypeId { get; set; }
    public string AuthorUserName { get; set; }
    [AllowNull]
    public DocumentType? DocumentType { get; set; }
}
