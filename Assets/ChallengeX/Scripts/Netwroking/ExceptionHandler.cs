
using System;
public class ExceptionHandler : Exception
{
    private WebRequestHelper _request;

    private bool _isHttpError;

    private bool _isNetworkError;

    private long _statusCode;

    private string _serverMessage;

    private string _response;


    public bool IsHttpError
    {
        get
        {
            return _isHttpError;
        }
        private set
        {
            _isHttpError = value;
        }
    }

    public bool IsNetworkError
    {
        get
        {
            return _isNetworkError;
        }
        private set
        {
            _isNetworkError = value;
        }
    }

    public long StatusCode
    {
        get
        {
            return _statusCode;
        }
        private set
        {
            _statusCode = value;
        }
    }

    public string ServerMessage
    {
        get
        {
            return _serverMessage;
        }
        set
        {
            _serverMessage = value;
        }
    }

    public string Response
    {
        get
        {
            return _response;
        }
        set
        {
            _response = value;
        }
    }

    public ExceptionHandler(WebRequestHelper request, string message, bool isHttpError, bool isNetworkError, long statusCode, string response)
    : base(message)
    {
        _request = request;
        _isHttpError = isHttpError;
        _isNetworkError = isNetworkError;
        _statusCode = statusCode;
        _response = response;
    }

    public ExceptionHandler(string message, bool isHttpError, bool isNetworkError, long statusCode, string response)
        : base(message)
    {
        _isHttpError = isHttpError;
        _isNetworkError = isNetworkError;
        _statusCode = statusCode;
        _response = response;
    }
}
