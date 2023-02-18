using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Purpose of this Class is to hold all information of the Game which persists throughout the application
public static class GameConfig
{
    public static int level = 1;
    //==========PLAYER SETTINGS===========
    public static bool fppToggle = false;

    public static int[,] waveInfo = new int[,] { {1, 3, 5, 7, 9 },
                                                 {1, 2, 3, 4, 5 },
                                                 {1, 3, 4, 5, 6 } };
    public static KeyCode PRIMARY_FIRE_BUTTON = KeyCode.Mouse0;
    public static KeyCode SECONDARY_FIRE_BUTTON = KeyCode.Mouse1;
    public static KeyCode RELOAD_BUTTON = KeyCode.R;
    //public static KeyCode FORWARD = KeyCode.W;

    public static class Constants
    {
        //=======PHYSICS======
        public const float MAX_GAME_GRAVITY = -9.81f;

        //=========TAGS=======
        public const string GROUND_TAG = "Ground";
        public const int GROUND_TAG_INDEX = 8;
        public const string PLAYER_TAG = "Player";
        public const string JACK_TAG = "JackSpot";
        public const string FPPTAG = "FPP-Position";
        public const string TARGETTAG = "TargetForward";

        //========INPUT=======
        public const string INPUT_HORIZONTAL = "Horizontal";
        public const string INPUT_VERTICAL = "Vertical";

        //QTE
        public static KeyCode[] QTEKeys = { KeyCode.W , KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Mouse0, KeyCode.Mouse1};

        //public static Transform GetGFX(Transform t)
        //{
        //    return t.GetChild(0);
        //}
    }
}
