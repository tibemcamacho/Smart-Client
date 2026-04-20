using System;
using System.Collections.Generic;
using System.Text;

namespace ElipgoVideo.Commons.Utils
{
    public class H264SPSParser
    {
        static int pos;
        static byte[] data;

        public static void ParseSpsNal(byte[] data, out int width, out int height)
        {
            ParseSpsNal(data, out width, out height, false, null);
        }

        public static void ParseSpsNal(byte[] data, out int width, out int height, bool skipStartup, List<bool> newsps)
        {
            width = 0;
            height = 0;
            try
            {
                int index = 0;

                //Identifying startup secuence.
                if (!skipStartup)
                {
                    bool startupSequence = false;
                    while (index < data.Length && !startupSequence)
                    {
                        startupSequence = data[index] == 1;
                        index++;
                    }
                }

                //Console.WriteLine();
                //Console.WriteLine(BitConverter.ToString(data));
                //Console.WriteLine(BitConverter.ToString(data, 0, data.Length));
                //Console.WriteLine();
                pos = index * 8; // 8 bit for each byte.
                H264SPSParser.data = data;

                int forbidden_zero_bit = getU(1);
                //Console.WriteLine("forbidden_zero_bit " + forbidden_zero_bit);
                int nal_ref_idc = getU(2);
                int nal_unit_type = getU(5);
                //Console.WriteLine("nal_unit_type (should be 7 for SPS) " + nal_unit_type);
                //END of NAL_header

                //Start of SPS data
                int profile_idc = getU(8);
                int constraint_set0_flag = getU(1);
                int constraint_set1_flag = getU(1);
                int constraint_set2_flag = getU(1);
                int constraint_set3_flag = getU(1);
                int constraint_set4_flag = getU(1);
                int constraint_set5_flag = getU(1);
                //The current version of the spec states that there are two reserved bits
                int reserved_zero_2bits = getU(2);
                //Console.WriteLine("reserved_zero_2bits" + reserved_zero_2bits);
                int level_idc = getU(8);
                int seq_parameter_set_id = uev();
                int log2_max_frame_num_minus4 = uev();
                int pict_order_cnt_type = uev();
                //Console.WriteLine("pict_order_cnt_type=" + pict_order_cnt_type);
                if (pict_order_cnt_type == 0)
                {
                    uev();
                }
                else if (pict_order_cnt_type == 1)
                {
                    getU(1);
                    sev();
                    sev();
                    int n = uev();
                    //Console.WriteLine("n*sev, n=" + n);
                    for (int i = 0; i < n; i++)
                        sev();
                }
                int num_ref_frames = uev();
                getU(1);
                width = (uev() + 1) * 16;
                height = (uev() + 1) * 16;
                int frame_mbs_only_flag = getU(1);
                //Console.WriteLine(width + " x " + height);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static int ev(bool signed)
        {
            int bitcount = 0;
            StringBuilder expGolomb = new StringBuilder();
            while (getBit() == 0)
            {
                expGolomb.Append('0');
                bitcount++;
            }
            expGolomb.Append("/1");
            int result = 1;
            for (int i = 0; i < bitcount; i++)
            {
                int b = getBit();
                expGolomb.Append(b);
                result = result * 2 + b;
            }
            result--;
            if (signed)
            {
                result = (result + 1) / 2 * (result % 2 == 0 ? -1 : 1);
                //Console.WriteLine("getSe(v) = " + (result) + " " + expGolomb);
            }
            else
            {
                //Console.WriteLine("getUe(v) = " + (result) + " " + expGolomb);
            }
            return result;
        }

        private static int uev()
        {
            return ev(false);
        }

        private static int sev()
        {
            return ev(true);
        }

        private static int getU(int bits)
        {
            int result = 0;
            for (int i = 0; i < bits; i++)
            {
                result = result * 2 + getBit();
            }
            //Console.WriteLine("getU(" + bits + ") = " + result);
            return result;
        }

        private static int getBit()
        {
            int mask = 1 << (7 - (pos & 7));
            int idx = pos >> 3;
            pos++;

            string mascara = Convert.ToString(mask, 2);
            string num = Convert.ToString(data[idx], 2);
            string numHex = Convert.ToString(data[idx], 16);

            while (numHex.Length < 2)
                numHex = "0" + numHex;


            while (mascara.Length < 8)
                mascara = "0" + mascara;

            while (num.Length < 8)
                num = "0" + num;



            Console.WriteLine("BytePos: " + idx + " - Mask: " + mascara + " - Bin: " + num + " - 0x" + numHex);

            return ((data[idx] & mask) == 0) ? 0 : 1;
        }
    }
}
