using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    public float speed = 4f;
    public int maxHealth = 5;
    public float timeInvincible = 2f;
    public ParticleSystem getHealth;
    public ParticleSystem getHurt;

    public GameObject projectilePrefab;

    public int health { get { return currentHealth; } } // gets the private int currentHealth and returns it as public int health so it can be read elsewhere
    int currentHealth;                                  // sort of like dependancy injection in a way? 
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audioSource;

    public AudioClip throwCog;
    public AudioClip playerHit;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Vector2 lookDirection = new Vector2(1, 0);
        currentHealth = maxHealth;
        Debug.Log("char health is " + currentHealth + "/" + maxHealth);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //moving character
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0f) || !Mathf.Approximately(move.y, 0f)) // check to see if move.x or move.y is NOT zero. Approximately helps with floats. 
        {
            lookDirection.Set(move.x, move.y); // if movement is detected, look direction is set per frame, then normalized (rounded)
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x); //sets floats in animator blend tree to determine what animation to play (left, right, up, down)
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2d.position; //gets current position from rigidbody 

        position = position + move * speed * Time.deltaTime; // determining new player position

        rigidbody2d.MovePosition(position); // moves the player to position previously determined

        //invincibility timer when taking damage
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime; // counting down invincibility frames
            if (invincibleTimer < 0)
            {
                isInvincible = false; // disables invincibility if timer reaches zero
            }
        }

        if (Input.GetButtonDown("Fire1")) // checks per update to see if player pressed the attack button
        {
            Launch();
        }

        if (Input.GetButtonDown("Fire2")) // talk to NPCs
        {
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object" + hit.collider.gameObject);
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0) // if something lowered health...
        {
            if (isInvincible)  // checks to see if within invincibility frames...
                return;

            animator.SetTrigger("Hit"); // if not, will trigger the hit animation...
            Instantiate(getHurt, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); // and instantiate the getHurt particle effect

            isInvincible = true; // ...and will also activate invincibility...
            invincibleTimer = timeInvincible; // ..and this sets the amount of time from the public variable we set up top
            PlaySound(playerHit);
        }
        else // if something raised health
        {
            Instantiate(getHealth, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); // if amount is positive, getHealth particle effect plays
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth); // clamp makes sure that your number stays between the minimum and maximum values, in this example being 0 and maxHealth
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        Debug.Log("char health is " + currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); // instantiates the projectile object. you give it the object, position, and rotation. Quaternion.identity means no rotation
        
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300); // calls the projectile's launch method and passes in the direction and force variables
        PlaySound(throwCog);
        animator.SetTrigger("Launch");

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
