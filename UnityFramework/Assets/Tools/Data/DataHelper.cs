using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using ProtoBuf;
//using HSGameEngine.GameEngine.Network.Protocol;
//using ComponentAce.Compression.Libs.zlib;
//using UnityEngine;

namespace SPSGame.Tools
{
    /// <summary>
    /// 数据操作辅助
    /// </summary>
    public class ByteHelper
    {
        /// <summary>
        /// 字节数据拷贝
        /// </summary>
        /// <param name="copyTo">目标字节数组</param>
        /// <param name="offsetTo">目标字节数组的拷贝偏移量</param>
        /// <param name="copyFrom">源字节数组</param>
        /// <param name="offsetFrom">源字节数组的拷贝偏移量</param>
        /// <param name="count">拷贝的字节个数</param>
        public static void CopyBytes(byte[] copyTo, int offsetTo, byte[] copyFrom, int offsetFrom, int count)
        {
            /*for (int i = 0; i < count; i++)
            {
                copyTo[offsetTo + i] = copyFrom[offsetFrom + i];
            }*/
            Array.Copy(copyFrom, offsetFrom, copyTo, offsetTo, count);
        }

        /// <summary>
        /// 字节数据排序
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="offsetTo"></param>
        /// <param name="count"></param>
        public static void SortBytes(byte[] bytesData, int offsetTo, int count)
        {
            byte temp = 0;
            byte[] keyBytes = BitConverter.GetBytes((int)20110529);
            for (int i = 0; i < keyBytes.Length; i++)
            {
                temp += keyBytes[i];
            }

            for (int x = offsetTo; x < (offsetTo + count); x++)
            {
                bytesData[x] ^= temp;
            }
        }

        /// <summary>
        /// 比较两个字节数组是否相同
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool CompBytes(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }

            bool ret = true;
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 产生并填充随机数
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public static void RandBytes(byte[] buffer, int offset, int count)
        {
            long tick = DateTime.Now.Ticks;
            System.Random rnd = new System.Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = (byte)rnd.Next(0, 0xFF);
            }
        }

        /// <summary>
        /// 将字节流转换为Hex编码的字符串(无分隔符号)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Bytes2HexString(byte[] b)
        {
            int ch = 0;
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ch = (b[i] & 0xFF);
                ret += ch.ToString("X2").ToUpper();
            }

            return ret;
        }

        /// <summary>
        /// 将Hex编码的字符串转换为字节流(无分隔符号)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexString2Bytes(string s)
        {
            if (s.Length % 2 != 0) //非法的字符串
            {
                return null;
            }

            int b = 0;
            string hexstr = "";
            byte[] bytesData = new byte[s.Length / 2];
            for (int i = 0; i < s.Length / 2; i++)
            {
                hexstr = s.Substring(i * 2, 2);
                b = Int32.Parse(hexstr, System.Globalization.NumberStyles.HexNumber) & 0xFF;
                bytesData[i] = (byte)b;
            }

            return bytesData;
        }
    }

    public class ConvertHelper
    {
        /// <summary>
        /// 判断如果不是 "*", 则转为指定的值, 否则默认值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int32 ConvertToInt32(string str, Int32 defVal)
        {
            try
            {
                if ("*" != str)
                {
                    return Convert.ToInt32(str);
                }

                return defVal;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return defVal;
        }

        /// <summary>
        /// 判断如果不是 "*", 则转为指定的值, 否则默认值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToStr(string str, string defVal)
        {
            if ("*" != str)
            {
                return str;
            }

            return defVal;
        }

        /// <summary>
        /// 将日期时间字符串转为整数表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ConvertToTicks(string str, long defVal)
        {
            if ("*" == str)
            {
                return defVal;
            }

            str = str.Replace('$', ':');

            try
            {
                DateTime dt;
                if (!DateTime.TryParse(str, out dt))
                {
                    return 0L;
                }

                return dt.Ticks / 10000;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return 0L;
        }

        /// <summary>
        /// 将日期时间字符串转为整数表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ConvertToTicks(string str)
        {
            try
            {
                DateTime dt;
            if (!DateTime.TryParse(str, out dt))
            {
                return 0L;
            }

            return dt.Ticks / 10000;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return 0L;
        }

        /// <summary>
        /// Unix秒的起始计算毫秒时间(相对系统时间)
        /// </summary>
        private static long UnixStartTicks = ConvertHelper.ConvertToTicks("1970-01-01 08:00");

        /// <summary>
        /// 将Unix秒表示的时间转换为系统毫秒表示的时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long UnixSecondsToTicks(int secs)
        {
            return UnixStartTicks + ((long)secs * 1000);
        }

        /// <summary>
        /// 将Unix秒表示的时间转换为系统毫秒表示的时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long UnixSecondsToTicks(string secs)
        {
            int intSecs = Convert.ToInt32(secs);
            return UnixSecondsToTicks(intSecs);
        }

        /// <summary>
        /// 获取Unix秒表示的当前时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int UnixSecondsNow()
        {
            long ticks = DateTime.Now.Ticks / 10000;
            return SysTicksToUnixSeconds(ticks);
        }

        /// <summary>
        /// 将系统毫秒表示的时间转换为Unix秒表示的时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int SysTicksToUnixSeconds(long ticks)
        {
            long secs = (ticks - UnixStartTicks) / 1000;
            return (int)secs;
        }

        /*
        /// <summary>
        /// 将对象转为TCP协议流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="pool"></param>
        /// <param name="cmdID"></param>
        /// <returns></returns>
            public static TCPOutPacket ObjectToTCPOutPacket<T>(T instance, TCPOutPacketPool pool, int cmdID)
            {
                byte[] bytesCmd = ObjectToBytes<T>(instance);
                return TCPOutPacket.MakeTCPOutPacket(pool, bytesCmd, 0, bytesCmd.Length, cmdID);
            }
         
        /// <summary>
        /// 压缩的大小
        /// </summary>
        public static int MinZipBytesSize = 256;

        /*
        /// <summary>
        /// 将对象转为字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="pool"></param>
        /// <param name="cmdID"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes<T>(T instance)
        {
            try
            {
                byte[] bytesCmd = null;
                if (null == instance)
                {
                    bytesCmd = new byte[0];
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    Serializer.Serialize<T>(ms, instance);
                    bytesCmd = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(bytesCmd, 0, bytesCmd.Length);
                    ms.Dispose();
                    ms = null;
                }

                if (bytesCmd.Length > DataHelper.MinZipBytesSize) //大于256字节的才压缩, 节省cpu占用，想一想，每秒10兆小流量的吐出，都在压缩，cpu占用当然会高, 带宽其实不是问题, 不会达到上限(100兆共享)
                {
                    //zlib压缩算法
                    byte[] newBytes = DataHelper.Compress(bytesCmd);
                    if (null != newBytes)
                    {
                        if (newBytes.Length < bytesCmd.Length)
                        {
                            //System.Diagnostics.DebugMod.WriteLine(string.Format("{0}压缩率: {1}", instance.GetType(), ((double)newBytes.Length / bytesCmd.Length) * 100.0));
                            bytesCmd = newBytes;
                        }
                    }
                }

                return bytesCmd;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return new byte[0];
        }

        /// <summary>
        /// 将字节数据转为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        public static T BytesToObject<T>(byte[] bytesData, int offset, int length)
        {
            if (bytesData.Length == 0) return default(T);

            try
            {
                //zlib解压缩算法
                byte[] copyData = new byte[length];
                DataHelper.CopyBytes(copyData, 0, bytesData, offset, length);
                copyData = DataHelper.Uncompress(copyData);

                MemoryStream ms = new MemoryStream();
                ms.Write(copyData, 0, copyData.Length);
                ms.Position = 0;
                T t = Serializer.Deserialize<T>(ms);
                ms.Dispose();
                ms = null;
                return t;
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return default(T);
        }

        /// <summary>
        /// zlib 压缩算法
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                using (ZOutputStream outZStream = new ZOutputStream(ms, zlibConst.Z_DEFAULT_COMPRESSION))
                {
                    outZStream.Write(bytes, 0, bytes.Length);
                    outZStream.Flush();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// zlib 解压缩算法
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Uncompress(byte[] bytes)
        {
            //小于2个字节肯定是非压缩的
            if (bytes.Length < 2)
            {
                return bytes;
            }

            //判断是否是压缩数据，是才执行解开压缩操作
            if (0x78 != bytes[0])
            {
                return bytes;
            }

            if (0x9C != bytes[1] && 0xDA != bytes[1])
            {
                return bytes;
            }

            using (var ms = new MemoryStream())
            {
                using (ZOutputStream outZStream = new ZOutputStream(ms))
                {
                    outZStream.Write(bytes, 0, bytes.Length);
                    outZStream.Flush();
                }

                return ms.ToArray();
            }
        }
        */
        /// <summary>
        /// UTF8 汉字字节流转成 Unicode 汉字字节流
        /// </summary>
        /// <param name="input"></param>
        /// <see cref="http://hi.baidu.com/hyqsoft/blog/item/263795a164d1728346106464.html"/>
        public static byte[] Utf8_2_Unicode(byte[] input)
        {
            var ret = new List<byte>();
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] >= 240) // 11110xxx
                {
                    //i += 3;
                    //throw new Exception("四字节的 UTF- 8 字符不能转换成两字节的 Unicode 字符！");
                    return null;
                }
                //else if (input[i] >= 224)
                if (input[i] >= 224) // 1110xxxx
                {
                    ret.Add((byte)((input[i + 2] & 63) | ((input[i + 1] & 3) << 6)));
                    ret.Add((byte)((input[i] << 4) | ((input[i + 1] & 60) >> 2)));
                    i += 2;
                }
                else if (input[i] >= 192) // 110xxxxx
                {
                    ret.Add((byte)((input[i + 1] & 63) | ((input[i] & 3) << 6)));
                    ret.Add((byte)((input[i] & 28) >> 2));
                    i += 1;
                }
                else
                {
                    ret.Add(input[i]);
                    ret.Add(0);
                }
            }
            return ret.ToArray();
        }

        /*
        #region 字符串压缩并且做base64转换

        /// <summary>
        /// 将原来的字符串=>字节=>压缩=>base64
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ZipStringToBase64(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "";
                }

                byte[] bytes = new UTF8Encoding().GetBytes(text);

                //zlib压缩算法
                if (bytes.Length > 128)
                {
                    bytes = DataHelper.Compress(bytes);
                }

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return "";
        }

        /// <summary>
        /// 将原来的base64=>字节=>解开压缩=>原字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnZipStringToBase64(string base64)
        {
            try
            {
                if (string.IsNullOrEmpty(base64))
                {
                    return "";
                }

                byte[] bytes = Convert.FromBase64String(base64);
                bytes = DataHelper.Uncompress(bytes);
                return new UTF8Encoding().GetString(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                DebugMod.LogException(e);
            }

            return "";
        }

        #endregion 字符串压缩并且做base64转换
         * */
    }
}
