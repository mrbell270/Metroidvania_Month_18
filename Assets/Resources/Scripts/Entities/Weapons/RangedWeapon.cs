using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    GameObject skin;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    float pauseTime;

    public override void Start()
    {
        base.Start();
    }
    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        float elapsedTime = 0f;
        direction.Normalize();

        Vector2 idlePosition = skin.transform.localPosition;
        Vector2 peakPosition = (Range * direction) + idlePosition;

        Vector3 idleRotation = skin.transform.localEulerAngles;
        float peakAngle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 peakRotation = new Vector3(0, 0, peakAngle);

        skin.transform.localEulerAngles = peakRotation;
        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            float tPos = elapsedTime / AttackTime;
            tPos = 1 - Mathf.Pow((tPos - 1), 4);
            skin.transform.localPosition = Vector2.Lerp(idlePosition, peakPosition, tPos);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        skin.transform.localPosition = peakPosition;

        ProjectileWeapon projectile = Instantiate(projectilePrefab, transform).GetComponent<ProjectileWeapon>();
        if (projectile != null)
        {
            projectile.transform.SetLocalPositionAndRotation(direction, Quaternion.identity);
            projectile.gameObject.SetActive(true);
            projectile.Initialize();
            projectile.rb.AddForce(direction.normalized * projectile.speed);
        }

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float t = elapsedTime / ReleaseTime;
            t = Mathf.Pow((t - 1), 2);
            skin.transform.localPosition = Vector2.Lerp(idlePosition, peakPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        skin.transform.localPosition = idlePosition;
        skin.transform.localEulerAngles = idleRotation;

        yield return new WaitForSeconds(pauseTime);

        IsBusy = false;
    }
}
