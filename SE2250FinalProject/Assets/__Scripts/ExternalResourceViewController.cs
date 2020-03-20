using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// External Resource View Controller
// This handles the screen where you hire consultants.
public class ExternalResourceViewController : ComputerViewController
{

    // variables for text
    private string[] _consultantText = new string[]
    {
        "Message From Henry: Hey, I miss you! It's always fun workin' with ya. You know the drill. " +
        "I'm an enigneer and I get the job done. Not always the most exciting work, " +
        "but it'll do. It's against policy, but I know what you want. " +
        "I don't mind doing your work for you... for the right price. $200 and we'll get started." +
        "\nLow Risk, Low Reward.",

        "Message From Jim: Hey! Remember me? As an astrophysist, I offer quality work that's rarely ever (maybe sometimes) " +
        "a little eccentric. Your job will either love me or hate me, but I don't really care. " +
        "I would offer you the standard free consult, but I won't even humour you. $250 and we can give it a go." +
        "\nModerate risk, Moderate reward.",

        "Message From Piper: Hi! Yay! I get to work with you again. I'm young but I'm smart - " +
        "plus I like to think I'm cute. Am I cute enough for $300 this time? I'll do your work better than everyone else!" +
        "\nHigh risk, High reward.",

        "Welcome to QuantBit's External Resource system. You have three new messages from your contacts. " +
        "Hover over a button to view, and click to get a consult. REMINDER: QuantBit does NOT condone the use " +
        "of paid consults. But, in all honesty, we don't really know how to enforce that. It's 100% your money, " + 
        "and 100% not my problem. "

    };

    private string _messageString;

    // UI Element
    public Text MessageText;

    // Set the message to the default. Init the game manager.
    protected override void Awake()
    {
        base.Awake();
        _messageString = _consultantText[3];
    }
    // Keep the text updated
    protected override void Update()
    {
        base.Update();
        MessageText.text = _messageString;
    }

    // Set the appropriate text when hovering over the contractor buttoms
    public void HenryText()
    {
        _messageString = _consultantText[0];
    }

    public void JimText()
    {
        _messageString = _consultantText[1];
    }

    public void PiperText()
    {
        _messageString = _consultantText[2];
    }

    public void DefaultText() {
        _messageString = _consultantText[3];
    }

    // Hire the correct consultant on click.
    public void HireHenry()
    {
        _gameManager.HireHenry();
    }

    public void HireJim()
    {
        _gameManager.HireJim();
    }

    public void HirePiper()
    {
        _gameManager.HirePiper();
    }



}
