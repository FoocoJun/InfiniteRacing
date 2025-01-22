using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SideMoveButton : UI_Base
{
    enum GameObjects
    {
        LeftButtonBG,
        RightButtonBG,
    }
    
    private GameObject _leftButtonBG;
    private GameObject _rightButtonBG;

    private Vector2 _touchPos;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        
        BindObjects(typeof(GameObjects));
        
        _leftButtonBG = GetObject((int)GameObjects.LeftButtonBG);
        _leftButtonBG.BindEvent(OnLeftButtonPointerDown, type: Define.EUIEvent.PointerDown);
        _leftButtonBG.BindEvent(OnLeftButtonPointerUp, type: Define.EUIEvent.PointerUp);
        
        
        _rightButtonBG = GetObject((int)GameObjects.RightButtonBG);
        _rightButtonBG.BindEvent(OnRightButtonPointerDown, type: Define.EUIEvent.PointerDown);
        _rightButtonBG.BindEvent(OnRightButtonPointerUp, type: Define.EUIEvent.PointerUp);

        return true;
    }

    #region Events
    private void OnLeftButtonPointerDown(PointerEventData eventData)
    {
        UpdateMoveButtonState(Define.EMoveButtonState.LeftDown, Define.EMoveButtonState.RightDown, true);
    }

    private void OnLeftButtonPointerUp(PointerEventData eventData)
    {
        UpdateMoveButtonState(Define.EMoveButtonState.LeftDown, Define.EMoveButtonState.RightDown, false);
    }

    private void OnRightButtonPointerDown(PointerEventData eventData)
    {
        UpdateMoveButtonState(Define.EMoveButtonState.RightDown, Define.EMoveButtonState.LeftDown, true);
    }

    private void OnRightButtonPointerUp(PointerEventData eventData)
    {
        UpdateMoveButtonState(Define.EMoveButtonState.RightDown, Define.EMoveButtonState.LeftDown, false);
    }
    
    private void UpdateMoveButtonState(Define.EMoveButtonState currentState, Define.EMoveButtonState otherState, bool isDown)
    {
        if (isDown)
        {
            Managers.Game.MoveButtonState = (Managers.Game.MoveButtonState == otherState) 
                ? Define.EMoveButtonState.BothDown 
                : currentState;
        }
        else
        {
            Managers.Game.MoveButtonState = (Managers.Game.MoveButtonState == Define.EMoveButtonState.BothDown) 
                ? otherState 
                : Define.EMoveButtonState.None;
        }
    }
    #endregion
}
