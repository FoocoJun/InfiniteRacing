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
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));

        Managers.Game.OnCurrentScoreChanged -= HandleOnCurrentScoreChanged;
        Managers.Game.OnCurrentScoreChanged += HandleOnCurrentScoreChanged;

        Managers.Game.OnCurrentRemainGasChanged -= HandleOnCurrentRemainGasSliderChanged;
        Managers.Game.OnCurrentRemainGasChanged += HandleOnCurrentRemainGasSliderChanged;
        
        Refresh();
        return true;
    }
    
    private void Update()
    {
        RefreshCurrentFPSText();
    }

    private void Refresh()
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

    void RefreshCurrentScoreValue()
    {
        GetText((int)Texts.ScoreValueText).text = Managers.Game.CurrentScore.ToString();
    }

    void RefreshCurrentRemainGasSlider()
    {
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