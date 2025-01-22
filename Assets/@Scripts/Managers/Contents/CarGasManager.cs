using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class CarGasManager
{
    List<GameObject> _carGasList = new List<GameObject>();
    
    public void InitCarGas()
    {
        Clear();
        
        for (int i = 0; i < 10; i++)
        {
            var carGas = Managers.Resource.Instantiate("CarGas", null, true);
            var randomPos = GetCarGasRandomPosition();
            randomPos.y = i * CarGasGapDistance;
            carGas.transform.position = randomPos;
            
            _carGasList.Add(carGas);
        }
    }
    
    public void MoveCarGasDown()
    {
        foreach (GameObject carGas in _carGasList)
        {
            carGas.transform.position += Vector3.down * (BackgroundMoveSpeed * Managers.Background.BackgroundSpeedMultiplier * Time.deltaTime);
        }

        CheckCarGasPosition();
    }

    private void CheckCarGasPosition()
    {
        foreach (GameObject carGas in _carGasList)
        {
            var vector3 = carGas.transform.position;
            
            // 화면에서 사라지면 위로 올리기
            if (vector3.y > -1.5 * BackgroundHeight)
            {
                continue;
            }

            var randomPos = GetCarGasRandomPosition();
            randomPos.y = vector3.y + (_carGasList.Count * CarGasGapDistance);
            
            Managers.Resource.Destroy(carGas);
            var newCarGas = Managers.Resource.Instantiate("CarGas", null, true);
            newCarGas.transform.position = randomPos;
        }
    }

    Vector3 GetCarGasRandomPosition()
    {
        var randomX = Random.Range(-1, 2);
        return new Vector3(randomX * 2, 0, 0);
    }
    
    public void Clear()
    {
        _carGasList.ForEach(Object.Destroy);
        _carGasList.Clear();
    }
}
