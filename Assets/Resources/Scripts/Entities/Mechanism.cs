using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Mechanism : MonoBehaviour
{
    [SerializeField, ReadOnly]
    int sceneIdx;
    [SerializeField, ReadOnly]
    string mechName;
    [SerializeField]
    bool isOn = false;
    Animator animator;

    public bool IsOn { get => isOn; set => isOn = value; }
    public int SceneIdx { get => sceneIdx; set => sceneIdx = value; }
    public string MechName { get => mechName; set => mechName = value; }

    public void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null) animator.SetBool("isOn", IsOn);

        SceneIdx = gameObject.scene.buildIndex;
        MechName = gameObject.name;
    }

    public void ChangeState(bool shouldBe)
    {
        IsOn = shouldBe;
        if (animator != null) animator.SetBool("isOn", shouldBe);
    }
}
