using System;

namespace MediaSuite.Common.TransformContext
{
    public class TransformContextResultCompleteEventArgs : EventArgs
    {
        public TransformContextResultCompleteEventArgs(TransformContextResult contextResult)
        {
            Result = contextResult;
        }

        public TransformContextResult Result { get; }
    }
}