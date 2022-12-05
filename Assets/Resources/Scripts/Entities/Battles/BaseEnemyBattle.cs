
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBattle : BattleController
{
    GameObject hitPrefab;
    private void Start()
    {
        InitializeBattleController();
        hitPrefab = Resources.Load("Prefabs/Hit") as GameObject;
    }

    public override void TakeDamage(Weapon att, bool mainDamage = true)
    {
        if(gameObject.activeInHierarchy) StartCoroutine(PlayHit());
        base.TakeDamage(att, mainDamage);
    }

    IEnumerator PlayHit()
    {
        GameObject hit = Instantiate(hitPrefab, transform);
        hit.transform.SetLocalPositionAndRotation(new Vector3(0, 0.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(hit);
    }
}
