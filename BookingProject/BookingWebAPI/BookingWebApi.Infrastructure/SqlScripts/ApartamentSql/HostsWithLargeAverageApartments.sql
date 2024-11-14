SELECT "HostId", COUNT(*) AS "ApartamentCount",
AVG("Area") AS "AverageArea"
FROM "Apartaments"
GROUP BY "HostId"
HAVING AVG("Area") > 50;
