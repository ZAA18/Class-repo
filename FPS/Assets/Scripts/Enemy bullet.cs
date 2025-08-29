using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    [SerializeField] private float maxhealth = 50;

    private float currentHealth;

    private HEALTHSYTEM HEALTHSYTEM;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
        HEALTHSYTEM.UpdatehealthBar(maxhealth, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("hit" + collision.gameObject.name + "!");
            currentHealth -= Random.Range(0.5f, 1.5f);
            Destroy(gameObject);
            HEALTHSYTEM.UpdatehealthBar(maxhealth, currentHealth);
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            print("hit the wall" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }
    }
}
