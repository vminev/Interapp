﻿namespace Interapp.Web.Areas.Student.Models.DocumentsViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Common.Constants;
    using Data.Models;
    using Infrastructure.Mapping;

    public class DocumentViewModel : IMapFrom<Document>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ModelConstants.DocumentNameMinLength)]
        [MaxLength(ModelConstants.DocumentNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        public UniversityViewModel University { get; set; }

        public int? UniversityId { get; set; }

        public string AuthorId { get; set; }
    }
}