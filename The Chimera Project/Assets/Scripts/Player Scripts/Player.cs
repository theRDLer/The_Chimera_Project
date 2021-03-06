﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int attack;
    public int enemyAttack;

    public int currentHealth = 0;
    public int maxHealth = 100;
    public int enemyHealth = 25;
    public bool isAttacking = false;

    public bool enemyNear;
    public bool enemyDead;

    public GameObject thisOne;

    public HealthBar healthBar;

    private Rigidbody2D rb2d;
    public Animator anim;

    public SpriteRenderer playerRenderer;
    Color originalColor = Color.white;

    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyNear = true;
            thisOne = col.gameObject;
        }

        if (col.CompareTag("HealthPack"))
        {
            currentHealth += 15;
            healthBar.SetHealth(currentHealth);

            col.gameObject.SetActive(false);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemyNear = false;
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1")) & enemyNear)
        {

            isAttacking = true;
            anim.SetBool("IsAttacking", isAttacking);

            attack = Random.Range(1, 16);
            enemyAttack = Random.Range(1, 11);

            if (enemyAttack >= 5)
            {
                currentHealth -= enemyAttack;
                healthBar.SetHealth(currentHealth);

                StartCoroutine(Flasher(playerRenderer));
            }
            enemyHealth -= attack;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetBool("IsAttacking", isAttacking);
        }

        if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Fire1"))
        {
            isAttacking = false;
            anim.SetBool("IsAttacking", isAttacking);
        }

        if ((Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Fire1")) & enemyNear)
        {
            isAttacking = false;
            anim.SetBool("IsAttacking", isAttacking);
        }

        if (enemyHealth <= 0)
        {
            enemyDead = true;
            isAttacking = false;
            anim.SetBool("IsAttacking", isAttacking);
        }
        else
        {
            enemyDead = false;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(currentHealth <= 0)
        {
            die();
        }
    }

    void die()
    {
        Application.LoadLevel("Death Screen");
    }

    IEnumerator Flasher(SpriteRenderer spriteRenderer)
    {
        playerRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerRenderer.material.color = originalColor;
    }

}
