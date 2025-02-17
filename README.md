# Challenge-SDK-Assignment


![RESET API HELPER](https://github.com/IrfanKMT/Challenge-SDK-Assignment/blob/main/Images/URL.png)
![](https://github.com/IrfanKMT/Challenge-SDK-Assignment/blob/main/Images/Menu.png)


- Please add URL for POST and GET API

# Setup
- Clone this repository
- Open the Unity project in Unity Editor.
- Update the post_url & get_url  variable in the code to point to your server endpoint.

## Error Handling

- Network Errors: Captured by checking if error is not null.

- Invalid Responses: Handled by checking response.statusCode within the callback.

- Authentication Errors: Ensure that a valid jwtToken is passed during the request.

## Important Methods
 - ChallengeSDK Class  CreateChallenge
The default methods **(GET, POST, PUT, DELETE, HEAD)** are:
```csharp
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
            callback(error, response);
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

```
csharp



## Features 🎮
- Make **HTTP** requests from Unity
- Supports **HTTPS/SSL**
- Built on top of **[UnityWebRequest](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html)** system
- Transform request and response data (**JSON** serialization with **[JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html)** or other tools)
- Supports default **HTTP** Methods **(GET, POST, PUT, DELETE, HEAD, PATCH)**
- Generic **REQUEST** method to create any http request
- Ability to work during scene transition
- Handle HTTP exceptions and retry requests easily


## Supported platforms 📱 🖥 
The [UnityWebRequest](https://docs.unity3d.com/Manual/UnityWebRequest.html) system supports most Unity platforms:

* All versions of the Editor and Standalone players
* WebGL
* Mobile platforms: iOS, Android
* Universal Windows Platform
