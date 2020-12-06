using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    
    const float impactVelMax = 30f;
    private Rigidbody rb;
    private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DestroyIfMissed());
    }

    // Kill the player if we hit them
    void OnCollisionEnter(Collision other)
    {
        PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
        if (player != null)
        {
            player.Die(Vector3.ClampMagnitude(rb.velocity, impactVelMax));
        }
        Destroy(gameObject);
    }

    // Destroy the laser if we very clearly missed the target.
    private IEnumerator DestroyIfMissed()
    {
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }

    
}
