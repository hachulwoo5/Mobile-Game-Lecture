using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Player player;
    public static bool ItemActivated = false;
    public Text GoldTxt;
    public Text SilverTxt;
    public Text BronzeTxt;

    
    //필요한 컴포넌트 
    [SerializeField]
    private GameObject go_ItemBase;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenItem();
    }

    void LateUpdate()
    {
        GoldTxt.text = player.Gold + " 개 ";
        SilverTxt.text = player.Silver + " 개 ";
        BronzeTxt.text = player.Bronze + " 개 ";
    }

    private void TryOpenItem()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ItemActivated = !ItemActivated;

            if (ItemActivated)
                OpenItem();
            else
                CloseItem();
        }
    }

    private void OpenItem()
    {
        go_ItemBase.SetActive(true);
    }

    private void CloseItem()
    {
        go_ItemBase.SetActive(false);
    }
}
