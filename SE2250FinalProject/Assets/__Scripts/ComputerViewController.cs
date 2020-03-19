using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerViewController : MonoBehaviour
{
    public Text DescriptionText;

    public Text DisplayTitleText;

    public Text TimeText;

    public Text ResourcesText;

    private float _timeLeft;

    private GameManager _gameManager;

    private PlayerController _player;

    private string[] _descriptionTexts = new string[] {
        "Welcome to your terminal. Please select a function.",
        "Quantam Bit has a variety of external consultants you can access. " +
        "Feel free to chat with these resources and use them to your advantage!",
        "Access your desktop and all of your completed work. " +
        "Please note a numerical PIN number will be required."
    };

    void Awake()
    {
        // TODO: Set title of the screen.
        _gameManager = GameManager.Instance;

        DefaultText();

        _timeLeft = _gameManager.GetComputerTime();

        _player = _gameManager.GetCurrentPlayer().GetComponent<PlayerController>();



    }

    public void UpdateResourceText()
    {
        ResourcesText.text = "Influence: " + _player.GetInfluence() + "\n" +
             "Influence Multiplier: " + _player.GetMultiplier() + "x \n" +
             "Cash: $" + _player.GetCash() + "\n";
            
    }

    public void DesktopText()
    {
        Debug.Log("Desktop Text");
        DescriptionText.text = _descriptionTexts[2];
    }

    public void ExternalContactText()
    {
        DescriptionText.text = _descriptionTexts[1];
    }

    public void DefaultText()
    {
        DescriptionText.text = _descriptionTexts[0];
    }

    void UpdateTimeText()
    {
        TimeText.text = "Time Until User Returns: " + Mathf.Round(_timeLeft) + " sec";
    }


    // Update is called once per frame
    void Update()
    {
        _timeLeft = _gameManager.ReduceComputerTime(Time.deltaTime);
        UpdateTimeText();
        UpdateResourceText();

    }
}
