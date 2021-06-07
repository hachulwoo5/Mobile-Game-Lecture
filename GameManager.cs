using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;



    public void GameStart()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        
    }
    void LateUpdate()
    {   
        playerHealthTxt.text = player.health + " / " + player.maxHealth;               //플레이어 체력 표시 
        if(player.equipWeapon ==null)
        {
            playerAmmoTxt.text = " - / " + player.ammo;                               //플레이어 총알 수 표시 
        }
        else
        {
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;   //플레이어 총알 수 표시 
        }
        
    }
}
