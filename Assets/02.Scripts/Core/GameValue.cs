using System.Runtime.Serialization;

public static class GameValue
{
    public static float GRAVITY = -9.81f;

    public enum ItemType
    {
        [EnumMember(Value = "None")]
        None = -1,
        [EnumMember(Value = "Weapon")]
        Weapon,
        [EnumMember(Value = "Shield")]
        Sheld,
        [EnumMember(Value = "Bow")]
        Bow,
        [EnumMember(Value = "Armor")]
        Armor,
        [EnumMember(Value = "Food")]
        Food,
        [EnumMember(Value = "Etc")]
        Etc,
    }
}
