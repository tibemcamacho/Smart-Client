using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using StreamCoders;

namespace MediaSuite.Common.TransformContext
{
    [ComVisible(false)]
    public abstract class TransformContext<T> where T : ITransform, IDisposable
    {
        protected readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        protected readonly BlockingCollection<TransformContextResult> InputQueue = new BlockingCollection<TransformContextResult>();
        protected          ITransform                                 InnerTransform;

        private   long   sequenceNumber;
        protected Thread TransformThread;

        protected TransformContext(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
        }

        public CodecOperationStatus Transform(MediaBuffer<byte> inputBuffer)
        {
            var cResult = new TransformContextResult(++sequenceNumber, CodecOperationStatus.Deferred, inputBuffer);
            InputQueue.Add(cResult, CancellationTokenSource.Token);
            return cResult.CodecOperationStatus;
        }

        private void StopThread()
        {
            CancellationTokenSource.Cancel();
        }

        protected abstract CodecOperationStatus InitializeTransform();

        protected abstract void TransformThreadProc();
    }
}