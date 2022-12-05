using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : Weapon
{
    [SerializeField]
    GameObject aura;
    [SerializeField]
    float peakTime;
    [SerializeField]
    float pauseTime;

    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        GetComponent<Collider2D>().enabled = true;

        float elapsedTime;
        Vector2 idleScale = new(0.1f, 0.1f);
        Vector2 peakScale = Range * Vector2.one;

        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            float t = elapsedTime / AttackTime;
            t = t * t * t;
            transform.localScale = Vector2.Lerp(idleScale, peakScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = peakScale;

        yield return new WaitForSeconds(peakTime);

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float t = elapsedTime / ReleaseTime;
            t = 1 - t * t;
            transform.localScale = Vector2.Lerp(idleScale, peakScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = idleScale;

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(pauseTime);

        IsBusy = false;
    }
}
