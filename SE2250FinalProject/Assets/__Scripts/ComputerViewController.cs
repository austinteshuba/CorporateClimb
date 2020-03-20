using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Computer View Controller
// General Computer View controller handles UI elements common to all computer Scenes.
public class ComputerViewController : MonoBehaviour
{
    // fields for time remaining on computer and game manager
    protected float _timeLeft;

    protected GameManager _gameManager;

    //UI elements

    public Text TimeText;

    public Text ResourcesText;

    
    // Set time left and game manager instance.
    protected virtual void Awake()
    {
        _gameManager = GameManager.Instance;

        _timeLeft = _gameManager.GetComputerTime();

        


    }

    // Make sure the resource text is accurate
    public void UpdateResourceText()
    {
        ResourcesText.text = "Influence: " + _gameManager.GetInfluence() + "\n" +
             "Influence Multiplier: " + _gameManager.GetMultiplier() + "x \n" +
             "Cash: $" + _gameManager.GetCash() + "\n";
            
    }

   
    // Update the time text
    protected void UpdateTimeText()
    {
        TimeText.text = "Time Until User Returns: " + Mathf.Round(_timeLeft) + " sec";
    }


    // Always update the text so the latest info is presented.
    protected virtual void Update()
    {
        _timeLeft = _gameManager.GetComputerTime();
        UpdateTimeText();
        UpdateResourceText();

    }

    // Leave computer without consequences.
    public void ExitTerminal()
    {
        _gameManager.GoToGameScene();
    }
}
