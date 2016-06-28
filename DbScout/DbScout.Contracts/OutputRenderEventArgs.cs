using System;

namespace DbScout.Contracts
{
    public class OutputRenderEventArgs : EventArgs
    {
        public IDatabaseObject DatabaseObject { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; } 
    }
}