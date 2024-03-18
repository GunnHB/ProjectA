using System.IO;
using System.Collections.Generic;

namespace ModelDefault
{
    public class Data
    {
        public System.Int64 id;
		public GameValue.WeaponType type;
		public System.Int32 offensive_power;
		public System.Single speed;
		public System.String equip_holder;
		public System.Boolean able_equip_shield;
    }

    public class Model
    {
        private static List<Data> _dataList = new();
        private static Dictionary<long, Data> _dataDic = new();

        private static bool _isInit = false;
        
        /// <summary>
        /// 초기화하기
        /// </summary>
        public static void Initialize()
        {
            if (_isInit)
                return;

            var jsonData = File.ReadAllText("Assets/08.Tables/Json/Default.json");
            JsonUtil.Deserialize(jsonData, _dataList);

            foreach (var item in _dataList)
                _dataDic.Add(item.id, item);
            
            _isInit = true;
        }

        public static List<Data> DataList => _dataList;
        public static Dictionary<long, Data> DataDic => _dataDic;
    }
}