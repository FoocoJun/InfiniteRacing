using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.GameScene;
        
        // 씬 UI 생성
        UI_GameScene sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        // 배경 생성
        Managers.Background.InitBackgrounds();

        // 좌우 이동 버튼 생성
        Managers.UI.ShowBaseUI<UI_SideMoveButton>();

        // 플레이어 트럭 생성
        var player = Managers.Game.SpawnFireTruck();

        return true;
    }
    
    void Update()
    {
        Managers.Background.MoveBackgroundDown();
    }

    public override void Clear()
    {
        Managers.Background.Clear();
    }
}