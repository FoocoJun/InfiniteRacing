using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class FireTruck : InitBase
{
    EFireTruckMoveState _moveState = EFireTruckMoveState.None;

    Coroutine _moveCoroutine;
    private readonly float _moveSpeed = 5f;
    
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
        while (Managers.Game.IsPlaying) {
            if (MoveState == EFireTruckMoveState.Left)
            {
                var vector3 = transform.position;
                vector3.x = Mathf.Clamp(vector3.x - (_moveSpeed * Time.deltaTime), MaximumXPosition * -1, MaximumXPosition);
                transform.position = vector3;
            }

            if (MoveState == EFireTruckMoveState.Right)
            {
                var vector3 = transform.position;
                vector3.x = Mathf.Clamp(vector3.x + (_moveSpeed * Time.deltaTime), MaximumXPosition * -1, MaximumXPosition);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: 추후 CarGas로 이전 필요
        if (other.gameObject.CompareTag("CarGas"))
        {
            Managers.Resource.Destroy(other.gameObject);

            Managers.Game.CurrentRemainGas += 30;
        }
    }

    private void OnDestroy()
    {
        Managers.Game.OnMoveButtonStateChanged -= HandleOnMoveButtonStateChanged;
    }
}
