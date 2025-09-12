using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject hit;
    public GameObject Fire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.AddForce(transform.forward * 2000);
        Instantiate(Fire, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(hit, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
