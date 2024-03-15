using System.Runtime.Serialization;

public static class GameValue
{
    public static float _gravity = -9.81f;

    // 아이템 타입에 따른 인벤토리의 초기 슬롯 수
    public static int _initWeaponItemAmount = 4;
    public static int _initShieldItemAmount = 4;
    public static int _initBowItemAmount = 8;
    public static int _initArmorItemAmount = 4;
    public static int _initFoodItemAmount = 30;
    public static int _initDefaultItemAmount = 30;

    // 한 줄에 들어가는 슬롯 수
    public static int _inventoryRowAmount = 6;

    public const string ANIM_LAYER_ONEHAND = "OneHand";
    public const string ANIM_LAYER_ONEHAND_UPPER = "OneHand_upper";

    public const string ANIM_LAYER_TOWHAND = "TwoHand";
    public const string ANIM_LAYER_TOWHAND_UPPER = "TwoHand_upper";

    public enum ItemType
    {
        [EnumMember(Value = "None")]
        None = -1,
        [EnumMember(Value = "Weapon")]
        Weapon,
        [EnumMember(Value = "Shield")]
        Shield,
        [EnumMember(Value = "Bow")]
        Bow,
        [EnumMember(Value = "Armor")]
        Armor,
        [EnumMember(Value = "Food")]
        Food,
        [EnumMember(Value = "Default")]
        Default,
    }

    public enum WeaponType
    {
        [EnumMember(Value = "None")]
        None = -1,
        [EnumMember(Value = "OneHand")]
        NoWeapon,
        [EnumMember(Value = "OneHand")]
        OneHand,
        [EnumMember(Value = "TwoHand")]
        TwoHand,
    }

    // enum 추가하고 싶으면 아래에 추가하기
    // 안그럼 기존 값들이 다 밀림요...ㅠ
    public enum PartsKey
    {
        None = -1,
        Head,
        Eyebrows,
        FacialHair,
        Torso,
        UpperArmRight,
        UpperArmLeft,
        LowerArmRight,
        LowerArmLeft,
        HandRight,
        HandLeft,
        Hip,
        LegRight,
        LegLeft,
        Hair,
    }

    public enum GenderType
    {
        None = -1,
        Male,
        Female,
    }
}
