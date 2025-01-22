using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene,
    }
    
    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        Drag,
    }
    
    public static readonly int BackgroundHeight = 10;
    public static readonly int BackgroundMoveSpeed = 3;

    public static readonly int CarGasGapDistance = 8;

    public static readonly float MaximumXPosition = 3.5f;
    public static readonly float OffsetYPosition = -2f;

    public enum EMoveButtonState
    {
        LeftDown,
        RightDown,
        BothDown,
        None,
    }

    public enum EFireTruckMoveState
    {
        Left,
        Right,
        None,
    }
}