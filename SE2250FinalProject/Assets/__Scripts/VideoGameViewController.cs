using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VideoGameViewController : ResizeableView
{

    private int _playerGoalScore;
    private int _partnerGoalScore;
    public Button cancelButton;
    private GameManager _gameManager;

    private PongManager _pongManager;

    
    // Start is called before the first frame update
    void Start()
    {
        
        _gameManager = GameManager.Instance;
        cancelButton.onClick.AddListener(OnClick);


        

        CreateDisplayTargetScore();



        _pongManager = GameObject.Find("PongManager").GetComponent<PongManager>();

    }

    

    private void CreateDisplayTargetScore()
    {
        bool _win = Random.Range(1, 2) == 1 ? true : false;

        if (_win)
        {
            _playerGoalScore = 7;
            _partnerGoalScore = Random.Range(2, 6);
        } else
        {
            _partnerGoalScore = 7;
            _playerGoalScore = Random.Range(2, 6);
        }
        Debug.Log("Message send");
        _gameManager.GlobalAlert("Your managing partner has chosen to play Pong. To maximize how much they like you, aim to " + (_win ? "win" : "lose") + " with a score of 7 - " + (_win ? _partnerGoalScore : _playerGoalScore) );
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_gameManager.alertDisplayed && !_pongManager.GetPlaying())
        {
            _pongManager.StartGame();
        }
        
    }

   

    void OnClick()
    {
        _gameManager.GoToGameScene();
    }
}
