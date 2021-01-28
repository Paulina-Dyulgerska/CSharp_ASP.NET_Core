namespace ConformityCheck.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "ConformityCheck";

        public const string AdministratorRoleName = "Administrator";

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
	  ,COUNT(*) AS ConformityTypesCountAll
	  -- taka hvashtam unikalni ConformityTypes:
	  ,COUNT(DISTINCT F.ConfTypeId) AS ConformityTypesCountDictinct
	  ,COUNT(F.IsValid) as IsValid
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
	  ,F.ConformityTypeCount AS ConformityTypeCount
	  -- taka hvashtam vsichki iztriti i dobaveni nanovo ConformityTypes:
	  ,COUNT(*) AS ConformityTypesCountAll
	  -- taka hvashtam unikalni ConformityTypes:
	  ,COUNT(DISTINCT F.ConfTypeId) AS ConformityTypesCountDictinct
	  ,COUNT(F.IsValid) as IsValid
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
	LEFT JOIN (SELECT COUNT(*)*CTCTE.ConformityTypeCount AS ConformityTypeCount, 
					Asup.ArticleId as ArticleId
				FROM ArticleSuppliers AS ASup
				JOIN (SELECT COUNT(*) AS ConformityTypeCount, 
							ArticleId as ArticleId
						FROM ArticleConformityTypes 
						GROUP BY ArticleId) AS CTCTE ON CTCTE.ArticleId = ASup.ArticleId
				GROUP BY ASup.ArticleId, CTCTE.ConformityTypeCount) AS CTN ON CTN.ArticleId = A.Id
 --Include this for check if just MAIN SUPPLIER has all confirmed:
--WHERE A.IsDeleted = 0 -- AND ASUP.IsMainSupplier = 1
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
    }
}
