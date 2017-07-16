using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : ITickable
{
    public int Budget = 1000;
    public int PoliceCost = 500;
    public int BarrierCost = 400;
    public int CasualtyCounter;
	public int WoundedPolicemenCounter;
	public int SolvedDemoCounter;
	public int DestroyedShopCounter;
    public int RoundCasualties;

    public int[] WaveReward =
    {
	    900,
	    600,
	    750,
	    550
    };
	public const float RoundDuration = 20f;
	public const float MercyDuration = 5f;
	public const int SpawnAmount = 5;
	public const int SpawnAmountRoundMultiplier = 2;
	public Vector2[] StartingPoint;
	public Vector2 TargetPoint;

	public delegate void BudgetChangedDelegate(int newBudget, int deltaBudget);
	public event BudgetChangedDelegate OnBudgetChanged;
	
	public delegate void RoundDelegate(int round);
	public event RoundDelegate OnRoundStarted;
	
	public delegate void GameStartedDelegate();
	public event GameStartedDelegate OnGameStarted;
	
	public delegate void EnemySpawnDelegate(int enemyCount);
	public event EnemySpawnDelegate OnEnemySpawn;

    public event Action<int> OnRoundEnding = i => { };
	
	private float _currentRoundDuration = 0f;
	private int _round = 1;
	private bool _isMercyDurationOver = false;

	private bool _gameStarted = false;
	private GameObject _endView;

	public void Setup(Vector3[] startingPosition, Vector3 targetPosition, GameObject endView)
	{
		_endView = endView;
		StartingPoint = new Vector2[startingPosition.Length];
		for (int i = 0; i < startingPosition.Length; i++)
		{
			StartingPoint[i] = new Vector2(startingPosition[i].x, startingPosition[i].z);
		}
		TargetPoint = new Vector2(targetPosition.x, targetPosition.z);
	}
	
	public void StartGame()
	{
		ResetGame();
		_gameStarted = true;
		if (OnGameStarted != null)
		{
			OnGameStarted();
		}		
	}

	public void ResetGame()
	{
		_gameStarted = false;
		_round = 0;
		_isMercyDurationOver = false;
		_currentRoundDuration = 0;
	}

    public void FailGame()
    {
	    _endView.SetActive(true);
    }

	public void Tick()
	{
		// Game not running
		if (!_gameStarted)
		{
			return;
		}

		if (!_isMercyDurationOver)
		{
			if (_currentRoundDuration < MercyDuration)
			{
				_currentRoundDuration += Time.deltaTime;
				return;
			}
			_currentRoundDuration = 0;
			_isMercyDurationOver = true;
			StartNextRound();
		} 
		
		// Check for next round
		if (_currentRoundDuration < RoundDuration)
		{
			_currentRoundDuration += Time.deltaTime;
		}
		else
		{
		    CasualtyCounter += RoundCasualties;
		    OnRoundEnding(RoundCasualties);
            RoundCasualties = 0;

			StartNextRound();
		}
	}

    public void RegisterCasualty(int amount)
    {
        RoundCasualties += amount;
    }

	private void StartNextRound()
	{
        RewardPlayer();
		_round++;
		_currentRoundDuration = 0;
		Debug.Log("Start round " + _round);
		if (OnRoundStarted != null)
		{
			OnRoundStarted(_round);
		}
	
		SpawnEnemies();
	}
	
	public int GetCurrentGameRound()
	{
		return _round;
	}

    private void RewardPlayer()
    {
       UpdateBudget(WaveReward[Mathf.Clamp(_round, 0, WaveReward.Length-1)]);
    }

	public void UpdateBudget(int deltaBudget)
	{
		Budget += deltaBudget;
		if (OnBudgetChanged != null)
		{
			OnBudgetChanged(Budget, deltaBudget);
		}
	}

    private void SpawnEnemies()
	{
		int spawnCount = SpawnAmount + (_round - 1) * SpawnAmountRoundMultiplier;
		Debug.Log("Should spawn " + spawnCount + " enemies");
		if (OnEnemySpawn != null)
		{
			OnEnemySpawn(spawnCount);
		}
	}

	public int GetCost(UnitType unitType)
	{
		switch (unitType)
		{
			case UnitType.Police:
				return PoliceCost;
			case UnitType.Barrier:
				return BarrierCost;
			default:
				throw new ArgumentOutOfRangeException("unitType", unitType, null);
		}
	}
}
