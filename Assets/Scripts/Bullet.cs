using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D _rigidbody2d;

    public bool ownedByEnemy;

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Add(transform);
    }

    void Start()
    {
        _rigidbody2d.velocity = transform.TransformDirection(Vector2.up * speed);

        Destroy(gameObject, (GameManager.instance.mapSize.x * 2f) / speed); // bullet lifetime = screen width / speed
    }

    void OnDestroy()
    {        
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Remove(transform);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!ownedByEnemy) return;

            col.GetComponent<Player>().ReceiveDamage();
        }
    }
}
