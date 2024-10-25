using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public EnemyController enemy;
    public Transform shooter;
    public GameObject bullet;
    public bool canfire = true;
    public bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.inchase == true & canfire)
        {
            StartCoroutine("Fire");
        }
    }
    IEnumerator Fire()
    {
        enemy.agent.isStopped = true;
        yield return new WaitForSeconds(2);
        if (enemy.inchase == true & canfire)
        {
            GameObject b = Instantiate(bullet, shooter.position, shooter.rotation);
            b.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * 1500);
            Destroy(b, .5f);
            canfire = false;
            enemy.agent.isStopped = false;
            StartCoroutine("Cooldown");
        }
        else
            enemy.agent.isStopped = false;
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);
        canfire = true;
    }
}
