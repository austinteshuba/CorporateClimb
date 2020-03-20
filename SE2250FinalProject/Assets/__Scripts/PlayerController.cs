﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Basic player properties
    public float rotateSpeed;
    public float speed;
    public bool isSitting;
    public string Name;
    public string OutfitName;

    // Will hold Animator instance and link scripted variables to animator variables
    private Animator animator;

    // Get GameManager instance
    private GameManager _gameManager = GameManager.Instance;

    // Flags for collision and actions
    private bool _stealingFlag;
    private bool _socializingFlag;
    private bool _sittingFlag;
    private bool _isColliding;
    private bool _isCollidingWithDesk;

    // Holds bot being collided with
    private GameObject _collidingBot;

    /*
    private readonly string[] CONVERSATION_TEXTS = {
        "You joked about golf. It worked!",
        "Your coworker made some really odd comment about ones and zeroes. You Laughed - good call!",
        "You complimented your coworkers shoes."
    };
    */

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        _stealingFlag = false;
        _socializingFlag = false;
        _sittingFlag = false;
        _isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        float translation = Input.GetAxis("Vertical") * speed;
        animator.SetFloat("Speed", translation);
        animator.SetBool("isSitting", isSitting);
        float strafe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        transform.Translate(0, 0, translation);

        if (Input.GetKey(KeyCode.Space)) { ToggleSit(); }

        if (_isColliding)
        {
            if (Input.GetKey(KeyCode.T)) { Socialize(_collidingBot); }
            //if (Input.GetKey(KeyCode.S)) { TryStealing(); }
        }

        if (_isCollidingWithDesk && isSitting)
        {
            if (Input.GetKey(KeyCode.C))
            {
                isSitting = false;
                GameManager.Instance.GoToComputerView();
            }
        }
    }

    public bool GetColliding()
    {
        return _isColliding;
    }

    public bool GetCollidingWithDesk()
    {
        return _isCollidingWithDesk;
    }

    
    // Flag resets
    void ResetStealingFlag()
    {
        _stealingFlag = false;
    }

    void ResetSocializingFlag()
    {
        _socializingFlag = false;
    }

    void ResetSittingFlag()
    {
        _sittingFlag = false;
    }

    // Toggle sitting variable
    void ToggleSit()
    {
        if (_sittingFlag == false)
        {
            isSitting = !isSitting;
            _sittingFlag = true;
            Invoke("ResetSittingFlag", 0.5f);
        }
        
    }

    //void TryStealing()
    //{
    //    if (_stealingFlag == false)
    //    {
    //        int random = Random.Range(1, 3);

    //        _stealingFlag = true;

    //        if (random == 1)
    //        {
    //            _alertText = "You Tried Stealing, and You Got Caught!!!";
    //            _influence = Mathf.Min(0, _influence - 10);
    //            _multiplier = _multiplier * 0.9f;
    //            Invoke("ClearAlert", 4);
    //        }
    //        else if (random == 2)
    //        {
    //            _alertText = "You Tried Stealing, and It Worked! Score!";
    //            _influence += Mathf.RoundToInt(10 * _multiplier);
    //            Invoke("ClearAlert", 4);
    //        }
    //        Invoke("ResetStealingFlag", 5);

    //    }
    //}

    // Check collision types
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            _isColliding = true;
            
            _collidingBot = other.gameObject;
        } else if (other.CompareTag("Desk"))
        {
            _isCollidingWithDesk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            _isColliding = false;
        } else if (other.CompareTag("Desk"))
        {
            _isCollidingWithDesk = false;
        }
    }

    // Activate dialogue manager if socialization is selected
    void Socialize(GameObject collidingBot)
    {
        if (_socializingFlag == false)
        {
            if (collidingBot == null)
            {
                Debug.Log("Attempting to talk to null bot");
            } else
            {
                DialogueManager dm = FindObjectOfType<DialogueManager>();
                dm.InitializeDialogue(_gameManager.GetIsHans(), collidingBot);
            }
            /*
            int rand = Random.Range(0, CONVERSATION_TEXTS.Length);
            _alertText = CONVERSATION_TEXTS[rand];
            _multiplier = (float) System.Math.Round(_multiplier*1.1, 2);
            _influence += 1;
            */
            _socializingFlag = true;
            //Invoke("ClearAlert", 4);
            Invoke("ResetSocializingFlag", 5);
        }

    }
}
