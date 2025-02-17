
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class ResponseHelper
{
    public UnityWebRequest Request { get; private set; }

    public long StatusCode => Request.responseCode;

    public byte[] Data
    {
        get
        {
            try
            {
                return Request.downloadHandler.data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public string Text
    {
        get
        {
            try
            {
                return Request.downloadHandler.text;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }

    public string Error => Request.error;

    public Dictionary<string, string> Headers => Request.GetResponseHeaders();

    public ResponseHelper(UnityWebRequest request)
    {
        Request = request;
    }

    public string GetHeader(string name)
    {
        return Request.GetResponseHeader(name);
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this, prettyPrint: true);
    }
}
