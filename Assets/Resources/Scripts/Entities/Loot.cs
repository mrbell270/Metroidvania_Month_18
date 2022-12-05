using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    List<string> collectorTags = new List<string> { "Player" };
    [SerializeField]
    LootType type;
    [SerializeField]
    GameObject weaponPrefab;
    [SerializeField]
    bool isBusy = true;
    [SerializeField]
    SpriteRenderer lootSkin;

    [Header("Attraction")]
    [SerializeField]
    bool isAttractable;
    [SerializeField]
    float attractSpeed = 10f;
    [SerializeField]
    float attractEndDistance = 0.2f;
    [SerializeField]
    float attractStartDistance = 0.75f;
    Transform attractTransform;

    public LootType Type { get => type; set => type = value; }
    public GameObject WeaponPrefab { get => weaponPrefab; set => weaponPrefab = value; }
    public bool IsAttractable { get => isAttractable; set => isAttractable = value; }
    public bool IsBusy { get => isBusy; set => isBusy = value; }

    private void Start()
    {
        attractTransform = null;
        GetComponent<CircleCollider2D>().radius = isAttractable ? attractStartDistance : attractEndDistance;
        if(type == LootType.Weapon && lootSkin != null && weaponPrefab != null)
        {
            SpriteRenderer wsr = weaponPrefab.GetComponent<SpriteRenderer>();
            if (wsr != null) lootSkin.sprite = wsr.sprite;
        } 
    }

    void FixedUpdate()
    {
        if (attractTransform != null)
        {
            float distance = Vector3.Distance(attractTransform.position, transform.position);
            if (distance < attractEndDistance)
            {
                SetLooted(attractTransform.gameObject);
            }
            transform.position += attractSpeed * Time.deltaTime * (attractTransform.position - transform.position).normalized;
            attractSpeed += attractSpeed * Time.deltaTime;
        }
    }

    void SetLooted(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null && !IsBusy)
        {
            IsBusy = true;
            player.Loot(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsBusy) return;

        if (collectorTags.Contains(collision.gameObject.tag))
        {
            if (isAttractable) 
            { 
                attractTransform = collision.gameObject.transform; 
            }
            else
            {
                SetLooted(collision.gameObject);
            }
        }
    }
}
