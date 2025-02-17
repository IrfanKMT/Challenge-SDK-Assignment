using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChallengeSDK
{
    private string _url;

    public ResponseHelper ResponseHelper;
    public ExceptionHandler RequestException;

    public ChallengeSDK(string _url)
    {
        this._url = _url;
    }


    /// <summary>
    /// CREATES CHALLENGE 
    /// </summary>
    /// Object reference of user data
    /// <param name="challenge"></param>
    /// Token
    /// <param name="jwtToken"></param>
    /// Please change String "Authorization" in Token
    /// <returns></returns>
    public async Task CreateChallenge(CreateChallengeDto challenge, string jwtToken)
    {

        WebRequestHelper _web = new WebRequestHelper
        {
            Uri = _url,
            BodyString = JsonUtility.ToJson(challenge),
            Headers = new Dictionary<string, string> {
                    { "Authorization", jwtToken }
                }
        };

        await Post(_web, (error, response) =>
        {
            if (error != null)
            {
                //FAILED
                Debug.LogError($"Request failed: {error.Message}");
            }
            else
            {
                //SUCCESS
                Debug.Log($"Request succeeded: {response.Text}");
            }
        });
    }

    /// <summary>
    /// GET CHALLENGE DATA
    /// </summary>
    /// Token
    /// <param name="jwtToken"></param>
    /// Please change String "Authorization" in Token
    /// <returns></returns>
    public async Task GetChallengeData(string jwtToken)
    {
        WebRequestHelper _web = new WebRequestHelper
        {
            Uri = _url,
            Headers = new Dictionary<string, string> {
                    { "Authorization", jwtToken }
                }
        };

        await Get(_web, (error, response) =>
        {
            if (error != null)
            {
                //FAILED
                Debug.LogError($"Request failed: {error.Message}");
            }
            else
            {
                //SUCCESS
                Debug.Log($"Request succeeded: {response.Text}");
            }
        });
    }


    public async Task Post(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        options.Method = "POST";
        await Task.Run(() => Request(options, callback));
    }

    public async Task Get(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        options.Method = "GET";
        await Task.Run(() => Request(options, callback));
    }


    public static void Request(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        StaticCoroutine.StartCoroutine(NetworkHandler.UnityWebRequest(options, callback));
    }
}


#region SERIALIZED CLASS

[System.Serializable]
public class CreateChallengeDto
{
    public string ChallengeName;
    public string ChallengeDescription;
    public double StartDate;
    public double EndDate;
    public int GameID;
    public int MaxParticipants;
    public int Wager;
    public int Target;
    public int ChallengeCreator;
    public bool AllowSideBets;
    public int SideBetsWager;
    public string Unit;
    public bool IsPrivate;
    public VERIFIED_CURRENCY Currency;
    public CHALLENGE_CATEGORIES ChallengeCategory;
    public string NFTMedia;
    public string Media;
    public DateTime ActualStartDate;
    public string UserAddress;
}

[System.Serializable]
public enum VERIFIED_CURRENCY
{
    CREDITS,
    USDC,
    SOL,
    BONK,
    SEND,
    WIF,
    POPCAT,
    PNUT,
    GIGA,
    TRUMP,
    MELANIA
}

[System.Serializable]
public enum CHALLENGE_CATEGORIES
{
    FITNESS,
    ART,
    TRAVEL,
    ADVENTURE,
    LIFESTYLE,
    GAMING,
    SPORTS,
    SOCIAL_MEDIA,
    EVENT,
    RANDOM,
    ESPORT,
    FITNESS_PUSHUP,
    FITNESS_PULLUP,
    FITNESS_SQUAT,
    FITNESS_BURPEE,
    FITNESS_PLANK,
    FITNESS_JUMPING_JACK
}
#endregion