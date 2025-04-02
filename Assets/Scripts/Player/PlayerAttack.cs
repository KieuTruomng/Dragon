using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Melee Attack")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip meleeAttackSound;
    private Animator anim;
    private bool canAttack = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned in PlayerAttack script!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            StartCoroutine(PerformMeleeAttack());
        }
    }

    private IEnumerator PerformMeleeAttack()
    {
        if (attackPoint == null) yield break;
        
        canAttack = false;
        anim.SetTrigger("shoot");
        
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(meleeAttackSound);
        }
        else
        {
            Debug.LogError("SoundManager instance is null!");
        }

        yield return new WaitForSeconds(0.2f);
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Health>() != null)
            {
                enemy.GetComponent<Health>().TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
