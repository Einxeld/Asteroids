using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] int scoreReward;
    
    enum Size { Big, Medium, Small }
    [SerializeField] Size asteroidSize;

    Rigidbody2D _rigidbody2d;

    void Start()
    {
        GameManager.instance.AsteroidSpawned(gameObject);
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Add(transform);

        _rigidbody2d = GetComponent<Rigidbody2D>();
        
        if (_rigidbody2d.velocity == Vector2.zero) 
        {
            _rigidbody2d.velocity = Random.onUnitSphere;
            _rigidbody2d.velocity *= Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    public void SplitIntoSmaller()
    {
        if (asteroidSize != Size.Small)
        {
            GameObject asteroidPrefab = asteroidSize == Size.Big ? GameManager.instance.asteroidPrefabMedium : GameManager.instance.asteroidPrefabSmall;
            
            Asteroid childAsteroid1 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity.MakeRandomZ()).GetComponent<Asteroid>();
            Asteroid childAsteroid2 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity.MakeRandomZ()).GetComponent<Asteroid>();
            
            float newAsteroidSpeed = Random.Range(childAsteroid1.minMoveSpeed, maxMoveSpeed);

            childAsteroid1.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * newAsteroidSpeed * Time.deltaTime);
            childAsteroid2.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * newAsteroidSpeed * Time.deltaTime);          
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            SplitIntoSmaller();
            Destroy(col.gameObject);
            GameManager.instance.AsteroidKilled(gameObject, scoreReward);
            Destroy(gameObject);
        }

        if (col.tag == "Player")
        {
            col.GetComponent<Player>().ReceiveDamage();
            GameManager.instance.AsteroidKilled(gameObject, scoreReward);
            Destroy(gameObject);
        }

        if (col.tag == "UFO")
        {
            GameManager.instance.AsteroidKilled(gameObject, scoreReward);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {        
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Remove(transform);
    }
}
