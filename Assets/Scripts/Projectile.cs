using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    Rigidbody2D rigidbody2D;

    void Awake() // we use awake instead of start because awake is called immediately (when instantiate is called), instead of at the start of the next frame
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

 
    public void Launch(Vector2 direction, float force) // we get the direction and force variables from the rubycontroller's launch method, when rubycontroller calls this. 
    {
        rigidbody2D.AddForce(direction * force);
        RubyController controller = GetComponent<RubyController>();
    }

    void OnCollisionEnter2D(Collision2D other) // note: the projectile doesnt collide with ruby because they are on different LAYERS
    {
        Debug.Log("Projectile collision with " + other.gameObject); // logs out what the projectile is hitting
        Destroy(gameObject);

        EnemyController e = other.collider.GetComponent<EnemyController>(); // checks to see if projectile is colliding with enemycontroller box collider
        if (e != null) 
        {
            e.Fix(); // if true, then run enemycontroller's fix function
        }

    }

    private void Update()
    {
        if (transform.position.magnitude > 120f) // every frame this checks to see if the distance the projectile is from ruby is farther than 120. 
        {
            Destroy(gameObject); //if the difference is over 1000 the projectile despawns
        }
    }
}
