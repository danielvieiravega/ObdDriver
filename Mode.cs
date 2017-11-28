namespace TCC.ODBDriver
{
    /// <summary>
    /// List of supported modes
    /// <see cref="https://www.elmelectronics.com/wp-content/uploads/2016/07/ELM327DS.pdf"/>
    /// </summary>
    public enum Mode
    {
        CurrentData = 0x01, //Show current data
        FreezeFrameData = 0x02, //Show freeze frame data
        DiagnosticTroubleCodes = 0x03 //show diagnostic trouble codes
    }
}
