using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ElipgoVideo.Commons.Data
{


    public struct ElipgoHeader
    {
        public const int HEADER_SIZE = 64;

        public int formatVersion;
        public int camId;

        public string camName;
        public string codec;
        public int fpsRate;
        public int frameBackOff;
        public int frameLength;
        public int frameType;
        public long time;


        public ElipgoHeader(byte[] head)
        {
            // Bytes 1 - 4
            formatVersion = BitConverter.ToInt32(SubArray(head, 0, 4), 0);

            //Bytes 5 - 8
            camId = BitConverter.ToInt32(SubArray(head, 4, 4), 0);

            //Bytes 9 - 38
            camName = Encoding.UTF8.GetString(SubArray(head, 8, 30));

            //Bytes 39 - 46
            time = (long)BitConverter.ToUInt64(SubArray(head, 38, 8), 0);

            //DateTime date = new DateTime(long.Parse(ticks));
            //date.ToString("yyyy-MM-ddThh:mm:ssZ");

            //Byte 47
            frameType = head[46];

            //Bytes 48 - 52
            codec = Encoding.UTF8.GetString(SubArray(head, 47, 5));

            //Bytes 53 - 56
            fpsRate = BitConverter.ToInt32(SubArray(head, 52, 4), 0);

            //Bytes 57 - 60
            frameLength = BitConverter.ToInt32(SubArray(head, 56, 4), 0);

            //Bytes 61 - 64
            frameBackOff = BitConverter.ToInt32(SubArray(head, 60, 4), 0);
        }

        public ElipgoHeader(DATHeader dat, int pCam, string pCamName)
        {
            formatVersion = 1;
            camId = pCam;
            camName = pCamName;
            codec = dat.codec.Length > 5 ? dat.codec.Substring(0, 5) : dat.codec;
            fpsRate = (int)(dat.fps * 100);
            frameLength = dat.frameForward - 256;
            frameType = dat.frameType;
            time = dat.time;
            frameBackOff = dat.frameBack;
        }

        public byte[] toByteArray()
        {
            try
            {
                var ms = new MemoryStream();

                byte[] versionBytes = BitConverter.GetBytes(formatVersion);
                byte[] camIdBytes = BitConverter.GetBytes(camId);
                var camNameBytes = new byte[30];
                byte[] timeBytes = BitConverter.GetBytes(time);
                byte frameTypeByte = Byte.Parse(frameType.ToString());
                var codecBytes = new byte[5];
                byte[] fpsRateBytes = BitConverter.GetBytes(fpsRate);
                byte[] frameLenghtBytes = BitConverter.GetBytes(frameLength);
                byte[] frameBackBytes = BitConverter.GetBytes(frameBackOff);

                byte[] aux = Encoding.ASCII.GetBytes(camName);
                if (aux.Length > 30)
                {
                    throw new Exception("Cam Name length > 30 Bytes");
                }
                for (int i = 0; i < aux.Length; i++)
                {
                    camNameBytes[i] = aux[i];
                }

                aux = Encoding.ASCII.GetBytes(codec);
                if (aux.Length > 5)
                {
                    throw new Exception("Codec length > 5 Bytes");
                }
                for (int i = 0; i < aux.Length; i++)
                {
                    codecBytes[i] = aux[i];
                }
                ms.Write(versionBytes, 0, versionBytes.Length);
                ms.Write(camIdBytes, 0, camIdBytes.Length);
                ms.Write(camNameBytes, 0, camNameBytes.Length);
                ms.Write(timeBytes, 0, timeBytes.Length);
                ms.WriteByte(frameTypeByte);
                ms.Write(codecBytes, 0, codecBytes.Length);
                ms.Write(fpsRateBytes, 0, fpsRateBytes.Length);
                ms.Write(frameLenghtBytes, 0, frameLenghtBytes.Length);
                ms.Write(frameBackBytes, 0, frameBackBytes.Length);
                byte[] res = ms.ToArray();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                var _FileStream =
                    new FileStream(_FileName, FileMode.Create,
                                   FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception);
            }

            // error occured, return false
            return false;
        }
    }

    public struct DATHeader
    {
        public string codec;
        public string eventLabel;
        public double fps;
        public int frameBack;
        public int frameForward;
        public int frameNumb;
        public int frameType;
        public long time;

        public DATHeader(byte[] head)
        {
            string header = Encoding.UTF8.GetString(head);

            // FrameForward
            int offStart = header.IndexOf("<FF>") + 4;
            int offEnd = header.IndexOf("</FF>");
            frameForward = int.Parse(header.Substring(offStart, offEnd - offStart));

            // FrameBack
            offStart = header.IndexOf("<FB>") + 4;
            offEnd = header.IndexOf("</FB>");
            frameBack = int.Parse(header.Substring(offStart, offEnd - offStart));

            // Time
            offStart = header.IndexOf("<TS>") + 4;
            offEnd = header.IndexOf("</TS>");
            string strTime = header.Substring(offStart, offEnd - offStart);
            string format = "yyyyMMddHHmmssfff";
            DateTime date = DateTime.ParseExact(strTime, format, CultureInfo.InvariantCulture);
            time = date.ToBinary();

            // FPS
            offStart = header.IndexOf("<FS>") + 4;
            offEnd = header.IndexOf("</FS>");
            fps = double.Parse(header.Substring(offStart, offEnd - offStart));

            // EventLabel
            offStart = header.IndexOf("<LA>") + 4;
            offEnd = header.IndexOf("</LA>");
            eventLabel = header.Substring(offStart, offEnd - offStart);

            // FrameNumb
            offStart = header.IndexOf("<FN>") + 4;
            offEnd = header.IndexOf("</FN>");
            frameNumb = int.Parse(header.Substring(offStart, offEnd - offStart));

            // FrameType
            offStart = header.IndexOf("<KF>") + 4;
            offEnd = header.IndexOf("</KF>");
            string type = header.Substring(offStart, offEnd - offStart);
            if (type.Equals("False"))
                frameType = 0;
            else
                frameType = 1;

            // Codec
            offStart = header.IndexOf("<VC>") + 4;
            offEnd = header.IndexOf("</VC>");
            codec = header.Substring(offStart, offEnd - offStart);
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

    public struct SeekData
    {
        private int keyFrameOffset;
        private int cameraId;
        private DateTime frameDateTime;
        private int position;

        public SeekData(int offset, int cameraId, DateTime frameDateTime)
        {
            this.keyFrameOffset = offset;
            this.cameraId = cameraId;
            this.frameDateTime = frameDateTime;
            this.position = 0;
        }

        public int KeyFrameOffset { get { return keyFrameOffset; } }
        public int CameraId { get { return cameraId; } }
        public DateTime FrameDateTime { get { return frameDateTime; } }

        public int Position { get { return position; } set { position = value; } }

    }
}
