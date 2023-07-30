using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioSource CollectionSoundEffect;
    
    private int Coin = 0;
   [SerializeField] private Text CoinText;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            CollectionSoundEffect.Play();
            Coin++;
           CoinText.text = "x " + Coin;
        }    
    }
}
