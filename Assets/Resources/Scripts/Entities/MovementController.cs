using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    Actor parentActor;
    bool isLocked;
    [SerializeField]
    bool isDead = false;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 10f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    private Rigidbody2D rb;

    Vector3 m_Velocity = Vector3.zero;
    Vector2 movementVector = Vector2.zero;

    public Vector2 MovementVector { get => movementVector; set => movementVector = value; }
    public Actor ParentActor { get => parentActor; set => parentActor = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float MovementSmoothing { get => movementSmoothing; set => movementSmoothing = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    protected void InitializeMovement()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead)
        {
            movementSpeed = 0;
            movementVector = Vector2.zero;
        }
        if (isLocked)
        {
            AnimatorStateInfo animInfo = ParentActor.animationController.Anim.GetCurrentAnimatorStateInfo(0);
            if (!(animInfo.IsName("spawn") && animInfo.normalizedTime < 1))
            {
                isLocked = false;
            }
        }
    }

    public virtual void Move()
    {
        if (!IsLocked)
        {
            Vector3 targetVelocity = MovementVector * MovementSpeed;
            Rb.velocity = Vector3.SmoothDamp(Rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        }
    }

    public virtual void Stop()
    {
        MovementVector = Vector2.zero;
        Rb.velocity = Vector3.zero;
    }

    public virtual void Deactivate()
    {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
    }

    public void ResetAfterDeath()
    {
        movementVector = Vector2.zero;
        isDead = false;
        isLocked = true;
    }
}
