using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistWeapon : Weapon
{
    [SerializeField]
    GameObject fist1;
    [SerializeField]
    GameObject fist2;
    [SerializeField]
    float pauseTime;
    Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        float elapsedTime = 0f;

        animator.SetTrigger("punch");

        Vector2 idlePosition = transform.localPosition;
        Vector2 peakPosition = (Range * direction) + idlePosition;

        Vector3 idleRotation = transform.localEulerAngles;
        float peakAngle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 peakRotation = new Vector3(0, 0, peakAngle);

        GetComponent<Collider2D>().enabled = true;

        transform.localEulerAngles = peakRotation;
        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            float tPos = elapsedTime / AttackTime;
            tPos = 1 - Mathf.Pow((tPos - 1), 4);
            transform.localPosition = Vector2.Lerp(idlePosition, peakPosition, tPos);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = peakPosition;

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float t = elapsedTime / ReleaseTime;
            t = Mathf.Pow((t - 1), 2);
            transform.localPosition = Vector2.Lerp(idlePosition, peakPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = idlePosition;
        transform.localEulerAngles = idleRotation;

        IsBusy = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
