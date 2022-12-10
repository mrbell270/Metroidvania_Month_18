using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelBossEnemy : MonoBehaviour
{
    Animator animator;

    [Header("Movement")]
    Vector2 minPos;
    Vector2 maxPos;
    [SerializeField] private float movementSpeed = 10f;
    private Rigidbody2D rb;
    Vector2 movementVector = Vector2.zero;
    Seeker seeker;
    bool isSearchingPath = false;
    List<Vector3> pathLeftToGo = new List<Vector3>();
    [SerializeField] float gridRadius = 0.02f;

    [Header("Drops")]
    [SerializeField]
    List<BossDrop> drops = new List<BossDrop>();
    [SerializeField]
    BossTriggerMechanism bossTrigger;

    [Header("Battle")]
    [SerializeField]
    int maxHealthPoints;
    [SerializeField]
    int curHealthPoints;
    [SerializeField]
    float waitingTime = 2f;

    [Header("Attacks")]
    [SerializeField]
    List<GameObject> prefabs = new();
    [SerializeField]
    List<GameObject> spawners = new();

    public int CurHealthPoints { get => curHealthPoints; set => curHealthPoints = value; }
    public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }
    public float WaitingTime { get => waitingTime; set => waitingTime = value; }


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        curHealthPoints = maxHealthPoints;
        minPos = new Vector2(transform.position.x - 14, transform.position.y - 6.5f);
        maxPos = new Vector2(transform.position.x + 14, transform.position.y + 6.5f);
    }


    public void Move()
    {
        if (!isSearchingPath)
        {
            if (pathLeftToGo.Count > 0)
            {
                movementVector = (pathLeftToGo[0] - transform.position).normalized;
                if ((transform.position - pathLeftToGo[0]).sqrMagnitude < gridRadius)
                {
                    movementVector = Vector2.zero;
                    pathLeftToGo.RemoveAt(0);
                }
            }
            else
            {
                movementVector = Vector2.zero;
                animator.SetTrigger("Wait");
            }

            transform.position += (Vector3)movementVector * movementSpeed * Time.deltaTime;
            //Vector3 targetVelocity = movementVector * movementSpeed;
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        }
    }
    //////////////////
    // Navigation
    public void GetMoveCommandRandom()
    {
        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);
        Vector2 randomPoint = new Vector2(randomX, randomY);
        GetMoveCommand(randomPoint);
    }

    void GetMoveCommand(Vector2 target)
    {
        if (!isSearchingPath)
        {
            isSearchingPath = true;
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        isSearchingPath = false;
        if (p.error)
        {
            isSearchingPath = false;
            Debug.Log("ERROR: No path for " + gameObject.name);
        }
        else
        {
            pathLeftToGo = p.vectorPath;
        }
    }
    // Navigation end
    //////////////////


    public void Dig()
    {
        int prefabIdx = Random.Range(0, prefabs.Count);

        Instantiate(prefabs[prefabIdx], transform.position, Quaternion.identity).SetActive(true);
    }

    public virtual void TakeDamage(Weapon attack)
    {
        // TODO: Animate hit
        curHealthPoints -= attack.Damage;
        if (curHealthPoints <= maxHealthPoints / 2)
        {
            animator.SetTrigger("Upgrade");
        }
        if (curHealthPoints <= 0)
        {
            SetDead();
        }
    }


    public void SetDead()
    {
        bossTrigger.EndBossFight();
        animator.SetBool("isDead", true);
        // colliders
        Drop();
    }


    void Drop()
    {
        foreach (BossDrop drop in drops)
        {
            for (int i = 0; i < drop.amount; i++)
            {
                float x = Random.Range(-0.5f, 0.5f);
                float y = Random.Range(-0.5f, 0.5f);
                Vector3 direction = new Vector3(x, y, 0).normalized;
                GameObject d = Instantiate(drop.loot, transform);
                Loot l = d.GetComponent<Loot>();
                if (l != null) l.Move(direction);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Weapon w = collision.gameObject.GetComponent<Weapon>();
            if (w != null) TakeDamage(w);
        }
    }
}

[System.Serializable]
struct BossDrop
{
    public GameObject loot;
    public int amount;
}