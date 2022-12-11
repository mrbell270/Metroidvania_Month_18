using System.Collections;
using UnityEngine;
public class ShovelWeapon : Weapon
{
    [SerializeField]
    float prepTime;
    [SerializeField]
    float angle = 60;
    [SerializeField]
    GameObject body;
    TrailRenderer tr;
    protected override IEnumerator AttackCoroutine(Actor actor, Vector2 direction)
    {
        float elapsedTime;
        direction.Normalize();
        tr = body.GetComponent<TrailRenderer>();
        tr.enabled = false;

        Vector2 idlePosition = body.transform.localPosition;
        Vector2 peakPosition = (Range * Vector2.right) + idlePosition;

        Vector3 idleRotation = transform.localEulerAngles;
        float peakAngle = Vector2.SignedAngle(Vector2.right, direction);
        float startAngle = peakAngle - angle;
        float endAngle = peakAngle + angle;
        Vector3 startRotation = new Vector3(0, 0, startAngle);
        Vector3 endRotation = new Vector3(0, 0, endAngle);


        elapsedTime = 0f;
        while (elapsedTime < prepTime)
        {
            float tPos = elapsedTime / prepTime;
            tPos = tPos * tPos;
            float tRot = 1 - Mathf.Pow((1 - elapsedTime / prepTime), 4);
            body.transform.localPosition = Vector2.Lerp(idlePosition, peakPosition, tPos);
            transform.localEulerAngles = Vector3.Lerp(idleRotation, startRotation, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        body.transform.localPosition = peakPosition;
        transform.localEulerAngles = startRotation;

        tr.enabled = true;
        GetComponent<Collider2D>().enabled = true;

        elapsedTime = 0f;
        while (elapsedTime < AttackTime)
        {
            float tPos = elapsedTime / AttackTime;
            tPos = Mathf.Pow(tPos, 4);
            float tRot = tPos;
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localEulerAngles = endRotation;

        tr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        elapsedTime = 0f;
        while (elapsedTime < ReleaseTime)
        {
            float tPos = elapsedTime / ReleaseTime;
            tPos = tPos * tPos;
            float tRot = Mathf.Pow(elapsedTime / ReleaseTime, 4);
            body.transform.localPosition = Vector2.Lerp(peakPosition, idlePosition, tPos);
            transform.localEulerAngles = Vector3.Lerp(endRotation, idleRotation, tRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        body.transform.localPosition = idlePosition;
        transform.localEulerAngles = idleRotation;

        IsBusy = false;
    }

    public override void Update()
    {
        body.GetComponent<SpriteRenderer>().enabled = Vector2.SqrMagnitude(body.transform.localPosition) > VisibilityRangeSqr;
    }
}
