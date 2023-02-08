﻿using NullGuard;

namespace EDO.Database.Models;

public class Document
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }

    public int DocumentTypeId { get; set; }
    [AllowNull]
    public DocumentType? DocumentType { get; set; }
}
