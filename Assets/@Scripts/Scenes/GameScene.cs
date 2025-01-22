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
        
        // 가스통 생성
        Managers.CarGas.InitCarGas();

        // 좌우 이동 버튼 생성
        Managers.UI.ShowBaseUI<UI_SideMoveButton>();

        // 플레이어 트럭 생성
        var player = Managers.Game.SpawnFireTruck();

        StartGame();

        return true;
    }
    
    void Update()
    {
        if (!Managers.Game.IsPlaying)
        {
            return;
        }
        
        Managers.Background.MoveBackgroundDown();
        
        Managers.CarGas.MoveCarGasDown();
    }

    Coroutine playingCoroutine;

    public void StartGame()
    {
        Managers.Game.IsPlaying = true;
        playingCoroutine = StartCoroutine(PlayGame());
    }

    void StopGame()
    {
        Managers.Game.IsPlaying = false;
        StopCoroutine(playingCoroutine);

        Managers.UI.GetSceneUI<UI_GameScene>().ShowRetry();
    }
    
    #region Play
    IEnumerator PlayGame()
    {
        while (Managers.Game.CurrentRemainGas > 0)
        {
            yield return new WaitForSeconds(1f);
            
            // 가스 감소
            Managers.Game.CurrentRemainGas -= 10f;
            
            // 점수 증가
            Managers.Game.CurrentScore += (int)(10 * Managers.Background.BackgroundSpeedMultiplier);
            
            // 속도 증가
            Managers.Background.BackgroundSpeedMultiplier *= 1.3f;

            if (Managers.Game.CurrentRemainGas <= 0)
            {
                StopGame();
            }
        }
    }
    #endregion

    public override void Clear()
    {
        Managers.Background.Clear();
    }
}