using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Endless : MonoBehaviour
{
    public GameObject EndlessStart;
    public EnemySpawner Spawner;
    public TextMeshProUGUI Wave;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Wave.text = "Wave " + Spawner.WaveNumber;
        if ((Input.GetKeyDown(KeyCode.Space)) && (EndlessStart.activeInHierarchy))

            byeES();
        
    }
    public void byeES()
    {
        EndlessStart.SetActive(false);
        Time.timeScale = 1;
    }
}
