-- OPTIONAL: apne database ka naam yahan likho
-- USE RewardSystemDb;
-- GO

/* TOP 3 employees jinke sabse zyada reward points earn hue hain */

SELECT TOP (3)
    u.Id          AS UserId,
    u.Name        AS EmployeeName,
    u.Email       AS Email,
    ua.Points     AS CurrentPointsBalance,
    ISNULL(SUM(rt.PointsGranted), 0) AS TotalPointsEarned
FROM Users u
LEFT JOIN UserAccounts ua
    ON ua.UserId = u.Id
LEFT JOIN RewardTransactions rt
    ON rt.UserId = u.Id
GROUP BY
    u.Id,
    u.Name,
    u.Email,
    ua.Points
ORDER BY
    TotalPointsEarned DESC;

