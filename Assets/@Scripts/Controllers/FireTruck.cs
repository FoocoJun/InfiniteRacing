using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class FireTruck : InitBase
{
    EFireTruckMoveState _moveState = EFireTruckMoveState.None;

    Coroutine _moveCoroutine;
    private readonly float _moveSpeed = 3f;
    
    public EFireTruckMoveState MoveState
    {
        get { return _moveState; }
        set
        {
            _moveState = value;
            
            switch (value)
            {
                case EFireTruckMoveState.None:
                    StopCoroutine(_moveCoroutine);
                    break;
                case EFireTruckMoveState.Left:
                case EFireTruckMoveState.Right:
                    _moveCoroutine = StartCoroutine(Move());
                    break;
            }
        }
    }
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.OnMoveButtonStateChanged -= HandleOnMoveButtonStateChanged;
        Managers.Game.OnMoveButtonStateChanged += HandleOnMoveButtonStateChanged;

        return true;
    }

    private IEnumerator Move()
    {
        while (true) {
            if (MoveState == EFireTruckMoveState.Left)
            {
                var vector3 = transform.position;
                vector3.x -= _moveSpeed * Time.deltaTime;
                transform.position = vector3;
            }

            if (MoveState == EFireTruckMoveState.Right)
            {
                var vector3 = transform.position;
                vector3.x += _moveSpeed * Time.deltaTime;
                transform.position = vector3;
            }
            
            yield return null;
        }
    }
    
    private void HandleOnMoveButtonStateChanged(EMoveButtonState moveButtonState)
    {
        switch (moveButtonState)
        {
            case EMoveButtonState.None:
            case EMoveButtonState.BothDown:
                MoveState = EFireTruckMoveState.None;
                break;
            case EMoveButtonState.LeftDown:
                MoveState = EFireTruckMoveState.Left;
                break;
            case EMoveButtonState.RightDown:
                MoveState = EFireTruckMoveState.Right;
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        Managers.Game.OnMoveButtonStateChanged -= HandleOnMoveButtonStateChanged;
    }
}
