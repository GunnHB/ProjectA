using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameValue
{
    public static float GRAVITY = -9.81f;

    public enum CategoryTab
    {
        None = -1,
        CateWeapon,
        CateShield,
        CateBow,
        CateArmor,
        CateFood,
        CateEtc,
    }
}
