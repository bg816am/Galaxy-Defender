using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AudioSource _explosionSound = default;
    [SerializeField] private float destroyDelay = 0.25f;
    [SerializeField] private GameObject explosion = default;
    [SerializeField] private float rotationSpeed = 3f;
    private SpawnManager _spawnManager = default;

    private void Start()
    {
        _explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Sound is null");
        }
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0,0,rotationSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Instantiate(explosion,transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            _explosionSound.Play();
            Destroy(gameObject, destroyDelay);
        }
    }
}
