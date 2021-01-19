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
	  ,COUNT(*) AS ConformityTypesCount
	  ,COUNT(F.IsValid) as IsValid
	  ,CASE
		WHEN (COUNT(*) = COUNT(F.IsValid)) THEN 1
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
	,ASUP.SupplierId
	,ASUP.IsMainSupplier
	,CONF.IsAccepted
	,CONF.ValidityDate
	,CONF.IsDeleted AS ConformityIsDeleted
	,CASE
		WHEN (IsAccepted = 1 AND ValidityDate >= GETDATE() AND CONF.IsDeleted=0) THEN 1
		ELSE NULL
	END AS IsValid
FROM Articles AS A
  LEFT JOIN ArticleConformityTypes AS ACT ON A.Id = ACT.ArticleId
  LEFT JOIN ConformityTypes AS CT ON ACT.ConformityTypeId = CT.Id
  LEFT JOIN ArticleSuppliers AS ASUP ON ASUP.ArticleId = A.Id
  LEFT JOIN Conformities AS CONF ON 
				(CONF.ArticleId = A.Id AND 
				CONF.SupplierId = ASUP.SupplierId AND 
				CONF.ConformityTypeId = ACT.ConformityTypeId)
 --Include this for check if the conformity is not deleted:
 --WHERE CONF.IsDeleted = 0
 --Include this for check if just MAIN SUPPLIER has all confirmed:
 WHERE ASUP.IsMainSupplier = 1
 --WHERE A.Id = 'd9ef0ba6-f12b-4731-8064-8e0c4cea796e'
) AS F
GROUP BY F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
--Take only the supplier/s that has/ve confirmed:
--HAVING COUNT(*) = COUNT(F.IsValid)";

        public const string QueryArticlesOrderedByConfirmedByAllSuppliers = @"SELECT F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
	  ,COUNT(*) AS ConformityTypesCount
	  ,COUNT(F.IsValid) as IsValid
	  ,CASE
		WHEN (COUNT(*) = COUNT(F.IsValid)) THEN 1
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
	,ASUP.SupplierId
	,ASUP.IsMainSupplier
	,CONF.IsAccepted
	,CONF.ValidityDate
	,CONF.IsDeleted AS ConformityIsDeleted
	,CASE
		WHEN (IsAccepted = 1 AND ValidityDate >= GETDATE() AND CONF.IsDeleted=0) THEN 1
		ELSE NULL
	END AS IsValid
FROM Articles AS A
  LEFT JOIN ArticleConformityTypes AS ACT ON A.Id = ACT.ArticleId
  LEFT JOIN ConformityTypes AS CT ON ACT.ConformityTypeId = CT.Id
  LEFT JOIN ArticleSuppliers AS ASUP ON ASUP.ArticleId = A.Id
  LEFT JOIN Conformities AS CONF ON 
				(CONF.ArticleId = A.Id AND 
				CONF.SupplierId = ASUP.SupplierId AND 
				CONF.ConformityTypeId = ACT.ConformityTypeId)
 --Include this for check if the conformity is not deleted:
 --WHERE CONF.IsDeleted = 0
 --Include this for check if just MAIN SUPPLIER has all confirmed:
 --WHERE ASUP.IsMainSupplier = 1
 --WHERE A.Id = 'd9ef0ba6-f12b-4731-8064-8e0c4cea796e'
) AS F
GROUP BY F.[Id]
      ,F.[CreatedOn]
      ,F.[ModifiedOn]
      ,F.[IsDeleted]
      ,F.[DeletedOn]
      ,F.[Number]
      ,F.[Description]
      ,F.[UserId]
--Take only the supplier/s that has/ve confirmed:
--HAVING COUNT(*) = COUNT(F.IsValid)";
    }
}
