using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class BackgroundManager
{
    List<GameObject> _backgrounds = new List<GameObject>();
    GameObject BackgroundRoot;
    
    private float _backgroundSpeedMultiplier;

    public float BackgroundSpeedMultiplier
    {
        get => _backgroundSpeedMultiplier;
        set => _backgroundSpeedMultiplier = Mathf.Clamp(value, 0, 5);
    }

    public void InitBackgrounds()
    {
        Clear();
        
        if (BackgroundRoot == null)
        {
            BackgroundRoot = new GameObject("@BackgroundRoot");
        }
        
        for (int i = 0; i < 3; i++)
        {
            var background = Managers.Resource.Instantiate("Background", BackgroundRoot.transform);
            _backgrounds.Add(background);
        }

        for (int i = 0; i < _backgrounds.Count; i++)
        {
            var backgroundHeight = -BackgroundHeight + i * BackgroundHeight;
            _backgrounds[i].transform.position = new Vector3(0, backgroundHeight, 0);
        }
    }

    public void MoveBackgroundDown()
    {
        foreach (GameObject background in _backgrounds)
        {
            background.transform.position += Vector3.down * (BackgroundMoveSpeed * BackgroundSpeedMultiplier * Time.deltaTime);
        }

        CheckBackgroundPosition();
    }

    private void CheckBackgroundPosition()
    {
        foreach (GameObject background in _backgrounds)
        {
            var vector3 = background.transform.position;
            
            // 화면에서 사라지면 위로 올리기
            if (vector3.y <= -1.5 * BackgroundHeight)
            {
                vector3.y += BackgroundHeight * _backgrounds.Count;
                background.transform.position = vector3;
            }
        }
    }

    public void Clear()
    {
        _backgrounds.ForEach(Object.Destroy);
        _backgrounds.Clear();
    }
}