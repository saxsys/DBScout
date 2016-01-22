using System;

namespace DataDictionary.Services.Tools
{
    public class NotifyFileSavedEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public string Message { get; set; }
    }
}
