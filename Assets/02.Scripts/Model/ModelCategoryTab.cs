using System.IO;
using System.Collections.Generic;

public class ModelCategoryTab
{
	public System.Int64 id;
	public System.String tab_name;
	public System.String normal_sprite;
	public System.String select_sprite;

	private static List<ModelCategoryTab> modelList = new();
	private static Dictionary<long, ModelCategoryTab> modelDic = new();

    /// <summary>
    /// 초기화하기
    /// </summary>
    public static void Initialize()
    {
        var jsonData = File.ReadAllText("Assets/Tables/Json/CategoryTab.json");
        JsonUtil.Deserialize(jsonData, modelList);

        foreach(var item in modelList)
            modelDic.Add(item.id, item);
    }

    /// <summary>
    /// 모델 리스트 가져오기
    /// </summary>
    public static List<ModelCategoryTab> GetModelList()
    {
        return modelList;
    }
    
    /// <summary>
    /// 모델 딕셔너리 가져오기
    /// </summary>
    public static Dictionary<long, ModelCategoryTab> GetDictionary()
    {
        return modelDic;
    }
    
}