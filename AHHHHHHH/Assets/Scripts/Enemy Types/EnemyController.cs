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
    public PlayerController player;
    public NavMeshAgent agent;
    public bool inchase = false;
    public float hitCD = 2f;
    public bool canHit = true;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("EnemySpawn").GetComponent<EnemySpawner>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hesl <= 0)
        {

            if (Random.Range(0, 2) == 1)
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
            Destroy(gameObject);
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
    }
    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hesl -= 1;
            Destroy(collision.gameObject);
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

}
