using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startScreen;
    public PlayerController playerControllerScript;
    public Image buttonImage;
    public Sprite closedSprite;
    public Sprite openedSprite;
    public GameObject gameMusicObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStartButtonClicked()
    {
        startScreen.SetActive(false);
        playerControllerScript.enabled = true;


    }
    public void OnSoundButtonClicked()
    {
        if(GameManager.Instance.playSound == true)
        {
            GameManager.Instance.playSound = false;
            buttonImage.sprite = closedSprite;
            gameMusicObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.playSound = true;
            buttonImage.sprite = openedSprite;
            gameMusicObject.SetActive(true);
        }
    }
}
