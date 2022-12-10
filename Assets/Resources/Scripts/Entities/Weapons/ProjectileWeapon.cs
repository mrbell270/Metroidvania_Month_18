using System.Collections;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Rigidbody2D rb;
    public float speed;

    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        yield return null;
    }

    IEnumerator Explode()
    {
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize()
    {        
        gameObject.tag = transform.parent.gameObject.tag;
    }

    public void SetLayer(int layer)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Explode());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Explode());
    }
}