using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    Vector3 moveDirection;
    [SerializeField] int scoreReward;
    [SerializeField] float shootDelay;
    [SerializeField] GameObject bulletPrefab;

    void Start()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        if (transform.position.x > 0)
            moveDirection = Vector3.left;
        else 
            moveDirection = Vector3.right;

        GameManager.instance._mapBorderTeleporter.teleportableObjects.Add(transform);

        StartCoroutine(ShootIE());
    }
    
    void FixedUpdate()
    {
        transform.Translate(moveDirection * moveSpeed / 50f);
    }

    IEnumerator ShootIE()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, Player.instance.transform.position - transform.position));
        }
    }

    void OnDestroy()
    {        
        GameManager.instance._mapBorderTeleporter.teleportableObjects.Remove(transform);
        GameManager.instance.UFOKilled(scoreReward);
        StopAllCoroutines();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            if (col.GetComponent<Bullet>().ownedByEnemy) return;

            Destroy(col.gameObject);
            GameManager.instance.UFOKilled(scoreReward);
            Destroy(gameObject);
        }

        if (col.tag == "Player")
        {
            col.GetComponent<Player>().ReceiveDamage();
            GameManager.instance.UFOKilled(scoreReward);
            Destroy(gameObject);
        }
    }
}
