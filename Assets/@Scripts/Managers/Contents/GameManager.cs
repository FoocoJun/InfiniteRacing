using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;

[Serializable]
public class GameSaveData
{
    public string LastSaveDate;

    public float BackgroundSpeedMultiplier;

    public float CurrentRemainGas;
    
    public float StartPositionX;
    
    public int CurrentScore;
    
    // TODO: 기름통의 위치를 굳이 저장해야할까?
}

public class GameManager
{
    #region GameData
    public GameSaveData SaveData { get; set; } = new GameSaveData();

    public bool IsPlaying = false;
    #endregion
    
    #region Player
    private FireTruck _player;
    private float _startPositionX;
    private float _currentRemainGas;
    private int _currentScore;

    public float CurrentRemainGas
    {
	    get => _currentRemainGas;
	    set
	    {
			_currentRemainGas = value;
		    OnCurrentRemainGasChanged?.Invoke();
	    }
    }

    public int CurrentScore
    {
	    get => _currentScore;
	    set
	    {
			_currentScore = value;
		    OnCurrentScoreChanged?.Invoke();
	    }
    }

    public FireTruck SpawnFireTruck()
    {
	    GameObject player = Managers.Resource.Instantiate("FireTruck");
	    player.transform.position = new Vector3(_startPositionX, OffsetYPosition, 0);
	    _player = player.GetOrAddComponent<FireTruck>();
	    return _player;
    }
    
    private EMoveButtonState _moveButtonState;
    public EMoveButtonState MoveButtonState
    {
	    get { return _moveButtonState; }
	    set
	    {
		    _moveButtonState = value;
		    OnMoveButtonStateChanged?.Invoke(_moveButtonState);
	    }
    }
    #endregion
    
    #region Save & Load
    private string Path { get { return Application.persistentDataPath + "/SaveData.json"; } }
    
    public void InitGame()
	{
		if (File.Exists(Path))
			return;
		
		// 저장된 데이터 없을 시 초기 데이터 선언 TODO: 팩토리 패턴 쓰면 어떨까
		// player
		{
			_startPositionX = 0;
			_currentRemainGas = 100f;
			_currentScore = 0;
		}
		
		// background
		{
			Managers.Background.BackgroundSpeedMultiplier = 1.0f;
		}
	}

	public void SaveGame()
	{
		// 저장이 필요한 데이터 SaveData에 재할당
		// player
		{
			if (_player != null)
			{
				SaveData.StartPositionX = _player.transform.position.x;
			}
			SaveData.CurrentRemainGas = CurrentRemainGas;
			SaveData.CurrentScore = CurrentScore;
		}

		// background
		{
			SaveData.BackgroundSpeedMultiplier = Managers.Background.BackgroundSpeedMultiplier;
		}
        
        // common
        {
			SaveData.LastSaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        // Save TODO: 비화 어떻게 하지? 1.0.0 이후 고민하기
        string jsonStr = JsonUtility.ToJson(SaveData);
        string encryptedStr = Util.Encrypt(jsonStr);
        File.WriteAllText(Path, encryptedStr);
        
		Debug.Log($"Save Game Completed : {Path}");
	}

	public bool LoadGame()
	{
		// Load
		if (File.Exists(Path) == false)
			return false;

		string encryptedStr = File.ReadAllText(Path);
		string jsonStr = Util.Decrypt(encryptedStr);
		GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonStr);

		if (data != null)
			SaveData = data;

		// TODO: 저장 데이터 형태가 기존과 맞지 않으면 어떡하지? 1.0.0 이후 고민하기. 세이브 데이터에 저장 버전을 담아도 될듯.
		// 불러올 데이터 SaveData에서 추출 및 할당
		// player
		{
			_startPositionX = SaveData.StartPositionX;
			CurrentRemainGas = SaveData.CurrentRemainGas;
			CurrentScore = SaveData.CurrentScore;
		}
		
		// background
		{
			Managers.Background.BackgroundSpeedMultiplier = SaveData.BackgroundSpeedMultiplier;
		}
		
		Debug.Log($"Save Game Loaded : {Path}");
		return true;
	}

    public void ClearSaveData()
    {
        SaveData = null;
        File.Delete(Path);
    }
    #endregion
    
    #region Action
    public event Action<Define.EMoveButtonState> OnMoveButtonStateChanged;
    public event Action OnCurrentRemainGasChanged;
    public event Action OnCurrentScoreChanged;
    #endregion
}
