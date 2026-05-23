using UnityEngine;

public class SoldierFall : MonoBehaviour
{
    private Animator animator;
    private bool hasFallen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasFallen)
            return;

        if (other.CompareTag("Player"))
        {
            hasFallen = true;

            if (animator != null)
                animator.SetBool("Fall", true);
        }
    }
}
