using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public GameObject powerupIndicator;
    public GameObject powerupIndicator2;
    public GameObject powerupIndicator3;    

    public bool hasPowerup;
    public bool hasPowerup2;
    public bool hasPowerup3;
    public bool isOnGround = true;

    public GameObject projectilePrefab; // Used for the prefab
    private GameObject tmpProjectile; // Used for the spawning

    public GameObject grenadePrefab;
    private GameObject tmpGrenade;

    public float speed = 5f;
    public float jumpForce = 10f;
    private float powerupStrenght = 15f;
    private float explosionTime = 0.5f;

    void Start() {

        playerRb = GetComponent<Rigidbody>();
    }

    void Update() {

        // Movement
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(Vector3.forward * forwardInput * speed);

        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * horizontalInput * speed);

        // Stick the Powerup indicators to the player
        powerupIndicator.gameObject.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator2.gameObject.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        powerupIndicator3.gameObject.transform.position = transform.position + new Vector3(0, 2f, 0);

        // Shoot
        if (hasPowerup2 && Input.GetKeyDown(KeyCode.Space)) {
            Projectile();
        }

        // Jump
        if (hasPowerup3 && Input.GetKeyDown(KeyCode.Space)) {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        if (transform.position.y > 3) {
            Physics.gravity = new Vector3(0, -100, 0);
        }
    }

    // OnTrigger when you try to understand triggers between colliders,
    // OnCollision when you try to do something with physics.
    void OnTriggerEnter(Collider other) {
        
        // Takes Powerup
        if (other.CompareTag("Powerup") && !hasPowerup && !hasPowerup2 && !hasPowerup3) {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(startCountdown());
        }

        if (other.CompareTag("Powerup2") && !hasPowerup && !hasPowerup2 && !hasPowerup3) {
            hasPowerup2 = true;
            powerupIndicator2.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(startCountdown());
        }

        if (other.CompareTag("Powerup3") && !hasPowerup && !hasPowerup2 && !hasPowerup3) {
            hasPowerup3 = true;
            powerupIndicator3.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(startCountdown());
        }
    }

    IEnumerator startCountdown() {

        yield return new WaitForSeconds(7);
        hasPowerup = false;
        hasPowerup2 = false;
        hasPowerup3 = false;
        powerupIndicator.SetActive(false);
        powerupIndicator2.SetActive(false);
        powerupIndicator3.SetActive(false);
    }

    void OnCollisionEnter(Collision collision) {

        // Reset gravity when back after jumping
        if (collision.gameObject.CompareTag("Ground") && !isOnGround) {
            isOnGround = true;
            Physics.gravity = new Vector3(0, -10, 0);
            if (hasPowerup3) {
                tmpGrenade = Instantiate(grenadePrefab, transform.position, grenadePrefab.transform.rotation);
                Destroy(tmpGrenade, explosionTime);
            }
        }
        
        // If collide with enemy while has powerup, apply a force to enemy's rigidbody
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup) {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 enemyPos = collision.gameObject.transform.position;
            enemyRb.AddForce(AwayFromPlayer(enemyPos) * powerupStrenght, ForceMode.Impulse);
        }
    }

    Vector3 AwayFromPlayer(Vector3 enemyPos) {
        Vector3 direction = (enemyPos - gameObject.transform.position);
        return direction;
    }

    void Projectile() {

        // Launch projectile from above the player to stop collision to pushing us back
        foreach (var enemy in FindObjectsOfType<Enemy>()) {
            if(enemy != null) {
                tmpProjectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity);
                tmpProjectile.GetComponent<Projectile>().Fire(enemy.transform);
                //GameObject bullet = Instantiate(projectilePrefab, transform.position + Vector3.up, projectilePrefab.transform.rotation);
                //bullet.gameObject.GetComponent<Rigidbody>().AddForce((AwayFromPlayer(enemy.transform.position).normalized * 20f), ForceMode.Impulse);
            }            
        }
    }
}
