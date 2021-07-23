using System;

namespace ESCPOS_NET
{
    public class PrinterStatusEventArgs : EventArgs
    {
        public bool? IsWaitingForOnlineRecovery { get; set; }

        public bool? IsPaperCurrentlyFeeding { get; set; }

        public bool? IsPaperFeedButtonPushed { get; set; }

        public bool? IsPrinterOnline { get; set; }

        public bool? IsCashDrawerOpen { get; set; }

        public bool? IsCoverOpen { get; set; }

        public bool? IsPaperLow { get; set; }

        public bool? IsPaperOut { get; set; }

        public bool? IsInErrorState => (DidRecoverableErrorOccur ?? false) || (DidUnrecoverableErrorOccur ?? false) || (DidAutocutterErrorOccur ?? false) || (DidRecoverableNonAutocutterErrorOccur ?? false);

        public bool? DidRecoverableErrorOccur { get; set; }

        public bool? DidUnrecoverableErrorOccur { get; set; }

        public bool? DidAutocutterErrorOccur { get; set; }

        public bool? DidRecoverableNonAutocutterErrorOccur { get; set; }

    }
}