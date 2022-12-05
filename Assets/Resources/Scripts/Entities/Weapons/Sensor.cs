using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    float radius = 2f;
    public bool isSensoring;
    public Vector2 position;
    public Vector2 direction;
    string comparedTag = "Player";

    private Vector2 noDirection = Vector2.zero;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(comparedTag))
        {
            isSensoring = true;
            position = collision.gameObject.transform.position;
            direction = collision.gameObject.transform.position - transform.position;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(comparedTag))
        {
            isSensoring = true;
            position = collision.gameObject.transform.position;
            direction = collision.gameObject.transform.position - transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(comparedTag))
        {
            isSensoring = false;
            position = noDirection;
            direction = noDirection;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
