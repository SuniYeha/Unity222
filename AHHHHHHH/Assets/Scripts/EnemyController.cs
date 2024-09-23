using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{

    public float hesl = 2;
    public GameObject Heal;
    public GameObject Ammo;
    public Transform Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hesl -= 1;
        }

    }
   
    
}
