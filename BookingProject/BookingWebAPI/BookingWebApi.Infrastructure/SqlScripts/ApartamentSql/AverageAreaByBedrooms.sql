SELECT "Bedrooms", AVG("Area") AS "AverageArea"
FROM "Apartaments"
GROUP BY "Bedrooms"