using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Computer View Controller
// General Computer View controller handles UI elements common to all computer Scenes.
public class ComputerViewController : ResizeableView
{
    // fields for time remaining on computer and game manager
    protected float _timeLeft;

    protected GameManager _gameManager;

    //UI elements

    public Text TimeText;

    public Text ResourcesText;

    protected Vector2 _resolution; 




    
    // Set time left and game manager instance.
    protected virtual void Awake()
    {
        HARDCODED_RESOLUTION = new Vector2(1530f, 765f);
        _gameManager = GameManager.Instance;

        _timeLeft = _gameManager.GetComputerTime();
        _resolution = HARDCODED_RESOLUTION;

        ResizeUIComponents();
        _resolution = new Vector2(Screen.width, Screen.height);


    }



    // Make sure the resource text is accurate
    public void UpdateResourceText()
    {
        ResourcesText.text = "Influence: " + _gameManager.GetInfluence() + "\n" +
             "Influence Multiplier: " + _gameManager.GetMultiplier() + "x \n" +
             "Cash: $" + _gameManager.GetCash() + "\n";
            
    }

    protected virtual void ResizeUIComponents()
    {
        
        ResizeText(TimeText, _resolution);
        ResizeText(ResourcesText, _resolution);
        

    }

    protected void ResizeText(Text obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.rectTransform;

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
    }

    protected void ResizeButton(Button obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.GetComponent<RectTransform>();

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
    }

    protected void ResizeGameObject(GameObject obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.GetComponent<RectTransform>();

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
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

        if (Screen.width != _resolution.x || Screen.height != _resolution.y)
        {
            ResizeUIComponents();
            _resolution = new Vector2(Screen.width, Screen.height);
        }
        

    }

    // Leave computer without consequences.
    public void ExitTerminal()
    {
        _gameManager.GoToGameScene();
    }
}
