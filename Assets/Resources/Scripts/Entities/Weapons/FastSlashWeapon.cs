using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSlashWeapon : Weapon
{
    [SerializeField]
    float prepTime;
    [SerializeField]
    float pauseTime;
    [SerializeField]
    float angle = 30;
    TrailRenderer tr;
        
    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        float elapsedTime;
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;

        Vector2 idlePosition = transform.localPosition;
        float sin1 = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos1 = Mathf.Cos(angle * Mathf.Deg2Rad);
        float sin2 = Mathf.Sin(-angle * Mathf.Deg2Rad);
        float cos2 = Mathf.Cos(-angle * Mathf.Deg2Rad);
        Vector2 peakDirection1 = new Vector2((cos1 * direction.x) - (sin1 * direction.y), (sin1 * direction.x) + (cos1 * direction.y));
        Vector2 peakDirection2 = new Vector2((cos2 * direction.x) - (sin2 * direction.y), (sin2 * direction.x) + (cos2 * direction.y));
        Vector2 peakPosition1 = (Range * peakDirection1) + idlePosition;
        Vector2 peakPosition2 = (Range * peakDirection2) + idlePosition;

        Vector3 idleRotation = transform.localEulerAngles;
        float peakAngle1 = Vector2.SignedAngle(Vector2.right, peakDirection1);
        float peakAngle2 = Vector2.SignedAngle(Vector2.right, peakDirection2);
        bool peakAngleFixFlg = peakAngle1 < 0 && peakAngle2 > 0;
        Vector3 peakRotation1 = new Vector3(0, 0, peakAngle1);
        Vector3 peakRotation2 = new Vector3(0, 0, peakAngle2);


        elapsedTime = 0f;
        while (elapsedTime < prepTime)
        {
            float tPos = elapsedTime / prepTime;
            tPos = tPos * tPos;
            float tRot = 1 - Mathf.Pow((1 - elapsedTime / prepTime), 4);
            transform.localPosition = Vector2.Lerp(idlePosition, peakPosition1, tPos);
            transform.localEulerAngles = Vector3.Lerp(idleRotation, peakRotation1, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = peakPosition1;
        transform.localEulerAngles = peakRotation1;

        GetComponent<Collider2D>().enabled = true;
        tr.enabled = true;

        if (peakAngleFixFlg)
        {
            peakRotation1 = new Vector3(0, 0, 360 + peakAngle1);
        }
        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            float tPos = elapsedTime / AttackTime;
            tPos = Mathf.Pow(tPos, 4);
            float tRot = tPos;
            transform.localPosition = Vector2.Lerp(peakPosition1, peakPosition2, tPos);
            transform.localEulerAngles = Vector3.Lerp(peakRotation1, peakRotation2, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = peakPosition2;
        transform.localEulerAngles = peakRotation2;

        tr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float tPos = elapsedTime / ReleaseTime;
            tPos = tPos * tPos;
            float tRot = Mathf.Pow(elapsedTime / ReleaseTime, 4);
            transform.localPosition = Vector2.Lerp(peakPosition2, idlePosition, tPos);
            transform.localEulerAngles = Vector3.Lerp(peakRotation2, idleRotation, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = idlePosition;
        transform.localEulerAngles = idleRotation;

        yield return new WaitForSeconds(pauseTime);
        IsBusy = false;
    }
}
