using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent<AttackType> OnAttack, PostAttack;
    public Enemy enemy; // Reference to the enemy script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAttackStart(){
        // This method will be called when the attack animation starts
        // You can add your logic here, such as enabling hitboxes or triggering effects
        Debug.Log("Attack animation has started.");
        OnAttack?.Invoke(AttackType.Primary);

    }

    private void OnAnimationEnd()
    {
        // This method will be called when the animation ends
        // You can add your logic here, such as resetting the animator or triggering other events
        Debug.Log("Animation has ended.");
        if(enemy != null){
            enemy.SetAttackState(false,Time.time);
        }
        PostAttack?.Invoke(AttackType.Primary);
    }
}
