using System.Reflection;
using Newtonsoft.Json;
using SPSGame;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using SPSGame.CsShare;
using SPSGame.Tools;
using SPSGame.CsShare.Data;

public class NetReader
{
    //class RECORDINFO
    //{
    //    public int RecordSize { get; set; }
    //    public int RecordReadSize { get; set; }
    //    public RECORDINFO(int _RecordSize, int _RecordReadSize)
    //    {
    //        RecordSize = _RecordSize;
    //        RecordReadSize = _RecordReadSize;
    //    }
    //}

    //private PackageHead _head;
    private IHeadFormater _formater;

    private byte[] _packParseBuff = null;

    private byte[] _packParseBuffZip = null;
    private int streamPos = 0;
    //Stack<RECORDINFO> RecordStack = new Stack<RECORDINFO>();

    public int PackParseLen { get; set; }
    //public int PackParseZipLen { get; set; }


    public byte[] ParseBuff
    {
        get
        {
            return _packParseBuff;
        }
    }

    public byte[] ParseBuffZip
    {
        get
        {
            return _packParseBuffZip;
        }
    }


    //public NetReader()
    //    : this(new DefaultHeadFormater())
    //{
    //}

    public NetReader(IHeadFormater formater)
    {
        _formater = formater;
        _packParseBuff = new byte[(int)TcpCommonEnum.SOCKET_BUF_SIZE_SERVER_SEND * 16];
        _packParseBuffZip = new byte[(int)TcpCommonEnum.SOCKET_BUF_SIZE_SERVER_SEND];
    }

    //public bool Success
    //{
    //    get { return StatusCode == 0; }
    //}

    //public int StatusCode
    //{
    //    get { return _head == null ? 10000 : _head.StatusCode; }
    //}

    //public string Description
    //{
    //    get { return _head == null ? "" : _head.Description; }
    //}

    //public int ActionId
    //{
    //    get { return _head == null ? 0 : _head.ActionId; }
    //}

    //public int RmId
    //{
    //    get { return _head == null ? 0 : _head.MsgId; }
    //}

    //public string StrTime
    //{
    //    get { return _head == null ? "" : _head.StrTime; }
    //}

    //private object Data;

    public void Reset()
    {
        streamPos = 0;
    }

    /// <summary>
    /// 设置字节流，并解开包的头部信息
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="type"></param>
    /// <param name="respContentType"></param>
    /// <returns></returns>
    //public bool pushNetStream(byte[] buffer, NetworkType type, ResponseContentType respContentType)
    //{
    //    if (respContentType == ResponseContentType.Json)
    //    {
    //        string jsonData = Encoding.UTF8.GetString(buffer);
    //        DebugMod.Log("response json:" + jsonData);
    //        if (!_formater.TryParse(jsonData, type, out _head, out Data))
    //        {
    //            SPSGame.DebugMod.LogError(" Failed: NetReader's pushNetStream parse error.");
    //            return false;
    //        }
    //        SetBuffer(new byte[0]);
    //        DebugMod.Log("parse json ok." + _head.Description);
    //        return true;
    //    }
    //    byte[] data;
    //    if (!_formater.TryParse(buffer, out _head, out data))
    //    {
    //        SPSGame.DebugMod.LogError(" Failed: NetReader's pushNetStream parse head error: buffer Length " + buffer.Length);
    //        return false;
    //    }
    //    SetBuffer(data);
    //    return true;
    //}

    /// <summary>
    /// Gzip解压
    /// </summary>
    /// <param name="buf"></param>
    /// <returns></returns>
    public static bool Decompression(byte[] bufSrc, byte[] bufDst, out int len)
    {
        GZipInputStream zip = new GZipInputStream(new MemoryStream(bufSrc));

        len = zip.Read(bufDst, 0, bufDst.Length);

        if (len == bufDst.Length && zip.Read(bufDst, 0, bufDst.Length) > 0)
        {
            len = 0;
            DebugMod.LogError("decompress data too big!");
            return false;
        }

        return true;
    }

    //public byte[] Buffer
    //{
    //    get { return _bytes ?? new byte[0]; }
    //}

    //public T readValue<T>()
    //{
    //    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Data));
    //}

    //public bool recordBegin()
    //{
    //    int nLen = sizeof(int);
    //    if (streamPos + nLen > PackParseLen)
    //    {
    //        DebugMod.Log(" Failed: 长度越界 NetReader: recordBegin ");
    //        return false;
    //    }

    //    int nRecoredSize = getInt();

    //    RECORDINFO info = new RECORDINFO(nRecoredSize, nLen);
    //    RecordStack.Push(info);

    //    return nRecoredSize > 4;
    //}

    //public void recordEnd()
    //{
    //    if (RecordStack.Count > 0)
    //    {
    //        RECORDINFO info = RecordStack.Pop();
    //        this.streamPos += (info.RecordSize - info.RecordReadSize);
    //        if (RecordStack.Count > 0)
    //        {
    //            RECORDINFO parent = RecordStack.Peek();
    //            parent.RecordReadSize += info.RecordSize - 4;
    //        }
    //    }
    //}

    //public byte getRecordNumber()
    //{
    //    byte bt = _bytes[this.streamPos];
    //    this.streamPos += 1;
    //    return bt;
    //}

    public bool getBool()
    {
        int nLen = sizeof(bool);
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界  NetReader: getBYTE ");
            return false;
        }

        bool value = BitConverter.ToBoolean(ParseBuff, this.streamPos);
        this.streamPos += nLen;

        return value;
    }

    public char getChar()
    {
        int nLen = sizeof(char);
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界  NetReader: getBYTE ");
            return '\0';
        }

        char value = BitConverter.ToChar(ParseBuff, this.streamPos);
        this.streamPos += nLen;

        return value;
    }

    public byte getByte()
    {
        int nLen = sizeof(byte);
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界  NetReader: getBYTE ");
            return 0;
        }

        byte bt = ParseBuff[this.streamPos];
        this.streamPos += nLen;

        return bt;
    }

    public sbyte getSByte()
    {
        int nLen = sizeof(sbyte);
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界  NetReader: getSByte ");
            return 0;
        }

        sbyte bt = Convert.ToSByte(ParseBuff[this.streamPos]);
        this.streamPos += nLen;

        //if (this.RecordStack.Count > 0)
        //{
        //    RECORDINFO info = RecordStack.Peek();

        //    if (info.RecordReadSize + nLen > info.RecordSize)
        //    {
        //        DebugMod.Log(" Failed: 记录长度越界 NetReader: getSByte");
        //        return 0;
        //    }

        //    info.RecordReadSize += nLen;
        //}

        return bt;
    }

    public short getShort()
    {
        int nLen = sizeof(short);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getShort");
            return 0;
        }

        short val = BitConverter.ToInt16(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (this.RecordStack.Count > 0)
        //{
        //    RECORDINFO info = RecordStack.Peek();

        //    if (info.RecordReadSize + nLen > info.RecordSize)
        //    {
        //        DebugMod.Log(" Failed: 记录长度越界 NetReader: getShort");
        //        return 0;
        //    }

        //    info.RecordReadSize += nLen;
        //}

        return val;
    }

    public ushort getUShort()
    {
        int nLen = sizeof(ushort);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getUShort");
            return 0;
        }

        ushort val = BitConverter.ToUInt16(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (this.RecordStack.Count > 0)
        //{
        //    RECORDINFO info = RecordStack.Peek();

        //    if (info.RecordReadSize + nLen > info.RecordSize)
        //    {
        //        DebugMod.Log(" Failed: 记录长度越界 NetReader: getUShort");
        //        return 0;
        //    }

        //    info.RecordReadSize += nLen;
        //}

        return val;
    }

    public int getInt()
    {
        int nLen = sizeof(int);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getInt");
            return 0;
        }

        int val = BitConverter.ToInt32(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getInt" + ActionId);
        //    return 0;
        //}
        return val;
    }

    //private bool CheckRecordSize(int nLen)
    //{
    //    if (this.RecordStack.Count > 0)
    //    {
    //        RECORDINFO info = RecordStack.Peek();
    //        if (info.RecordReadSize + nLen > info.RecordSize)
    //        {
    //            return true;
    //        }
    //        info.RecordReadSize += nLen;
    //    }
    //    return false;
    //}

    public uint getUInt()
    {
        int nLen = sizeof(uint);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getUInt");
            return 0;
        }

        uint val = BitConverter.ToUInt32(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getUInt");
        //    return 0;
        //}

        return val;
    }
    public float getFloat()
    {
        int nLen = sizeof(float);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getFloat");
            return 0;
        }

        float val = BitConverter.ToSingle(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getFloat");
        //    return 0;
        //}

        return val;
    }

    public double getDouble()
    {
        int nLen = sizeof(double);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getDouble");
            return 0;
        }

        double val = BitConverter.ToDouble(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getDouble");
        //    return 0;
        //}
        return val;
    }

    public ulong getULong()
    {
        int nLen = sizeof(ulong);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getULong");
            return 0;
        }

        ulong val = BitConverter.ToUInt64(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getULong");
        //    return 0;
        //}
        return val;
    }
    public Int64 getLong()
    {
        return readInt64();
    }
    public Int64 readInt64()
    {
        int nLen = sizeof(Int64);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: readInt64");
            return 0;
        }

        Int64 val = BitConverter.ToInt64(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: readInt64");
        //    return 0;
        //}
        return val;
    }

    public ushort ReverseUShort()
    {
        ushort data = getUShort();
        byte[] array = System.BitConverter.GetBytes(data);
        Array.Reverse(array);
        return System.BitConverter.ToUInt16(array, 0);
    }
    public string ReadReverseString()
    {
        ushort nLen = this.ReverseUShort();
        return this.getString(nLen);
    }
    public DateTime getDateTime()
    {
        int nLen = sizeof(long);
        if (streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getDateTime");
            return DateTime.MinValue;
        }

        long val = BitConverter.ToInt64(ParseBuff, this.streamPos);
        streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getDateTime");
        //    return DateTime.MinValue;
        //}
        return FromUnixTime(Convert.ToDouble(val));
    }

    private const long UnixEpoch = 621355968000000000L;
    private static readonly DateTime UnixEpochDateTime = new DateTime(UnixEpoch);
    private static DateTime FromUnixTime(double unixTime)
    {
        return (UnixEpochDateTime + TimeSpan.FromSeconds(unixTime)).ToLocalTime();
    }

    public string getString(ushort nLen)
    {
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getString");
            return null;
        }

        string str = Encoding.UTF8.GetString(ParseBuff, this.streamPos, nLen);
        this.streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: getString");
        //    return null;
        //}
        return str;
    }

    public string readString()
    {
        ushort nLen = this.getUShort();
        return this.getString(nLen);
    }

    public byte[] readBytes()
    {
        int nLen = this.getInt();
        return this.readBytes(nLen);
    }

    public byte[] readBytes(int nLen)
    {
        if (this.streamPos + nLen > PackParseLen)
        {
            DebugMod.LogError(" Failed: 长度越界 NetReader: getString");
            return null;
        }
        byte[] buffer = new byte[nLen];
        Array.Copy(ParseBuff, this.streamPos, buffer, 0, buffer.Length);
        this.streamPos += nLen;

        //if (CheckRecordSize(nLen))
        //{
        //    DebugMod.Log(" Failed: 记录长度越界 NetReader: readBytes");
        //    return new Byte[0];
        //}
        return buffer;
    }

    public bool ReadData<T>(T obj)
    {
        PropertyInfo[] props = obj.GetType().GetProperties();
        foreach (PropertyInfo pi in props)
        {
            NetDataAttribute atrr = Attribute.GetCustomAttribute(pi, typeof(NetDataAttribute)) as NetDataAttribute;
            if (atrr == null)
                continue;

            if (pi.PropertyType == typeof(string))
            {
                ushort len = this.getUShort();
                pi.SetValue(obj, this.getString(len), null);
            }
            else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(Int32))
            {
                pi.SetValue(obj, this.getInt(), null);
            }
            else if (pi.PropertyType == typeof(short) || pi.PropertyType == typeof(Int16))
            {
                pi.SetValue(obj, this.getShort(), null);
            }
            else if (pi.PropertyType == typeof(uint) || pi.PropertyType == typeof(UInt32))
            {
                pi.SetValue(obj, this.getUInt(), null);
            }
            else if (pi.PropertyType == typeof(ushort) || pi.PropertyType == typeof(UInt16))
            {
                pi.SetValue(obj, this.getUShort(), null);
            }
            else if (pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(Boolean))
            {
                pi.SetValue(obj, this.getBool(), null);
            }
            else if (pi.PropertyType == typeof(long) || pi.PropertyType == typeof(Int64))
            {
                pi.SetValue(obj, this.getLong(), null);
            }
            else if (pi.PropertyType == typeof(ulong) || pi.PropertyType == typeof(UInt64))
            {
                pi.SetValue(obj, this.getULong(), null);
            }
            else if (pi.PropertyType == typeof(byte) || pi.PropertyType == typeof(Byte))
            {
                pi.SetValue(obj, this.getByte(), null);
            }
            else if (pi.PropertyType == typeof(char) || pi.PropertyType == typeof(Char))
            {
                pi.SetValue(obj, this.getChar(), null);
            }
            else if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(Double))
            {
                pi.SetValue(obj, this.getDouble(), null);
            }
            else if (pi.PropertyType == typeof(float) || pi.PropertyType == typeof(Single))
            {
                pi.SetValue(obj, this.getFloat(), null);
            }
        }
        return true;
    }

    public int Msg2NetData<T>(T obj)
    {
        if (null == obj)
            return 0;

        if (!ReadData<T>(obj))
            return 0;

        return 1;
    }

    public int Msg2NetDataList<T>(List<T> list) where T : new()
    {
        if (list == null)
            return 0;

        byte flag1 = this.getByte();
        byte flag2 = this.getByte();
        if (flag1 != (byte)0x33 || flag2 != (byte)0x66)
            return 0;

        int listcount = this.getInt();
        if (listcount > 1000000 || 0 > listcount)
            return 0;

        for (int i = 0; i < listcount; i++)
        {
            T obj = new T();
            if (!ReadData<T>(obj))
                return 0;
            list.Add(obj);
        }

        return 1;
    }
}
