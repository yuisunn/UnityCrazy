using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
        
namespace SPSGame.Tools
{       
    public class DataUnit
    {   
        private CSVMod csvMod = null;
        //private List<Dictionary<string,string>> tempList = null;

        public DataUnit(string csvStr, bool isCSVFile = false)
        {
            csvMod = new CSVMod(csvStr, isCSVFile);
            csvMod.LoadCsvStr();
        }

        public string GetDataById(int id, string item)
        {
            return csvMod.GetData(getRowNumById(id), item);            
        }

        private int getRowNumById(int id)
        {
            int index = 0;
            List<Dictionary<string,string>> tempList = null;

            tempList = csvMod.GetAllData();
            foreach (var temp in tempList)
            {
                if (id == int.Parse(temp["ID"]))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        public List<int> GetAllID()
        {
            List<int> list = new List<int>();
            List<Dictionary<string, string>> tempList = null;

            tempList = csvMod.GetAllData();
            foreach (var temp in tempList)
            {
                list.Add(int.Parse(temp["ID"]));
            }

            return list;
        }
    }
}
