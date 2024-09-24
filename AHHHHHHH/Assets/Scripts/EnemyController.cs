using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{

    public float hesl = 2;
    public float pushBackForce = 100;
    public GameObject Heal;
    public GameObject Ammo;
    public Transform Enemy;
    public EnemySpawner spawner;
    public PlayerController player;
    public NavMeshAgent agent;
    public bool inchase = false;
    public float hitCD = 3f;
    public bool canHit = true;

    // Start is called before the first frame update
    void Start()
    {
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
        EnemyController.Destroy(gameObject);
        }
        if (inchase == true)
            agent.destination = player.transform.position;
    }
    private void OnDestroy()
    {
        spawner.CurrentEnemies -= 1;
    }
    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hesl -= 1;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canHit == true)
        {
            player.health -= 1;
            canHit = false;
            print(player.health);
            StartCoroutine("Cooldown");

        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(hitCD);
        canHit = true;
    }

}
