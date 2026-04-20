using ElipgoVideo.Commons.Data;
using FFmpeg.AutoGen;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ElipgoVideo.Core.Decoders
{
    public unsafe class VideoDecoder
    {

        private AVCodecContext* avCodecCtx;
        private SwsContext* swsCtx;

        public int camId;
        private int frameNumber;
        private bool isInitialized;

        private byte[] videoData;

        public VideoDecoder()
        {
        }

        public VideoDecoder(int pCamId, string codec)
        {
            camId = pCamId;
            isInitialized = false;
            // Init(codec);
        }

        public void Init(string codec, int width = 800, int height = 600)
        {
            frameNumber = 0;
            videoData = null;

            //  Register all formats and codecs
            FFmpegInvoke.av_register_all();
            FFmpegInvoke.avcodec_register_all();
            FFmpegInvoke.avformat_network_init();

            // Get the decoder
            AVCodec* pCodec;
            switch (codec)
            {
                case "MPEG4":
                    pCodec = FFmpegInvoke.avcodec_find_decoder(AVCodecID.CODEC_ID_MPEG4);
                    break;
                default:
                    pCodec = FFmpegInvoke.avcodec_find_decoder(AVCodecID.CODEC_ID_H264);
                    break;
            }

            // Get and configure the CodecContext
            avCodecCtx = FFmpegInvoke.avcodec_alloc_context3(pCodec);

            if ((pCodec->capabilities & FFmpegInvoke.CODEC_CAP_TRUNCATED) != 0)
                avCodecCtx->flags = (avCodecCtx->flags | FFmpegInvoke.CODEC_FLAG_TRUNCATED);

            avCodecCtx->pix_fmt = AVPixelFormat.PIX_FMT_YUV420P;
            avCodecCtx->codec_type = AVMediaType.AVMEDIA_TYPE_VIDEO;
            avCodecCtx->width = width; //TODO
            avCodecCtx->height = height;  //TODO

            // Open pCodec
            if (FFmpegInvoke.avcodec_open2(avCodecCtx, pCodec, null) < 0)
            {
                Console.WriteLine("Could not open Codec");
            }

            //swsCtx = FFmpegInvoke.sws_getContext(avCodecCtx->width, avCodecCtx->height, avCodecCtx->pix_fmt, avCodecCtx->width, avCodecCtx->height, AVPixelFormat.PIX_FMT_BGR24,
            //    FFmpegInvoke.SWS_BILINEAR, null, null, null);
            swsCtx = FFmpegInvoke.sws_getContext(avCodecCtx->width, avCodecCtx->height, avCodecCtx->pix_fmt, avCodecCtx->width, avCodecCtx->height, AVPixelFormat.PIX_FMT_RGBA,
                FFmpegInvoke.SWS_BILINEAR, null, null, null);
        }

        public void InitDecoderToImage(string codec, int width = 2048, int height = 1536)
        {
            frameNumber = 0;
            videoData = null;

            //  Register all formats and codecs
            FFmpegInvoke.av_register_all();
            FFmpegInvoke.avcodec_register_all();
            FFmpegInvoke.avformat_network_init();

            // Get the decoder
            AVCodec* pCodec;
            switch (codec)
            {
                case "MPEG4":
                    pCodec = FFmpegInvoke.avcodec_find_decoder(AVCodecID.CODEC_ID_MPEG4);
                    break;
                default:
                    pCodec = FFmpegInvoke.avcodec_find_decoder(AVCodecID.CODEC_ID_H264);
                    break;
            }

            // Get and configure the CodecContext
            avCodecCtx = FFmpegInvoke.avcodec_alloc_context3(pCodec);

            if ((pCodec->capabilities & FFmpegInvoke.CODEC_CAP_TRUNCATED) != 0)
                avCodecCtx->flags = (avCodecCtx->flags | FFmpegInvoke.CODEC_FLAG_TRUNCATED);

            avCodecCtx->pix_fmt = AVPixelFormat.PIX_FMT_YUV420P;
            avCodecCtx->codec_type = AVMediaType.AVMEDIA_TYPE_VIDEO;
            avCodecCtx->width = width; //TODO
            avCodecCtx->height = height;  //TODO

            // Open pCodec
            if (FFmpegInvoke.avcodec_open2(avCodecCtx, pCodec, null) < 0)
            {
                Console.WriteLine("Could not open Codec");
            }

            //swsCtx = FFmpegInvoke.sws_getContext(avCodecCtx->width, avCodecCtx->height, avCodecCtx->pix_fmt, avCodecCtx->width, avCodecCtx->height, AVPixelFormat.PIX_FMT_BGR24,
            //    FFmpegInvoke.SWS_BILINEAR, null, null, null);
            swsCtx = FFmpegInvoke.sws_getContext(avCodecCtx->width, avCodecCtx->height, avCodecCtx->pix_fmt, avCodecCtx->width, avCodecCtx->height, AVPixelFormat.PIX_FMT_BGR24,
                FFmpegInvoke.SWS_BILINEAR, null, null, null);
        }

        //Free memory resources (bitmap uses).
        [DllImport("gdi32.dll")]
        public static extern void DeleteObject(IntPtr intPtr);

        public unsafe Image DecodeToImage(byte[] frameArray, long dateLong)
        {

            String hora = new DateTime(dateLong).ToString("hhmm");
            Image imageFrame = null;
            // Load the frame, the context and the packet
            videoData = frameArray;

            #region Frame initialization.

            AVFrame* pFrame;
            AVPicture* pFrameBGR;

            // Allocate an AVFrame structure
            pFrameBGR = (AVPicture*)FFmpegInvoke.avcodec_alloc_frame();
            if (pFrameBGR->Equals(null))
            {
                Console.WriteLine("Could not allocate RGB frame");
            }

            // Allocate video frame
            pFrame = FFmpegInvoke.avcodec_alloc_frame();

            if (pFrame->Equals(null))
            {
                Console.WriteLine("Could not allocate video frame");
            }

            // Determine required buffer size and allocate buffer
            //int numBytes = FFmpegInvoke.avpicture_get_size(AVPixelFormat.PIX_FMT_BGR24, avCodecCtx.width, avCodecCtx.height);
            int numBytes = FFmpegInvoke.avpicture_get_size(AVPixelFormat.PIX_FMT_BGR24, 2048, 1536);

            var pBuffer = (byte*)FFmpegInvoke.av_malloc((uint)numBytes);

            // Assign appropriate parts of buffer to image planes in pFrameRGB
            // Note that pFrameRGB is an AVFrame, but AVFrame is a superset
            // of AVPicture
            FFmpegInvoke.avpicture_fill(pFrameBGR, pBuffer, AVPixelFormat.PIX_FMT_BGR24, 2048,
                1536);

            #endregion

            #region Decoding frames

            fixed (byte* ptr = videoData)
            {

                if (!isInitialized)
                {
                    avCodecCtx->extradata_size = videoData.Length;
                    avCodecCtx->extradata = ptr;
                    isInitialized = true;
                }

                //Inicializar Packet
                var packetInstance = new AVPacket();
                FFmpegInvoke.av_init_packet(&packetInstance);

                packetInstance.size = videoData.Length;
                packetInstance.data = ptr;

                frameNumber++;

                //string path = String.Format("{0}{1}", Application.StartupPath, @"\frames\");
                //string fileName = String.Format("{0}{1}-{2}{3}", camId, hora, frameNumber, ".jpeg");
                //string fullpath = String.Format("{0}{1}", path, fileName);

                try
                {
                    int gotPicture = 0;
                    int decodeValue = FFmpegInvoke.avcodec_decode_video2(avCodecCtx, pFrame, &gotPicture, &packetInstance);
                    if (decodeValue < 0)
                    {
                        Console.WriteLine("Error while decoding frame {0}\n", frameNumber);
                        //Application.Exit();
                    }
                    if (gotPicture == 1)
                    {
                        byte** src = &pFrame->data_0;
                        byte** dst = &pFrameBGR->data_0;
                        FFmpegInvoke.sws_scale(swsCtx, src, pFrame->linesize, 0,
                                               avCodecCtx->height, dst,
                                               pFrameBGR->linesize);

                        byte* convertedFrameAddress = pFrameBGR->data_0;

                        var imageBufferPtr = new IntPtr(convertedFrameAddress);
                        //Console.WriteLine("Processing frame: " + Path.GetFileName(fullpath));
                        int linesize = pFrameBGR->linesize[0];
                        using (var bitmap = new Bitmap(avCodecCtx->width, avCodecCtx->height, linesize, PixelFormat.Format24bppRgb, imageBufferPtr))
                        {
                            IntPtr intPtr = bitmap.GetHbitmap();
                            imageFrame = Image.FromHbitmap(intPtr);
                            DeleteObject(intPtr);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            #endregion

            #region Destroy resources.
            if (!pFrameBGR->Equals(null))
            {
                FFmpegInvoke.av_free(pFrameBGR);
            }

            if (!pFrame->Equals(null))
            {
                FFmpegInvoke.av_free(pFrame);
            }

            if (!pBuffer->Equals(null))
            {
                FFmpegInvoke.av_free(pBuffer);
            }
            #endregion

            return imageFrame;
        }

        public unsafe DecodedFrame Decode(byte[] frameArray, long dateLong)
        {
            String hora = new DateTime(dateLong).ToString("hhmm");
            DecodedFrame decodedFrame = null;
            // Load the frame, the context and the packet
            videoData = frameArray;

            #region Frame initialization.

            AVFrame* pFrame;
            AVPicture* pFrameBGR;

            // Allocate an AVFrame structure
            pFrameBGR = (AVPicture*)FFmpegInvoke.avcodec_alloc_frame();
            if (pFrameBGR->Equals(null))
            {
                Console.WriteLine("Could not allocate RGB frame");
            }

            // Allocate video frame
            pFrame = FFmpegInvoke.avcodec_alloc_frame();

            if (pFrame->Equals(null))
            {
                Console.WriteLine("Could not allocate video frame");
            }

            // Determine required buffer size and allocate buffer
            //int numBytes = FFmpegInvoke.avpicture_get_size(AVPixelFormat.PIX_FMT_BGR24, avCodecCtx.width, avCodecCtx.height);
            int numBytes = FFmpegInvoke.avpicture_get_size(AVPixelFormat.PIX_FMT_RGBA, 800, 600);

            var pBuffer = (byte*)FFmpegInvoke.av_malloc((uint)numBytes);

            // Assign appropriate parts of buffer to image planes in pFrameRGB
            // Note that pFrameRGB is an AVFrame, but AVFrame is a superset
            // of AVPicture
            FFmpegInvoke.avpicture_fill(pFrameBGR, pBuffer, AVPixelFormat.PIX_FMT_RGBA, 800,
                600);

            #endregion

            #region Decoding frames

            fixed (byte* ptr = videoData)
            {

                if (!isInitialized)
                {
                    avCodecCtx->extradata_size = videoData.Length;
                    avCodecCtx->extradata = ptr;
                    isInitialized = true;
                }

                //Inicializar Packet
                var packetInstance = new AVPacket();
                FFmpegInvoke.av_init_packet(&packetInstance);

                packetInstance.size = videoData.Length;
                packetInstance.data = ptr;

                frameNumber++;

                //string path = String.Format("{0}{1}", Application.StartupPath, @"\frames\");
                //string fileName = String.Format("{0}{1}-{2}{3}", camId, hora, frameNumber, ".jpeg");
                //string fullpath = String.Format("{0}{1}", path, fileName);

                try
                {
                    int gotPicture = 0;
                    int decodeValue = FFmpegInvoke.avcodec_decode_video2(avCodecCtx, pFrame, &gotPicture, &packetInstance);
                    if (decodeValue < 0)
                    {
                        Console.WriteLine("Error while decoding frame {0}\n", frameNumber);
                    }
                    if (gotPicture == 1)
                    {
                        byte** src = &pFrame->data_0;
                        byte** dst = &pFrameBGR->data_0;
                        FFmpegInvoke.sws_scale(swsCtx, src, pFrame->linesize, 0,
                                               avCodecCtx->height, dst,
                                               pFrameBGR->linesize);

                        byte* convertedFrameAddress = pFrameBGR->data_0;

                        var imageBufferPtr = new IntPtr(convertedFrameAddress);
                        int linesize = pFrameBGR->linesize[0];

                        int bytes = linesize * avCodecCtx->height;
                        byte[] byteData = new byte[bytes];
                        Marshal.Copy(imageBufferPtr, byteData, 0, bytes);

                        decodedFrame = new DecodedFrame(avCodecCtx->width, avCodecCtx->height, linesize, PixelFormat.Format24bppRgb, byteData);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            #endregion

            #region Destroy resources.
            if (!pFrameBGR->Equals(null))
            {
                FFmpegInvoke.av_free(pFrameBGR);
            }

            if (!pFrame->Equals(null))
            {
                FFmpegInvoke.av_free(pFrame);
            }

            if (!pBuffer->Equals(null))
            {
                FFmpegInvoke.av_free(pBuffer);
            }
            #endregion

            return decodedFrame;
        }

        public unsafe void Destroy()
        {
            frameNumber = 0;

            if (!avCodecCtx->Equals(null))
            {
                FFmpegInvoke.avcodec_close(avCodecCtx);
            }
            if (!swsCtx->Equals(null))
            {
                FFmpegInvoke.sws_freeContext(swsCtx);
            }
        }

    }
}