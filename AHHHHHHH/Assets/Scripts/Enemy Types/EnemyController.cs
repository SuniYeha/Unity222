using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{

    public int hesl = 2;
    public float chaseRange = 20;
    public float pushBackForce = 100;
    public GameObject Heal;
    public GameObject Ammo;
    public Transform Enemy;
    public EnemySpawner spawner;
    public Objective Objective;
    public PlayerController player;
    public NavMeshAgent agent;
    public bool inchase = false;
    public float hitCD = 1.5f;
    public bool canHit = true;
    public AudioSource enemySpeaker;
    public AudioClip death;
    public AudioClip ow;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeaker = GetComponent<AudioSource>();
        enemySpeaker.resource = ow;
        spawner = GameObject.Find("EnemySpawn").GetComponent<EnemySpawner>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hesl <= 0)
        {
            enemySpeaker.resource = death;
            if (!enemySpeaker.isPlaying)
            {
                enemySpeaker.Play();
            }
            agent.isStopped = true;
            StartCoroutine("Dying");
        }

        else
        {
            if (inchase)
            {
                agent.isStopped = false;
                agent.destination = player.transform.position;
            }
            if (!inchase)
                agent.isStopped = true;
        }
    }
    // endless only
    private void OnDestroy()
    {
        spawner.CurrentEnemies--;
        Objective.enemies--;
    }
    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            enemySpeaker.Play();
            hesl -= 1;
            Destroy(collision.gameObject);
            inchase = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canHit == true)
        {
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * 100);
            player.health -= 1;
            canHit = false;
            inchase = false;
            StartCoroutine("Cooldown");

        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(hitCD);
        canHit = true;
    }
    IEnumerator Dying()
    { 
        yield return new WaitForSeconds(hitCD);
        if (Random.Range(0, 4) == 1)
        {
            if (Random.Range(0, 2) == 1)
            {
                GameObject H = Instantiate(Heal, Enemy.position, Enemy.rotation);
            }
            else
            {
                GameObject R = Instantiate(Ammo, Enemy.position, Enemy.rotation);
            }
        }
        EnemyController.Destroy(gameObject);

    }
}
