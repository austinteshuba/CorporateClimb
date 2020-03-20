using System.Collections;

using System.Collections.Generic;

using UnityEngine;

// BotController.cs
// This script is attached to each Bot object

public class BotController : MonoBehaviour
{
    // Variables to store name and success rates. 
    private string _characterName;
    private int _offerSuccess;
    private int _complimentSuccess;
    private int _insultSuccess;

    private int _influence;

    private GameManager _gameManager;

    // This function will return how successful the
    // talking combat should be, depending on option selected.
    public int GetProbabilityOfSuccess(string key)
    {
        if (key == "Offer")
            return _offerSuccess;
        else if (key == "Compliment")
            return _complimentSuccess;
        else if (key == "Insult Other")
            return _insultSuccess;
        else
            return -1;
    }


    // Start is called before the first frame update
    // Set the game manager instance, and increase bot's influence ever 10 seconds.
    void Awake()
    {
        _gameManager = GameManager.Instance;
        Invoke("IncreaseInfluence", 10);
    }

    // Increase influence by set amount. Communicates with GameManager to do this.
    public void IncrementInfluence(int incrementAmount)
    {
        _influence =_gameManager.IncrementBotInfluence(_characterName, incrementAmount);
    }

    // Check if the bot should be fired AKA destroyed/killed.
    private void Update()
    {
        if (_influence < 0)
        {
            _gameManager.DestroyBot(_characterName); // communicated with game manager for persistence purposes.
            Destroy(gameObject);
        }

       
    }
    // function to accumulate influence every 10 seconds.
    private void IncreaseInfluence()
    {
        IncrementInfluence(1);
        Invoke("IncreaseInfluence", 10);
    }

    // Getters and setters for private variables, where appropriate.
    public string GetCharacterName()
    {
        return _characterName;
    }

    public void SetCharacterName(string name) {
        _characterName = name;
    }

    public int GetOfferSuccess()
    {
        return _offerSuccess;
    }

    public int GetInsultSuccess()
    {
        return _offerSuccess;
    }

    public int GetComplimentSuccess() {
        return _complimentSuccess;
    }

    public void SetComplimentSuccess(int success)
    {
        _complimentSuccess = success;
    }

    public void SetInsultSuccess(int success)
    {
        _insultSuccess = success;
    }

    public void SetOfferSuccess(int success)
    {
        _offerSuccess = success;
    }

    public int GetInfluence()
    {
        return _influence;
    }

    public void SetInfluence(int influence)
    {
        _influence = influence;
    }

}