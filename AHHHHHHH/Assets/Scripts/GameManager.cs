using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool IsPaused = false;
    public bool dead = false;

    public GameObject PauseMenu;
    public GameObject DeathScreen;
    public PlayerController playerData;

    public Image healthBar;
    public TextMeshProUGUI BulletCountText;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (playerData.weaponID < 1)
            {
                BulletCountText.gameObject.SetActive(false);
            }
            else
            {
                BulletCountText.gameObject.SetActive(true);
            }

            healthBar.fillAmount = (float)playerData.health / (float)playerData.maxHealth;
            BulletCountText.text = "Bullets: " + playerData.currentAmmo + "/" + playerData.maxAmmo;

            if (Input.GetKeyDown(KeyCode.Escape) & (dead == false))
            {
                if (!IsPaused)
                {
                    PauseMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Time.timeScale = 0;

                    IsPaused = true;
                }
                else
                {
                    Resume();
                }

            }
            if (playerData.health <= 0)
            {
                dead = true;
                DeathScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerData.canfire = false;
                Time.timeScale = 0;

            }
            if (!dead)
                DeathScreen.SetActive(false);
        }
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        IsPaused = false;
    }
    public void QuitGame()
    { Application.Quit(); }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void LoadLevel(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }


}



