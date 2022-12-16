using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxRandomHealth = 5;
    public int maxHealth;
    public int health;
    public float leftMoveXBound = 0.5f;
    public HealthBar healthBar;
    public Text damageLbl;
    private Player player;
    private float attackDuration = 2;
    private float attackStartTime = 0;
    private bool canAttack = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = Random.Range(1, maxRandomHealth);
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (transform.position.x < leftMoveXBound && canAttack)
        {
            GetComponent<MoveLeft>().enabled = false;
            if (Time.time - attackStartTime > attackDuration && canAttack)
            {
                animator.SetTrigger("Attack");
                player.Hit(1);
                attackStartTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            int damage = other.GetComponent<Projectile>().power;
            health -= damage;
            if (health <= 0)
            {
                Debug.Log("Death!");
                animator.SetTrigger("Death");
                ScoreManager.instance.AddScore();
                canAttack = false;
            }
            StartCoroutine(ShowDamageLbl(damage));
            healthBar.SetHealth(health);
        }
    }

    private IEnumerator ShowDamageLbl(int value)
    {
        damageLbl.text = "-" + value;
        damageLbl.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageLbl.gameObject.SetActive(false);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
