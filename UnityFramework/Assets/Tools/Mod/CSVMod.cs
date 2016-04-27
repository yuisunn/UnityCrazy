using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SPSGame.Tools
{
    public class CSVMod
    {
        private List<Dictionary<string, string>> csvInfo; //存储解析出的csv信息
        private List<string> csvInfoName; //存储csv信息的名称
        private string csvStr; //csv字符串信息
		private string csvFilePath; //csv文件路径
		private Encoding encoding; //编码格式

        /// <summary>
        /// 默认构造方法
        /// </summary>
        public CSVMod()
        {
            csvInfo = new List<Dictionary<string, string>>();
            csvInfoName = new List<string>();
            csvStr = "";
            csvFilePath = "";
			encoding = Encoding.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvStr">csv字符串信息</param>
		/// <param name="isCSVFile">读取的是否为csv文件</param>
        public CSVMod(string csvStr, bool isCSVFile = false)
        {
            csvInfo = new List<Dictionary<string, string>>();
            csvInfoName = new List<string>();

            if (isCSVFile)
            {
                this.csvFilePath = csvStr;
                this.encoding = Encoding.Default;
            }
            else
            {
                this.csvStr = csvStr;
            }
            
        }
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="csvFilePath">csv文件路径</param>
        /// <param name="encoding">编码方式</param>
		public CSVMod(string csvFilePath, Encoding encoding)
		{
			csvInfo = new List<Dictionary<string, string>>();
            csvInfoName = new List<string>();
            this.csvFilePath = csvFilePath;
            this.encoding = encoding;
		}
		
        /// <summary>
        /// csv字符串的属性
        /// </summary>
        public string CsvStr
        {
            set
            {
                csvStr = value;
            }
        }
		
		/// <summary>
        /// csv文件路径的属性
        /// </summary>
		public string CsvFilePath
		{
			set
			{
				csvFilePath = value;
			}
		}
		
		/// <summary>
        /// csv文件解析时的编码格式
        /// </summary>
        public Encoding FileEncoding
        {
            set
            {
                encoding = value;
            }
        }
		
        /// <summary>
        /// csv数据的行数
        /// </summary>
        public int RowCount
        {
            get
            {
                return csvInfo.Count;
            }
        }

        
        /// <summary>
        ///csv数据的列数 
        /// </summary>
        public int ColCount
        {
            get
            {
                if (0 == csvInfo.Count)
                {
                    return 0;
                }
                else
                {
                    return csvInfo[0].Count;
                }
            }
        }

		/// <summary>
        /// 返回解析的所有csv数据
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetAllData ()
        {
            return csvInfo;
        }
		
        /// <summary>
        /// 返回csv中第rowNum行数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetRowData (int rowNum)
        {
            //输入的行数超出范围
            if (rowNum < 0 || rowNum > csvInfo.Count)
            {
                return null;
            }

            return csvInfo[rowNum];
        }

        /// <summary>
        /// 返回csv中名称为infoName的列数据
        /// </summary>
        /// <param name="infoName">表示想要获取的列名</param>
        /// <returns></returns>
        public List<string> GetColData (string infoName)
        {
            if (0 == csvInfo.Count)
            {
                return null;
            }

            if (!csvInfo[0].ContainsKey (infoName))
            {
                return null;
            }

            List<string> colData = new List<string>();

            foreach (Dictionary<string, string> dic in csvInfo)
            {
                colData.Add(dic[infoName]);
            }

            return colData;
        }

        /// <summary>
        /// 返回csv中某一行某一列的数据
        /// </summary>
        /// <param name="rowNum">行数</param>
        /// <param name="infoName">列名</param>
        /// <returns></returns>
        public string GetData (int rowNum, string infoName)
        {
            //输入的行数超出范围
            if (rowNum < 0 || rowNum >= csvInfo.Count)
            {
                return null;
            }

            if (!csvInfo[0].ContainsKey(infoName))
            {
                return null;
            }

            return csvInfo[rowNum][infoName];
        }
		
		/// <summary>
        /// 返回csv中某一列值为某特定值infoValue的信息，返回的最大行数为maxRowCount
        /// </summary>
        /// <param name="infoName">列名</param>
        /// <param name="infoValue">列值</param>
		/// <param name="maxRowCount">最大行数</param>
        /// <returns></returns>
		public List<Dictionary<string, string>> GetCertainColValueData (string infoName, string infoValue, int maxRowCount = -1)
		{
			if (0 == csvInfo.Count)
            {
                return null;
            }

            if (!csvInfo[0].ContainsKey (infoName))
            {
                return null;
            }
			
			int rowCount = 0;
			
			List<Dictionary<string, string>> res = new List<Dictionary<string, string>> ();
			
			for (int i = 0; i < csvInfo.Count; ++i)
			{
				if (infoValue == csvInfo[i][infoName])
				{
					res.Add (csvInfo[i]);
					++rowCount;
				}
				
				if (-1 != maxRowCount && maxRowCount >= 0 && rowCount >= maxRowCount)
				{
					break;
				}
			}
			
			return res;
		}

        /// <summary>
        /// 根据多列的值进行检索，最多返回maxRowCount行,maxRowCount = -1表示全部返回
        /// </summary>
        /// <param name="colValueDic"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetRowDataByMultiColValue (Dictionary<string, string> colValueDic, int maxRowCount = -1)
        {
            if (0 == csvInfo.Count)
            {
                return null;
            }

            int rowCount = 0;

            List<Dictionary<string, string>> res = new List<Dictionary<string,string>>();

            for (int i = 0; i < csvInfo.Count; ++i )
            {
                bool isNeed = true;

                foreach (KeyValuePair<string, string> pair in colValueDic)
                {
                    string infoName = pair.Key;
                    string infoVal = pair.Value;

                    if (!csvInfo[i].ContainsKey(infoName))
                    {
                        return null;
                    }

                    if (infoVal != csvInfo[i][infoName])
                    {
                        isNeed = false;
                        break;
                    }
                }

                if (isNeed)
                {
                    res.Add(csvInfo[i]);
                    ++rowCount;
                }

                if (-1 != maxRowCount && rowCount >= 0 && rowCount >= maxRowCount)
                {
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 加载读取csv字符串
        /// </summary>
        public bool LoadCsvStr ()
        {
            if (String.IsNullOrEmpty (this.csvStr))
            {
                //csv文件路径不存在
				DebugMod.LogError(new Exception(string.Format("csv信息为空")));
				return false;
            }

            string[] csvInfoArray = csvStr.Split(new string[]{"\r\n"}, StringSplitOptions.None);

            string csvInfoNameDataLine = csvInfoArray[0];

            ParseCsvInfoName(csvInfoNameDataLine); //从csv文件中解析出信息的名称

            string csvDataLine = ""; //表示一行csv文件数据

            for (int i = 1; i < csvInfoArray.Length; ++i )
            {
                string fileDataLine; //表示读取出的一行数据

                fileDataLine = csvInfoArray[i];

                if (String.IsNullOrEmpty(fileDataLine)) //表示读取到了文件结尾，跳出循环
                {
                    break;
                }

                if ("" == csvDataLine)
                {
                    csvDataLine = fileDataLine;
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;
                }

                if (!IfOddQuotation(csvDataLine)) //此时csvDataLine中包含偶数个引号，表示是完整的一行csv数据,如果csvDataLine中包含奇数个引号，表示不是完整的一行csv数据，数据被回车换行符分割。
                {
                    AddNewDataLine(csvDataLine);
                    csvDataLine = "";
                }
            }

            if (csvDataLine.Length > 0) //在读取玩全部csv数据后，如果csvDataLine中仍然存在数据，表示数据中出现了奇数个引号，csv文件格式错误。
            {
                //csv文件格式错误
				DebugMod.LogError(new Exception(string.Format("csv文件格式错误")));
				return false;
            }
			
			return true;
        }
		
		 /// <summary>
        /// 加载读取csv文件
        /// </summary>
        public bool LoadCsvFile ()
        {
            if (String.IsNullOrEmpty (this.csvFilePath))
            {
                //csv文件路径不存在
                DebugMod.LogError(new Exception(string.Format("csv文件路径不存在, csv文件路径:{0}", this.csvFilePath)));
				return false;
            }
            else if (!File.Exists(this.csvFilePath))
            {
                //csv文件不存在
                DebugMod.LogError(new Exception(string.Format("csv文件不存在, csv文件路径:{0}", this.csvFilePath)));
				return false;
            }

            if (null == this.encoding)
            {
                this.encoding = Encoding.Default;
            }

            StreamReader sr = new StreamReader(this.csvFilePath, this.encoding);

            string csvInfoNameDataLine = sr.ReadLine();

            if (String.IsNullOrEmpty (csvInfoNameDataLine)) //表示此文件为空
            {
                DebugMod.LogError(new Exception(string.Format("此csv文件为空, csv文件路径:{0}", this.csvFilePath)));

                sr.Close();
                return false;
            }

            ParseCsvInfoName(csvInfoNameDataLine); //从csv文件中解析出信息的名称

            string csvDataLine = ""; //表示一行csv文件数据

            while (true)
            {
                string fileDataLine; //表示读取出的一行数据

                fileDataLine = sr.ReadLine();

                if (String.IsNullOrEmpty(fileDataLine)) //表示读取到了文件结尾，跳出循环
                {
                    break;
                }

                if ("" == csvDataLine)
                {
                    csvDataLine = fileDataLine;
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;
                }

                if (!IfOddQuotation(csvDataLine)) //此时csvDataLine中包含偶数个引号，表示是完整的一行csv数据,如果csvDataLine中包含奇数个引号，表示不是完整的一行csv数据，数据被回车换行符分割。
                {
                    AddNewDataLine(csvDataLine);
                    csvDataLine = "";
                }
            }

            sr.Close();

            if (csvDataLine.Length > 0) //在读取玩全部csv数据后，如果csvDataLine中仍然存在数据，表示数据中出现了奇数个引号，csv文件格式错误。
            {
                //csv文件格式错误
                DebugMod.LogError(new Exception(string.Format("csv文件格式错误, csv文件路径:{0}", this.csvFilePath)));
				return false;
            }
			
			return true;
        }
        
        /// <summary>
        /// 解析csv信息名称
        /// </summary>
        /// <param name="dataLine">存储csv信息名称的数据行</param>
        private void ParseCsvInfoName (string dataLine)
        {
            string[] infoNames = dataLine.Split(','); //通过逗号将csv信息的名称分开

            for (int i = 0; i < infoNames.Length; ++i)
            {
                csvInfoName.Add(infoNames[i].Trim());
            }
        }

        /// <summary>
        /// 判断数据行中是否存在奇数个引号
        /// </summary>
        /// <param name="dataLine">数据行</param>
        /// <returns>是否为奇数个引号</returns>
        private bool IfOddQuotation (string dataLine)
        {
            int quotationCount = 0;
            bool isOddQuota = false;

            for (int i = 0; i < dataLine.Length; ++i)
            {
                if ('\"' == dataLine[i])
                {
                    ++quotationCount;
                }
            }

            if (1 == quotationCount % 2)
            {
                isOddQuota = true;
            }

            return isOddQuota;
        }

        /// <summary>
        /// 判断数据行是否以奇数个引号开始
        /// </summary>
        /// <param name="dataLine"></param>
        /// <returns></returns>
        private bool IfOddQuotationStart (string dataLine)
        {
            int quotationCount = 0;
            bool isOddQuotaStart = false;

            for (int i = 0; i < dataLine.Length; ++i)
            {
                if ('\"' == dataLine[i])
                {
                    ++quotationCount;
                }
                else
                {
                    break;
                }
            }

            if (1 == quotationCount % 2)
            {
                isOddQuotaStart = true;
            }

            return isOddQuotaStart;
        }


        /// <summary>
        /// 判断数据行是否以奇数个引号结束
        /// </summary>
        /// <param name="dataLine"></param>
        /// <returns></returns>
        private bool IfOddQuotationEnd (string dataLine)
        {
            int quotationCount = 0;
            bool isOddQuotationEnd = false;

            for (int i = dataLine.Length - 1; i >= 0; --i)
            {
                if ('\"' == dataLine[i])
                {
                    ++quotationCount;
                }
                else
                {
                    break;
                }
            }

            if (1 == quotationCount % 2)
            {
                isOddQuotationEnd = true;
            }

            return isOddQuotationEnd;
        }


        /// <summary>
        /// 将一行csv数据加入到csvInfo中
        /// </summary>
        /// <param name="newDataLine">代表一行csv数据</param>
       
        private void AddNewDataLine (string newDataLine)
        {
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            string[] rowDataArray = newDataLine.Split(','); //将一行数据通过逗号分隔成多个数据块

            bool isOddQuotaStart = false; //表示是否以奇数个引号开始
            string cellData = "";
            int nameIndex = 0; //表示csv信息名称的下标索引


            for (int i = 0; i < rowDataArray.Length; ++i)
            {
                if (isOddQuotaStart)
                {
                    //因为数据块用逗号分割，所以组合数据块时要在前面加上逗号
                    cellData += "," + rowDataArray[i];

                    //是否以奇数个引号结尾
                    if (IfOddQuotationEnd(rowDataArray[i]))
                    {
                        rowData.Add(csvInfoName[nameIndex], GetHandleData (cellData));
                        ++nameIndex;
                        isOddQuotaStart = false;
                        continue;
                    }
                }
                else
                {
                    //是否以奇数个引号开始
                    if (IfOddQuotationStart (rowDataArray[i]))
                    {
                        //是否以奇数个引号结束，不能是一个双引号，并且不是奇数个引号
                        if (IfOddQuotationEnd (rowDataArray[i]) && rowDataArray[i].Length > 2 && !IfOddQuotation (rowDataArray[i]))
                        {
                            rowData.Add(csvInfoName[nameIndex], GetHandleData(rowDataArray[i]));
                            ++nameIndex;
                            isOddQuotaStart = false;
                            continue;
                        }
                        else
                        {
                            isOddQuotaStart = true;
                            cellData = rowDataArray[i];
                            continue;
                        }
                    }
                    else
                    {
                        rowData.Add(csvInfoName[nameIndex], GetHandleData (rowDataArray[i]));
                        ++nameIndex;
                    }
                }
            }

            //将一行解析好的数据存放在csvInfo中
            csvInfo.Add(rowData);
        }


        /// <summary>
        /// 去掉数据块的首尾引号，一对双引号变为单个单引号
        /// </summary>
        /// <param name="fileCellData"></param>
        /// <returns></returns>
        private string GetHandleData (string fileCellData)
        {
            if ("" == fileCellData)
            {
                return "";
            }

            if (IfOddQuotationStart (fileCellData))
            {
                if (IfOddQuotationEnd (fileCellData))
                {
                    return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉数据块的首尾引号，一对双引号变为单个单引号
                }
                else
                {
					DebugMod.LogError(new Exception(string.Format("csv数据引号无法匹配, 无法匹配数据:{0}", fileCellData)));
                }
            }

            return fileCellData;
        }

    }
}