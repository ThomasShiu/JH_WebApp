/*******************************************************************************
 * Copyright © 2016 CCM.Framework 版權所有
 * Author: CCM
 * Description: CCM快速開發平臺
 * Website：http://www.ccm3s.com/
*********************************************************************************/
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CCM.Code
{
    public static class Serialize
    {
        /// <summary>
        /// 獲取物件序列化的二進位版本
        /// </summary>
        /// <param name="pObj">物件實體</param>
        /// <returns>如果物件實體為Null，則返回結果為Null。</returns>
        public static byte[] GetBytes(object pObj)
        {
            if (pObj == null) { return null; }
            MemoryStream serializationStream = new MemoryStream();
            new BinaryFormatter().Serialize(serializationStream, pObj);
            serializationStream.Position = 0L;
            byte[] buffer = new byte[serializationStream.Length];
            serializationStream.Read(buffer, 0, buffer.Length);
            serializationStream.Close();
            return buffer;
        }

        /// <summary>
        /// 獲取物件序列化的XmlDocument版本
        /// </summary>
        /// <param name="pObj">物件實體</param>
        /// <returns>如果物件實體為Null，則返回結果為Null。</returns>
        public static XmlDocument GetXmlDoc(object pObj)
        {
            if (pObj == null) { return null; }
            XmlSerializer serializer = new XmlSerializer(pObj.GetType());
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize((TextWriter)writer, pObj);
            XmlDocument document = new XmlDocument();
            document.LoadXml(sb.ToString());
            writer.Close();
            return document;
        }

        /// <summary>
        /// 從已序列化資料中(byte[])獲取物件實體
        /// </summary>
        /// <typeparam name="T">要返回的資料類型</typeparam>
        /// <param name="binData">二進位資料</param>
        /// <returns>物件實體</returns>
        public static T GetObject<T>(byte[] binData)
        {
            if (binData == null) { return default(T); }
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream(binData);
            return (T)formatter.Deserialize(serializationStream);
        }

        /// <summary>
        /// 從已序列化資料(XmlDocument)中獲取物件實體
        /// </summary>
        /// <typeparam name="T">要返回的資料類型</typeparam>
        /// <param name="xmlDoc">已序列化的文檔物件</param>
        /// <returns>物件實體</returns>
        public static T GetObject<T>(XmlDocument xmlDoc)
        {
            if (xmlDoc == null) { return default(T); }
            XmlNodeReader xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xmlReader);
        }

    }
}

