using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackSpeed = 2;
    public int attackPower = 1;
    private float lastAttackTime;
    public ParticleSystem dirtParticle;
    public Text attackPowerLbl;
    public Text attackSpeedLbl;
    public Text maxHealthLbl;

    public HealthBar healthBar;
    public Text damageLbl;
    public int maxHealth = 10;
    public int health;
    public ParticleSystem explosion;

    void Start()
    {
        lastAttackTime = Time.time;
        attackPowerLbl.text = "Attack Power (" + attackPower + ")";
        attackSpeedLbl.text = "Attack Speed (" + attackSpeed + ")";
        maxHealthLbl.text = "Max Health (" + maxHealth + ")";
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null && AttackIsAvailable())
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, (nearestEnemy.transform.position - transform.position), 6, 0.0F);
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(newDir)) as GameObject;
            projectile.GetComponent<Projectile>().power = attackPower;
            lastAttackTime = Time.time;
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
            return null;

        GameObject nearestEnemy = enemies[0];
        float distanceForNearestEnemy = Vector3.Distance(nearestEnemy.transform.position, transform.position);
        if (enemies.Length > 1)
        {
            for (int i = 1; i < enemies.Length; i++)
            {
                GameObject currentEnemy = enemies[i];
                float distanceForCurrentEnemy = Vector3.Distance(currentEnemy.transform.position, transform.position);
                if (distanceForCurrentEnemy < distanceForNearestEnemy)
                {
                    distanceForNearestEnemy = distanceForCurrentEnemy;
                    nearestEnemy = currentEnemy;
                }
            }
        }

        return nearestEnemy;
    }

    private bool AttackIsAvailable()
    {
        return Time.time - lastAttackTime > attackSpeed;
    }

    public void SetRun()
    {
        dirtParticle.gameObject.SetActive(true);
        dirtParticle.Play();
    }

    public void SetStop()
    {
        dirtParticle.Stop();
    }

    public void EnhanceAttackPower()
    {
        if (ScoreManager.instance.TakeOffScore(1))
        {
            attackPower++;
            attackPowerLbl.text = "Attack Power (" + attackPower + ")";
        }
    }

    public void EnhanceAttackSpeed(int price)
    {
        if (ScoreManager.instance.TakeOffScore(price))
        {
            attackSpeed -= 0.05f;
            attackSpeedLbl.text = "Attack Speed (" + attackSpeed + ")";
        }
    }

    public void EnhanceMaxHealth(int price)
    {
        if (ScoreManager.instance.TakeOffScore(price))
        {
            maxHealth += 1;
            maxHealthLbl.text = "Max Health (" + maxHealth + ")";
            healthBar.SetMaxHealth2(maxHealth);
        }
    }

    public void Heal(int price)
    {
        if (ScoreManager.instance.TakeOffScore(price))
        {
            health++;
            healthBar.SetHealth(health);
        }
    }

    public void Hit(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        StartCoroutine(ShowDamageLbl(damage));
        if (health <= 0)
        {
            explosion.Play();
            SpawnManager.instance.GameOver();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowDamageLbl(int value)
    {
        damageLbl.text = "-" + value;
        damageLbl.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageLbl.gameObject.SetActive(false);
    }
}
