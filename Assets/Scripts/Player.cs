using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : MonoBehaviour
{
    private Animator _playerExplosion = default;
    private AudioSource _audioSource = default;
    //Components
    private UIManager _uiManager = default;
    private SpawnManager _spawnManager = default;
    //Misc
    private int _score = 0;
    private float _projectileOffset = 1.0f;
    //PowerUp Toggles
    private bool _shieldsActive = false;
    private bool _tripleShotOn = false;

    [Header("PowerUps")] 
    [SerializeField] private int shieldHp = 0;
    [SerializeField] private float powerUpDuration = 5f;
    [FormerlySerializedAs("shields")] [SerializeField] private GameObject[] shieldsPrefab;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab = default;
    [SerializeField] private int playerLives = 3;

    [Header("Player Info")] 
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float boostSpeed = 20f;
    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private float firingRate = 0.5f;
    [SerializeField] private GameObject leftThruster;
    [SerializeField] private GameObject rightThruster;
    [SerializeField] private GameObject mainThruster;
    //Movement Limits
    private float topLimit = 0f;
    private float bottomLimit = -3.8f;
    private float leftLimit = -11.3f;
    private float rightLimit = 11.3f;
    private float _canFire = -1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _playerExplosion = GetComponent<Animator>();
        _audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        leftThruster.SetActive(false);
        rightThruster.SetActive(false);
        _uiManager = GameObject.FindObjectOfType<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        
        if (_audioSource == null)
        {
            Debug.LogError("Laser sound is null");
        }
        if (_uiManager == null)
        {
           Debug.LogError("UI is null"); 
        }
        
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        //take the current position and assign it to 0,0,0 position
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void PlayerMovement()
    {
       IsBoostActive();

        //Player movement and speed
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate( horizontalInput * (playerSpeed * Time.deltaTime), verticalInput * (playerSpeed *Time.deltaTime),0);
        
        //Clamp y positions
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottomLimit, topLimit));
        
        //wrap around for X
        if (transform.position.x <= leftLimit)
        {
            transform.position = new Vector3(rightLimit, transform.position.y,0);
        }
        else if (transform.position.x >= rightLimit)
        {
            transform.position = new Vector3(leftLimit,transform.position.y,0);
        }  
    }

    void FireLaser()
    {
        Vector3 shotOffset = new Vector3(0, _projectileOffset, 0);
        _canFire = Time.time + firingRate;
        if (_tripleShotOn == true)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, (transform.position + shotOffset), Quaternion.identity); 
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shieldsActive == true)
        {
            shieldHp--;
            if (shieldHp == 2)
            {
                shieldsPrefab[0].SetActive(false);
                shieldsPrefab[1].SetActive(true);
                return;
            } else if (shieldHp == 1)
            {
                shieldsPrefab[1].SetActive(false);
                shieldsPrefab[2].SetActive(true);
                return;
            }
            else
            {
                _shieldsActive = false;
                shieldsPrefab[2].SetActive(false);
                return;
            }
           
        }

        if (_shieldsActive == false)
        {

            playerLives -= 1;
            if (playerLives == 2)
            {
                leftThruster.SetActive(true);
            }

            if (playerLives == 1)
            {
                rightThruster.SetActive(true);
            }

            //if lives is one, other thruster
            _uiManager.UpdateLives(playerLives);
            if (playerLives < 1)
            {
                mainThruster.SetActive(false);
                leftThruster.SetActive(false);
                rightThruster.SetActive(false);
                _playerExplosion.SetTrigger("OnEnemyDeath");
                _uiManager.GameOver();
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject, 2.8f);
            }
        }
    }
        

    public void SpeedBoostActive()
    {
        playerSpeed = 25;
        StartCoroutine(PowerUpCooldown());
    }

    public void TripleShotActive()
    {
        _tripleShotOn = true;
        StartCoroutine(PowerUpCooldown());
    }
    
    public void ActivateShields()
    {
        shieldHp = 3;
        _shieldsActive = true;
        shieldsPrefab[0].SetActive(true);
        shieldsPrefab[1].SetActive(false);
        shieldsPrefab[2].SetActive(false);
    }
    IEnumerator PowerUpCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        _tripleShotOn = false;
        playerSpeed = 10f;
    }

    public void AddToScore(int pointsPerEnemy)
    {
        _score += pointsPerEnemy;
    }

    public int GetScore()
    {
        return _score;
    }

    private void IsBoostActive()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = boostSpeed;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = baseSpeed;
        }
    }
}


