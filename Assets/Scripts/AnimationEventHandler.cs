using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent<AttackType> OnAttack, PostAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Hitbox;
    private void OnAttackStart(){
        OnAttack?.Invoke(AttackType.Primary);
        PostAttack?.Invoke(AttackType.Primary);
    }

    private void OnAnimationEnd()
    {
        Debug.Log("end");
        // PostAttack?.Invoke(AttackType.Primary);
    }

    private void OnRangeAttackStart(){
        OnAttack?.Invoke(AttackType.Secondary);
    }

    private void OnRangeAnimationEnd()
    {
        PostAttack?.Invoke(AttackType.Secondary);
    }

    private void OnStartDoingDamage(){
        BoxCollider2D box = Hitbox.AddComponent<BoxCollider2D>();

        // Set the custom offset and size
        box.offset = new Vector2(-0.02240656f, -0.1531123f);  // X, Y offset
        box.size = new Vector2(0.3244791f, 0.6237754f);        // Width, Height
        box.isTrigger = true;

    }
}
