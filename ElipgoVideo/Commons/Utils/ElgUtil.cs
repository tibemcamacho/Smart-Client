using ElipgoVideo.Commons.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;

namespace ElipgoVideo.Commons.Utils
{
    public static class ElgUtil
    {
        private const int CAPACITY = 512000; // 512 Kb

        #region DAT files parsing

        /// <summary>
        /// Compile in elg file a sequence of frames that belong to one or more video cameras.
        /// </summary>
        /// <param name="foldersFiles">Video file's containing folders</param>
        /// <param name="storePath">Path to save a elf file.</param>
        /// <param name="dateStart">Starting date of the sequence</param>
        /// <param name="dateStop">End date of the sequence</param>
        /// <param name="hourOffset">GMT hour offset</param>
        public static void Dat2Elipgo(Dictionary<string, List<string>> foldersFiles, string storePath, string dateStart, string dateStop, int hourOffset = 0)
        {
            var ms = new MemoryStream(CAPACITY);
            FileStream fileStream = File.Create(storePath);
            var bw = new BinaryWriter(fileStream);
            try
            {
                int cameraFiles = 0;
                int greatestNumberOfFilesByCamera = 0;
                foreach (string folder in foldersFiles.Keys)
                {
                    int camera = Convert.ToInt32(folder.Substring(folder.LastIndexOf("camera", System.StringComparison.Ordinal) + "camera".Length));
                    if (!new List<int>(VideoPlayUtil.CameraList.Keys).Contains(camera))
                    {
                        VideoPlayUtil.RegisterCamera(camera, 0);
                    }
                    if (foldersFiles[folder].Count > greatestNumberOfFilesByCamera)
                    {
                        greatestNumberOfFilesByCamera = foldersFiles[folder].Count;
                    }
                }

                while (cameraFiles < greatestNumberOfFilesByCamera)
                {
                    List<string> files = new List<string>();
                    foreach (string folder in foldersFiles.Keys)
                    {
                        if (cameraFiles < foldersFiles[folder].Count)
                        {
                            files.Add(foldersFiles[folder].ToArray()[cameraFiles]);
                        }
                    }
                    Dat2Elipgo(files.ToArray(), dateStart, dateStop, hourOffset, ms, bw);
                    cameraFiles++;
                }

                //Writing .elg file and closing streams.
                bw.Write(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
                bw.Close();
                fileStream.Close();
                fileStream.Dispose();
            }

        }

        //public static void Dat2Elipgo(string[] pFiles, string dateStart, string dateStop, MemoryStream ms)
        //{
        //    int mix = 0;

        //    string format = "yyyyMMddHHmmss";
        //    DateTime start = DateTime.ParseExact(dateStart, format, CultureInfo.InvariantCulture);
        //    DateTime stop = DateTime.ParseExact(dateStop, format, CultureInfo.InvariantCulture);

        //    FileStream[] datFileReader = new FileStream[pFiles.Length];
        //    for (int i = 0; i < datFileReader.Length; i++)
        //    {
        //        datFileReader[i] = File.OpenRead(pFiles[i]);
        //        datFileReader[i].Position = 1024;
        //    }

        //    bool[] firstFrameInSequence = new bool[pFiles.Length];

        //    bool finish = false;

        //    object[] decodedFrameBuffer = new object[VideoPlayUtil.CameraList.Count];
        //    bool[] processedCameras = new bool[VideoPlayUtil.CameraList.Count];

        //    while (!finish && !AllProcessedCameras(processedCameras))
        //    {
        //        if (!processedCameras[mix] && datFileReader[mix].Position < datFileReader[mix].Length - 1)
        //        {
        //            //Get a 256 bytes from .DAT file's header
        //            var head = new byte[256];
        //            datFileReader[mix].Read(head, 0, 256);

        //            //Creating DATHeader's structure
        //            var datHeader = new DATHeader(head);

        //            //Aqui podria ir el filtro de la fecha
        //            processedCameras[mix] = DateTime.FromBinary(datHeader.time).CompareTo(stop) > 0;

        //            if (!processedCameras[mix])
        //            {
        //                //Finding sequence first key frame.
        //                byte[] frameBytes = null;
        //                while (!firstFrameInSequence[mix] && datFileReader[mix].Position < datFileReader[mix].Length)
        //                {
        //                    //Get frame's byte.                   
        //                    if (datHeader.frameType == 1 && DateTime.FromBinary(datHeader.time).CompareTo(start) >= 0)
        //                    {
        //                        firstFrameInSequence[mix] = true;
        //                    }
        //                    else
        //                    {
        //                        datFileReader[mix].Position += datHeader.frameForward - 256;
        //                        datFileReader[mix].Read(head, 0, 256);
        //                        datHeader = new DATHeader(head);
        //                    }
        //                }

        //                //Creating ElipgoHeader from DATHeader
        //                var elgHeader = new ElipgoHeader(datHeader, new List<int>(VideoPlayUtil.CameraList.Keys)[mix], "Camera " + mix);

        //                //Get frame's byte.                   
        //                frameBytes = new byte[elgHeader.frameLength];
        //                datFileReader[mix].Read(frameBytes, 0, elgHeader.frameLength);
        //                byte[] elgHeaderBytes = elgHeader.toByteArray();

        //                byte[] fr = new byte[elgHeaderBytes.Length + frameBytes.Length];
        //                elgHeaderBytes.CopyTo(fr, 0);
        //                frameBytes.CopyTo(fr, elgHeaderBytes.Length);

        //                decodedFrameBuffer[mix] = fr;
        //                if (FlushFrameBuffer(decodedFrameBuffer, (VideoPlayUtil.CameraList.Count - ProcessedCameras(processedCameras))))
        //                {
        //                    for (int j = 0; j < decodedFrameBuffer.Length; j++)
        //                    {
        //                        if (decodedFrameBuffer[j] != null)
        //                        {
        //                            ms.Write((byte[])decodedFrameBuffer[j], 0, ((byte[])decodedFrameBuffer[j]).Length);
        //                        }
        //                        decodedFrameBuffer[j] = null;
        //                    }
        //                }
        //                processedCameras[mix] = processedCameras[mix] || datFileReader[mix].Position == datFileReader[mix].Length;
        //            }
        //            //else
        //            //{
        //            //    for (int j = 0; j < decodedFrameBuffer.Length; j++)
        //            //    {
        //            //        if (decodedFrameBuffer[j] != null)
        //            //        {
        //            //            ms.Write((byte[])decodedFrameBuffer[j], 0, ((byte[])decodedFrameBuffer[j]).Length);
        //            //        }
        //            //        decodedFrameBuffer[j] = null;
        //            //    }
        //            //}
        //        }
        //        //Verifiy the file reading.
        //        finish = true;
        //        int i = 0;
        //        while (i < pFiles.Length && finish)
        //        {
        //            if (datFileReader[i].Position < datFileReader[i].Length - 1)
        //                finish = false;
        //            else
        //                i++;
        //        }

        //        //Moving to next file or finish.
        //        if (!finish)
        //        {
        //            if (mix == pFiles.Length - 1)
        //                mix = 0;
        //            else
        //            {
        //                mix++;
        //            }
        //        }
        //    }
        //    for (int i = 0; i < datFileReader.Length; i++)
        //    {
        //        datFileReader[i].Close();
        //        datFileReader[i].Dispose();
        //    }
        //}

        /// <summary>
        /// Compile in elg file a sequence of frames that belong to one or more video cameras.
        /// </summary>
        /// <param name="pFiles">DAT files used a secuence souces</param>
        /// <param name="dateStart">Starting date of the sequence</param>
        /// <param name="dateStop">End date of the sequence</param>
        /// <param name="hourOffset">GMT hour offset</param>
        /// <param name="ms">Stream to new elg file.</param>
        public static void Dat2Elipgo(string[] pFiles, string dateStart, string dateStop, int hourOffset, MemoryStream ms, BinaryWriter bw)
        {
            int mix = 0;

            string format = "yyyyMMddHHmmss";
            DateTime start = DateTime.ParseExact(dateStart, format, CultureInfo.InvariantCulture);
            DateTime stop = DateTime.ParseExact(dateStop, format, CultureInfo.InvariantCulture);

            FileStream[] datFileReader = new FileStream[pFiles.Length];
            for (int i = 0; i < datFileReader.Length; i++)
            {
                datFileReader[i] = File.Open(pFiles[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                datFileReader[i].Position = 1024;
            }

            bool[] firstFrameInSequence = new bool[pFiles.Length];

            bool finish = false;

            object[] decodedFrameBuffer = new object[VideoPlayUtil.CameraList.Count];
            bool[] processedCameras = new bool[VideoPlayUtil.CameraList.Count];
            bool moveToNextFile = true;

            while (!finish && !AllProcessedCameras(processedCameras))
            {
                moveToNextFile = true;
                bool headerFragment = datFileReader[mix].Position + 256 > datFileReader[mix].Length;
                if (!processedCameras[mix] && datFileReader[mix].Position < datFileReader[mix].Length - 1 && !headerFragment)
                {
                    //Get a 256 bytes from .DAT file's header
                    var head = new byte[256];
                    datFileReader[mix].Read(head, 0, 256);

                    //Creating DATHeader's structure
                    var datHeader = new DATHeader(head);

                    processedCameras[mix] = DateTime.FromBinary(datHeader.time).CompareTo(stop) > 0;
                    if (!processedCameras[mix])
                    {
                        //Finding sequence first key frame.
                        byte[] frameBytes = null;
                        while (!firstFrameInSequence[mix] && datFileReader[mix].Position < datFileReader[mix].Length)
                        {
                            //Get frame's byte.                   
                            if (datHeader.frameType == 1 && DateTime.FromBinary(datHeader.time).CompareTo(start) >= 0)
                            {
                                firstFrameInSequence[mix] = true;
                            }
                            else
                            {
                                datFileReader[mix].Position += datHeader.frameForward - 256;
                                if (datFileReader[mix].Position <= datFileReader[mix].Length - 256)
                                {
                                    datFileReader[mix].Read(head, 0, 256);
                                    datHeader = new DATHeader(head);
                                }
                                else
                                {
                                    datFileReader[mix].Position = datFileReader[mix].Length;
                                }
                            }
                        }

                        DateTime frameLocalTime = DateTime.FromBinary(datHeader.time);
                        frameLocalTime = frameLocalTime.AddHours(hourOffset);
                        datHeader.time = frameLocalTime.Ticks;

                        bool frameFragment = datFileReader[mix].Position + (datHeader.frameForward - 256) > datFileReader[mix].Length;

                        if (!frameFragment)
                        {
                            //Creating ElipgoHeader from DATHeader
                            int cam = new List<int>(VideoPlayUtil.CameraList.Keys)[mix];
                            var elgHeader = new ElipgoHeader(datHeader, cam, String.Format("Camera {0}", cam));

                            //Get frame's byte.                   
                            frameBytes = new byte[elgHeader.frameLength];
                            datFileReader[mix].Read(frameBytes, 0, elgHeader.frameLength);
                            byte[] elgHeaderBytes = elgHeader.toByteArray();

                            byte[] fr = new byte[elgHeaderBytes.Length + frameBytes.Length];
                            elgHeaderBytes.CopyTo(fr, 0);
                            frameBytes.CopyTo(fr, elgHeaderBytes.Length);

                            decodedFrameBuffer[mix] = fr;
                            if (FlushFrameBuffer(decodedFrameBuffer, (VideoPlayUtil.CameraList.Count - ProcessedCameras(processedCameras))))
                            {
                                byte[] frame = GetTimelineFrame(decodedFrameBuffer, out mix);
                                if (frame != null)
                                {
                                    //Managing memory issues.
                                    if (ms.Length + frame.Length > CAPACITY)//Check memory stream capacity.
                                    {
                                        bw.Write(ms.ToArray());
                                        Clear(ms);
                                    }

                                    ms.Write(frame, 0, frame.Length);
                                }
                                decodedFrameBuffer[mix] = null;
                            }
                            else
                            {
                                if (mix == pFiles.Length - 1)
                                    mix = 0;
                                else
                                {
                                    mix++;
                                }
                            }
                        }
                        else
                        {
                            datFileReader[mix].Position = datFileReader[mix].Length;
                        }
                        processedCameras[mix] = processedCameras[mix] || datFileReader[mix].Position == datFileReader[mix].Length;
                        moveToNextFile = false;
                    }
                }
                if (headerFragment)
                {
                    datFileReader[mix].Position = datFileReader[mix].Length;
                }
                //Verifiy the file reading.
                finish = true;
                int i = 0;
                while (i < pFiles.Length && finish)
                {
                    if (datFileReader[i].Position < datFileReader[i].Length - 1)
                        finish = false;
                    else
                        i++;
                }

                if (moveToNextFile)
                {
                    //Moving to next file or finish.
                    if (!finish)
                    {
                        if (mix == pFiles.Length - 1)
                            mix = 0;
                        else
                        {
                            mix++;
                        }
                    }
                }
            }
            // Cleaning the frame's buffer.
            for (int i = 0; i < decodedFrameBuffer.Length; i++)
            {
                byte[] frame = GetTimelineFrame(decodedFrameBuffer, out mix);
                if (frame != null)
                {
                    //Managing memory issues.
                    if (ms.Length + frame.Length > CAPACITY)//Check memory stream capacity.
                    {
                        bw.Write(ms.ToArray());
                        Clear(ms);
                    }
                    ms.Write(frame, 0, frame.Length);
                }
                decodedFrameBuffer[mix] = null;
            }

            for (int i = 0; i < datFileReader.Length; i++)
            {
                datFileReader[i].Close();
                datFileReader[i].Dispose();
            }
        }

        private static void Clear(MemoryStream source)
        {
            byte[] buffer = source.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            source.Position = 0;
            source.SetLength(0);
        }

        /// <summary>
        /// Returns the number of processed cameras.
        /// </summary>
        /// <param name="processedCameras">Processed cameras flags</param>
        /// <returns>Count of processed cameras</returns>
        private static int ProcessedCameras(bool[] processedCameras)
        {
            int c = 0;
            for (int i = 0; i < processedCameras.Length; i++)
            {
                if (processedCameras[i])
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// It indicates when the frame buffer is ready to be written to the file
        /// </summary>
        /// <param name="frameBuffer">Frames buffer</param>
        /// <param name="processingCameras">Count of precessed cameras.</param>
        /// <returns>True if frame buffer is ready to be written to the file</returns>
        private static bool FlushFrameBuffer(object[] frameBuffer, int processingCameras)
        {
            int c = 0;
            for (int i = 0; i < frameBuffer.Length; i++)
            {
                if (frameBuffer[i] != null)
                {
                    c++;
                }
            }
            return c >= processingCameras;
        }

        /// <summary>
        /// It indicates when all camaras are processed.
        /// </summary>
        /// <param name="cameraSequenceComplete">Individual secuences complete.</param>
        /// <returns>True if all secuence is processed.</returns>
        private static bool AllProcessedCameras(bool[] cameraSequenceComplete)
        {
            int i = 0;
            while (i < cameraSequenceComplete.Length && cameraSequenceComplete[i])
            {
                i++;
            }
            return i == cameraSequenceComplete.Length;
        }

        /// <summary>
        /// Get the oldest frame of all on buffer.
        /// </summary>
        /// <param name="frameBuffer">Frames buffer.</param>
        /// <param name="mix">Camara to which belongs the oldest frame.</param>
        /// <returns></returns>
        private static byte[] GetTimelineFrame(object[] frameBuffer, out int mix)
        {
            byte[] header = new byte[ElipgoHeader.HEADER_SIZE];
            DateTime frameDateTime = DateTime.MaxValue;
            int index = 0;
            for (int i = 0; i < frameBuffer.Length; i++)
            {
                if (frameBuffer[i] != null)
                {
                    Array.Copy((byte[])frameBuffer[i], header, ElipgoHeader.HEADER_SIZE);
                    var datHeader = new ElipgoHeader(header);
                    if (frameDateTime.CompareTo(DateTime.FromBinary(datHeader.time)) > 0)
                    {
                        frameDateTime = DateTime.FromBinary(datHeader.time);
                        index = i;
                    }
                }
            }
            mix = index;
            return (byte[])frameBuffer[index];
        }

        /// <summary>
        /// Parse a elg file and determinate how many cameras and de offset of the I-Frame.
        /// </summary>
        /// <param name="elgFile">Stream to elg file</param>
        /// <param name="trackBarSize">Video tracking bar size</param>
        public static void ParseElgFile(Stream elgFile, int trackBarSize = 1)
        {
            int lon = (int)elgFile.Length;

            //File's offset.
            int offset = 0;
            //Starting and final point of video reproduction
            long startTrack = long.MaxValue;
            long finishTrack = long.MinValue;
            VideoPlayUtil.CleanCameraList();
            VideoPlayUtil.CleanKeyFramesPositions();
            int totalFrames = 0;
            int cameraID = 0;
            Dictionary<int, int> cameraGov = new Dictionary<int, int>();
            while (offset < lon - 1)
            {
                //Read Elipgo header.
                var head = new byte[ElipgoHeader.HEADER_SIZE];
                elgFile.Read(head, 0, ElipgoHeader.HEADER_SIZE);
                offset += ElipgoHeader.HEADER_SIZE;

                //Build the header structure.
                var bHeader = new ElipgoHeader(head);
                cameraID = cameraID == 0 ? bHeader.camId : cameraID;
                //Set starting configuurations.
                startTrack = startTrack.CompareTo(bHeader.time) > 0 ? bHeader.time : startTrack;
                finishTrack = finishTrack.CompareTo(bHeader.time) < 0 ? bHeader.time : finishTrack;

                VideoPlayUtil.RegisterCamera(bHeader.camId, offset + bHeader.frameLength);

                IEnumerator enumerator = VideoPlayUtil.CameraList.Keys.GetEnumerator();
                enumerator.MoveNext();
                object first = enumerator.Current;

                if (bHeader.frameType == 1)
                {
                    VideoPlayUtil.StoreKeyFrameOffset((offset - ElipgoHeader.HEADER_SIZE), bHeader.camId, DateTime.FromBinary(bHeader.time));
                    if (!cameraGov.ContainsKey(bHeader.camId) && VideoPlayUtil.CameraList[bHeader.camId].Gov == 0)
                    {
                        cameraGov.Add(bHeader.camId, 1);
                    }
                    else
                    {
                        VideoPlayUtil.CameraList[bHeader.camId].Gov = cameraGov[bHeader.camId];
                    }
                }
                else
                {
                    if (VideoPlayUtil.CameraList[bHeader.camId].Gov == 0 && cameraGov.ContainsKey(bHeader.camId))
                    {
                        cameraGov[bHeader.camId]++;
                    }
                }

                //Read freme's bytes.
                if (offset + bHeader.frameLength < lon)
                {
                    offset += bHeader.frameLength;
                    elgFile.Position = offset;
                }
                else
                {
                    offset = lon;
                    elgFile.Position = offset;
                }
                if (bHeader.camId == cameraID)
                {
                    totalFrames++;
                }
            }
            VideoPlayUtil.Initialize(totalFrames, trackBarSize);
            //VideoPlayUtil.InitializeReverse(VideoPlayUtil.KeyFramesPositions.Count, trackBarSize);
            //Reset the stream position
            elgFile.Position = 0;
        }

        #endregion

        #region Seproban secuence generation

        /// <summary>
        /// Code a frame following the Seproban rule
        /// </summary>
        /// <param name="branchCode">Branch code</param>
        /// <param name="branchName">Branch name</param>
        /// <param name="jpegFilesPath">Path to decoded frames</param>
        /// <param name="seprobanFilesPath">Seproban sequence storing path</param>
        /// <param name="dateStart">Starting date for a sequence</param>
        /// <param name="quality">Quality of the resulting images</param>
        /// <param name="maxSize">Max size of images's files</param>
        /// <param name="cameraId">Camera id</param>
        /// <param name="cameraCodes">Camera codes</param>
        /// <param name="timeDuringEvent">Event during</param>
        /// <param name="currentDate">Current frame date</param>
        /// <param name="frameNumber">Frame number</param>
        /// <param name="printTimestamp">Indicate if appear a tag on image with frame date.</param>
        /// <param name="printBranchName">Indicate if appear a tag on image with branch name.</param>
        public static void SeprobanRuleFrameCoded(string branchCode, string branchName, string jpegFilesPath, string seprobanFilesPath, DateTime dateStart, long quality, long maxSize, string cameraId,
            Dictionary<string, int> cameraCodes, ref bool timeDuringEvent, ref DateTime currentDate, ref int frameNumber, bool printTimestamp, bool printBranchName)
        {
            string[] jpegFiles;
            string framePattern;
            string seprobanFileName;
            if (currentDate.CompareTo(dateStart.AddMinutes(1)) < 0)
            {
                framePattern = currentDate.ToString("HHmmss");
                jpegFiles = Directory.GetFiles(jpegFilesPath, String.Format("*{0}_{1}*", cameraId, framePattern));
                if (jpegFiles.Length > 0)
                {
                    ResizeJpeg(jpegFiles[0], jpegFiles[0], 640, 480, quality, maxSize, branchName, currentDate, printTimestamp, printBranchName);
                    String sourceFileName = jpegFiles[0].Substring(jpegFiles[0].LastIndexOf('\\') + 1,
                                                                   (jpegFiles[0].IndexOf('.') -
                                                                    jpegFiles[0].LastIndexOf('\\') - 1));

                    string[] sourceFileNameComponents = sourceFileName.Split('_');
                    seprobanFileName = String.Format("{0}{1}{2}{3}{4}.{5}{6}", branchCode, "001",
                                                     cameraCodes[cameraId].ToString("D2"),
                                                     sourceFileNameComponents[1], "A", "I",
                                                     frameNumber.ToString("D6"));
                    File.Copy(jpegFiles[0], String.Format("{0}{1}", seprobanFilesPath, seprobanFileName));
                    frameNumber++;
                }
                framePattern = currentDate.AddMinutes(1).ToString("HHmmss");
                jpegFiles = Directory.GetFiles(jpegFilesPath, String.Format("*{0}_{1}*", cameraId, framePattern));
                if (jpegFiles.Length > 0)
                {
                    ResizeJpeg(jpegFiles[0], jpegFiles[0], 640, 480, quality, maxSize, branchName, currentDate.AddMinutes(1), printTimestamp, printBranchName);
                    String sourceFileName = jpegFiles[0].Substring(jpegFiles[0].LastIndexOf('\\') + 1,
                                                                   (jpegFiles[0].IndexOf('.') -
                                                                    jpegFiles[0].LastIndexOf('\\') - 1));

                    string[] sourceFileNameComponents = sourceFileName.Split('_');
                    seprobanFileName = String.Format("{0}{1}{2}{3}{4}.{5}{6}", branchCode, "001",
                                                     cameraCodes[cameraId].ToString("D2"),
                                                     sourceFileNameComponents[1], "D", "I",
                                                     frameNumber.ToString("D6"));
                    File.Copy(jpegFiles[0], String.Format("{0}{1}", seprobanFilesPath, seprobanFileName));
                    frameNumber++;
                }
            }
            else
            {
                if (!timeDuringEvent)
                {
                    currentDate = currentDate.AddMinutes(1);
                    timeDuringEvent = true;
                }
                framePattern = currentDate.ToString("HHmmss");
                jpegFiles = Directory.GetFiles(jpegFilesPath, String.Format("*{0}_{1}*", cameraId, framePattern));
                if (jpegFiles.Length > 0)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (jpegFiles.Length > j)
                        {
                            ResizeJpeg(jpegFiles[j], jpegFiles[j], 640, 480, quality, maxSize, branchName, currentDate, printTimestamp, printBranchName);
                            String sourceFileName = jpegFiles[j].Substring(jpegFiles[j].LastIndexOf('\\') + 1,
                                                                           (jpegFiles[j].IndexOf('.') -
                                                                            jpegFiles[j].LastIndexOf('\\') - 1));

                            string[] sourceFileNameComponents = sourceFileName.Split('_');
                            seprobanFileName = String.Format("{0}{1}{2}{3}{4}.{5}{6}", branchCode, "001",
                                                             cameraCodes[cameraId].ToString("D2"),
                                                             sourceFileNameComponents[1], "D", "I",
                                                             frameNumber.ToString("D6"));
                            File.Copy(jpegFiles[j], String.Format("{0}{1}", seprobanFilesPath, seprobanFileName));
                            frameNumber++;
                        }
                    }
                }
            }
        }

        #endregion

        #region Image processing

        /// <summary>
        /// Resize a jpeg image
        /// </summary>
        /// <param name="imageFile">Image to resize.</param>
        /// <param name="imageOutFile">Image resized</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="quality">Image quality</param>
        /// <param name="MaxSize">Max image size.</param>
        /// <param name="branchName">Name of bank branch. Used conforming the image tag.</param>
        /// <param name="timeStamp">Frame timestamp. Used conforming the image tag.</param>
        /// <param name="printTimestamp">Indicate if appear a tag on image with frame date.</param>
        /// <param name="printBranchName">Indicate if appear a tag on image with branch name.</param>
        private static void ResizeJpeg(string imageFile, string imageOutFile, int width, int height, long quality, long MaxSize, string branchName, DateTime timeStamp, bool printTimestamp,
            bool printBranchName)
        {
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            Encoder myEncoder;
            myEncoder = Encoder.Quality;
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");


            System.Drawing.Image original = null;
            using (FileStream fin = new FileStream(imageFile, FileMode.Open))
            {
                original = System.Drawing.Image.FromStream(fin);
            }

            if ((original.Width == width) && (original.Height == height))
                return;

            System.Drawing.Image auxImage = ResizeImage(original, new Size(width, height), false);
            System.Drawing.Image resized = null;

            resized = printTimestamp || printBranchName ? TimeStamp(branchName, timeStamp, auxImage, printTimestamp, printBranchName) : auxImage;

            long length = long.MaxValue;
            do
            {
                if (quality > 0)
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    using (MemoryStream mem = new MemoryStream())
                    {
                        resized.Save(mem, myImageCodecInfo, myEncoderParameters);
                        length = mem.Length;
                    }
                    quality = quality - 10;
                }
            }
            while ((length >= MaxSize) && (quality > 0));

            if (quality <= 0)
                quality = 10;
            else
                quality = quality + 10;

            EncoderParameter myEncoderParameterFout = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameterFout;

            using (FileStream fout = new FileStream(imageOutFile, FileMode.Create))
            {
                resized.Save(fout, myImageCodecInfo, myEncoderParameters);
                fout.Close();
            }
        }

        /// <summary>
        /// Create a image encode
        /// </summary>
        /// <param name="mimeType">Standard format file.</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// Implement the resize operation
        /// </summary>
        /// <param name="image">Image to resize</param>
        /// <param name="size">New size</param>
        /// <param name="preserveAspectRatio">Define if preserve the aspect ratio of original image</param>
        /// <returns>Resized image</returns>
        private static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        /// <summary>
        /// Print a timestamp over the image. Used for ElipgoSeproban
        /// </summary>
        /// <param name="branchName">Bank branch image.</param>
        /// <param name="dt">Frame datetime.</param>
        /// <param name="bmp">Image to resize.</param>
        /// <param name="printTimestamp">Indicate if appear a tag on image with frame date.</param>
        /// <param name="printBranchName">Indicate if appear a tag on image with branch name.</param>
        /// <returns>Tagged image.</returns>
        private static Image TimeStamp(string branchName, DateTime dt, Image bmp, bool printTimestamp,
            bool printBranchName)
        {
            System.Drawing.Image newImage = new Bitmap(bmp.Width, bmp.Height);
            //Hack if bmp is not 24bpp => convert it to 24bpp

            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bmp = ConvertTo24(bmp);
            }

            Graphics graphic = Graphics.FromImage(newImage);

            string ImageText = String.Format("{0}{1}{2}", printTimestamp ? dt.ToString("yyyy/MM/dd HH:mm:ss") : String.Empty,
                printTimestamp && printBranchName ? " - " : String.Empty,
                printBranchName ? (branchName.Length > 30 ? branchName.Substring(0, 30) : branchName) : String.Empty);

            float fontsize = 12;
            Font font = new Font("Arial", fontsize, FontStyle.Bold, GraphicsUnit.Pixel);

            StringFormat stringformat = new StringFormat(StringFormat.GenericTypographic);

            int height = Convert.ToInt32(graphic.MeasureString(ImageText, font, new PointF(0, 0), stringformat).Height) + 6;

            int width = Convert.ToInt32(graphic.MeasureString(ImageText, font, new PointF(0, 0), stringformat).Width);
            width += Convert.ToInt32(0.05 * width) + 10;

            graphic.TextRenderingHint = TextRenderingHint.SystemDefault;

            SolidBrush brush = new SolidBrush(Color.White);

            SolidBrush brushBack = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

            Rectangle rectBack = new Rectangle(0, 0, width, height);

            Rectangle rect = new Rectangle(2, 2, width, height);

            graphic.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);

            graphic.FillRectangle(brushBack, rectBack);

            graphic.DrawString(ImageText, font, brush, rect);

            graphic.Save();

            graphic.Dispose();

            return newImage;
        }

        /// <summary>
        /// Convert to 24 bit per pixels.
        /// </summary>
        /// <param name="bmpIn">Image to convert.</param>
        /// <returns>Converted image</returns>
        private static Image ConvertTo24(Image bmpIn)
        {
            Image converted = new Bitmap(bmpIn.Width, bmpIn.Height, PixelFormat.Format24bppRgb);

            using (Graphics g = Graphics.FromImage(converted))
            {

                // Prevent DPI conversion

                g.PageUnit = GraphicsUnit.Pixel;

                // Draw the image

                g.DrawImageUnscaled(bmpIn, 0, 0);

            }

            return converted;
        }

        /// <summary>
        /// Resize a jpeg image
        /// </summary>
        /// <param name="original">Image to resize.</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <param name="quality">Image quality</param>
        /// <param name="MaxSize">Max image size.</param>
        /// <param name="branchName">Name of bank branch. Used conforming the image tag.</param>
        /// <param name="timeStamp">Frame timestamp. Used conforming the image tag.</param>
        /// <returns>Resized image</returns>
        public static Image ResizeJpeg(Image original, int width, int height, long quality, long MaxSize, string branchName, DateTime timeStamp, Font customFont)
        {
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            Encoder myEncoder;
            myEncoder = Encoder.Quality;
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");


            if ((original.Width == width) && (original.Height == height))
                return original;

            Image auxImage = ResizeImage(original, new Size(width, height), false);
            Image resized = TimeStampPlayer(branchName, timeStamp, auxImage, customFont);

            long length = long.MaxValue;
            do
            {
                if (quality > 0)
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    using (MemoryStream mem = new MemoryStream())
                    {
                        resized.Save(mem, myImageCodecInfo, myEncoderParameters);
                        length = mem.Length;
                    }
                    quality = quality - 10;
                }
            }
            while ((length >= MaxSize) && (quality > 0));

            if (quality <= 0)
                quality = 10;
            else
                quality = quality + 10;

            return resized;
        }

        /// <summary>
        /// Print a timestamp over the image. Used for ElipgoPlayer
        /// </summary>
        /// <param name="branchName">Bank branch image.</param>
        /// <param name="dt">Frame datetime.</param>
        /// <param name="bmp">Image to resize.</param>
        /// <returns>Tagged image.</returns>
        public static Image TimeStampPlayer(string branchName, DateTime dt, Image bmp, Font customFont)
        {
            Image newImage = new Bitmap(bmp.Width, bmp.Height);
            //Hack if bmp is not 24bpp => convert it to 24bpp

            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bmp = ConvertTo24(bmp);
            }

            Graphics graphic = Graphics.FromImage(newImage);

            //Font font = new Font("Arial", fontsize, FontStyle.Bold, GraphicsUnit.Pixel);
            string ImageText = String.Format("{0}{1}", dt.ToString("yyyy/MM/dd HH:mm:ss"),
                branchName != String.Empty ?
                String.Format(" - {0}", branchName.Length > 30 ? branchName.Substring(0, 30) : branchName) : String.Empty);

            StringFormat stringformat = new StringFormat(StringFormat.GenericTypographic);

            int height = Convert.ToInt32(graphic.MeasureString(ImageText, customFont, new PointF(0, 0), stringformat).Height) + 6;

            int width = Convert.ToInt32(graphic.MeasureString(ImageText, customFont, new PointF(0, 0), stringformat).Width);
            width += Convert.ToInt32(0.05 * width) + 10;

            graphic.TextRenderingHint = TextRenderingHint.SystemDefault;

            SolidBrush brush = new SolidBrush(Color.White);

            SolidBrush brushBack = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

            Rectangle rectBack = new Rectangle(0, 0, width, height);

            Rectangle rect = new Rectangle(2, 2, width, height);

            graphic.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);

            graphic.FillRectangle(brushBack, rectBack);

            graphic.DrawString(ImageText, customFont, brush, rect);

            graphic.Save();

            graphic.Dispose();

            return newImage;
        }

        #endregion
    }
}