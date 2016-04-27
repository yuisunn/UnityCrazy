using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPSGame.Tools;

namespace SPSGame.Unity
{
    public class ItemManager 
    {
        //缓存服务器所有数据列表
        List<ItemData> itemDatas = new List<ItemData>();
        //缓存服务器新增数据列表
        List<ItemData> itemUpdateDatas = new List<ItemData>();



        /// <summary>
        /// 从服务器获取数据并解析
        /// </summary>
        /// <param name="_info"></param>
        public void GetServerInfo(short type, List<Dictionary<string, string>> _info)
        {
            //第一次传入数据
            if (0 == type)
            {
                itemDatas.Clear();
                itemUpdateDatas.Clear();

                for (int i = 0; i < _info.Count; ++i)
                {
                    ItemData data = new ItemData();
                    data.ID = long.Parse(_info[i]["ItemID"]);
                    data.name = _info[i]["ItemNo"];
                    data.num = int.Parse(_info[i]["Num"]);
                    data.type = int.Parse(_info[i]["Type"]);
                    data.location = int.Parse(_info[i]["Location"]);
                    //从配置文件中取到道具的spriteName
                    Dictionary<string, string> req = new Dictionary<string, string>();
                    req.Add("ID", data.name);
                    List<Dictionary<string, string>> res = DataManager.Instance.mIconData.GetRowDataByMultiColValue(req);

                    if (null == res || 0 == res.Count)
                    {
                        continue;
                    }

                    data.spriteName = res[0]["SpriteName"];

                    itemDatas.Add(data);

                }
            }
            //更新数据
            else if (1 == type)
            {
                itemUpdateDatas.Clear();

                for (int i = 0; i < _info.Count; ++i)
                {
                    bool isFindItem = false;
                    long id = long.Parse(_info[i]["ItemID"]);
                    for (int j = 0; j < itemDatas.Count; ++j)
                    {
                        if (id == itemDatas[j].ID)
                        {
                            isFindItem = true;

                            if ("0" == _info[i]["Num"])
                            {
                                if ("0" != _info[i]["Location"])
                                {
                                    itemDatas[j].num = 0;
                                    itemUpdateDatas.Add(itemDatas[j]);
                                    itemDatas.Remove(itemDatas[j]);
                                }
                                else
                                {
                                    itemDatas[j].num = 0;
                                    itemDatas[j].location = 0;
                                    itemUpdateDatas.Add(itemDatas[j]);
                                    itemDatas.Remove(itemDatas[j]);
                                }
                            }
                            else
                            {
                                itemDatas[j].name = _info[i]["ItemNo"];
                                itemDatas[j].num = int.Parse(_info[i]["Num"]);
                                itemDatas[j].type = int.Parse(_info[i]["Type"]);
                                itemDatas[j].location = int.Parse(_info[i]["Location"]);

                                itemUpdateDatas.Add(itemDatas[j]);
                            }

                            break;
                        }
                    }

                    if (!isFindItem)
                    {
                        ItemData data = new ItemData();
                        data.ID = long.Parse (_info[i]["ItemID"]);
                        data.name = _info[i]["ItemNo"];
                        data.num = int.Parse(_info[i]["Num"]);
                        data.type = int.Parse(_info[i]["Type"]);
                        data.location = int.Parse(_info[i]["Location"]);

                        //从配置文件中取到道具的spriteName
                        Dictionary<string, string> req = new Dictionary<string, string>();
                        req.Add("ID", data.name);
                        List<Dictionary<string, string>> res = DataManager.Instance.mIconData.GetRowDataByMultiColValue(req);

                        if (null == res || 0 == res.Count)
                        {
                            continue;
                        }

                        data.spriteName = res[0]["SpriteName"];

                        itemDatas.Add(data);
                        itemUpdateDatas.Add(data);
                    }
                }
            }
            
        }
        /// <summary>
        /// 根据不同的情况显示不同的数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ItemData> getItem(short type)
        {
            if ( 0 == type )
            {
                return itemDatas;
            }
            else if (1 == type)
            {
                return itemUpdateDatas;
            }
            else
            {
                return null;
            }
        }

        public List<ItemData> GetItemType(int _type) 
        {
            if (_type == -1)
            {
                foreach (ItemData d in itemDatas)
                {
                }
                return itemDatas;
            }

            List<ItemData> datas = new List<ItemData>();
            foreach (ItemData d in itemDatas)
            {
                if (d.type == _type)
                {
                    datas.Add(d);
                }
                else continue;
            }

            return datas;
        }
    }
}


