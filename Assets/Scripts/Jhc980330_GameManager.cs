using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jhc980330_GameManager : MonoBehaviour
{
    
    
    private static Jhc980330_GameManager instance = null;
    public GameObject player;
    public GameObject Game;
    public GameObject StartUI;
    public Vector2 savePoint;
    GameObject savePointObject;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { 
            Destroy(this.gameObject);
        }
    }
    public static Jhc980330_GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Start()
    {
        this.savePoint = player.transform.position; 
        
    }
    public void GameOver()
    {
        player.transform.position = savePoint; 
    }
    public void SetSavePoint(GameObject gameObject)
    {
        savePointObject = gameObject;
    }
    public bool isSavePoint(GameObject gameObject)
    {
        return gameObject.Equals(savePointObject);
    }
    public void StartGame()
    {
        StartUI.gameObject.SetActive(false);
        Game.SetActive(true);
    }
}
