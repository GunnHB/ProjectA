using System.IO;
using System.Collections.Generic;

namespace ModelItem
{
    public class Data
    {
        public System.Int64 id;
		public System.String name;
		public System.String desc;
		public System.String sprite;
		public System.String prefab;
		public GameValue.ItemType type;
    }

    public class Model
    {
        private static List<Data> _dataList = new();
        private static Dictionary<long, Data> _dataDic = new();
        
        /// <summary>
        /// 초기화하기
        /// </summary>
        public static void Initialize()
        {
            var jsonData = File.ReadAllText("Assets/Tables/Json/Item.json");
            JsonUtil.Deserialize(jsonData, _dataList);

            foreach(var item in _dataList)
                _dataDic.Add(item.id, item);
        }

        public static List<Data> DataList => _dataList;
        public static Dictionary<long, Data> DataDic => _dataDic;
    }
}