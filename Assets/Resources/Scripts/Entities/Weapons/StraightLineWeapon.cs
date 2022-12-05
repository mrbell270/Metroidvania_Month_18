using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineWeapon : Weapon
{
    [SerializeField]
    float prepTime;
    [SerializeField]
    float pauseTime;
    [SerializeField]
    float minWidth = 0.2f;
    [SerializeField]
    float maxWidth = 0.8f;
    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        float elapsedTime;

        Vector2 idlePosition = transform.localPosition;
        Vector2 peakPosition = (Range/2 * direction.normalized) + idlePosition;
        transform.localPosition = peakPosition;

        Vector3 idleRotation = transform.localEulerAngles;
        float peakAngle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 peakRotation = new Vector3(0, 0, peakAngle);
        transform.localEulerAngles = peakRotation;

        Vector2 idleScale = transform.localScale;
        Vector3 startScale = new Vector3(Range, minWidth, 0);
        Vector3 peakScale = new Vector3(Range, maxWidth, 0);
        transform.localScale = startScale;


        elapsedTime = 0f;
        while (elapsedTime < prepTime)
        {
            float tScale = elapsedTime / prepTime;
            tScale = Mathf.Pow(tScale, 20);
            float tRot = 1 - Mathf.Pow((1 - elapsedTime / prepTime), 4);
            transform.localScale = Vector2.Lerp(startScale, peakScale, tScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = peakScale;


        GetComponent<Collider2D>().enabled = true;

        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GetComponent<Collider2D>().enabled = false;

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float tScale = elapsedTime / ReleaseTime;
            tScale = Mathf.Pow(tScale, 5);
            transform.localScale = Vector2.Lerp(peakScale, startScale, tScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = startScale;


        transform.localPosition = idlePosition;
        transform.localEulerAngles = idleRotation;
        transform.localScale = idleScale;

        yield return new WaitForSeconds(pauseTime);
        IsBusy = false;
    }
}
