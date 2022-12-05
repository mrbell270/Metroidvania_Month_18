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

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOn", IsOn);

        SceneIdx = gameObject.scene.buildIndex;
        MechName = gameObject.name;
    }

    public void ChangeState(bool shouldBe)
    {
        IsOn = shouldBe;
        animator.SetBool("isOn", shouldBe);
    }

    public string GetState()
    {
        return JsonUtility.ToJson(this); ;
    }

    public string SetState(string json)
    {
        return JsonUtility.ToJson(this); ;
    }
}
