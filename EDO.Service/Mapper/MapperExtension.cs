﻿using EDO.Shared;
using EDO.Database.Models;

namespace EDO.Service.Mapper;

public static class MapperExtension
{
    public static DocumentDTO ConvertToDTO(this Document document)
    {
        if (document == null) return null;

        return new DocumentDTO()
        {
            Id = document.Id,
            Name = document.Name,
            Description = document.Description,
            FilePath = document.FilePath,
            Status = document.Status,
            DocumentTypeId = document.DocumentTypeId,
            CreatedBy = document.CreatedBy,
            CreateDate = document.CreateDate,
            Deadline = document.Deadline
        };
    }

    public static Document ConvertToEntity(this DocumentDTO documentDTO)
    {
        if (documentDTO == null) return null;

        return new Document()
        {
            Id = documentDTO.Id,
            Name = documentDTO.Name,
            Description = documentDTO.Description,
            FilePath = documentDTO.FilePath,
            Status = documentDTO.Status,
            DocumentTypeId = documentDTO.DocumentTypeId,
            CreatedBy = documentDTO.CreatedBy,
            CreateDate = documentDTO.CreateDate,
            Deadline = documentDTO.Deadline
        };
    }

    public static IEnumerable<DocumentDTO> ConvertToDTO(this IEnumerable<DocumentDTO> documentDTOs) =>
        documentDTOs.Select(documentDTO => new DocumentDTO()
        {
            Id = documentDTO.Id,
            Name = documentDTO.Name,
            Description = documentDTO.Description,
            FilePath = documentDTO.FilePath,
            Status = documentDTO.Status,
            DocumentTypeId = documentDTO.DocumentTypeId,
            CreatedBy = documentDTO.CreatedBy,
            CreateDate = documentDTO.CreateDate,
            Deadline = documentDTO.Deadline
        });

    public static DocumentTypeDTO ConvertToDTO(this DocumentType documentTypeDTO)
    {
        if (documentTypeDTO == null) return null;
        return new DocumentTypeDTO()
        {
            Id = documentTypeDTO.Id,
            Name = documentTypeDTO.Name,
            Description = documentTypeDTO.Description
        };
    }

    public static DocumentType ConvetToEntity(this DocumentTypeDTO documentType)
    {
        if (documentType == null) return null;

        return new DocumentType()
        {
            Id = documentType.Id,
            Name = documentType.Name,
            Description = documentType.Description,
        };
    }

    public static IEnumerable<DocumentTypeDTO> ConvertToDTO(this IEnumerable<DocumentTypeDTO> documentTypeDTOs) =>
        documentTypeDTOs.Select(documentTypeDTO => new DocumentTypeDTO()
        {
            Id = documentTypeDTO.Id,
            Name = documentTypeDTO.Name,
            Description = documentTypeDTO.Description
        });
}



