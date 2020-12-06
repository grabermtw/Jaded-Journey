using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public Transform camParent;
    public Ragdoll ragdoll;
    public Transform hips;
    public TextMeshProUGUI hint;
    private Animator animator;
    private Rigidbody rb;
    // private bool grounded;
    private bool jumped;
    private TalkingNPC talker;
    private GameObject interactor;
    private bool canDoStuff = true;
    private bool dead = false;
    private bool canRevive = false;
    private bool following = false;
    public GameObject getUpText;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ragdoll.SetRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.E) && talker != null)
            {
                Debug.Log("keydown");
                canDoStuff = talker.Talk();
                if (canDoStuff == true)
                {
                    talker = null;
                    interactor = null;
                    hint.enabled = false;
                }
            }
            if (canDoStuff)
            {

                animator.SetBool("Kick", Input.GetMouseButtonDown(0));
                if (Input.GetKey(KeyCode.W))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
                    {
                        transform.Translate(0, 0, 1f * Time.deltaTime);
                    }
                    transform.Translate(0, 0, 4f * Time.deltaTime);
                    animator.SetBool("Run", true);
                }
                else
                {
                    animator.SetBool("Run", false);
                }


                if (Input.GetKey(KeyCode.S))
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
                        transform.Translate(0, 0, -2.75f * Time.deltaTime);
                    animator.SetBool("Reverse", true);
                }
                else
                {
                    animator.SetBool("Reverse", false);
                }

                // if(Input.GetKey(KeyCode.Space) && grounded){
                //     animator.SetTrigger("Jump");
                //     rb.AddForce(new Vector3(0,300,0));
                //     grounded = false;
                // }

                if (Input.GetKey(KeyCode.D))
                {
                    transform.Rotate(0, 80 * Time.deltaTime, 0);
                    animator.SetInteger("Turn", 1);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(0, -80 * Time.deltaTime, 0);
                    animator.SetInteger("Turn", -1);
                }
                else
                {
                    animator.SetInteger("Turn", 0);
                }

                if (Input.GetKey(KeyCode.L))
                {
                    transform.position = new Vector3(64, 23, 97);
                }
            }
        }
        else if (dead && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Reb");
            Revive();
        }
    }

    public virtual void Die(Vector3 newMomentum) // handle if AddMomentum should be called
    {
        animator.enabled = false;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        ragdoll.SetRagdoll(true);
        Vector3 momentum;
        momentum = newMomentum;

        ragdoll.AddMomentum(momentum);
        dead = true;
        canRevive = false;
        getUpText.SetActive(true);
        StartCoroutine(RevivalEnabler());
        StartCoroutine(FollowRagdoll());
    }

    public Transform GetHips()
    {
        return hips;
    }

    protected virtual IEnumerator RevivalEnabler()
    {
        // Don't allow a revival until we stop moving on the ground
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        canRevive = true;
    }

    public virtual void Revive()
    {
        if (dead && canRevive)
        {
            ragdoll.SetRagdoll(false);
            animator.enabled = true;
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
            transform.position = hips.position;
            hips.localPosition = Vector3.zero;
            dead = false;
            getUpText.SetActive(false);
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 12)
        {
            rb.isKinematic = true;
            rb.mass = 0;
            rb.useGravity = false;
            animator.SetTrigger("Hang");
            gameObject.transform.parent = other.gameObject.transform;
            this.enabled = false;

        }
        else
        {
            // grounded = true;
            jumped = false;
        }
    }

    // void OnCollisionExit(Collision other){
    //   grounded = false;
    //}

    void OnCollisionStay()
    {
        if (!dead && Input.GetKey(KeyCode.Space) && animator != null && !jumped && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            animator.SetTrigger("Jump");
            jumped = true;
            rb.AddForce(new Vector3(0, 300, 0));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TalkingNPC>() != null)
        {
            talker = other.gameObject.GetComponent<TalkingNPC>();
            if (talker.IsTalkable())
            {
                interactor = other.gameObject;
                hint.enabled = true;
            }

        }

    }


    // Start this to have the camera follow the character when they ragdoll
    // Start this again to stop following
    public IEnumerator FollowRagdoll()
    {
        Debug.Log("FollowRagdoll");
        if (following)
        {
            Debug.Log("Stop Following!");
            following = false;
            yield break;
        }
        following = true;
        while (following)
        {
            
            camParent.position = hips.position;
            Debug.Log("our pos: " + camParent.position);
            Debug.Log("character pos: " + hips.position);
            yield return null;
        }
        Debug.Log("End!");
        camParent.localPosition = new Vector3(0, 1.5f, 0);
    }



}

