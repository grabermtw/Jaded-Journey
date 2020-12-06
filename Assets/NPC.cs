using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float romaingRadius = 3;

    private Animator anim;
    private UnityEngine.AI.NavMeshAgent nav;
    private Transform target;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim.SetTrigger("StandardMovement");
        target = GameObject.FindWithTag("Player").GetComponent<PlayerControl>().GetHips();
    }

    protected virtual void Start()
    {
        StartCoroutine(Roaming());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Calculate angular velocity
        Vector3 s = transform.InverseTransformDirection(nav.velocity).normalized;
        float turn = s.x;

        // let the animator know what's going on
        anim.SetFloat("Speed", nav.velocity.magnitude);
        anim.SetFloat("Turn", turn * 2);
    }

    protected IEnumerator Roaming()
    {
        yield return new WaitForSeconds(0.5f);
        if (Vector3.Distance(target.position, transform.position) > romaingRadius)
        {
            nav.SetDestination(GetNewDestination(target.position, romaingRadius, -1));
        }
        StartCoroutine(Roaming());
        yield return null;
    }

    // Simply interpolate a straight line from start to end.
    IEnumerator Walk(UnityEngine.AI.OffMeshLinkData data)
    {
        float timeLeft = Vector3.Distance(data.offMeshLink.startTransform.position, data.offMeshLink.endTransform.position) * (1 / nav.speed) + 1;
        float passTime = timeLeft;

        Vector3 startPos = transform.position;
        Vector3 endPos = data.endPos + nav.baseOffset * Vector3.up;

        Vector3 lookPos = endPos - startPos;
        lookPos.y = 0;

        while (timeLeft > 0)
        {
            transform.position = Vector3.Lerp(endPos, startPos, timeLeft / passTime);
            anim.SetFloat("Speed", nav.speed - 0.2f);

            timeLeft -= Time.deltaTime;

            yield return null;
        }
    }

    public static Vector3 GetNewDestination(Vector3 origin, float radius, int layermask) {
        // Get a random new destination
        Vector3 newDirection = Random.insideUnitSphere * radius;

        newDirection += origin;
        
        UnityEngine.AI.NavMeshHit navHit;
 
        UnityEngine.AI.NavMesh.SamplePosition(newDirection, out navHit, radius, layermask);
    
        return navHit.position;
    }
}
