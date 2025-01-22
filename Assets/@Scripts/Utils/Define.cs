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
    
    public const int BackgroundHeight = 10;
    public const int BackgroundMoveSpeed = 3;
}