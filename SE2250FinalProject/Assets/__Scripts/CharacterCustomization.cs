using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    private GameObject _player;
    private GameObject _playerInstance;
    private GameManager _gameManager;

    public Button ChangeCharacter;
    public Button ChangeOutfit;
    public Text Instructions;

    void Awake()
    {
        _gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = _gameManager.GetCurrentPlayer();
        _playerInstance = Instantiate(_player);

        UpdateCharacterButtonText();
        UpdateOutfitButtonText();

        
    }

    // Update is called once per frame
    void Update()
    {
        Instructions.text = GameManager.Instance.GetCharacterText();
    }

    void UpdateCharacterButtonText()
    {
        ChangeCharacter.transform.Find("CharacterText").GetComponent<Text>().text = _player.GetComponent<PlayerController>().Name;
        UpdateOutfitButtonText();
    }

    void UpdateOutfitButtonText()
    {
        ChangeOutfit.transform.Find("OutfitText").GetComponent<Text>().text = _player.GetComponent<PlayerController>().OutfitName;
    }

    public void ToggleOutfit()
    {
        Destroy(_playerInstance);
        _player = GameManager.Instance.ToggleOutfit();

        UpdateOutfitButtonText();

        _playerInstance = Instantiate(_player);
    }

    public void ToggleCharacter()
    {
        Destroy(_playerInstance);
        _player = GameManager.Instance.ToggleCharacters();

        UpdateCharacterButtonText();

        _playerInstance = Instantiate(_player);
    }
}
