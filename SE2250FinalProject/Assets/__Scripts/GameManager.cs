using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

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

    private GameObject[] _kiras;

    private GameObject[] _hanses;

    private int _selectedOutfitIndex;

    private bool _isHans;

    private GameObject _currentPlayer;

    private string[] _sceneNames = new string[]{ "CharacterCustomization", "LevelOne", "ComputerView" };

    private string _currentSceneName;





    private void Awake()
    {
        Debug.Log("Awake Is Called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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


            _selectedOutfitIndex = 0;

            _isHans = true;

            _currentPlayer = GetNewCharacter();



        } else {
            Destroy(gameObject);
            Debug.Log("This singleton already exists");
        }

    }

    

    public GameObject ToggleCharacters() {
        _isHans = !_isHans;
        _selectedOutfitIndex = 0;
        _currentPlayer = GetNewCharacter();

        // Should do some work here to make sure the player's data persists (if we
        // even allow for customization during gameplay).
        return _currentPlayer;
    }

    private GameObject GetNewCharacter()
    {
        GameObject _returnPlayer;
        if (_isHans)
        {
            _returnPlayer = _hanses[_selectedOutfitIndex];
            
        } else
        {
            _returnPlayer = _kiras[_selectedOutfitIndex];
        }

        Debug.Log("Made it here!");
        Debug.Log(_returnPlayer.GetComponent<Transform>().position.x);

        return _returnPlayer;
    }

    public GameObject GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    public string GetCurrentPlayerName()
    {
        return _isHans ? "Hans" : "Kira";
    }

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

    public string GetCharacterText() {
        if (_isHans)
        {
            return "Hans is a socialite. He's really likeable, but can come off a little strong sometimes. By selecting Hans, you'll be able to make more meaningful" +
                "relationships, faster, by having more meaningful conversations. But be careful, come off too strong and you might get fired.";
        } else
        {
            return "Kira couldn't be bothered with what Sandra had for dinner. She might not know about quantam technology, but she has wits, especially with the " +
                "art of theivery. With Kira you'll be able to steal from your coworkers to get work done, but be careful! Get caught and you might get fired!.";
        }
    }


    public void GoToGameScene()
    {
        SceneManager.LoadScene(_sceneNames[1]);
    }

    public void GoToPlayerCustomizationScene()
    {
        SceneManager.LoadScene(_sceneNames[0]);
    }
   



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
