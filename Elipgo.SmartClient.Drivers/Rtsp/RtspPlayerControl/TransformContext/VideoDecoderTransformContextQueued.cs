using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using StreamCoders;
using StreamCoders.Decoder;

namespace MediaSuite.Common.TransformContext
{
    [ClassInterface(ClassInterfaceType.None)]
    public class VideoDecoderTransformContextQueued : TransformContext<IVideoDecoderBase>, IDisposable
    {
        protected readonly ManualResetEvent          InitializationEvent    = new ManualResetEvent(false);
        private readonly   TimeSpan                  initializationWaitTime = TimeSpan.FromSeconds(4000);
        protected          VideoDecoderConfiguration Configuration;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="instance">Instance of video decoder to use</param>
        /// <param name="cancellationTokenSource">
        ///     An optional cancellation token to unblock queue operations. If set to null, then
        ///     an internal one will be created.
        /// </param>
        protected VideoDecoderTransformContextQueued(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource = null) : base(cancellationTokenSource)
        {
            InnerTransform = instance;
        }

        public BlockingCollection<MediaBuffer<byte>> OutputQueue => InnerTransform.OutputQueue;

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            TransformThread.Join();

            InitializationEvent?.Dispose();
            (InnerTransform as IVideoDecoderBase)?.Dispose();
        }

        ~VideoDecoderTransformContextQueued()
        {
        }

        /// <summary>
        ///     Creates a TransformContext from an already existing instance. If the default constructor is used, then the instance
        ///     is newly created.
        /// </summary>
        /// <param name="instance">Instance of an IVideoDecoderBase object.</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public static VideoDecoderTransformContextQueued FromInstance(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource)
        {
            return new VideoDecoderTransformContextQueued(instance, cancellationTokenSource);
        }

        protected override CodecOperationStatus InitializeTransform()
        {
            var t = InnerTransform as IVideoDecoderBase;
            Configuration = t.Init(Configuration);
            InitializationEvent.Set();
            return Configuration.InitializationStatus;
        }

        public VideoDecoderConfiguration Init(VideoDecoderConfiguration configuration)
        {
            Configuration = configuration;

            TransformThread = new Thread(TransformThreadProc)
            {
                IsBackground = true,
                Name         = "VideoDecoderTransformContext"
            };
            TransformThread.Start();

            if (InitializationEvent.WaitOne(initializationWaitTime) == false)
            {
                throw new Exception("Initialization signal timed out.");
            }

            return configuration;
        }

        protected override void TransformThreadProc()
        {
            if (InitializeTransform() != CodecOperationStatus.Success)
            {
                return;
            }

            while (CancellationTokenSource.Token.IsCancellationRequested == false)
            {
                try
                {
                    var contextResult = InputQueue.Take(CancellationTokenSource.Token);
                    if (TransformInternal(contextResult))
                    {
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        protected bool TransformInternal(TransformContextResult contextResult)
        {
            contextResult.CodecOperationStatus = InnerTransform.Transform(contextResult.Buffer);
            return contextResult.CodecOperationStatus != CodecOperationStatus.Success && contextResult.CodecOperationStatus != CodecOperationStatus.Deferred && InnerTransform.OutputQueue.Count == 0;
        }
    }
}