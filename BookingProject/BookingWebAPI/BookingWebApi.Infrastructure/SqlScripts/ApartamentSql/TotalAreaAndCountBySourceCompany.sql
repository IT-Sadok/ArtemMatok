SELECT "SourceCompanyId",
SUM("Area") AS "TotalArea",
COUNT(*) AS "ApartamentCount"
FROM "Apartaments"
GROUP BY "SourceCompanyId"

