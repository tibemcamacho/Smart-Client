using StreamCoders;
using StreamCoders.Decoder;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MediaSuite.Common.TransformContext
{
    [ClassInterface(ClassInterfaceType.None)]
    public class Mpeg42VideoDecoderTransformContextQueued : VideoDecoderTransformContextQueued
    {
        private readonly bool allowLateInitialization;
        private bool isInitialized;

        private Mpeg42VideoDecoderTransformContextQueued(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource, bool allowLateInitialization) : base(instance,
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
        public static Mpeg42VideoDecoderTransformContextQueued FromInstance(IVideoDecoderBase instance, CancellationTokenSource cancellationTokenSource = null, bool allowLateInitialization = true)
        {
            return new Mpeg42VideoDecoderTransformContextQueued(instance, cancellationTokenSource, allowLateInitialization);
        }

        protected override void TransformThreadProc()
        {
            CodecOperationStatus initResult = InitializeTransform();
            if (initResult != CodecOperationStatus.Success && allowLateInitialization == false)
            {
                return;
            }

            isInitialized = initResult == CodecOperationStatus.Success;

            while (CancellationTokenSource.Token.IsCancellationRequested == false)
            {
                try
                {
                    TransformContextResult contextResult = InputQueue.Take(CancellationTokenSource.Token);

                    if (!isInitialized)
                    {
                        // Since the decoder is not yet initialized and late initialization is enabled
                        // we try to discover the parameter sets necessary for initialization.

                        bool videoObjectSequenceFound = false;
                        for (int i = 0; i < contextResult.Buffer.Buffer.Count - 3; i++)
                        {
                            if (contextResult.Buffer.Buffer[i] == 0x0 && contextResult.Buffer.Buffer[i + 1] == 0x0 && contextResult.Buffer.Buffer[i + 2] == 0x01 && contextResult.Buffer.Buffer[i + 3] == 0xb0)
                            {
                                videoObjectSequenceFound = true;
                                break;
                            }
                        }

                        if (videoObjectSequenceFound == false)
                        {
                            continue;
                        }

                        IVideoDecoderBase decoder = InnerTransform as IVideoDecoderBase;

                        // We can start initializing the decoder
                        Configuration.DecoderSpecificData = new DecoderSpecificData(contextResult.Buffer.Buffer);
                        Configuration = decoder.Init(Configuration);
                        if (Configuration.InitializationStatus == CodecOperationStatus.Success)
                        {
                            isInitialized = true;
                            // Also feed this frame to the decoder, as it may also contain visual information.
                            decoder.Transform(contextResult.Buffer);
                        }
                        //}

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