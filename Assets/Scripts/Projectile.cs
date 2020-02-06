using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject effect;
    public float areaOfEffect;
    public LayerMask whatIsDestructible;
    public int damage;
    [HideInInspector] public float speed;

    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 2.0f);
    }
    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Environment"))
        {
            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, areaOfEffect, whatIsDestructible);
            foreach (Collider2D objects in objectsToDamage)
            {
                objects.GetComponent<Destructable>().health -= damage;
            }
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } 
    }

    
private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}
