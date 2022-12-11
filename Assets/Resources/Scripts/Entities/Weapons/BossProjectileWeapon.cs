using System.Collections;
using UnityEngine;

public class BossProjectileWeapon : ProjectileWeapon
{
    [SerializeField]
    float totalAttackTime = 10f;
    [SerializeField]
    float phaseChangeTime = 3f;
    float elapsedTime;

    [SerializeField]
    float startSpeed = 10;
    [SerializeField]
    float attackSpeed = 300;
    public override void Initialize(Vector2 direction)
    {
        base.Initialize(direction);
        Speed = startSpeed;
        transform.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        StartCoroutine(StartTimer());
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // don't stop
    }
    IEnumerator StartTimer()
    {
        while (elapsedTime <= totalAttackTime)
        {
            if (elapsedTime >= phaseChangeTime && Speed != attackSpeed)
            {
                Speed = attackSpeed;
                rb.AddForce(Direction * Speed);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}