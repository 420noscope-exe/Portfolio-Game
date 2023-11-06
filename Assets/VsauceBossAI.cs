using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsauceBossAI : MonoBehaviour
{
    [SerializeField]private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField]private List<AudioClip> hit = new List<AudioClip>();
    [SerializeField]private int action;
    [SerializeField]private AudioClip slagAC;
    [SerializeField]private AudioClip thermiteBallsAC;
    [SerializeField]private AudioClip thermiteBallsShortAC;
    [SerializeField]private AudioClip punchAC;
    [SerializeField]private AudioClip teleportAC;
    [SerializeField]private GameObject explosionEffect;
    [SerializeField]private GameObject rightHand;
    private AudioSource aSource;
    private GameObject player;
    private Animator animator;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponentInChildren<Animator>();
        aSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(action == 1)
        {
            idle();
        }
        else if(action == 2)
        {
            walk();
        }
        else if(action == 3)
        {
            punchL();
        }
        else if(action == 4)
        {
            punchR();
        }
        else if(action == 5)
        {
            thermiteBallsShort();
        }
        else if(action == 6)
        {
            slag();
        }
        action = 0;
    }

    private void idle()
    {
        animator.Play("Base.Idle");
    }

    private void walk()
    {
        Vector3 temp = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        gameObject.transform.LookAt(temp);
        animator.Play("Walk");
        playVoiceLine();
    }

    private void punchL()
    {
        animator.Play("Base.PunchL");
        aSource.PlayOneShot(punchAC);
    }

    private void punchR()
    {
        animator.Play("Base.PunchR");
        aSource.PlayOneShot(punchAC);
    }

    private void thermiteBallsShort()
    {
        animator.Play("Base.ThermiteBalls");
        aSource.PlayOneShot(thermiteBallsShortAC);
        StartCoroutine(thermiteExplosion());
    }

    private void slag()
    {
        animator.Play("Base.Slag");
        aSource.PlayOneShot(slagAC);
    }

    IEnumerator thermiteExplosion()
    {
        float delayInMs = 0.6f;
        float ms = Time.deltaTime;

        while(ms <= delayInMs )
            {
                ms += Time.deltaTime;
                yield return null;
            }
        GameObject temp = Instantiate(explosionEffect, rightHand.transform);
        Destroy(temp, 1.0f);
    }

    private void playVoiceLine()
    {
        aSource.PlayOneShot(clips[Random.Range(0,clips.Count)]);
    }
}
