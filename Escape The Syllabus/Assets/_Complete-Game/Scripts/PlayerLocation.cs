using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerLocation {
    public static float x;
    public static float y;
    public static string enemy;
    public static bool updatePlayer;
    public static bool[] enemies = new bool[] {true, true, true, true, true, true, true};
    public static Vector3 lvl1start = new Vector3(-7.47f, -3.34f);
    public static Vector3 lvl2start = new Vector3(-7.47f, 9.5f);
    public static Vector3 lvl3start = new Vector3(-7.47f, -3.34f);
}
