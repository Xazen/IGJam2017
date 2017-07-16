using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PathfinderAgent))]
public class Rioter : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private float _invicibiltySeconds;
    [SerializeField] private int _damagePoints;
    [SerializeField] private float _attackRate;
    [SerializeField] private Animator _animator;

    [SerializeField] private PathfinderAgent _pathfinder;
    [SerializeField] private GameObject _representation;

    [SerializeField] private Vector2 _randomPositionVector;

    public delegate void RioterDie(Rioter rioter);
    public event RioterDie OnRioterDie;

    private Vector3 _target;
    private float _lastAttackTimestamp;
    private int _currentHealth;
    private float _currentInvicibilitySecounds;
    private GameController _gameController;
    private bool _death;

    [Inject]
    public void Inject(GameController gameController)
    {
        _gameController = gameController;
    }

    public void Randomize()
    {
        var displacementVector = new Vector3(Random.Range(-_randomPositionVector.x, _randomPositionVector.x), 0,
            Random.Range(-_randomPositionVector.y, _randomPositionVector.y));
        _representation.transform.position += displacementVector;
    }
    
    public int TryAttack()
    {
        if (Time.time - _lastAttackTimestamp >= _attackRate)
        {
            Debug.Log("attack");
            _lastAttackTimestamp = Time.time;
            return _damagePoints;
        }
        return 0;
    }

    public void MoveTo(Vector3 target)
    {
        _target = target;
        _pathfinder.CalculatePath(target);
    }

    private void Start()
    {
        _lastAttackTimestamp = Time.time;
        _currentHealth = _health;
        gameObject.tag = TagConstants.Enemy;
    }

    private void Update()
    {
        if (_currentInvicibilitySecounds > 0)
        {
            _currentInvicibilitySecounds -= Time.deltaTime;
        }
    }

    public void Stop()
    {
        _pathfinder.Stop();
    }

    public void Continue()
    {
        if (_currentHealth > 0)
        {
            _pathfinder.CalculatePath(_target);    
        }
    }

    public void TryTakeDamage(int damage)
    {
        if (_currentInvicibilitySecounds <= 0)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            StartCoroutine(BlinkDamange());
            _currentInvicibilitySecounds = _invicibiltySeconds;
        }
        
        if (_currentHealth == 0 && !_death)
        {
            _death = true;
            StartCoroutine(StartDie());
            if (OnRioterDie != null)
            {
                OnRioterDie(this);
            }
        }
    }

    private IEnumerator StartDie()
    {
        _gameController.SolvedDemoCounter++;
        _animator.SetTrigger("Dissolve");
        yield return new WaitForSeconds(2);
        if (gameObject)
        {
            Destroy(gameObject);    
        }
    }

    private IEnumerator BlinkDamange()
    {
        Color originalColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_EmissionColor");

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetColor("_EmissionColor", Color.red);
        }
        yield return new WaitForEndOfFrame();
        
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetColor("_EmissionColor", originalColor);
        }
    }
}
