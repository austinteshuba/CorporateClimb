using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    public GameObject playerObject;
    public GameObject botObject;
    public float turnSpeed;
    private Transform _playerTransform;
    private GameObject _player;
    private GameObject _bot;
    private Vector3 _offset;
    private Vector3 _playerScale = new Vector3(3f, 3f, 3f);

    private bool _canInteract;

    public Text cashText;
    public Text influenceText;
    public Text alertText;
    public Text instructionsText; // TODO: Implement the instructions only if they are near another player
    public Text salaryText;
    public Text multiplierText;
    

    private readonly string _instructions = "Press 'T' to Talk \n Press 'S' to Steal";


    void Start()
    {
        _canInteract = false;
        // Instantiate player
        _player = Instantiate(playerObject);
        _playerTransform = _player.GetComponent<Transform>();
        _playerTransform.position = new Vector3(.02f, 0f, -1.135623f);
        _playerTransform.localScale = _playerScale;

        // Instantiate bot
        _bot = Instantiate(botObject);
        _bot.transform.position = new Vector3(10, 0, 0);
        _bot.transform.localScale = _playerScale;
        

        _offset = transform.position - _playerTransform.position;


    }

    void Update()
    {
        cashText.text = "Cash: $" + _player.GetComponent<PlayerController>().GetCash();
        influenceText.text = "Influence: " + _player.GetComponent<PlayerController>().GetInfluence();
        alertText.text = _player.GetComponent<PlayerController>().GetAlertText();
        salaryText.text = "Current Salary: $" + _player.GetComponent<PlayerController>().GetSalary();
        multiplierText.text = "Multiplier: " + _player.GetComponent<PlayerController>().GetMultiplier();
        UpdateInstructionsText();
    }

    void UpdateInstructionsText()
    {
        // Use the _canInteract flag to check if you can do interactions
        // Feel free to reimplement if you want - don't know how this will work with the trigger thing
        if (_canInteract)
        {
            instructionsText.text = _instructions;
        } else
        {
            instructionsText.text = "Current Position: " + _player.GetComponent<PlayerController>().GetPosition() + "\n" + (100 - _player.GetComponent<PlayerController>().GetInfluence()) + " influence to a raise!";
        }
    }

// TODO: When it is appropriate, use the _canInteract flag to make it possible to do the stealing and the socializing. There are methods made for this in Player. 
    void LateUpdate()
    {

        _offset = Quaternion.AngleAxis(Input.GetAxis("Horizontal") * turnSpeed, Vector3.up) * _offset;
        transform.position = _playerTransform.position + _offset;
        transform.LookAt(_playerTransform.position);
        
    }
}
