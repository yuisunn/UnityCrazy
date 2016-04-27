using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPSGame.Tools
{
    public class RandomName
    {
        private CSVMod csvmod = null; //使用该对象操作Name.csv文件
        private Dictionary<int, List<string>> NameContainer = null;//按前后名代号将名字分类，分别装入各自的List中
        private List<Dictionary<string, string>> tempList = null;//临时存储csv对象加载下的信息
        private List<string> firstNameList = null;//存储选过的前名
        private List<string> lastNameList = null;//存储选过的后名
        private static int ClickCounter = 1;//记录用户选择名字（点击按钮）的次数

        /// <summary>
        /// 默认构造函数，打开该路径下的"Name.csv"文件。
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NameFilePath"></param>
        public RandomName(string FileStr, bool isCSVFail = false)
        {

            csvmod = new CSVMod(FileStr, isCSVFail);
            NameContainer = new Dictionary<int, List<string>>();
            firstNameList = new List<string>();
            lastNameList = new List<string>();
            InitNameDictionary();
        }


        /// <summary>
        /// 获得csvmod加载的文件内容,并按照“前后名id”,放到NameContainer的各自的List中(0->只做前名，1->只做后名,2->前后名都可以做)
        /// NameContainer< <0, <前名...> > , <1, <后名...> , <2 ,<前/后...> > >
        /// </summary>
        private void InitNameDictionary()
        {
            if (0 == (tempList = csvmod.GetAllData()).Count)//若是没加载Name.csv文件，先加载.
            {
                if (!csvmod.LoadCsvStr())
                {
                    DebugMod.Log("Load NameFile Failed!");
                }
                else
                {
                    for (int index = 0; index < tempList.Count; index++)
                    {
                        int nameId = int.Parse(tempList[index]["前后代号"]);//前后代号：各种名字的id
                        string name = tempList[index]["备用名字"];// 代号对应的名字

                        if (NameContainer.ContainsKey(nameId))
                        {
                            NameContainer[nameId].Add(name);
                        }
                        else
                        {
                            NameContainer.Add(nameId, new List<string>());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 产生随机名字:每次先从(0,2)对应的list中随机选择一个名字作为前名,从(1,2)对应的list中选择后名。
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomName()
        {
            int firstRandomNum = 0;
            int secondRandomNum = 0;
            string firstName = null;
            string secondName = null;
            bool state = true;

            while (state)
            {

                while (1 == (firstRandomNum = (SPSRand.GetRandomNumber(0, 1000000) % 3)))//用SPSRand.GetRandomNumber(0,2)选择0到2之前的随机数时，绝大多数是产生0，2几乎选不到，因此先产生（0，10000000（可选））之间的，再%3获得较平均的随机数，以此类推
                    ;
                int temp = NameContainer[firstRandomNum].Count;
                firstName = NameContainer[firstRandomNum][SPSRand.GetRandomNumber(0, 10000000) % temp];
                if (!firstNameList.Contains(firstName))
                {
                    firstNameList.Add(firstName);
                    state = false;
                }
            }

            state = true;

            while (state)
            {
                secondRandomNum = SPSRand.GetRandomNumber(0, 10000000) % 2 + 1;//随机产生1或2
                int temp = NameContainer[secondRandomNum].Count;
                secondName = NameContainer[secondRandomNum][SPSRand.GetRandomNumber(0, 10000000) % temp];

                if (!lastNameList.Contains(secondName))
                {
                    lastNameList.Add(secondName);
                    state = false;
                }
            }

            if (0 == ClickCounter % 30)//定时清理firstName，lastName中存储的得到过的值，25是可变的，名字多，可以改成更大值，产生相同名字概率越小。
            {
                firstNameList.Clear();
                lastNameList.Clear();
            }

            ClickCounter++;

            return firstName + secondName;
           
        }

        #region 另一种思路

        //{
        //    secondRandomNum = SPSRand.GetRandomNumber(1,2);
        //    secondName = NameContainer[secondRandomNum][SPSRand.GetRandomNumber(0,NameContainer[secondRandomNum].Count - 1)];
        //    myName = firstName + secondName;
        //}
        //else if (2 == firstRandomNum)
        //{
        //    secondRandomNum = SPSRand.GetRandomNumber(0,2);
        //    secondName = NameContainer[secondRandomNum][SPSRand.GetRandomNumber(0,NameContainer[secondRandomNum].Count - 1)];

        //    if(0 == secondRandomNum)
        //    {
        //        myName = firstName + secondName;
        //    }
        //    else
        //    {
        //        myName = secondName + firstName;
        //    }
        //}
        //else
        //{ 
        //    if(0 == ClickCounter % 2)
        //    {
        //        secondName = NameContainer[0][SPSRand.GetRandomNumber(0,NameContainer[0].Count - 1)];

        //    }
        //    else
        //    {
        //        secondName = NameContainer[2][SPSRand.GetRandomNumber(0,NameContainer[2].Count - 1)];
        //    }
        //    myName = secondName + firstName;
        //}

        //if(firstNameList.Contains())
        //if()
        //{

        //}

        #endregion //另一种思路
    }
}
