using System.IO;
using System.Collections.Generic;
public class CategoryTabModel
{
	public System.Int64 id;
	public System.String tab_name;
	public System.String normal_sprite;
	public System.String select_sprite;

	private static CategoryTabModel _instance;
	public static CategoryTabModel Instance => _instance;

	private static List<CategoryTabModel> modelList = new();
	private static Dictionary<long, CategoryTabModel> modelDic = new();


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
    /// 모델 데이터 가져오기
    /// </summary>
    public CategoryTabModel GetModel(long id)
    {
        return modelDic[id];
    }
}