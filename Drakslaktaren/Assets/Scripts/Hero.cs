using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hero : MonoBehaviour
{
    public Animator animator;
    public AudioSource footsteps;
    public AudioSource swordAttack;
    public UnityEngine.Experimental.Rendering.Universal.Light2D Light2D;
    public bool isGrounded;
    public float attackRange = 0.5f;
    public float portalTriggerRange = 0.5f;
    public float jumpSpeed = 5f;
    public float moveSpeed = 12f;
    public LayerMask enemyLayers;
    public LayerMask groundLayers;
    public LayerMask portalLayers;
    public Rigidbody2D rb;
    public Transform attackPoint;
    public Transform portalTrigger;
    public int attackDamage = 10;
    public float attackRate = 1.8f;
    float nextAttackTime = 0f;
    public int maxHealth = 100;
    int currentHealth;
    public AudioSource deathSound;
    public AudioSource hitSound;
    public GameObject bird;


    void Start()
    {
        currentHealth = maxHealth;
    }

    /*  public void TakeDamage(int damage)
     {
         currentHealth -= damage;
         animator.SetTrigger("heroHurt");
         hitSound.volume = Random.Range(0.035f, 0.065f); //random volume
         hitSound.pitch = Random.Range(0.95f, 1.05f); //random pitch
         hitSound.Play();

         if (currentHealth <= 0)
         {
             Die();
         }
     } */
    /* void Die()
    {
        Debug.Log("You died!");

        animator.SetBool("heroDead", true);
        Light2D.enabled = false;
        deathSound.volume = Random.Range(0.035f, 0.065f); //random volume
        deathSound.pitch = Random.Range(0.95f, 1.05f); //random pitch
        deathSound.Play();
    }
 */

    void Update()
    {
        Attack();
        Jump();

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 1.7f, transform.position.y - 1.7f),       //isGrounded?
            new Vector2(transform.position.x + 1.7f, transform.position.y - 1.7f), groundLayers);

        //Horizontal movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;


        //Changes values to trigger animations
        animator.SetFloat("Speed", movement.x);
        animator.SetBool("Jump", Input.GetButtonDown("Jump"));



        //Flip character y-axis (3d) if direction of movement is changed
        if (movement.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }


        //Footsteps
        if (isGrounded && movement.x != 0f && footsteps.isPlaying == false && Input.GetMouseButton(0) == false)
        {
            footsteps.volume = Random.Range(0.035f, 0.065f); //random volume
            footsteps.pitch = Random.Range(0.95f, 1.05f); //random pitch
            footsteps.Play();
        }
        else if (isGrounded && movement.x != 0f && footsteps.isPlaying == false)
        {
            footsteps.volume = Random.Range(0.035f, 0.065f); //random volume
            footsteps.pitch = Random.Range(0.95f, 1.05f); //random pitch
            footsteps.Play();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && footsteps.isPlaying == true)
        {
            footsteps.Stop();
        }
        else if (isGrounded == false && footsteps.isPlaying == true)
        {
            footsteps.Stop();                   //stop playing footsteps audio while jumping
        }

        Collider2D[] hitPortals = Physics2D.OverlapCircleAll(portalTrigger.position, portalTriggerRange, portalLayers);
        foreach (Collider2D portal in hitPortals)
        {
            Debug.Log("portal hit");
        }
    }
    void Jump()                 //Jump function
    {
        if (Input.GetButtonDown("Jump") && isGrounded)  //Jump if spacebar is down and if player is grounded
        {
            rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    void Attack()               //Attack function
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                swordAttack.volume = Random.Range(0.035f, 0.065f); //random volume
                swordAttack.pitch = Random.Range(0.95f, 1.05f); //random pitch
                swordAttack.Play();
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            moveSpeed = 0f;
        }
        else
        {
            moveSpeed = 5f;
        }
    }


    //attackPoint gizmos for measurments
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if (portalTrigger == null)
            return;

        Gizmos.DrawWireSphere(portalTrigger.position, portalTriggerRange);
    }
}