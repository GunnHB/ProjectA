using System.IO;
using System.Collections.Generic;

namespace ModelCategoryTab
{
    public class Data
    {
        public System.Int64 id;
		public GameValue.ItemType type;
		public System.String normal_sprite;
		public System.String select_sprite;
		public System.Int32 order;
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
            var jsonData = File.ReadAllText("Assets/Tables/Json/CategoryTab.json");
            JsonUtil.Deserialize(jsonData, _dataList);

            foreach(var item in _dataList)
                _dataDic.Add(item.id, item);
        }

        public static List<Data> DataList => _dataList;
        public static Dictionary<long, Data> DataDic => _dataDic;
    }
}