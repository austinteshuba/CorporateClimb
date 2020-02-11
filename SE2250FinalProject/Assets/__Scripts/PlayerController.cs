using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    public float rotateSpeed;
    public float speed;

    private int _influence;
    private float _multiplier;
    private float _cash;
    private int _salary;
    private string _jobPosition;
    private string _alertText;
    private bool _stealingFlag;
    private bool _socializingFlag;
    private bool _isColliding;

    private readonly string[] CONVERSATION_TEXTS = {
        "You joked about golf. It worked!",
        "Your coworker made some really odd comment about ones and zeroes. You Laughed - good call!",
        "You complimented your coworkers shoes."
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        _influence = 0;
        _multiplier = 1.0f;
        _cash = 1000f;
        _salary = 43000;
        _jobPosition = "Intern";
        _alertText = "";
        _stealingFlag = false;
        _socializingFlag = false;
        _isColliding = false;

        EarnMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
        float translation = Input.GetAxis("Vertical") * speed;
        animator.SetFloat("Speed", translation);
        float strafe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        transform.Translate(0, 0, translation);

        CheckForRaise();

        if (_isColliding)
        {
            if (Input.GetKey(KeyCode.T)) { Socialize(); }
            if (Input.GetKey(KeyCode.S)) { TryStealing(); }

        }
    }

    void CheckForRaise()
    {
        if (_influence >= 100)
        {
            _influence = _influence - 100;
            _salary = Mathf.RoundToInt(_salary * 1.1f);
            _alertText = "You got a Raise! On your way to a promotion :)";
            Invoke("ClearAlert", 4);
        }
    }

    public float GetCash()
    {
        return (float) System.Math.Round(_cash, 2);
    }
    public int GetInfluence()
    {
        return _influence;
    }

    public float GetMultiplier()
    {
        return _multiplier;
    }

    public int GetSalary()
    {
        return _salary;
    }

    public string GetPosition()
    {
        return _jobPosition;
    }

    public string GetAlertText()
    {
        return _alertText;
    }

    public bool GetColliding()
    {
        return _isColliding;
    }

    void EarnMoney()
    {
        _cash += _salary / (365 * 8);
        Invoke("EarnMoney", 10);
    }

    void ClearAlert()
    {
        _alertText = "";
        Debug.Log("Clear Text Called");
    }

    void ResetStealingFlag()
    {
        _stealingFlag = false;
    }

    void ResetSocializingFlag()
    {
        _socializingFlag = false;
    }

    void TryStealing()
    {
        if (_stealingFlag == false)
        {
            int random = Random.Range(1, 3);

            _stealingFlag = true;

            if (random == 1)
            {
                _alertText = "You Tried Stealing, and You Got Caught!!!";
                _influence = Mathf.Min(0, _influence - 10);
                _multiplier = _multiplier * 0.9f;
                Invoke("ClearAlert", 4);
            }
            else if (random == 2)
            {
                _alertText = "You Tried Stealing, and It Worked! Score!";
                _influence += Mathf.RoundToInt(10 * _multiplier);
                Invoke("ClearAlert", 4);
            }
            Invoke("ResetStealingFlag", 5);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            _isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            _isColliding = false;
        }
    }

    void Socialize()
    {
        if (_socializingFlag == false)
        {
            int rand = Random.Range(0, CONVERSATION_TEXTS.Length);
            _alertText = CONVERSATION_TEXTS[rand];
            _socializingFlag = true;
            _multiplier = (float) System.Math.Round(_multiplier*1.1, 2);
            _influence += 1;
            Invoke("ClearAlert", 4);
            Invoke("ResetSocializingFlag", 5);
        }

    }
}
