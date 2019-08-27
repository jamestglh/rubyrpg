using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{

    public AudioClip collectedClip;


    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null) // checks to see if its the ruby game object colliding with the health collectible
        {
            if (controller.health < controller.maxHealth) // checks to see if rubys health is less than max
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);

                Debug.Log("Object that entered the trigger: " + other);
            }
        }
    }
}
