using UnityEngine;

public class Laser : MonoBehaviour
{
    private bool _isEnemyLaser = false;
    private float _screenTop = 8f;
    private float _screenBottom = -5.5f;
    [SerializeField] private float laserSpeed = 8f;
   
    private void Update()
    {
        if (_isEnemyLaser == false)
        {
            ShootPlayerLaser();
        }
        else
        {
            ShootEnemyLaser();
        }
        
        
    }

    private void ShootPlayerLaser()
    {
        transform.Translate(Vector3.up * (laserSpeed * Time.deltaTime));
            if (transform.position.y >= _screenTop)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(gameObject);
            }
    }
    private void ShootEnemyLaser()
    {
        transform.Translate(Vector3.down * (laserSpeed * Time.deltaTime));
            if (transform.position.y <= _screenBottom)
            {
                Destroy(gameObject);
            }
    }

    public void TagAsEnemy()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}


