using System.Collections;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Rigidbody2D rb;
    [SerializeField]
    float speed;
    Vector2 direction;

    public float Speed { get => speed; set => speed = value; }
    public Vector2 Direction { get => direction; set => direction = value; }

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

    public virtual void Initialize(Vector2 direction)
    {
        this.Direction = direction;
        gameObject.tag = transform.parent.gameObject.tag;
    }

    public void SetLayer(int layer)
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Explode());
    }
}