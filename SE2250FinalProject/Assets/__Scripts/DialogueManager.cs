using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject infoBoxPrefab;
    private GameObject _dialogueBox;
    private GameObject _textObject;
    private Text _messageText;


    // Start is called before the first frame update
    void Start()
    {
        _dialogueBox = Instantiate(infoBoxPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        _dialogueBox.transform.parent = gameObject.transform;

        _dialogueBox.SetActive(false);


        _textObject = new GameObject();
        _textObject.transform.parent = _dialogueBox.transform;

        _messageText = _textObject.AddComponent<Text>();
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        _messageText.font = ArialFont;
        _messageText.material = ArialFont.material;
        _messageText.fontSize = 20;
        _messageText.transform.parent = _dialogueBox.transform;

        // Set position of text
        RectTransform rectTransform = _messageText.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);

        // Hide Text
        _textObject.SetActive(false);
    }

    public void ShowBox()
    {
        _textObject.SetActive(true);
        _dialogueBox.SetActive(true);
    }

    public void HideBox()
    {
        _textObject.SetActive(false);
        _dialogueBox.SetActive(false);
    }

    public void SetText(string message)
    {
        _messageText.text = message;
    }
}
