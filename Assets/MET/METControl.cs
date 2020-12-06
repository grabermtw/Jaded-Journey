using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class METControl : MonoBehaviour
{
    const float laserSpeed = 120f;
    const float aimSpeed = 6f;
    public GameObject projectile;
    public Transform[] cannons;
    public float range = 200;
    public GameObject jadeCam;
    Transform target;
    Rigidbody targetRb;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        targetRb = target.GetComponent<Rigidbody>();
        StartCoroutine(CannonControl());
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Handle aiming the cannon
    private IEnumerator CannonControl()
    {
        
        if (!jadeCam.activeSelf && Vector3.Distance(target.position, transform.position) <= range)
        {
            // Determine how long to aim for before firing the cannon
            float aimTime = Random.Range(1, 6);
            float currentAimTime = 0;
            while (currentAimTime < aimTime)
            {
                // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                    Vector3 targetPosition = target.position + targetRb.velocity * Vector3.Distance(target.position, transform.position) / laserSpeed;
                    // Calculate where to aim
                    Quaternion direction = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), Time.deltaTime * aimSpeed);
                    // Clamp the rotation so that the barrel doesn't clip through the platform
                    if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                    {
                        transform.rotation = direction;
                    }
                foreach (Transform cannon in cannons)
                {
                    // Estimate the target's future position by the time the laser reaches them based on their velocity and the laser's speed
                     targetPosition = target.position + targetRb.velocity * Vector3.Distance(target.position, cannon.position) / laserSpeed;
                    // Calculate where to aim
                    direction = Quaternion.Slerp(cannon.rotation, Quaternion.LookRotation(targetPosition - cannon.position), Time.deltaTime * aimSpeed);
                    // Clamp the rotation so that the barrel doesn't clip through the platform
                    if (direction.eulerAngles.x < 240 || direction.eulerAngles.x > 325)
                    {
                        cannon.rotation = direction;
                    }
                }
              //  transform.LookAt(target);
                currentAimTime += Time.deltaTime;
                yield return null;
            }
            // Fire the cannon
            foreach (Transform cannon in cannons)
            {
                GameObject laser = Instantiate(projectile, 2 * cannon.forward + cannon.position, cannon.rotation);
                laser.GetComponent<LaserBolt>().SetSpeed(laserSpeed);
            }
        }
        yield return null;
        StartCoroutine(CannonControl());
    }
}
