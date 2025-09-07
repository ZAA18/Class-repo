using UnityEngine;

public class BulletBoth : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float lifetime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bullet hit the player!");

            HEALTHSYTEM playerHealth = collision.gameObject.GetComponent<HEALTHSYTEM>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit a wall!");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        { Debug.Log("Bullet hit the ground");
            Destroy(gameObject);
        }
    }
}
