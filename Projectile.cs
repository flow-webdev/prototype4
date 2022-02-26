using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;    
    private float speed = 25f;
    private float strenght = 25f;
    private float aliveTimer = 2f;
    //private bool homing;

    void Update() {
        // Move projectile in the enemy direction 
        if(target != null) {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    void OnCollisionEnter(Collision collision) {
        // If collide, apply a force to enemy's rigidbody
        if(target != null) {//&& homing
            if (collision.gameObject.CompareTag("Enemy")) {
                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                //Vector3 away2 = -collision.contacts[0].normal; //(collision.gameObject.transform.position - player.transform.position);
                Vector3 away = -collision.GetContact(0).normal;
                enemyRb.AddForce(away * strenght, ForceMode.Impulse);
                Destroy(gameObject);
                
            } else if (collision.gameObject.CompareTag("Player")) {
                Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.GetContact(0).normal;
                playerRb.AddForce(away * (strenght - 10), ForceMode.Impulse);
                Destroy(gameObject);
            }
        }        
    }

    public void Fire(Transform newTarget) {
        target = newTarget;        
        Destroy(gameObject, aliveTimer);
        //homing = true;
    }
}
