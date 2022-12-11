using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int damageOverTime;
    [SerializeField]
    private AttackType type;
    [SerializeField]
    private float attackTime;
    [SerializeField]
    private float releaseTime;
    [SerializeField]
    private float range;
    [SerializeField]
    private bool alwaysVisible = false;
    [SerializeField]
    private float visibilityRangeSqr = 0.25f;
    private bool isBusy = false;
    private Vector2 startLocalPosition;
    private List<Renderer> visuals;

    public int Damage { get => damage; set => damage = value; }
    public AttackType Type { get => type; set => type = value; }
    public float Range { get => range; set => range = value; }
    public float AttackTime { get => attackTime; set => attackTime = value; }
    public float ReleaseTime { get => releaseTime; set => releaseTime = value; }
    public int DamageOverTime { get => damageOverTime; set => damageOverTime = value; }
    public bool IsBusy { get => isBusy; set => isBusy = value; }
    public float VisibilityRangeSqr { get => visibilityRangeSqr; set => visibilityRangeSqr = value; }

    public virtual void Start()
    {
        startLocalPosition = transform.localPosition;
        visuals = new List<Renderer>(GetComponentsInChildren<SpriteRenderer>());
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            visuals.Add(sr);
        }
    }

    public virtual void Update()
    {
        if (!alwaysVisible)
        {
            foreach (SpriteRenderer sr in visuals)
            {
                sr.enabled = Vector2.SqrMagnitude((Vector2)transform.localPosition - startLocalPosition) > VisibilityRangeSqr;
            }
        }
    }

    public virtual void Animate(Actor actor, Vector2 direction)
    {
        if (!IsBusy)
        {
            IsBusy = true;
            StartCoroutine(AttackCoroutine(actor, direction));
        }
    }

    protected abstract IEnumerator AttackCoroutine(Actor actor, Vector2 direction);
}
