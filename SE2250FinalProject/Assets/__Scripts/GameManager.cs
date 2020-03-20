using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
// GameManager
// This is a class that handles any operation that
// Transcends one single scene/
// THis usually has to do with points, navigating between scenes,
// Perstitance for bots, and character customization stuff.
// This is a singleton, as required. Uses C# Properties, as required.
public class GameManager : MonoBehaviour
{
    // set Singleton instance. 
    public static GameManager Instance { get; private set; }

    // Game objects for the characters and outfits.
    public GameObject HansBrown;

    public GameObject HansBlack;

    public GameObject HansBlue;

    public GameObject HansTeal;

    public GameObject HansRed;

    public GameObject KiraEthnic;

    public GameObject KiraRed;

    public GameObject KiraBlack;

    public GameObject KiraYellow;

    public GameObject KiraWhite;

    public List<GameObject> Bots; // repository of all active bots.


    // Used for sending alerts
    private InfoBoxController _infoBox;

    // For Character customization
    private GameObject[] _kiras;

    private GameObject[] _hanses;

    private int _selectedOutfitIndex;

    private bool _isHans; // flag for permissions

    public string PlayerName
    {
        get { return _isHans ? "Hans" : "Kira"; }
        set { _isHans = value == "Hans"; }
    }

    private GameObject _currentPlayer;

    // Scene names for navigation
    private string[] _sceneNames = new string[]{ "CharacterCustomization", "LevelOne", "ComputerView", "ComputerExternalResourceView", "ComputerDesktopView" };

    private string _currentSceneName;

    // Variable to see if you were caught on computer
    private float _computerTimeLeft;

    private string _terminalOwner;

    // Timer for paycheques.
    private float _timeToNextEarning;

    // This has to do with earning influence, influenceMultiplier, Cash, and storing roles like position.
    // This is for persistence between scenes.

    private int _influence;

    private float _multiplier;

    private float _cash;

    private int _salary;

    private string _jobPosition;

    private bool _isEarning;


    // These are for alerts and presenting messages for plot
    private string _globalAlert;

    private bool _alertDisplayed;

    private Queue<string> _globalAlertQueue;

    private bool _customizeAlertsPresented;

    private bool _gameAlertsPresented;



    // Stores the name and influence values of bots. This is for persistence between scenes.
    private SortedDictionary<string, int> _bots;






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Init the instance

            // Set up player customization
            _currentSceneName = _sceneNames[0];

            _kiras = new GameObject[5];
            _kiras[0] = KiraEthnic;
            _kiras[1] = KiraRed;
            _kiras[2] = KiraBlack;
            _kiras[3] = KiraYellow;
            _kiras[4] = KiraWhite;

            _hanses = new GameObject[5];
            _hanses[0] = HansBlack;
            _hanses[1] = HansBrown;
            _hanses[2] = HansBlue;
            _hanses[3] = HansTeal;
            _hanses[4] = HansRed;

            // Set up the bots for the game
            _bots = new SortedDictionary<string, int>();

            _bots["Avery"] = 10;

            _bots["Paul"] = 10;

            _bots["James"] = 10;

            // Set default character selection

            _selectedOutfitIndex = 0;

            _isHans = true;

            _isEarning = false;

            _customizeAlertsPresented = false;

            _gameAlertsPresented = false;

            // Set default values for the player
            _influence = 0;
            _multiplier = 1.0f;
            _cash = 1000f;
            _salary = 43000;
            _jobPosition = "Intern";

            // Set default values for timers and the global alert system
            _globalAlert = "";

            _globalAlertQueue = new Queue<string>();

            _timeToNextEarning = 2;

            _alertDisplayed = false;

            _currentPlayer = GetNewCharacter(); // get the player

        } else {
            Destroy(gameObject);
            Debug.Log("This singleton already exists");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Keep track of pay
        if (_isEarning)
        {
            EarnMoney();
        }
        // Check if you beat the level  or lost
        CheckForPromotion();
        CheckFired();

        // present alerts if needed.
        if (_globalAlertQueue.Count > 0)
        {
            if (!_alertDisplayed)
            {
                _infoBox = FindObjectOfType<InfoBoxController>();
                _infoBox.SetText("                         ALERT\n\n\n" + _globalAlertQueue.Dequeue());
                _infoBox.ShowBox();
                _alertDisplayed = true;
            }   
        } else if (_globalAlertQueue.Count == 0 && _influence < -15) // Handles the case of being fired.
        {
            ResetGame();
        }

        // If you are on a computer scene, reduce computer time.
        if (_currentSceneName.Contains("Computer"))
        {
            ReduceComputerTime(Time.deltaTime);
        }


    }

    // Getters
    
    public SortedDictionary<string, int> GetBots()
    {
        return _bots;
    }

    public int GetBotInfluence(string name)
    {
        return (int)_bots[name];
    }

    public float GetCash()
    {
        return (float)System.Math.Round(_cash, 2);
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

    public GameObject GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public string GetCurrentPlayerName()
    {
        return _isHans ? "Hans" : "Kira";
    }

    public string GetGlobalAlert()
    {
        return _globalAlert;
    }

    public string GetTerminalOwnerName()
    {
        return _terminalOwner;
    }

    public bool GetIsHans()
    {
        return _isHans;
    }

    public float GetComputerTime()
    {
        return _computerTimeLeft;
    }
    // Gets the text for each character description
    public string GetCharacterText()
    {
        if (_isHans)
        {
            return "Hans is a socialite. He's really likeable, but can come off a little strong sometimes. By selecting Hans, you'll be able to make more meaningful" +
                " relationships, faster, by having more meaningful conversations. But be careful, come off too strong and you might get fired.";
        }
        else
        {
            return "Kira couldn't be bothered with what Sandra had for dinner. She might not know about quantam technology, but she has wits, especially with the " +
                "art of theivery. With Kira you'll be able to steal from your coworkers to get work done, but be careful! Get caught and you might get fired!.";
        }
    }

    // Gets the player object based on current character customization selections.
    private GameObject GetNewCharacter()
    {
        GameObject _returnPlayer;
        if (_isHans)
        {
            _returnPlayer = _hanses[_selectedOutfitIndex];

        }
        else
        {
            _returnPlayer = _kiras[_selectedOutfitIndex];
        }

        return _returnPlayer;
    }

    // gets the summary stats of name and influence for the bots.
    public string GetBotStats()
    {
        string retString = "";

        foreach (KeyValuePair<string, int> bot in _bots)
        {
            retString += bot.Key + ": " + bot.Value + " influence\n";
        }

        return retString;
    }

    // Alerts for the start of the game
    public void CharacterCustomizationIntroductionAlerts()
    {
        if (!_customizeAlertsPresented)
        {
            GlobalAlert("Welcome to Quantbit! Where everything is in ones and zeroes, and everything in between. You have one goal: Become the CEO. You are not an engineer, a physisict, or an expert" +
                " of any kind.");

            GlobalAlert("In fact, you're nothing. Useless. Knowledgeless. However, you're unemployed and pop tarts aren't going to buy themselves. Choose between Hans and Kira and begin your journey to CEO.");
            _customizeAlertsPresented = true;
        }
    }

    // ALerts for after the character has been selected and the player enters the world.
    public void GameIntroductionAlerts()
    {
        if (!_gameAlertsPresented)
        {
            GlobalAlert("You are an intern. The bottom of the totem pole. Around you are your coworkers - they're incredibly important to your success.");
            GlobalAlert("By chatting with them, you can raise your own influence, or maybe even lower someone else's. If you get someone fired, it'll be easier to get promoted!");

            GlobalAlert("You can also resort to different methods of moving your way up. See the lovely squatting desks? They're all the rage in South Africa, especially for interns.");
            GlobalAlert("Use those to find other ways to move up to the top. Be quick! If you don't get promoted in time, you'll have to go through a quarterly code review and you'll lose some points.");
            GlobalAlert("CONTROLS:\nTo Talk: Walk to a bot, press 'T' to Talk.\nTo Use Computer: Press 'Space' to squat at the squatting desk. Then, click 'C' to enter computer.");

            _gameAlertsPresented = true;
        }
    }

    // increments the influence of the bot. 
    public int IncrementBotInfluence(string name, int increment)
    {
        _bots[name] += increment;
        return _bots[name];
    }

    
    // Removes the bot from memory and fires an alert.
    public void DestroyBot(string name)
    {
        _bots.Remove(name);
        GlobalAlert(name + " was fired!");
    }

    
    // Checks to see if the player lost.
    void CheckFired()
    {
        if (_influence < -15)
        {
            GlobalAlert("You're fired! Start over and reconsider your bad decisions. Shame!");
            Invoke("ResetGame", 4);
        }
    }
    // checks to see if player won
    void CheckForPromotion()
    {
        if (_influence >= 100)
        {
            _influence -= 100;
            _salary = Mathf.RoundToInt(_salary * 1.1f);
            _jobPosition = "Analyst";
            GlobalAlert("You got a Promotion to Analyst! [Next Level]");
        }
    }

    // Resets game to begining if needed
    void ResetGame()
    {
        Instance = null;

        GoToPlayerCustomizationScene();

        Destroy(gameObject);

    }

    // Earns money based on salary
    void EarnMoney()
    {
        _timeToNextEarning -= Time.deltaTime;
        if (_timeToNextEarning <= 0)
        {
            _cash += _salary / (365 * 40); // 2 seconds is a fifth of an hour of pay.
            _timeToNextEarning = 2;
        }
    }
    // increments influence as accrued. Displays alert if you are close to losing.
    public void IncrementInfluence(int influenceChange)
    {
        if (_influence >= -15 && influenceChange < 0 && _influence + influenceChange >= -15 && _influence + influenceChange < 0)
        {
            GlobalAlert("You are " + (15 + _influence + influenceChange) + " influence away from getting fired. Get liked, NOW.");
        }
        _influence += Mathf.RoundToInt(influenceChange * _multiplier);


    }

    // Incremements multiplier by constant
    public void IncrementMultiplier(float change)
    {
        _multiplier += change;
    }

    // increments multiplier by factor
    public void IncrementMultiplierByFactor(float factor)
    {
        _multiplier *= factor;
    }

    // increments cash by constant
    public void IncrementCash(int cash)
    {
        _cash += cash;
    }

    // helper function for increment cash
    public void SpendMoney(int amount)
    {
        IncrementCash(-amount);
    }

    // Sends out a global alert by adding to queue
    public void GlobalAlert(string alertMessage)
    {
        _globalAlertQueue.Enqueue(alertMessage);
    }

    // disables alert as needed.
    public void DisableAlert()
    {
        _alertDisplayed = false;
    }

    // Successfully steals from bot.
    public void StealSuccessfully()
    {
        _bots[_terminalOwner] -= 10;
        IncrementInfluence(10);
        IncrementMultiplierByFactor(1.3f);
        string name = _terminalOwner;
        GoToGameScene();
        GlobalAlert("You successfully stole from " + name + "! You stole 10 Influence.");
    }



    public GameObject ToggleCharacters() {
        _isHans = !_isHans;
        _selectedOutfitIndex = 0;
        _currentPlayer = GetNewCharacter();

        return _currentPlayer;
    }

    
    // Randomly generates time user can spend on computer before getting caught
    private float GenerateComputerTime()
    {
        _computerTimeLeft = Random.Range(30, 45);
        return _computerTimeLeft;
    }

    
    // Decrememnts computer time. Checks if they get caught. 
    public float ReduceComputerTime(float time)
    {
        _computerTimeLeft -= time;
        if (_computerTimeLeft < 0)
        {
            CaughtOnComputer();
        }
        return _computerTimeLeft;
    }
    // Punishes user if they run out of time. Automatically retreats to game view.
    private void CaughtOnComputer()
    {
        GoToGameScene();
        IncrementInfluence(-10);
        IncrementMultiplierByFactor(0.9f);
        GlobalAlert("What were you doing on your colleague's computer? You got caught. Now, they like you a little bit less.");
    }

    // change outfit in character customization. Thus, change player selection.

    public GameObject ToggleOutfit(bool up=true)
    {
        if (up) {
            _selectedOutfitIndex = (_selectedOutfitIndex + 1) % 5;
        } else
        {
            _selectedOutfitIndex = (_selectedOutfitIndex + 4) % 5;
        }

        _currentPlayer = GetNewCharacter();

        return _currentPlayer;

    }

    
    // Go to the relevant scenes and toggle the right flags.

    public void GoToGameScene()
    {
        _currentSceneName = _sceneNames[1];
        _isEarning = true;
        _terminalOwner = null;
        UpdateScene();
    }

    public void GoToPlayerCustomizationScene()
    {
        _currentSceneName = _sceneNames[0];
        _isEarning = false;
        UpdateScene();
    }

    public string GetCurrentSceneName()
    {
        return _currentSceneName;
    }

    public void GoToExternalConnectionsScene()
    {
        _currentSceneName = _sceneNames[3];
        _isEarning = false;
        UpdateScene();
    }
    public void GoToDesktopScene()
    {
        _currentSceneName = _sceneNames[4];
        _isEarning = false;
        UpdateScene();
    }
    

    public void GoToComputerView()
    {
        _currentSceneName = _sceneNames[2];
        _isEarning = false;
        _terminalOwner = _bots.ElementAt(Random.Range(0, _bots.Count)).Key;
        GenerateComputerTime(); // With this implementation, it's important that you never backtrack to the desktop view in the game.
        UpdateScene();
    }
    // Based on changed settings, go to scene. 
    public void UpdateScene()
    {
        SceneManager.LoadScene(_currentSceneName);
    }

    // Hire the appropriate contractor/

    public void HireHenry()
    {
        GoToGameScene();
        if (_cash >= 200)
        {
            // deduct fees and create an outcome.
            IncrementCash(-200);

            float chances = Random.Range(0.0f, 1.0f);

            if (chances < .15)
            {
                IncrementInfluence(20);
                IncrementMultiplierByFactor(1.3f);
                GlobalAlert("Wow, impressive! Henry's work really paid off for you.");
            }
            else if (chances < .85)
            {
                IncrementInfluence(10);
                IncrementMultiplierByFactor(1.2f);
                GlobalAlert("Henry performed as expected. Nice!");
            }
            else
            {
                IncrementInfluence(3);
                IncrementMultiplierByFactor(1.05f);
                GlobalAlert("Henry didn't do so well for you, but still better than nothing.");
            }

        } else // if you can't afford him, you lose some influence for even trying. 
        {
            GlobalAlert("You didn't have enough cash to hire Henry. He told you to buzz off until you have enough money.");
            IncrementInfluence(-1);
        }
    }


    public void HireJim()
    {
        GoToGameScene();
        // deduct fees and create an outcome.
        if (_cash >= 250)
        {
            IncrementCash(-250);

            float chances = Random.Range(0.0f, 1.0f);

            if (chances < .35)
            {
                IncrementInfluence(25);
                IncrementMultiplierByFactor(1.4f);
                GlobalAlert("Jim was a rockstar on this one. It wasn't rocket science, but Jim still crushed it!");
            }
            else if (chances < .65)
            {
                IncrementInfluence(12);
                IncrementMultiplierByFactor(1.2f);
                GlobalAlert("Jim performed as expected - pretty nice!");
            }
            else
            {
                IncrementInfluence(-5);
                IncrementMultiplierByFactor(.9f);
                GlobalAlert("Jim didn't do so well for you. His work was a little bit 'much'");
            }

        }
        else // if you can't afford him, you lose some influence for even trying. 
        {
            GlobalAlert("You didn't have enough cash to hire Jim. He told you to fly into the sun until you have enough money.");
            IncrementInfluence(-1);
        }
    }

    public void HirePiper()
    {
        GoToGameScene();// deduct fees and create an outcome.
        if (_cash >= 300)
        {
            IncrementCash(-300);

            float chances = Random.Range(0.0f, 1.0f);

            if (chances < .45)
            {
                IncrementInfluence(40);
                IncrementMultiplierByFactor(1.6f);
                GlobalAlert("Piper killed it! Love that wiz kid.");
            }
            else if (chances < .55)
            {
                IncrementInfluence(15);
                IncrementMultiplierByFactor(1.2f);
                GlobalAlert("Pretty rare, but Piper was pretty run of the mill. Not bad.");
            }
            else
            {
                IncrementInfluence(-15);
                IncrementMultiplierByFactor(.8f);
                GlobalAlert("The work really wasn't good. Maybe Piper isn't that smart after all.");
            }
        } else
        {
            GlobalAlert("You didn't have enough cash to hire Piper. He cried because you tempted him with candy money, then ripped it away.");// if you can't afford him, you lose some influence for even trying. 
            IncrementInfluence(-1);
        }
    }

    

    

}
