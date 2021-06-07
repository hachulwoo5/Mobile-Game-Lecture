using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;   //인풋 아우풋 데이터 가져옴


[System.Serializable]  //데이터 직렬화 
public class SaveData
{
    public Vector3 playerPos;  //플레이어의 위치 기억
}

public class Save : MonoBehaviour
{
    private SaveData saveData = new SaveData();


    [SerializeField]

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";  //세이브 파일을 만들어줌
    private Player thePlayer;


    // Sta is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); //디렉토리가 없으면 세이브 폴더를 만들어줌 에셋폴더 안에

    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<Player>();
        saveData.playerPos = thePlayer.transform.position;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json); //실제 물리적인 파일이 생기게함

        Debug.Log("저장 완료");
        Debug.Log(json);

    }
    public void LoadData()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY+ SAVE_FILENAME))//버그 방지를 위해 파일 이있는지 체크
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);   //json화 된 데이터를 다시 되돌림

            thePlayer = FindObjectOfType<Player>();
            thePlayer.transform.position = saveData.playerPos;
            Debug.Log("로드 완료");
        }
       else
        {
            Debug.Log("세이브 파일이 없다");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}




  

   
