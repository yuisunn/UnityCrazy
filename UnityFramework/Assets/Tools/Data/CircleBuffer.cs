using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSGame.Tools
{
    public class CircleBuffer : IDisposable
    {
        protected byte[] m_Buffer = null;

        protected int m_iBufSize = 1024 * 128;
        protected int m_iHeadPos = 0;
        protected int m_iTailPos = 0;
        protected int m_nValidCount = 0;
        private int _maxLen = 1024 * 128;
        public CircleBuffer(int buffersize)
        {
            if (buffersize <= 0)
                buffersize = 1024 * 128;
            m_iBufSize = buffersize;
            m_Buffer = new byte[buffersize];
        }

        ~CircleBuffer()
        {
            m_Buffer = null;
        }

        public void Dispose()
        {
            m_Buffer = null;
        }

        public byte GetHeadData()
        {
            return m_Buffer[m_iHeadPos];
        }

        public int	PutData(byte[] byteData, int offset, int len)
        {
            if (len <= 0 || len > _maxLen)
            {
                //PRINT("CCircularBuffer::PutData len is <=0\n");
                return 0;
            }

            while(IsOverFlowCondition(len))
            {
                BufferResize();
            }

            m_nValidCount += len;

            try
            {
                if (IsIndexOverFlow(len))
                {
                    int FirstCopyLen = m_iBufSize - m_iTailPos;
                    int SecondCopyLen = len - FirstCopyLen;
                    Tools.Assert.Check(FirstCopyLen > 0);
                    Array.Copy(byteData, offset, m_Buffer, m_iTailPos, FirstCopyLen);
                    if (SecondCopyLen > 0)
                    {
                        Array.Copy(byteData, offset + FirstCopyLen, m_Buffer, 0, SecondCopyLen);
                        m_iTailPos = SecondCopyLen;
                    }
                    else
                    {
                        m_iTailPos = 0;
                    }
                }
                else
                {
                    Array.Copy(byteData, offset, m_Buffer, m_iTailPos, len);
                    m_iTailPos += len;
                }
            }
            catch(Exception e)
            {
                DebugMod.LogException(e, "CircleBuffer.PutData", false);
            }
            return len;
        }
	    public void	CopyData(byte[] byteData, int dstoffset, int srcoffset, int len)
        {
            Tools.Assert.Check(len > 0 && srcoffset + len <= GetValidCount());

            int fc = m_iBufSize - m_iHeadPos;
            if (srcoffset + len < fc)
            {
                Array.Copy(m_Buffer, m_iHeadPos + srcoffset, byteData, dstoffset, len);
            }
            else if (srcoffset >= fc)
            {
                Array.Copy(m_Buffer, srcoffset - fc, byteData, dstoffset, len);
            }
            else
            {
                int sc = len - (fc - srcoffset);
                Array.Copy(m_Buffer, m_iHeadPos + srcoffset, byteData, dstoffset, fc - srcoffset);
                if (sc > 0)
                {
                    Array.Copy(m_Buffer, 0, byteData, dstoffset + fc - srcoffset, sc);
                }
            }
        }

        public bool GetByte(int offset, out byte byteGet)
        {
            byteGet = 0;
            if (offset < 0 || offset > GetValidCount())
                return false;
            int dstPos = m_iHeadPos + offset;
            if (dstPos < m_iBufSize)
            {
                byteGet = m_Buffer[dstPos];
                return true;
            }
            else
            {
                byteGet = m_Buffer[dstPos - m_iBufSize];
                return true;
            }
        }

	    public int		GetOutData(byte[] byteData, int offset)
        {
            int len = GetValidCount();
            int fc, sc;
            fc = m_iBufSize - m_iHeadPos;
            if (len > fc)
            {
                sc = len - fc;
                Array.Copy(m_Buffer, m_iHeadPos, byteData, offset, fc);
                Array.Copy(m_Buffer, 0, byteData, offset + fc, fc);
                m_iHeadPos = sc;
                Tools.Assert.Check(m_iHeadPos == m_iTailPos);
            }
            else
            {
                Array.Copy(m_Buffer, m_iHeadPos, byteData, offset, len);
                m_iHeadPos += len;
                if (m_iHeadPos == m_iBufSize) 
                    m_iHeadPos = 0;
            }
            return len;
        }
	    public int		PutData(byte data)
        {
            if (IsOverFlowCondition(1))
            {
                BufferResize();
            }

            //int len = 1;
            m_nValidCount++;

            m_Buffer[m_iTailPos++] = data;
            if (m_iTailPos == m_iBufSize)
                m_iTailPos = 0;
            return 1;
        }
	    public bool	HeadIncrease(int increasement=1)
        {
            if (increasement > GetValidCount())
                increasement = GetValidCount();

            m_nValidCount -= increasement;

            if (m_nValidCount <= 0)
            {
                SetEmpty();
            }
            else
            {
                m_iHeadPos += increasement;
                m_iHeadPos %= m_iBufSize;
            }
            return m_iHeadPos != m_iTailPos;
        }

	    public void	SetEmpty()
	    {
		    m_iHeadPos=0;
		    m_iTailPos=0;
		    m_nValidCount = 0;
	    }

	    public int 	GetBufferSize()	
        {
            return m_iBufSize;
        }
	    public int	GetHeadPos()		
        {
            return m_iHeadPos;
        }
	    public int	GetTailPos()
        {
            return m_iTailPos;
        }
	    public int  GetValidCount()
        {
            return m_nValidCount;
        }
	    public byte[]	GetBuffer() { 
            return m_Buffer; 
        }

	    public bool	IsOverFlowCondition(int len)
	    {
		    return (len >= m_iBufSize-GetValidCount()) ? true: false;
	    }
	    public bool	IsIndexOverFlow(int len)
	    {
            return (len + m_iTailPos >= m_iBufSize) ? true : false;
	    }
	    public void	BufferResize()
        {
            int prevBufSize = m_iBufSize;
            m_iBufSize *= 2;
            byte[] byteNewData = new byte[m_iBufSize];
            if (null != byteNewData)
            {
                Array.Copy(m_Buffer, 0, byteNewData, 0, prevBufSize);
                if (m_iTailPos < m_iHeadPos)
                {
                    Array.Copy(m_Buffer, 0, byteNewData, prevBufSize, m_iTailPos);
                    m_iTailPos += prevBufSize;
                }

                m_Buffer = byteNewData;
            }
        }
    }
}
