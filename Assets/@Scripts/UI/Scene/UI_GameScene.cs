using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum Texts
    {
        CurrentFPSText,
        ScoreValueText,
    }

    enum Sliders
    {
        CurrentRemainGasSlider,
    }

    enum GameObjects
    {
        RetryBackground,
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        BindObjects(typeof(GameObjects));

        Managers.Game.OnCurrentScoreChanged -= HandleOnCurrentScoreChanged;
        Managers.Game.OnCurrentScoreChanged += HandleOnCurrentScoreChanged;

        Managers.Game.OnCurrentRemainGasChanged -= HandleOnCurrentRemainGasSliderChanged;
        Managers.Game.OnCurrentRemainGasChanged += HandleOnCurrentRemainGasSliderChanged;
        
        GetObject((int)GameObjects.RetryBackground).SetActive(false);
        GetObject((int)GameObjects.RetryBackground).BindEvent((evt) => OnClickRetry());
        
        Refresh();
        return true;
    }
    
    private void Update()
    {
        RefreshCurrentFPSText();
    }

    public void Refresh()
    {
        RefreshCurrentScoreValue();
        RefreshCurrentRemainGasSlider();
        RefreshCurrentFPSText();
    }
    
    private float _elapsedTime = 0.0f;
    private readonly float _updateInterval = 1.0f;
    
    void RefreshCurrentFPSText()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _updateInterval)
        {
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;
            string text = $"{fps:N1} FPS ({ms:N1}ms)";
            GetText((int)Texts.CurrentFPSText).text = text;

            _elapsedTime = 0;
        }
    }

    public void ShowRetry()
    {
        GetObject((int)GameObjects.RetryBackground).SetActive(true);
    }

    void OnClickRetry()
    {
        GetObject((int)GameObjects.RetryBackground).SetActive(false);
        Managers.Scene.CurrentScene.GetComponent<GameScene>().RetryGame();
    }

    void RefreshCurrentScoreValue()
    {
        GetText((int)Texts.ScoreValueText).text = Managers.Game.CurrentScore.ToString();
    }

    void RefreshCurrentRemainGasSlider()
    {
        // TODO: 수치에 따라 색 바꾸기
        
        // slider value needs 0 to 1, but value 0 to 100
        GetSlider((int)Sliders.CurrentRemainGasSlider).value = Managers.Game.CurrentRemainGas * 0.01f;
    }

    void HandleOnCurrentScoreChanged()
    {
        RefreshCurrentScoreValue();
    }

    void HandleOnCurrentRemainGasSliderChanged()
    {
        RefreshCurrentRemainGasSlider();
    }
}