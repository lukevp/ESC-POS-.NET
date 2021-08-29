namespace ESCPOS_NET
{
    public class PrinterOptions
    {
        int IdleTimeoutSeconds { get; set; } = 60;
        int StatusPollingIntervalMilliseconds { get; set; }  = 500;
    }
}