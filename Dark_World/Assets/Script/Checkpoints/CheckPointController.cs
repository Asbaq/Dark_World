using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Sprite unlit;
    public Sprite lit;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;


    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            checkpointSpriteRenderer.sprite = lit;
            checkpointReached = true;

        }
    }
}
