using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player instance;

    Rigidbody2D _rigidbody2d;

    [Header("Stats")]
    [SerializeField] float thrustPower;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] int maxHealth = 3;
    int health;
    bool invincible;

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    float lastShootTime;
    [SerializeField] float shootDelay;

    [Header("Graphics")]
    [SerializeField] GameObject thrustEffect;
    [SerializeField] Renderer[] playerGraphics;

    [Header("UI")]
    [SerializeField] GameObject[] healthUiIndicators;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Add(transform);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody2d.AddRelativeForce(Vector2.up * thrustPower * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            thrustEffect.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            thrustEffect.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastShootTime + shootDelay)
        {
            Instantiate(bulletPrefab, transform.position + transform.TransformDirection(Vector2.up * 0.3f), transform.rotation);
            lastShootTime = Time.time;
        }

        if (_rigidbody2d.velocity.magnitude > maxSpeed)
        {
            _rigidbody2d.velocity.Clamp(-maxSpeed, maxSpeed);
        }
    }

    public void ReceiveDamage()
    {
        if (invincible) return;

        health--;
        for (int i = 0; i < maxHealth; i++)
        {
            healthUiIndicators[i].SetActive(i < health);
        }

        if (health == 0)
        {
            GameManager.instance.GameOver();
            Time.timeScale = 0f;
        }

        transform.position = Vector3.zero;
        _rigidbody2d.velocity = Vector2.zero;

        StartCoroutine(BlinkInvincibleIE());
    }

    IEnumerator BlinkInvincibleIE()
    {
        invincible = true;

        for (int i = 0; i < 3; i++)
        {
            foreach (Renderer rend in playerGraphics)
            {
                rend.material.color = Color.gray;
            }

            yield return new WaitForSeconds(0.5f);

            foreach (Renderer rend in playerGraphics)
            {
                rend.material.color = Color.white;
            }

            yield return new WaitForSeconds(0.5f);
        }

        invincible = false;
    }
}
