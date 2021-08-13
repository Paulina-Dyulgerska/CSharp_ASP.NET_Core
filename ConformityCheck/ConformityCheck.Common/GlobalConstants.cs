﻿namespace ConformityCheck.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Conformity Check";

        public const string SystemEmail = "admin@dotnetweb.net";

        public const string AdministratorRoleName = "Administrator";

        public const string JsonContentType = "application/json";

        public const string JwtCookieName = "ConformityCheck.AspNetCore.Identity.Application";

        public const string TempDataMessagePropertyName = "Message";
        public const string TempDataErrorMessagePropertyName = "ErrorMessage";

        public const string ArticleCreatedSuccessfullyMessage = "Article is successfully created.";
        public const string ArticleEditedSuccessfullyMessage = "Article is successfully edited.";
        public const string ArticleDeletedSuccessfullyMessage = "Article is successfully deleted.";
        public const string ArticleInvalidId = "No article with such id found.";

        public const string SupplierCreatedSuccessfullyMessage = "Supplier is successfully created.";
        public const string SupplierEditedSuccessfullyMessage = "Supplier is successfully edited.";
        public const string SupplierDeletedSuccessfullyMessage = "Supplier is successfully deleted.";

        public const string ConformityTypeCreatedSuccessfullyMessage = "Conformity type is successfully created.";
        public const string ConformityTypeEditedSuccessfullyMessage = "Conformity type is successfully edited.";
        public const string ConformityTypeDeletedSuccessfullyMessage = "Conformity type is successfully deleted.";

        public const string ConformityCreatedSuccessfullyMessage = "Conformity is successfully created.";
        public const string ConformityEditedSuccessfullyMessage = "Conformity is successfully edited.";
        public const string ConformityDeletedSuccessfullyMessage = "Conformity is successfully deleted.";

        public const string RequestSentSuccessfullyMessage = "Request is successfully send.";

        public const string IdSortParam = "id";
        public const string IdSortParamDesc = "idDesc";
        public const string NumberSortParam = "number";
        public const string NumberSortParamDesc = "numberDesc";
        public const string NameSortParam = "name";
        public const string NameSortParamDesc = "nameDesc";
        public const string DescriptionSortParam = "description";
        public const string DescriptionSortParamDesc = "descriptionDesc";
        public const string ConformityTypeSortParam = "conformityType";
        public const string ConformityTypeSortParamDesc = "conformityTypeDesc";
        public const string HasConformitySortParam = "hasConformity";
        public const string HasConformitySortParamDesc = "hasConformityDesc";
        public const string AcceptedConformitySortParam = "acceptedConformity";
        public const string AcceptedConformitySortParamDesc = "acceptedConformityDesc";
        public const string ValidConformitySortParam = "validConformity";
        public const string ValidConformitySortParamDesc = "validConformityDesc";
        public const string CreatedOnSortParam = "createdOn";
        public const string CreatedOnSortParamDesc = "createdOnDesc";
        public const string ModifiedOnSortParam = "modifiedOn";
        public const string ModifiedOnSortParamDesc = "modifiedOnDesc";
        public const string ArticlesCountSortParam = "articleCount";
        public const string ArticlesCountSortParamDesc = "articleCountDesc";
        public const string UserEmailSortParam = "userEmail";
        public const string UserEmailSortParamDesc = "userEmailDesc";
        public const string ArticleNumberSortParam = "articleNumber";
        public const string ArticleNumberSortParamDesc = "articleNumberDesc";
        public const string ArticleDescriptionSortParam = "articleDescription";
        public const string ArticleDescriptionSortParamDesc = "articleDescriptionDesc";
        public const string SupplierNumberSortParam = "supplierNumber";
        public const string SupplierNumberSortParamDesc = "supplierNumberDesc";
        public const string SupplierNameSortParam = "supplierName";
        public const string SupplierNameSortParamDesc = "supplierNameDesc";
        public const string ConformityTypeDescriptionSortParam = "conformityTypeDescriptionSortParam";
        public const string ConformityTypeDescriptionSortParamDesc = "conformityTypeDescriptionSortParamDesc";
        public const string IsAcceptedSortParam = "isAssepted";
        public const string IsAcceptedSortParamDesc = "isAsseptedDesc";
        public const string IsValidSortParam = "isValid";
        public const string IsValidSortParamDesc = "isValidDesc";
        public const string MainSupplierNameSortParam = "mainSupplierName";
        public const string MainSupplierNameSortParamDesc = "mainSupplierNameDesc";
        public const string MainSupplierNumberSortParam = "mainSupplierNumber";
        public const string MainSupplierNumberSortParamDesc = "mainSupplierNumberDesc";
        public const string MainSupplierAllConfirmedSortParam = "mainSupplierAllConfirmed";
        public const string MainSupplierAllConfirmedSortParamDesc = "mainSupplierAllConfirmedDesc";
        public const string AllSuppliersAllConfirmedSortParam = "allSuppliersAllConfirmed";
        public const string AllSuppliersAllConfirmedSortParamDesc = "allSuppliersAllConfirmedDesc";

        public const string CurrentSortDirectionAsc = "sortAsc";
        public const string CurrentSortDirectionDesc = "sortDesc";

        public const string QueryArticlesOrderedByConfirmedByMainSupplier = @"SELECT F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
	  ,F.ConformityTypeCount AS ConformityTypeCount
	  -- taka hvashtam vsichki iztriti i dobaveni nanovo ConformityTypes:
	  --,COUNT(*) AS ConformityTypesCountAll
	  -- taka hvashtam unikalni ConformityTypes:
	  --,COUNT(DISTINCT F.ConfTypeId) AS ConformityTypesCountDictinct
	  --,COUNT(F.IsValid) as IsValid
	  ,CASE
		WHEN (F.ConformityTypeCount = COUNT(F.IsValid)) THEN 1
		ELSE NULL
		END AS IsConfirmed
FROM 
( SELECT A.[Id]
    ,A.[CreatedOn]
    ,A.[ModifiedOn]
    ,A.[IsDeleted]
    ,A.[DeletedOn]
    ,A.[Number]
    ,A.[UserId]
    ,A.[Description]
	,CT.[Description] AS ConfTypeDescription
	,CT.[Id] AS ConfTypeId
	,ASUP.SupplierId
	,ASUP.IsMainSupplier
	,CONF.IsAccepted
	,CONF.ValidityDate
	,CONF.IsDeleted AS ConformityIsDeleted
	,CASE
		WHEN (IsAccepted = 1 AND GETDATE() <= ValidityDate AND 
		 (CONF.IsDeleted = 0 OR CONF.IsDeleted IS NULL) ) THEN 1
		ELSE NULL
		END AS IsValid
	,CTN.ConformityTypeCount AS ConformityTypeCount
FROM 
	Articles AS A
	LEFT JOIN ArticleConformityTypes AS ACT ON A.Id = ACT.ArticleId
	LEFT JOIN ConformityTypes AS CT ON ACT.ConformityTypeId = CT.Id
	LEFT JOIN ArticleSuppliers AS ASUP ON ASUP.ArticleId = A.Id
	LEFT JOIN Conformities AS CONF ON 
				(CONF.ArticleId = A.Id AND 
				CONF.SupplierId = ASUP.SupplierId AND 
				CONF.ConformityTypeId = ACT.ConformityTypeId)
	LEFT JOIN (SELECT COUNT(*) AS ConformityTypeCount, 
							ArticleId as ArticleId
						FROM ArticleConformityTypes 
						GROUP BY ArticleId) AS CTN ON CTN.ArticleId = A.Id
 --Include this for check if just MAIN SUPPLIER has all confirmed:
--WHERE A.IsDeleted = 0 AND ASUP.IsMainSupplier = 1
WHERE  ASUP.IsMainSupplier = 1
) AS F
GROUP BY F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
	  ,F.ConformityTypeCount";

        public const string QueryArticlesOrderedByConfirmedByAllSuppliers = @"SELECT F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
	  --,F.ConformityTypeCount AS ConformityTypeCount
	  -- taka hvashtam vsichki iztriti i dobaveni nanovo ConformityTypes:
	  --,COUNT(*) AS ConformityTypesCountAll
	  -- taka hvashtam unikalni ConformityTypes:
	  --,COUNT(DISTINCT F.ConfTypeId) AS ConformityTypesCountDictinct
	  --,COUNT(F.IsValid) as IsValid
	  ,CASE
		WHEN (F.ConformityTypeCount = COUNT(F.IsValid)) THEN 1
		ELSE NULL
		END AS IsConfirmed
FROM 
( SELECT A.[Id]
    ,A.[CreatedOn]
    ,A.[ModifiedOn]
    ,A.[IsDeleted]
    ,A.[DeletedOn]
    ,A.[Number]
    ,A.[UserId]
    ,A.[Description]
	,CT.[Description] AS ConfTypeDescription
	,CT.[Id] AS ConfTypeId
	,ASUP.SupplierId
	,ASUP.IsMainSupplier
	,CONF.IsAccepted
	,CONF.ValidityDate
	,CONF.IsDeleted AS ConformityIsDeleted
	,CASE
		WHEN (IsAccepted = 1 AND GETDATE() <= ValidityDate AND 
		 (CONF.IsDeleted = 0 OR CONF.IsDeleted IS NULL) ) THEN 1
		ELSE NULL
		END AS IsValid
	,CTN.ConformityTypeCount AS ConformityTypeCount
FROM 
	Articles AS A
	LEFT JOIN ArticleConformityTypes AS ACT ON A.Id = ACT.ArticleId
	LEFT JOIN ConformityTypes AS CT ON ACT.ConformityTypeId = CT.Id
	LEFT JOIN ArticleSuppliers AS ASUP ON ASUP.ArticleId = A.Id
	LEFT JOIN Conformities AS CONF ON 
				(CONF.ArticleId = A.Id AND 
				CONF.SupplierId = ASUP.SupplierId AND 
				CONF.ConformityTypeId = ACT.ConformityTypeId)
	LEFT JOIN (SELECT 
					-- COUNT(*)*CTCTE.ConformityTypeCount AS ConformityTypeCount, 
					SUM(CTCTE.ConformityTypeCount) AS ConformityTypeCount,
					Asup.ArticleId as ArticleId
				FROM ArticleSuppliers AS ASup
				JOIN (SELECT COUNT(*) AS ConformityTypeCount, 
							ArticleId as ArticleId
						FROM ArticleConformityTypes 
						GROUP BY ArticleId) AS CTCTE ON CTCTE.ArticleId = ASup.ArticleId
				GROUP BY ASup.ArticleId, CTCTE.ConformityTypeCount) AS CTN ON CTN.ArticleId = A.Id
 --Include this for check if just MAIN SUPPLIER has all confirmed:
--WHERE A.IsDeleted = 0 -- AND ASUP.IsMainSupplier = 1
--WHERE  ASUP.IsMainSupplier = 1
) AS F

	  --where F.Number = '262201601'
	  GROUP BY F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
	  ,F.ConformityTypeCount";
    }
}
