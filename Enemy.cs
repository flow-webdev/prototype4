using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private float grenadePower = 50f;
    private float rate = 1f;

    private Rigidbody enemyRb;
    private GameObject player;

    public GameObject projectilePrefab;
    private GameObject tmpProjectile;
    
    void Start() {    
        
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        // Boss projectiles
        if (gameObject.tag == "Boss") {
            InvokeRepeating("Projectile", rate, rate);
        }
    }

    void Update() {

        Vector3 lookDIrection = (player.transform.position - transform.position).normalized;

        // Player direction - enemy direction will give the exact direction (Vector) for the enemy to
        // follow in order to chase the player. Normalize will not let the enemy increase his force
        // with the speed
        enemyRb.AddForce(lookDIrection * speed);

        if (transform.position.y < -12) {
            Destroy(gameObject);
        }        
    }

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Grenade")) {
            Vector3 awayFromGrenade = (transform.position - other.gameObject.transform.position).normalized;
            enemyRb.AddForce(awayFromGrenade * grenadePower, ForceMode.Impulse);
        }
    }

    void Projectile() {

        if (player != null) {
            tmpProjectile = Instantiate(projectilePrefab, transform.position + new Vector3(0, 2, 0), projectilePrefab.transform.rotation);
            tmpProjectile.GetComponent<Projectile>().Fire(player.transform);
        }
    }
}
