using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;


namespace SPSGame.Tools
{
    public class XMLMod
    {
        /// <summary>
        /// 获取XElement文件树节点段XElement的列表
        /// </summary>
        /// <param name="XElement">XElement文件载体</param>
        /// <param name="newroot">要查找的独立节点</param>
        /// <returns>XElement列表</returns>
        public static List<XElement> GetXElementList(XElement xElement, string newroot = null)
        {
            List<XElement> xmlItemList = new List<XElement>();

            if (xElement != null)
            {
                IEnumerable<XElement> xmlItems = null;
                if (null == newroot)
                {
                    xmlItems = xElement.Elements();
                }
                else
                {
                    xmlItems = xElement.DescendantsAndSelf(newroot);
                }
                if (xmlItems != null)
                {
                    foreach (var xmlItem in xmlItems)
                    {
                        xmlItemList.Insert(xmlItemList.Count, xmlItem);
                    }
                }
            }
            else
            {
                DebugMod.LogError(new Exception(string.Format("GetXElement异常, 读取: {0} 失败, xml节点名: {1}", newroot, GetXElementNodePath(xElement))));
            }

            return xmlItemList;
        }


        /// <summary>
        /// 获取XElement文件树节点段XElement
        /// </summary>
        /// <param name="XElement">XElement文件载体</param>
        /// <param name="newroot">要查找的独立节点</param>
        /// <returns>独立节点XElement</returns>
        public static XElement GetXElement(XElement XElement, string newroot)
        {
            if (XElement != null)
            {
                IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
                if (xmlItems == null)
                    return null;
                foreach (var xmlItem in xmlItems)
                {
                    return xmlItem;
                }

                return null;
            }

            return null;
        }


        /// <summary>
        /// 获取XElement文件树节点段XElement
        /// </summary>
        /// <param name="xml">XElement文件载体</param>
        /// <param name="mainnode">要查找的主节点</param>
        /// <param name="attribute">主节点条件属性名</param>
        /// <param name="value">主节点条件属性值</param>
        /// <returns>以该主节点为根的XElement</returns>
        public static XElement GetXElement(XElement XElement, string newroot, string attribute, string value)
        {
            if (XElement != null)
            {
                IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
                if (xmlItems != null)
                {
                    foreach (var xmlItem in xmlItems)
                    {
                        XAttribute attrib = null;
                        if (xmlItem != null)
                            attrib = xmlItem.Attribute(attribute);
                        if (null != attrib && attrib.Value == value)
                        {
                            return xmlItem;
                        }
                    }
                }
                return null;
            }
            else
            {
                DebugMod.LogError(new Exception(string.Format("GetXElement异常, 读取: {0}/{1}={2} 失败, xml节点名: {3}", newroot, attribute, value, GetXElementNodePath(XElement))));
                return null;
            }

        }


        /// <summary>
        /// 获取XElement文件树节点段XElement
        /// </summary>
        /// <param name="xml">XElement文件载体</param>
        /// <param name="mainnode">要查找的主节点</param>
        /// <param name="attribute1">主节点条件属性名1</param>
        /// <param name="value1">主节点条件属性值1</param>
        /// <param name="attribute2">主节点条件属性名2</param>
        /// <param name="value2">主节点条件属性值2</param>
        /// <returns>以该主节点为根的XElement</returns>
        public static XElement GetXElement(XElement XElement, string newroot, string attribute1, string value1, string attribute2, string value2)
        {
            if (XElement != null)
            {
                IEnumerable<XElement> xmlItems = XElement.DescendantsAndSelf(newroot);
                foreach (var xmlItem in xmlItems)
                {
                    XAttribute attrib1 = xmlItem.Attribute(attribute1);
                    XAttribute attrib2 = xmlItem.Attribute(attribute2);
                    if (null != attrib1 && attrib1.Value == value1)
                    {
                        if (null != attrib2 && attrib2.Value == value2)
                        {
                            return xmlItem;
                        }
                    }
                }

                return null;
            }
            else
            {
                DebugMod.LogError(new Exception(string.Format("GetXElement异常, 读取: {0}/{1}={2}/{3}={4} 失败, xml节点名: {5}", newroot, attribute1, value1, attribute2, value2, GetXElementNodePath(XElement))));
                return null;
            }

        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static XAttribute GetAttribute(XElement XElement, string attribute)
        {
            if (null == XElement) return null;

            try
            {
                XAttribute attrib = XElement.Attribute(attribute);
                if (null == attrib)
                {
                    DebugMod.LogError(new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attribute, GetXElementNodePath(XElement))));
                    return null;
                }

                return attrib;
            }
            catch 
            {
                DebugMod.LogError(new Exception(string.Format("读取属性: {0} 失败, xml节点名: {1}", attribute, GetXElementNodePath(XElement))));
                return null;
            }

        }


        /// <summary>
        /// 安全获取xml文本字符串
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetXElementAttributeStr(XElement XElement, string attributeName)
        {
            XAttribute attrib = GetAttribute(XElement, attributeName);
            if (null == attrib) return "";
            return (string)attrib;
        }


        /// <summary>
        /// 安全获取xml整型数值
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static int GetXElementAttributeInt(XElement XElement, string attributeName)
        {
            XAttribute attrib = GetAttribute(XElement, attributeName);
            if (null == attrib) return -1;
            string str = (string)attrib;
            if (null == str || str == "") return -1;
			int nReturn = 0;
			if(int.TryParse(str,out nReturn))
			{
				return nReturn;
			}
			else
			{
                DebugMod.LogError(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
				return -1;
			}
        }


        /// <summary>
        /// 安全获取xml长整型数值
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static long GetXElementAttributeLong(XElement XElement, string attributeName)
        {
            XAttribute attrib = GetAttribute(XElement, attributeName);
            if (null == attrib) return -1;
            string str = (string)attrib;
            if (null == str || str == "") return -1;
			long nReturn = 0;
			if(long.TryParse(str,out nReturn))
			{
				return nReturn;
			}
			else
			{
                DebugMod.LogError(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
				return -1;
			}
        }


        /// <summary>
        /// 安全获取xml整型数值
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static double GetXElementAttributeDouble(XElement XElement, string attributeName)
        {
            XAttribute attrib = GetAttribute(XElement, attributeName);
            if (null == attrib) return -1;
            string str = (string)attrib;
            if (null == str || str == "") return -1;
			double nReturn = 0;
			if(double.TryParse(str,out nReturn))
			{
				return nReturn;
			}
			else
			{
                DebugMod.LogError(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
				return -1;
			}
        }


        /// <summary>
        /// 安全获取xml整型数值
        /// </summary>
        /// <param name="XElement"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static float GetXElementAttributeFloat(XElement XElement, string attributeName)
        {
            XAttribute attrib = GetAttribute(XElement, attributeName);
            if (null == attrib) return -1.0f;
            string str = (string)attrib;
            if (null == str || str == "") return -1.0f;
            float nReturn = 0.0f;
            if (float.TryParse(str, out nReturn))
            {
                return nReturn;
            }
            else
            {
                DebugMod.LogError(string.Format("读取属性: {0} 失败, xml节点名: {1}", attributeName, GetXElementNodePath(XElement)));
                return -1.0f;
            }
        }


        public static List<int> GetXElementAttributeIntList(XElement xElement, string attributeName, string newroot = null)
        {
            List<int> result = new List<int>();

            if (xElement != null)
            {
                IEnumerable<XElement> xmlItems = null;
                if (null == newroot)
                {
                    xmlItems = xElement.Elements();
                }
                else
                {
                    xmlItems = xElement.DescendantsAndSelf(newroot);
                }
                if (xmlItems != null)
                {
                    foreach (var xmlItem in xmlItems)
                    {
                        result.Add(GetXElementAttributeInt(xmlItem, attributeName));
                    }
                }
            }
            else
            {
                DebugMod.LogError(new Exception(string.Format("GetXElement异常, 读取: {0} 失败, xml节点名: {1}", newroot, GetXElementNodePath(xElement))));
            }

            return result;
        }


        /// <summary>
        /// 获取指定的xml节点的节点路径
        /// </summary>
        /// <param name="element"></param>
        static string GetXElementNodePath(XElement element)
        {
            if (element == null)
                return null;
            try
            {
                string path = element.Name.ToString();
                element = element.Parent;
                while (null != element)
                {
                    path = element.Name.ToString() + "/" + path;
                    element = element.Parent;
                }

                return path;
            }
            catch (Exception e)
            {
                DebugMod.LogError(e.Message);
            }
            return null;
        }


        /// <summary>
        /// 保存XML文件到本地
        /// </summary>
        /// <param name="path"></param>
        /// <param name="root"></param>
        public static void SaveXElementToFile(string path, XElement root)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(root.ToString());
            }
        }
    }
}
