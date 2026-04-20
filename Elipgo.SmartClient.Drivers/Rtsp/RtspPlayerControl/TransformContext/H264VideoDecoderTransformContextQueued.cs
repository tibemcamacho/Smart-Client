using System;
using System.Runtime.InteropServices;
using System.Threading;
using StreamCoders;
using StreamCoders.Container.MP4;
using StreamCoders.Decoder;

namespace MediaSuite.Common.TransformContext
{
    [ClassInterface(ClassInterfaceType.None)]
    public class H264VideoDecoderTransformContextQueued : VideoDecoderTransformContextQueued
    {
        private readonly bool allowLateInitialization;
        private bool isInitialized;

        private H264VideoDecoderTransformContextQueued(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource, bool allowLateInitialization) : base(instance,
            cancellationTokenSource)
        {
            this.allowLateInitialization = allowLateInitialization;
        }

        /// <summary>
        ///     Creates a TransformContext from an already existing instance. If the default constructor is used, then the instance
        ///     is newly created.
        /// </summary>
        /// <param name="instance">Instance of an IVideoDecoderBase object.</param>
        /// <param name="cancellationTokenSource">An optional cancellation token.</param>
        /// <param name="allowLateInitialization">
        ///     If set to true, the decoder initialization is allowed to fail if the decoder
        ///     specific data is not present. Decoding however can only start when the data is available later in the stream.
        /// </param>
        /// <returns></returns>
        public static H264VideoDecoderTransformContextQueued FromInstance(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource = null, bool allowLateInitialization = true)
        {
            return new H264VideoDecoderTransformContextQueued(instance, cancellationTokenSource, allowLateInitialization);
        }

        protected override void TransformThreadProc()
        {
            var initResult = InitializeTransform();
            if (initResult != CodecOperationStatus.Success && allowLateInitialization == false)
            {
                return;
            }

            isInitialized = initResult == CodecOperationStatus.Success;

            while (CancellationTokenSource.Token.IsCancellationRequested == false)
            {
                try
                {
                    var contextResult = InputQueue.Take(CancellationTokenSource.Token);

                    if (!isInitialized)
                    {
                        // Since the decoder is not yet initialized and late initialization is enabled
                        // we try to discover the parameter sets necessary for initialization.

                        var h264Transform = new H264Transform();
                        string sps = null, pps = null;

                        var decoder = InnerTransform as IVideoDecoderBase;
                        var auNals = h264Transform.TransformNal(contextResult.Buffer);
                        var avcc = new AVCDecoderConfigurationRecord();
                        if (auNals?.Sps != null)
                        {
                            sps = Convert.ToBase64String(auNals.Sps);
                        }

                        if (auNals?.Pps != null)
                        {
                            pps = Convert.ToBase64String(auNals.Pps);
                        }

                        if (sps != null && pps != null)
                        {
                            var spsPps = $"{sps},{pps}";
                            var parameterset = new H264ParameterSet(spsPps);
                            var metrics = parameterset.GetMetrics();

                            // We can start initializing the decoder
                            Configuration.DecoderSpecificData = new H264DecoderSpecificData(spsPps);
                            Configuration.InputWidth = metrics.PixelWidth;
                            Configuration.InputHeight = metrics.PixelHeight;

                            Configuration = decoder.Init(Configuration);

                            if (Configuration.InitializationStatus == CodecOperationStatus.Success)
                            {
                                isInitialized = true;
                                // Also feed this frame to the decoder, as it may also contain visual information.
                                decoder.Transform(contextResult.Buffer);
                            }
                        }

                        if (!isInitialized)
                        {
                            continue;
                        }
                    }

                    TransformInternal(contextResult);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        protected override CodecOperationStatus InitializeTransform()
        {
            try
            {
                return base.InitializeTransform();
            }
            catch (InvalidOperationException)
            {
                if (allowLateInitialization)
                {
                    Configuration.InitializationStatus = CodecOperationStatus.Success;
                }

                InitializationEvent.Set();
                return Configuration.InitializationStatus;
            }
        }
    }
}
