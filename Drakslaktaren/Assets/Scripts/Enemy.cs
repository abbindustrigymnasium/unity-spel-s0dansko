using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 10;
    int currentHealth;
    public Animator animator;
    public AudioSource hitSound;
    public AudioSource deathSound;
    public LayerMask heroLayers;
    public AudioSource swordAttack;
    public GameObject bird;

    void Start()
    {
        currentHealth = maxHealth;
    }

    //taking damage function
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        hitSound.volume = Random.Range(0.035f, 0.065f); //random volume
        hitSound.pitch = Random.Range(0.95f, 1.05f); //random pitch
        hitSound.Play();

        //Spawn a bird at random place between set positions
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 0.2f); //Destroy dead bird after .2 seconds
            Instantiate(bird, new Vector3(Random.Range(-10f, 12f), Random.Range(-2f, -1f), 0f), Quaternion.identity); // Spawn new bird
        }
    }
}
