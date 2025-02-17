
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WebRequestHelper
{
    private string _uri;

    private string _method;

    private object _body;

    private string _bodyString;

    private byte[] _bodyRaw;

    private int? _timeout;

    private string _contentType;

    private int _retries;

    private float _retrySecondsDelay;

    private bool _retryCallbackOnlyOnNetworkErrors = true;

    private Action<ExceptionHandler, int> _retryCallback;

    private Action<float> _progressCallback;


    private int? _redirectLimit;

    private bool _ignoreHttpException;

    private WWWForm _formData;

    private Dictionary<string, string> _simpleForm;

    private List<IMultipartFormSection> _formSections;

    private UploadHandler _uploadHandler;

    private DownloadHandler _downloadHandler;

    private Dictionary<string, string> _headers;

    private Dictionary<string, string> _params;

    private bool _parseResponseBody = true;

    private bool _isAborted;

    private bool _defaultContentType = true;

    public string Uri
    {
        get
        {
            return _uri;
        }
        set
        {
            _uri = value;
        }
    }

    public string Method
    {
        get
        {
            return _method;
        }
        set
        {
            _method = value;
        }
    }

    public object Body
    {
        get
        {
            return _body;
        }
        set
        {
            _body = value;
        }
    }

    public string BodyString
    {
        get
        {
            return _bodyString;
        }
        set
        {
            _bodyString = value;
        }
    }

    public byte[] BodyRaw
    {
        get
        {
            return _bodyRaw;
        }
        set
        {
            _bodyRaw = value;
        }
    }

    public int? Timeout
    {
        get
        {
            return _timeout;
        }
        set
        {
            _timeout = value;
        }
    }

    public string ContentType
    {
        get
        {
            return _contentType;
        }
        set
        {
            _contentType = value;
        }
    }

    public int Retries
    {
        get
        {
            return _retries;
        }
        set
        {
            _retries = value;
        }
    }

    public float RetrySecondsDelay
    {
        get
        {
            return _retrySecondsDelay;
        }
        set
        {
            _retrySecondsDelay = value;
        }
    }

    public bool RetryCallbackOnlyOnNetworkErrors
    {
        get
        {
            return _retryCallbackOnlyOnNetworkErrors;
        }
        set
        {
            _retryCallbackOnlyOnNetworkErrors = value;
        }
    }

    public Action<ExceptionHandler, int> RetryCallback
    {
        get
        {
            return _retryCallback;
        }
        set
        {
            _retryCallback = value;
        }
    }

    public Action<float> ProgressCallback
    {
        get
        {
            return _progressCallback;
        }
        set
        {
            _progressCallback = value;
        }
    }


    public int? RedirectLimit
    {
        get
        {
            return _redirectLimit;
        }
        set
        {
            _redirectLimit = value;
        }
    }

    public bool IgnoreHttpException
    {
        get
        {
            return _ignoreHttpException;
        }
        set
        {
            _ignoreHttpException = value;
        }
    }

    public WWWForm FormData
    {
        get
        {
            return _formData;
        }
        set
        {
            _formData = value;
        }
    }

    public Dictionary<string, string> SimpleForm
    {
        get
        {
            return _simpleForm;
        }
        set
        {
            _simpleForm = value;
        }
    }

    public List<IMultipartFormSection> FormSections
    {
        get
        {
            return _formSections;
        }
        set
        {
            _formSections = value;
        }
    }

    public UploadHandler UploadHandler
    {
        get
        {
            return _uploadHandler;
        }
        set
        {
            _uploadHandler = value;
        }
    }

    public DownloadHandler DownloadHandler
    {
        get
        {
            return _downloadHandler;
        }
        set
        {
            _downloadHandler = value;
        }
    }

    public Dictionary<string, string> Headers
    {
        get
        {
            if (_headers == null)
            {
                _headers = new Dictionary<string, string>();
            }

            return _headers;
        }
        set
        {
            _headers = value;
        }
    }

    public Dictionary<string, string> Params
    {
        get
        {
            if (_params == null)
            {
                _params = new Dictionary<string, string>();
            }

            return _params;
        }
        set
        {
            _params = value;
        }
    }

    public bool ParseResponseBody
    {
        get
        {
            return _parseResponseBody;
        }
        set
        {
            _parseResponseBody = value;
        }
    }

    public UnityWebRequest Request { private get; set; }

    public float UploadProgress
    {
        get
        {
            float result = 0f;
            if (Request != null)
            {
                result = Request.uploadProgress;
            }

            return result;
        }
    }

    public ulong UploadedBytes
    {
        get
        {
            ulong result = 0uL;
            if (Request != null)
            {
                result = Request.uploadedBytes;
            }

            return result;
        }
    }

    public float DownloadProgress
    {
        get
        {
            float result = 0f;
            if (Request != null)
            {
                result = Request.downloadProgress;
            }

            return result;
        }
    }

    public ulong DownloadedBytes
    {
        get
        {
            ulong result = 0uL;
            if (Request != null)
            {
                result = Request.downloadedBytes;
            }

            return result;
        }
    }

    public bool IsAborted
    {
        get
        {
            return _isAborted;
        }
        set
        {
            _isAborted = value;
        }
    }

    public bool DefaultContentType
    {
        get
        {
            return _defaultContentType;
        }
        set
        {
            _defaultContentType = value;
        }
    }

    public string GetHeader(string name)
    {
        if (Request != null)
        {
            return Request.GetRequestHeader(name);
        }

        Headers.TryGetValue(name, out var value);
        return value;
    }

    public void Abort()
    {
        if (IsAborted || Request == null)
        {
            return;
        }

        try
        {
            IsAborted = true;
            if (!Request.isDone)
            {
                Request.Abort();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            Request = null;
        }
    }


}
