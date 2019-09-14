using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public class Alliance
    {
        public static int AllianceNeutral = 0;
        public static int AllianceLink = 1;
        public static int AllianceEnemy = 2;
    }

    public enum HitboxMode
    {
        None,
        Tile,
        BoundingRect,
        DiagTopLeft,
        DiagTopRight,
        DiagBotLeft,
        DiagBotRight,
        Pixels,
        Custom,
        BoxTop,
        BoxBot,
        BoxLeft,
        BoxRight,
        BoxTopLeft,
        BoxTopRight,
        BoxBotLeft,
        BoxBotRight
    }

    public class ZIndex
    {
        public static int Default = 0;
        public static int Link = 1000000;
        public static int Foreground = 2000000;
        public static int HUD = 10000000;
        public static int AboveFont = 20000000;
    }

    public enum StatType
    {
        Health,
        Magic,
        Rupee,
        Bomb,
        Arrow
    }

    public enum Alignment
    {
        Left,
        Center,
        Right
    }
    
}
