using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public GamePhase gamePhase;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("sa");
            GetComponent<BoxCollider2D>().enabled = false;
            PlayerController.Instance.ChangeGameMode(gamePhase);
        }   
    }
}
