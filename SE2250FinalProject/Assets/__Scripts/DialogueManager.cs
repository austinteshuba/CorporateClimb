using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : InfoBoxController
{
    // List to contain option buttons
    private List<GameObject> _buttons;
    private List<string> _options;

    private DialogueScripts _dialogueScripts;
    private Dialogue[] dialogue;
    private string _dialogueType;

    public GameObject dialogueButton;

    private GameObject _bot;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Initialize button list
        _buttons = new List<GameObject>();

        // Get DialogueScripts
        _dialogueScripts = gameObject.GetComponent<DialogueScripts>();

        // Initialize options list
        _options = new List<string>();
        _options.Add("Offer");
        _options.Add("Compliment");
        _options.Add("Insult Other");
    }

    public void InitializeDialogue(bool isHans, GameObject interactingBot)
    {
        // Set bot
        _bot = interactingBot;

        // Display box
        ShowBox(false);

        // Get dialogue type from user

        if (isHans)
        {
            CreateButtonArray(_options, PresentDialogues);
            _dialogueType = null;
        } else
        {
            _dialogueType = "Compliment";
            PresentDialogues();
        }

        

    }

    void PresentDialogues()
    {
        // Get selected dialogue type
        if (_dialogueType == null)
        {
            GameObject buttonpressed = EventSystem.current.currentSelectedGameObject;
            _dialogueType = buttonpressed.GetComponentInChildren<Text>().text;
        }

        // Get dialogue options
        switch (_dialogueType)
        {
            case "Offer":
                dialogue = _dialogueScripts.offerDialogues;
                break;
            case "Compliment":
                dialogue = _dialogueScripts.complimentDialogues;
                break;
            case "Insult Other":
                dialogue = _dialogueScripts.insultDialogues;
                break;
            default:
                Debug.LogError("No dialogue initialized");
                return;
        }

        List<string> dialogueNames = new List<string>();

        foreach (Dialogue d in dialogue)
        {
            dialogueNames.Add(d.dialogueName);
        }

        // Create array of button options
        CreateButtonArray(dialogueNames, ActivateDialogue);
    }

    void CreateButtonArray(List<string> buttonLabels, UnityEngine.Events.UnityAction call)
    {
        // Clear any existing buttons
        ClearButtons();

        // 10 units of button spacing
        int buttonSpacing = Mathf.RoundToInt(dialogueButton.GetComponent<RectTransform>().rect.width) + 10;
        int buttonCount = 0;

        // Get position of buttons
        RectTransform dbRect = _infoBox.GetComponent<RectTransform>();
        int boxHeight = Mathf.RoundToInt(dbRect.rect.height / 4);
        int boxWidth = Mathf.RoundToInt(dbRect.rect.width / 8);

        foreach (string label in buttonLabels)
        {
            // Instantiate new button
            GameObject newButton = Instantiate(dialogueButton);
            newButton.GetComponent<Button>().onClick.AddListener(call);
            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = label;
            buttonText.fontSize = 12;
            newButton.transform.SetParent(_infoBox.transform);

            // Position button
            newButton.transform.localPosition = new Vector3(-boxWidth + buttonSpacing * buttonCount++, -boxHeight, 0);

            // Add to button list
            _buttons.Add(newButton);
        }

    }

    void ClearButtons()
    {
        foreach (GameObject b in _buttons)
        {
            Destroy(b);
        }
        _buttons.Clear();
    }

    void ActivateDialogue()
    {
        // Get selected dialogue from button
        GameObject buttonpressed = EventSystem.current.currentSelectedGameObject;
        string buttonName = buttonpressed.GetComponentInChildren<Text>().text;

        Dialogue selectedDialogue = null;

        // Find dialogue
        foreach (Dialogue d in dialogue)
        {
            if (d.dialogueName == buttonName)
            {
                selectedDialogue = d;
            }
        }

        if (selectedDialogue == null)
        {
            Debug.LogError("No dialogue found that matched button selection!");
            return;
        }

        // Get bot probability of success for selected category
        BotController botController = _bot.GetComponent<BotController>();
        int probOfSuccess = botController.GetProbabilityOfSuccess(_dialogueType);
        if (probOfSuccess < 0)
        {
            Debug.LogError("Invalid type key: " + _dialogueType);
            return;
        }

        if (isSuccessful(probOfSuccess))
        {
            DisplayDialogue(selectedDialogue.playerMessage, selectedDialogue.successResponse);
        } else
        {
            DisplayDialogue(selectedDialogue.playerMessage, selectedDialogue.failureResponse);
        }

        
    }

    void DisplayDialogue(string playerMessage, string botMessage)
    {
        string message = "You: " + playerMessage + "\n" + _bot.GetComponent<BotController>().characterName + ": " + botMessage;
        ClearButtons();
        SetText(message);
        Invoke("DisplayExitButton", 2);
    }

    void DisplayExitButton()
    {
        _exitButtonObject.SetActive(true);
    }

    bool isSuccessful(int probSuccess)
    {
        int randInt = Mathf.RoundToInt(Random.Range(0, 100));
        return randInt <= probSuccess;
    }
}
