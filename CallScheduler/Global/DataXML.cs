using CallScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CallScheduler.Global
{
    public static class DataXML
    {
        private enum NodeName
        {
            Alarm,
            Customer,
            Name,
            PhoneNumber,
            AlarmTime,
            Memo
        }

        public static bool XmlSave(List<DataModel> obj, string DataFilePath)
        {
            XmlDocument xml = new XmlDocument();

            XmlNode root = xml.CreateElement(NodeName.Alarm.ToString());
            xml.AppendChild(root);

            foreach (DataModel data in obj)
            {
                XmlNode Customer = xml.CreateElement(NodeName.Customer.ToString());
                root.AppendChild(Customer);

                XmlNode Name = xml.CreateElement(NodeName.Name.ToString());
                Name.InnerText = data.Name;
                Customer.AppendChild(Name);

                XmlNode PhoneNumber = xml.CreateElement(NodeName.PhoneNumber.ToString());
                PhoneNumber.InnerText = data.PhoneNumber;
                Customer.AppendChild(PhoneNumber);

                XmlNode AlarmTime = xml.CreateElement(NodeName.AlarmTime.ToString());
                AlarmTime.InnerText = data.AlarmTime.ToString("yyyy/MM/dd HH:mm");
                Customer.AppendChild(AlarmTime);

                XmlNode Memo = xml.CreateElement(NodeName.Memo.ToString());
                Memo.InnerText = data.Memo;
                Customer.AppendChild(Memo);
            }

            try
            {
                xml.Save(DataFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<DataModel> XmlLoad(string DataFilePath)
        {
            List<DataModel> Data = new List<DataModel>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(DataFilePath);

                XmlNodeList nodes = xml.SelectNodes($"/{NodeName.Alarm.ToString()}/{NodeName.Customer.ToString()}");

                foreach (XmlNode node in nodes)
                {
                    DataModel item = new DataModel();
                    item.Name = node[NodeName.Name.ToString()].InnerText;
                    item.PhoneNumber = node[NodeName.PhoneNumber.ToString()].InnerText;
                    item.AlarmTime = DateTime.ParseExact(node[NodeName.AlarmTime.ToString()].InnerText, "yyyy/MM/dd HH:mm", null);
                    item.Memo = node[NodeName.Memo.ToString()].InnerText;
                    Data.Add(item);
                }
            }
            catch
            {

            }

            return Data;
        }
    }
}
