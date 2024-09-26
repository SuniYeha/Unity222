using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius2 : MonoBehaviour
{
    public EnemyController Enemy;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && player.sprintMode)
            Enemy.inchase = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
            Enemy.inchase = false;
    }
}
