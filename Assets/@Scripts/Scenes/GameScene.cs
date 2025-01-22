using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    List<GameObject> _backgrounds = new List<GameObject>();
    private const int _backgroundHeight = 10;
    GameObject BackgroundRoot;
    
    float moveSpeed = 3f;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;
        
        // 이하 씬이 시작 할 때 원하는 셋팅
        UI_GameScene sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();
        
        BackgroundRoot = new GameObject("BackgroundRoot");

        InitBackgrounds();

        return true;
    }
    
    void Update()
    {
        MoveBackgroundDown();
    }

    public void InitBackgrounds()
    {
        for (int i = 0; i < 3; i++)
        {
            var background = Managers.Resource.Instantiate("Background", BackgroundRoot.transform);
            _backgrounds.Add(background);
        }

        for (int i = 0; i < _backgrounds.Count; i++)
        {
            var backgroundHeight = -_backgroundHeight + i * _backgroundHeight;
            _backgrounds[i].transform.position = new Vector3(0, backgroundHeight, 0);
        }
    }

    private void MoveBackgroundDown()
    {
        foreach (GameObject background in _backgrounds)
        {
            background.transform.position += Vector3.down * (moveSpeed * Time.deltaTime);
        }

        CheckBackgroundPosition();
    }

    private void CheckBackgroundPosition()
    {
        foreach (GameObject background in _backgrounds)
        {
            var vector3 = background.transform.position;
            
            // 화면에서 사라지면 위로 올리기
            if (vector3.y <= -1.5 * _backgroundHeight)
            {
                vector3.y += + _backgroundHeight * _backgrounds.Count;
                background.transform.position = vector3;
            }
        }
    }

    public override void Clear()
    {
        _backgrounds.Clear();
    }
}