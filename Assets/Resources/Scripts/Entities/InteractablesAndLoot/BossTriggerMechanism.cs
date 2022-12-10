using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerMechanism: Mechanism
{
    [SerializeField]
    List<Mechanism> gates = new();
    [SerializeField]
    GameObject boss;
    [SerializeField]
    GameObject loot;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !IsOn)
        {
            GetComponent<Collider2D>().enabled = false;
            StartBossFight();
        }
    }

    public void StartBossFight()
    {
        boss.SetActive(true);
        // TODO: proper animation
        loot.SetActive(false);
        foreach (Mechanism gate in gates)
        {
            gate.ChangeState(true);
        }
    }

    public void EndBossFight()
    {
        // TODO: proper animation
        loot.SetActive(true);
        foreach(Mechanism gate in gates)
        {
            gate.ChangeState(true);
        }
        ChangeState(true);
    }
}
