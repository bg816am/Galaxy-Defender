using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    [SerializeField] GameObject laserPrefab = default;
    private AudioSource _explosionSound = default;
    private Animator _anim = default;
    [SerializeField] private int pointValue = 10;
    [SerializeField] private float enemySpeed = 4f;
    private Player _player = default;
    private float _enemyCeiling = 7.4f;
    private float _enemyFloor = -5.4f;

    private void Start()
    {
        _explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.FindObjectOfType<Player>();
        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Sound is null");
        }
        
        if (_player == null)
        {
            Debug.LogError("Player does not exist");
        }

        if (_anim == null)
        {
            Debug.LogError("Animation does not exist");
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].TagAsEnemy();
            }
        }
    }

    void MoveEnemy()
    {
        float randomX = Random.Range(-8f, 8f);
        transform.Translate(Vector3.down *  (Time.deltaTime * enemySpeed));
        if (transform.position.y <= _enemyFloor)
        {
            transform.position = new Vector3(randomX,_enemyCeiling,0);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (_player != null)
            {
                _player.Damage();
            }
            DeathSequence();
            
        }
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddToScore(pointValue);
            }
            DeathSequence();
        }
        
    }

    private void DeathSequence()
    {
        _anim.SetTrigger("OnEnemyDeath");
        Destroy(GetComponent<Collider2D>());
        enemySpeed = 0;
        _explosionSound.Play();
        Destroy(gameObject,2.8f);
    }

}
