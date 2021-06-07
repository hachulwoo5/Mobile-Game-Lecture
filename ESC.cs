using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Save theSave;
    public static bool ESCActivated = false;        //평소에 ESC창을 꺼놓음

    
    [SerializeField]
    private GameObject go_ESC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryESC();
    }

    private void TryESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))           //ESC키를 누르면 저장/세이브 창이 나옴
        {
            ESCActivated = !ESCActivated;

            if (ESCActivated)
                OpenESC();
            else
                CloseESC();
        }
    }
    private void OpenESC()
    {
        go_ESC.SetActive(true);
    }

    private void CloseESC()
    {
        go_ESC.SetActive(false);
    }

    public void ClickSave()
    {
        Debug.Log("세입");
        theSave.SaveData();
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        theSave.LoadData();
    }
}

