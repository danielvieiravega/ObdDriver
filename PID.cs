namespace TCC.ODBDriver
{
    /// <summary>
    /// List of supported PIDs
    /// </summary>
    public enum PID
    {
        EngineRpm = 0x0C,
        Speed = 0x0D,
        EngineTemperature = 0x05,
        FuelPressure = 0x0A,
        ThrottlePosition = 0x11,
        IntakeAirTemperature = 0x0F,
        FuelTankLevelInput = 0x2F
    }
}
