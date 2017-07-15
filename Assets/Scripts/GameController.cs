using UnityEngine;
using Zenject;

public class GameController : ITickable
{
	public const float RoundDuration = 20f;
	public const int SpawnAmount = 10;
	public const int SpawnAmountRoundMultiplier = 2;
	public readonly Vector2 StartingPoint = new Vector2(0, 2);
	
	public delegate void RoundDelegate(int round);
	public event RoundDelegate OnRoundStarted;
	
	public delegate void GameStartedDelegate();
	public event GameStartedDelegate OnGameStarted;
	
	public delegate void EnemySpawnDelegate(int enemyCount);
	public event EnemySpawnDelegate OnEnemySpawn;
	
	private float _currentRoundDuration = 0f;
	private int _round = 1;

	private bool _gameStarted = false;
	
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
	}

	public void Tick()
	{
		// Game not running
		if (!_gameStarted)
		{
			return;
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
		_round++;
		_currentRoundDuration = 0;
		Debug.Log("Start round " + _round);
		if (OnRoundStarted != null)
		{
			OnRoundStarted(_round);
		}
	
		SpawnEnemies();
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
