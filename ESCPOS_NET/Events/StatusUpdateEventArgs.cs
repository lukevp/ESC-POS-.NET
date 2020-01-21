using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Events
{
    public class StatusUpdateEventArgs : EventArgs
    {
        public StatusUpdateEventArgs()
        {
            this.EventTimestamp = DateTime.Now;
        }

        public string Message
        {
            get;set;
        }

        public DateTime EventTimestamp
        {
            get;set;
        }

        public StatusEventType UpdateType
        {
            get;set;
        }

        public bool IsCashDrawerOpen
        {
            get; set;
        }

        public bool IsPrinterOnline
        {
            get; set;
        }

        public bool IsCoverOpen
        {
            get; set;
        }

        public bool IsPaperCurrentlyFeeding
        {
            get; set;
        }

        public bool IsWaitingForOnlineRecovery
        {
            get; set;
        }

        public bool IsPaperFeedButtonPushed
        {
            get; set;
        }

        public bool IsInErrorState => DidRecoverableErrorOccur || DidUnrecoverableErrorOccur || DidAutocutterErrorOccur || DidRecoverableNonAutocutterErrorOccur || IsPaperOut;

        public bool DidRecoverableNonAutocutterErrorOccur
        {
            get; set;
        }

        public bool DidAutocutterErrorOccur
        {
            get; set;
        }

        public bool DidUnrecoverableErrorOccur
        {
            get; set;
        }

        public bool DidRecoverableErrorOccur
        {
            get; set;
        }

        public bool IsPaperLow
        {
            get; set;
        }
        public bool IsPaperOut
        {
            get; set;
        }
    }
}
