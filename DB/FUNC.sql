CREATE OR ALTER FUNCTION dbo.fn_CalculateParkingCost 
(
    @TimeIn SMALLDATETIME, 
    @TimeOut SMALLDATETIME
)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @DurationMinutes INT;
    DECLARE @DurationHours INT;
    DECLARE @HourlyRate DECIMAL(10, 2);
    DECLARE @TotalCost DECIMAL(10, 2);

    SET @DurationMinutes = DATEDIFF(MINUTE, @TimeIn, @TimeOut);

    IF @DurationMinutes <= 0 SET @DurationMinutes = 1; 
    SET @DurationHours = CEILING(@DurationMinutes / 60.0);

    SELECT @HourlyRate = Стоимость 
    FROM Тариф 
    WHERE Продолжительность_часов = 1;

    SET @HourlyRate = ISNULL(@HourlyRate, 100.00);

    SET @TotalCost = @DurationHours * @HourlyRate;

    RETURN @TotalCost;
END;

