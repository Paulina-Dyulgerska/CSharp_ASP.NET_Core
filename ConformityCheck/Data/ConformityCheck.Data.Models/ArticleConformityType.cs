﻿namespace ConformityCheck.Data.Models
{
    using System;

    using ConformityCheck.Data.Common.Models;

    public class ArticleConformityType : BaseModel<int>
    {
        public DateTime? RequestDate { get; set; }

        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int ConformityTypeId { get; set; }

        public virtual ConformityType ConformityType { get; set; }
    }
}
