using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class GameValue
{
    public static float GRAVITY = -9.81f;

    public enum ItemType
    {
        None = -1,
        Weapon,
        Sheld,
        Bow,
        Armor,
        Food,
        Etc,
    }
}
