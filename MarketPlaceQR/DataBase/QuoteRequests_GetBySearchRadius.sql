ALTER proc [dbo].[QuoteRequests_GetBySearchRadius]
	@latpoint float(7),
	@lngpoint float(7),
	@radius float(2),
    @categoryidList AS dbo.CategoryList READONLY
	-- radius
	-- address / geocodes
	-- categories

AS

BEGIN

SELECT DISTINCT QuoteReqId
	,[Id]
	,[CompanyId]
	,[Name]
	,[Estimation]
	,[DueDate]
	,[QuoteRequestItemId] AS CategoryId
	,[Details]
	,[Address1]
	,[City]
	,[State]
	,[ZipCode]
	,[StatusId]
	,[LatPoint]
	,[LngPoint]
    , distance_in_mi
FROM (
    SELECT
		qr.Id AS QuoteReqId,
	    a.Id,
	    a.[CompanyId],
		qr.Name,
		qr.Estimation,
		qr.DueDate,
		qri.QuoteRequestItemId,
		qri.Name as [Details],
	    a.[Date],
		qr.StatusId,
        a.[Address1], a.City, a.State, a.ZipCode,
        [Latitude] as [LatPoint], [Longitude] as [LngPoint],
	    a.Slug,
	    a.AddressType,
        p.distance_unit
                    * DEGREES(ACOS(COS(RADIANS(p.latpoint))
                    * COS(RADIANS([Latitude]))
                    * COS(RADIANS(p.longpoint) - RADIANS([Longitude]))
                    + SIN(RADIANS(p.latpoint))
                    * SIN(RADIANS([Latitude])))) AS distance_in_mi


    FROM [dbo].[Addresses] AS a

    JOIN QuoteRequests AS qr ON qr.addressId = a.Id

    JOIN QuoteRequestItems as qri ON qri.QuoteRequestId = qr.Id

    JOIN (
        SELECT  
            @latpoint AS latpoint,  
            @lngpoint AS longpoint,
            @radius AS radius,
            69.0 AS distance_unit
    ) AS p ON 1=1

	WHERE
	(qri.QuoteRequestItemId IN (SELECT CategoryId FROM @categoryidList))
	-- Looks only for active QRs
	-- AND qr.StatusId = 1

	AND

    ([Latitude]
    BETWEEN p.latpoint  - (p.radius / p.distance_unit)
        AND p.latpoint  + (p.radius / p.distance_unit)

    AND [Longitude]
    BETWEEN p.longpoint - (p.radius / (p.distance_unit * COS(RADIANS(p.latpoint))))
        AND p.longpoint + (p.radius / (p.distance_unit * COS(RADIANS(p.latpoint))))
	/* add to skip the company itself  from the search */
	)

) AS d

WHERE distance_in_mi <= @radius
ORDER BY distance_in_mi

END