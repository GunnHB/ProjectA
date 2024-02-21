using System.IO;
using System.Collections.Generic;

public class ModelItem
{
	public System.Int64 id;
	public System.String name;
	public System.String desc;
	public System.String sprite;
	public System.String prefab;
	public GameValue.ItemType type;

	private static List<ModelItem> modelList = new();
	private static Dictionary<long, ModelItem> modelDic = new();

    /// <summary>
    /// 초기화하기
    /// </summary>
    public static void Initialize()
    {
        var jsonData = File.ReadAllText("Assets/Tables/Json/Item.json");
        JsonUtil.Deserialize(jsonData, modelList);

        foreach(var item in modelList)
            modelDic.Add(item.id, item);
    }

    /// <summary>
    /// 모델 리스트 가져오기
    /// </summary>
    public static List<ModelItem> GetModelList()
    {
        return modelList;
    }
    
    /// <summary>
    /// 모델 딕셔너리 가져오기
    /// </summary>
    public static Dictionary<long, ModelItem> GetDictionary()
    {
        return modelDic;
    }
    
}