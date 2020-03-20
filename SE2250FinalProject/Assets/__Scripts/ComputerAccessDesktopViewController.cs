using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// CompouterAccessDesktopViewController
// This file handles when user is attempting to steal from coworker and
// guess their pin

    // Inherits from the general Computer View Controller, which handles some
    // UI elements. (Use of Inheritence as required).
public class ComputerAccessDesktopViewController : ComputerViewController
{
    // fields for correct pin, text, and guesses.
    private int _correctPin;

    private string _guessText;

    private int _currentMax = 99;
    private int _currentMin = 0;

    private int _buttonOneGuess;
    private int _buttonTwoGuess;
    private int _buttonThreeGuess;

    // Fields for UI elements.
    public Text GuessText;
    public Text GuessOneText;
    public Text GuessTwoText;
    public Text GuessThreeText;

    // Set up text and initial guesses
    override protected void Awake()
    {
        base.Awake();
        _gameManager.GlobalAlert("You have to guess the pin before the time runs out!");
        _correctPin = Random.Range(_currentMin, _currentMax);
        _guessText = "QUANTBIT encryption. Select your PIN from the buttons on the left.";

        _buttonOneGuess = Random.Range(_currentMin, _currentMax);
        _buttonTwoGuess = Random.Range(_currentMin, _currentMax);
        _buttonThreeGuess = Random.Range(_currentMin, _currentMax);
    }

    // Update the guesses.
    protected override void Update()
    {
        base.Update();

        GuessOneText.text = _buttonOneGuess.ToString();
        GuessTwoText.text = _buttonTwoGuess.ToString();
        GuessThreeText.text = _buttonThreeGuess.ToString();
        GuessText.text = _guessText;


    }

    // Update text after a guess. If the user was too low, that's the new
    // Miniumum. If the user was too high, that's the max
    // If they were right, steal the work successfully. 
    private void UpdateGuesses(int guessValue)
    {
        if (guessValue < _correctPin)
        {
            _guessText += ("\n" + guessValue + ": Too Low");
            _currentMin = guessValue;
        } else if (guessValue > _correctPin)
        {
            _guessText += ("\n" + guessValue + ": Too High");
            _currentMax = guessValue;
        } else
        {
            _gameManager.StealSuccessfully();
        }

        _buttonOneGuess = Random.Range(_currentMin, _currentMax);
        _buttonTwoGuess = Random.Range(_currentMin, _currentMax);
        _buttonThreeGuess = Random.Range(_currentMin, _currentMax);
    }

    // Lock in guesses 
    public void ButtonOneGuess()
    {
        UpdateGuesses(_buttonOneGuess);
    }

    public void ButtonTwoGuess()
    {
        UpdateGuesses(_buttonTwoGuess);
    }

    public void ButtonThreeGuess()
    {
        UpdateGuesses(_buttonThreeGuess);
    }


}
