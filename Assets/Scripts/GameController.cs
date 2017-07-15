using System;
using UnityEngine;
using Zenject;

public class GameController : ITickable
{
    public int Budget = 1000;
    public int[] WaveReward;
	public const float RoundDuration = 20f;
	public const float MercyDuration = 15f;
	public const int SpawnAmount = 10;
	public const int SpawnAmountRoundMultiplier = 2;
	public Vector2 StartingPoint;
	public Vector2 TargetPoint;

	public delegate void RoundDelegate(int round);
	public event RoundDelegate OnRoundStarted;
	
	public delegate void GameStartedDelegate();
	public event GameStartedDelegate OnGameStarted;
	
	public delegate void EnemySpawnDelegate(int enemyCount);
	public event EnemySpawnDelegate OnEnemySpawn;
	
	private float _currentRoundDuration = 0f;
	private int _round = 1;
	private bool _isMercyDurationOver = false;

	private bool _gameStarted = false;

	public void Setup(Vector3 startingPosition, Vector3 targetPosition)
	{
		StartingPoint = new Vector2(startingPosition.x, startingPosition.z);
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
		
		StartNextRound();
	}

	public void ResetGame()
	{
		_gameStarted = false;
		_round = 0;
		_isMercyDurationOver = false;
		_currentRoundDuration = 0;
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
		} 
		
		// Check for next round
		if (_currentRoundDuration < RoundDuration)
		{
			_currentRoundDuration += Time.deltaTime;
		}
		else
		{
			StartNextRound();
		}
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

    private void RewardPlayer()
    {
       // Budget += WaveReward[_round];
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
}
