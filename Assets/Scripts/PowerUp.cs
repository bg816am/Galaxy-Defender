using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 3f;
    private float _screenBottom = -5.5f;
    private AudioSource _powerUpSound = default;
    [SerializeField] private int powerUpID = default;
    //0 is tripleshot
    //1 is speed boost
    //2 is shield
    private void Start()
    {
        _powerUpSound = GameObject.Find("PowerUpSound").GetComponent<AudioSource>();
        if (_powerUpSound == null)
        {
            Debug.LogError("Audio Source is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        transform.Translate(Vector3.down * (floatSpeed * Time.deltaTime));
        if (transform.position.y <= _screenBottom)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            _powerUpSound.Play();
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ActivateShields();
                        break;
                    default:
                        Debug.Log("Invalid PowerUp");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
