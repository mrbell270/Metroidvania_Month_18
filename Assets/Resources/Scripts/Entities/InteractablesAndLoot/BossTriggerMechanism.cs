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

    bool isFightActive = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !IsOn)
        {
            GetComponent<Collider2D>().enabled = false;
            StartBossFight();
        }
    }

    private void Update()
    {
        foreach (Mechanism gate in gates)
        {
            gate.ChangeState(!isFightActive);
        }
    }

    public void StartBossFight()
    {
        isFightActive = true;
        boss.SetActive(true);
        // TODO: proper animation
        loot.SetActive(false);
    }

    public void EndBossFight()
    {
        isFightActive = false;
        // TODO: proper animation
        loot.SetActive(true);
        ChangeState(true);
    }
}
