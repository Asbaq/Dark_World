using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool respawning = false;
    public float TimetoRespawn = 0f;
    public float currentRespawnTime = 0f;
    public bool active = true;
    public Vector2 respawnPoint;
    private float x = 2f; 
    [SerializeField] private float startinghealth;
    public float currentHealth {get; private set; }
     private Animator anim;
    [SerializeField] private GameObject AttackArea;
    [SerializeField] private AudioSource GateSound;
    [SerializeField] private AudioSource Checkpoints;
    [SerializeField] private AudioSource EvilSound;

   // AdMobManager adMobManager;

    private void Awake()
    {
        currentHealth = startinghealth;
        anim = GetComponent<Animator>();
    }

     void Start()
    {
        respawnPoint = transform.position;   
    }

    void Update()
    {  
        GetComponent<PlayerRespwan>().checkrespwaning();

        if(!active)
        {
            return;
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startinghealth);
         if(currentHealth > 0)
         {
            Debug.Log("hurt");
           anim.SetTrigger("hurt");
           GetComponent<PlayerMovement>().enabled = false;
         }
         else if(currentHealth <=0)
         {
            GetComponent<PlayerRespwan>().playerDefeated();
         }
    }

    private IEnumerator WaitForSceneLoad1()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("level_2");
       // adMobManager.ShowInterstitialAd();
    }

    private IEnumerator WaitForSceneLoad2()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("level_3");
       // adMobManager.ShowInterstitialAd();
    }

    private IEnumerator WaitForSceneLoad3()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("level_4");
      //  adMobManager.ShowInterstitialAd();
    }

    private IEnumerator WaitForSceneLoad4()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("level_5");
      //  adMobManager.ShowInterstitialAd();
    }

    private IEnumerator WaitForSceneLoad5()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("level_6");
      //  adMobManager.ShowInterstitialAd();
    }

    private IEnumerator WaitforSceneLoad6()
    {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("Final_level");
     //   adMobManager.ShowInterstitialAd();
    }
    private IEnumerator WaitforSceneLoad7()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Complete_Game");
     //   adMobManager.ShowInterstitialAd();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && AttackArea.activeInHierarchy == false)
        {
            TakeDamage(1);
            if (currentHealth > 0)
            {
                Debug.Log("Respawn");
                respawning = true;
            }
        }

        if (other.tag == "Ladders")
        {
            anim.SetTrigger("Climb");
        }

        if (other.tag == "MonsterCollider")
        {
            EvilSound.Play();
        }

        if (other.tag == "Checkpoints")
        {
            Checkpoints.Play();
            Debug.Log("Chek");
            respawnPoint = other.transform.position;
        }

        if (other.tag == "End1")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad1());
        }

        if (other.tag == "End2")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad2());
        }

        if (other.tag == "End3")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad3());
        }

        if (other.tag == "End4")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad4());
        }

        if (other.tag == "End5")
        {
            GateSound.Play();
            StartCoroutine(WaitForSceneLoad5());
        }

        if (other.tag == "End6")
        {
            GateSound.Play();
            StartCoroutine(WaitforSceneLoad6());
        }

        if (other.tag == "sister")
        {
            GateSound.Play();
            StartCoroutine(WaitforSceneLoad7());
        }
    }
}
