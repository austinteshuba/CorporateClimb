using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{


    public string characterName;
    public int offerSuccess;
    public int complimentSuccess;
    public int insultSuccess;

    private int _cash;
    private int _influence;

    public int GetProbabilityOfSuccess(string key)
    {
        if (key == "Offer")
            return offerSuccess;
        else if (key == "Compliment")
            return complimentSuccess;
        else if (key == "Insult Other")
            return insultSuccess;
        else
            return -1;
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
