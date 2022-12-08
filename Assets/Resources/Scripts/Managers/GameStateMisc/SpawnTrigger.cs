using System.Collections;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.GetInstance().SetLevelSpawn(transform.position);
        }
    }
}
