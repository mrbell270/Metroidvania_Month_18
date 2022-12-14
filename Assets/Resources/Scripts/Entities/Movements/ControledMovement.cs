using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ControledMovement : MovementController
{
    [SerializeField]
    float shiftTime = 0.5f;
    [SerializeField]
    float shiftRange = 1f;
    [SerializeField]
    LayerMask shiftableLayers;
    [SerializeField]
    LayerMask nonShiftableLayers;

    private void Awake()
    {
        InitializeMovement();
    }

    public override void Deactivate()
    {
        IsLocked = true;
        Stop();
    }

    public void Shift()
    {
        if (MovementVector.sqrMagnitude > 0.1f)
        {

            Vector2 shiftPoint = FindShiftPoint(MovementVector.normalized);

            ParentActor.battleController.DeactivateSoft();
            ParentActor.movementController.GetComponent<ControledMovement>().DeactivateSoft();
            ParentActor.animationController.Anim.SetBool("IsShifting", true);

            StartCoroutine(ShiftCoroutine(shiftPoint));
        }
    }

    Vector2 FindShiftPoint(Vector2 direction)
    {
        float multiplier;
        RaycastHit2D rh = Physics2D.Raycast(transform.position, direction, shiftRange, nonShiftableLayers);
        if(rh.distance == 0f)
        {
            multiplier = shiftRange;
        }
        else
        {
            multiplier = rh.distance;
        }
        Vector2 result;
        result = (Vector2)transform.position + multiplier * direction;
        while(multiplier > 0f && Physics2D.OverlapBox(result + new Vector2(0f, -0.25f), new Vector2(0.99f, 0.49f), 0f, shiftableLayers) != null)
        {
            multiplier -= 0.1f;
            result = (Vector2)transform.position + multiplier * direction;
        }

        return result;
    }

    IEnumerator ShiftCoroutine(Vector2 shiftPoint)
    {
        yield return new WaitForSeconds(0.15f);
        float elapsedTime = 0f;
        while (elapsedTime < shiftTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, shiftPoint, elapsedTime / shiftTime);
            yield return null;
        }
        transform.position = shiftPoint;

        ParentActor.animationController.Anim.SetBool("IsShifting", false);
        ParentActor.battleController.ReactivateSoft();
        ParentActor.movementController.GetComponent<ControledMovement>().ReactivateSoft();
    }

    public void DeactivateSoft()
    {
        IsLocked = true;
        IgnoreCollisionLayerMask(gameObject.layer, shiftableLayers, ignore: true);
    }

    public void ReactivateSoft()
    {
        IsLocked = false;
        IgnoreCollisionLayerMask(gameObject.layer, shiftableLayers, ignore: false);
    }

    void IgnoreCollisionLayerMask(int layer, LayerMask layerMask, bool ignore)
    {
        uint bitstring = (uint)layerMask.value;
        for (int i = 31; bitstring > 0; i--)
        {
            if ((bitstring >> i) > 0)
            {
                bitstring = ((bitstring << 32 - i) >> 32 - i);
                Physics2D.IgnoreLayerCollision(layer, i, ignore);
            }
        }
    }
}