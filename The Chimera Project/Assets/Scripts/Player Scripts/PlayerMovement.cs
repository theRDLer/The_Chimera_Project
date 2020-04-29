﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stamina = 20.0f;
    private float maxStamina = 20.0f;
    float speed = 0;

    public Rigidbody2D rb;

    public StaminaBar staminaBar;
    public Animator anim;
    public SpriteRenderer playerRenderer;

    Vector2 movement;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();

        stamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("StaminaPack"))
        {
            stamina += 5;
            col.gameObject.SetActive(false);
            staminaBar.SetStamina(stamina);
        }
    }

    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        speed = movement.magnitude / Time.deltaTime;

        anim.SetFloat("Speed", speed);

        if (movement.x > 0)
        {
            anim.SetBool("xInc", true);
            anim.SetBool("xDec", false);

            playerRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            anim.SetBool("xDec", true);
            anim.SetBool("xInc", false);

            playerRenderer.flipX = true;
        }
        else if (movement.x == 0)
        {
            anim.SetBool("xDec", false);
            anim.SetBool("xInc", false);
        }

        if (movement.y > 0)
        {
            anim.SetBool("yInc", true);
            anim.SetBool("yDec", false);

        }
        else if (movement.y < 0)
        {
            anim.SetBool("yDec", true);
            anim.SetBool("yInc", false);
        }
        else if (movement.y == 0)
        {
            anim.SetBool("yDec", false);
            anim.SetBool("yInc", false);
        }

        if (speed == 0)
        {
            anim.SetBool("xDec", false);
            anim.SetBool("xInc", false);
            anim.SetBool("yDec", false);
            anim.SetBool("yInc", false);
        }

        staminaBar.SetStamina(stamina);

        if (Input.GetKeyDown(KeyCode.LeftShift) & stamina == maxStamina)
        {
            CancelInvoke();
            moveSpeed = 10f;
            InvokeRepeating("loseStamina", 0, 0.1f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || stamina == 0)
        {
            CancelInvoke();
            moveSpeed = 5f;
            InvokeRepeating("gainStamina", 0, 0.1f);
        }

        if (stamina >= maxStamina)
        {
            stamina = maxStamina;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void loseStamina()
    {
        stamina--;
    }

    private void gainStamina()
    {
        stamina += 0.5f;
    }
}