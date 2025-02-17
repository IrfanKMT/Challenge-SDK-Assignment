using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChallengeSDK
{
    #region Variables
    private string _url;

    //public bool Success;
    //public ResponseHelper ResponseHelper;
    //public ExceptionHandler RequestException;

    #endregion

    #region Methods
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
    public async Task CreateChallenge(CreateChallengeDto challenge, string jwtToken, Action<ExceptionHandler, ResponseHelper> callback)
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

            callback(error, response);
        });
    }

    /// <summary>
    /// GET CHALLENGE DATA
    /// </summary>
    /// Token
    /// <param name="jwtToken"></param>
    /// Please change String "Authorization" in Token
    /// <returns></returns>
    public async Task GetChallengeData(string jwtToken, Action<ExceptionHandler, ResponseHelper> callback)
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

            callback(error, response);
        });
    }

    /// <summary>
    /// TASK FOR POST API
    /// </summary>
    /// <param name="options"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public async Task Post(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        options.Method = "POST";
        await Task.Run(() => Request(options, callback));
    }

    /// <summary>
    /// TASK FOR Get API
    /// </summary>
    /// <param name="options"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public async Task Get(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        options.Method = "GET";
        await Task.Run(() => Request(options, callback));
    }


    public static void Request(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        StaticCoroutine.StartCoroutine(NetworkHandler.UnityWebRequest(options, callback));
    }

    #endregion
}