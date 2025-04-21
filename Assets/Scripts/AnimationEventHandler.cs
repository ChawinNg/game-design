using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent<AttackType> OnAttack, PostAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAttackStart(){
        OnAttack?.Invoke(AttackType.Primary);

    }

    private void OnAnimationEnd()
    {
        PostAttack?.Invoke(AttackType.Primary);
    }

    private void OnRangeAttackStart(){
        OnAttack?.Invoke(AttackType.Secondary);
    }

    private void OnRangeAnimationEnd()
    {
        PostAttack?.Invoke(AttackType.Secondary);
    }
}
