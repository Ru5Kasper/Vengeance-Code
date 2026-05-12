using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void TriggerDie()
    {
        anim.SetTrigger("Die");
    }


}
