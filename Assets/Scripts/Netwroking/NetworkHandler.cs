using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class NetworkHandler
{
    /// <summary>
    /// Creatiung R
    /// </summary>
    /// <param name="options"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private static IEnumerator CreateRequestAndRetry(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        int retries = 0;
        do
        {
            using UnityWebRequest request = CreateRequest(options);
            bool IsNetworkError = request.isNetworkError;
            AsyncOperation sendRequest = request.SendWebRequestWithOptions(options);
            if (options.ProgressCallback == null)
            {
                yield return sendRequest;
            }
            else
            {
                options.ProgressCallback(0f);
                while (!sendRequest.isDone)
                {
                    options.ProgressCallback(sendRequest.progress);
                    yield return null;
                }

                options.ProgressCallback(1f);
            }

            ResponseHelper response = request.CreateWebResponse();
            if (request.IsValidRequest(options))
            {
                callback(null, response);
                break;
            }
            if (!options.IsAborted && retries < options.Retries && (!options.RetryCallbackOnlyOnNetworkErrors || IsNetworkError))
            {
                if (options.RetryCallback != null)
                {
                    options.RetryCallback(CreateException(options, request), retries);
                }
                yield return new WaitForSeconds(options.RetrySecondsDelay);
                retries++;
                continue;
            }

            ExceptionHandler err = CreateException(options, request);
            callback(err, response);
            break;
        }
        while (retries <= options.Retries);
    }


    public static IEnumerator UnityWebRequest(WebRequestHelper options, Action<ExceptionHandler, ResponseHelper> callback)
    {
        return CreateRequestAndRetry(options, callback);
    }



    /// <summary>
    /// Creates Unity WebRequests Based on Data
    /// </summary>
    /// <param name="requestdata"></param>
    /// <returns></returns>
    private static UnityWebRequest CreateRequest(WebRequestHelper requestdata)
    {
        if (requestdata.FormData != null && requestdata.Method == "POST")
        {
            return UnityEngine.Networking.UnityWebRequest.Post(requestdata.Uri, requestdata.FormData);
        }

        return new UnityWebRequest(requestdata.Uri, requestdata.Method);
    }

    #region WEBREQUEST CONFIGS

    public static bool IsValidRequest(this UnityWebRequest request, WebRequestHelper options)
    {
        bool isNetworkError = request.isNetworkError;
        bool isHttpError = request.isHttpError;
        return request.isDone && !isNetworkError && (!isHttpError);
    }

    private static ExceptionHandler CreateException(WebRequestHelper options, UnityWebRequest request)
    {
        bool isNetworkError = request.isNetworkError;
        bool isHttpError = request.isHttpError;
        return new ExceptionHandler(options, request.error, isHttpError, isNetworkError, request.responseCode, options.ParseResponseBody ? request.downloadHandler.text : "body not parsed");
    }

    public static ResponseHelper CreateWebResponse(this UnityWebRequest request)
    {
        return new ResponseHelper(request);
    }


    public static AsyncOperation SendWebRequestWithOptions(this UnityWebRequest request, WebRequestHelper options)
    {
        byte[] bodyRaw = options.BodyRaw;
        string value = string.Empty;
        if (!options.Headers.TryGetValue("Content-Type", out value) && options.DefaultContentType)
        {
            value = "application/json";
        }

        if (options.Body != null || !string.IsNullOrEmpty(options.BodyString))
        {
            string text = options.BodyString;
            if (options.Body != null)
            {
                text = JsonUtility.ToJson(options.Body);
            }

            bodyRaw = Encoding.UTF8.GetBytes(text.ToCharArray());
        }
        else if (options.SimpleForm != null && options.SimpleForm.Count > 0)
        {
            bodyRaw = UnityEngine.Networking.UnityWebRequest.SerializeSimpleForm(options.SimpleForm);
            value = "application/x-www-form-urlencoded";
        }
        else if (options.FormSections != null && options.FormSections.Count > 0)
        {
            value = GetFormSectionsContentType(out bodyRaw, options);
        }
        else if (options.FormData != null)
        {
            value = string.Empty;
        }

        if (!string.IsNullOrEmpty(options.ContentType))
        {
            value = options.ContentType;
        }

        ConfigureWebRequestWithOptions(request, bodyRaw, value, options);
        return request.Send();
    }

    private static string GetFormSectionsContentType(out byte[] bodyRaw, WebRequestHelper options)
    {
        byte[] array = UnityEngine.Networking.UnityWebRequest.GenerateBoundary();
        byte[] array2 = UnityEngine.Networking.UnityWebRequest.SerializeFormSections(options.FormSections, array);
        byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + Encoding.UTF8.GetString(array) + "--");
        bodyRaw = new byte[array2.Length + bytes.Length];
        Buffer.BlockCopy(array2, 0, bodyRaw, 0, array2.Length);
        Buffer.BlockCopy(bytes, 0, bodyRaw, array2.Length, bytes.Length);
        return "multipart/form-data; boundary=" + Encoding.UTF8.GetString(array);
    }

    private static void ConfigureWebRequestWithOptions(UnityWebRequest request, byte[] bodyRaw, string contentType, WebRequestHelper options)
    {
        if (options.UploadHandler != null)
        {
            request.uploadHandler = options.UploadHandler;
        }

        if (bodyRaw != null)
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.uploadHandler.contentType = contentType;
        }

        if (options.DownloadHandler != null)
        {
            request.downloadHandler = options.DownloadHandler;
            options.ParseResponseBody = options.DownloadHandler is DownloadHandlerBuffer;
        }
        else
        {
            request.downloadHandler = new DownloadHandlerBuffer();
        }

        if (!string.IsNullOrEmpty(contentType))
        {
            request.SetRequestHeader("Content-Type", contentType);
        }


        foreach (KeyValuePair<string, string> header in options.Headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        if (options.Timeout.HasValue)
        {
            request.timeout = options.Timeout.Value;
        }


        if (options.RedirectLimit.HasValue)
        {
            request.redirectLimit = options.RedirectLimit.Value;
        }

        options.Request = request;
    }
    #endregion
}
