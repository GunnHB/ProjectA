using System.IO;
using System.Collections.Generic;

namespace ModelWeapon
{
    public class Data
    {
        public System.Int64 id;
		public GameValue.WeaponType type;
		public System.Int32 offensive_power;
		public System.Single speed;
		public System.String equip_holder;
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
            var jsonData = File.ReadAllText("Assets/08.Tables/Json/Weapon.json");
            JsonUtil.Deserialize(jsonData, _dataList);

            foreach(var item in _dataList)
                _dataDic.Add(item.id, item);
        }

        public static List<Data> DataList => _dataList;
        public static Dictionary<long, Data> DataDic => _dataDic;
    }
}