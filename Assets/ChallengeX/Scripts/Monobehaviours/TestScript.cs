using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    #region VARIABLES
    [Header("UI PART")]
    public GameObject ResultPanel;
    public TextMeshProUGUI ResultText;
    [Space]
    [Header("Input Fields")]
    public TMP_InputField ChallengeName;
    public TMP_InputField ChallengeDescription;
    public TMP_InputField GameID;
    //public TMP_InputField MaxParticipants;
    public TMP_InputField Wager;
    public TMP_InputField Target;
    //public TMP_Dropdown ChallengeCreator;
    public TMP_Dropdown AllowSideBets;
    public TMP_Dropdown IsPrivate;

    public TMP_Dropdown Currency;
    public TMP_Dropdown ChallengeCategory;

    public TMP_Dropdown DayStart;
    public TMP_Dropdown MonthStart;
    public TMP_Dropdown Yeartart;

    public TMP_Dropdown DayEnd;
    public TMP_Dropdown MonthEnd;
    public TMP_Dropdown YearEnd;
    [Space]
    public VERIFIED_CURRENCY verified_curruncy;
    public CHALLENGE_CATEGORIES chellenge_categories;

    [Space]
    [Header("URL")]
    public string m_url;

    [Header("USER CHALLENGE DATA")]
    public CreateChallengeDto Challenge;


    string jwtToken = "your_jwt_token_here";

    #endregion

    #region UNITYMETHDOS
    private async void Start()
    {
        PopulateDropDownWithEnum(ChallengeCategory, chellenge_categories);
        PopulateDropDownWithEnum(Currency, verified_curruncy);


        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 1; i < 32; i++)
        {

            newOptions.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        DayStart.ClearOptions();//Clear old options
        DayStart.AddOptions(newOptions);//Add new options

        DayEnd.ClearOptions();//Clear old options
        DayEnd.AddOptions(newOptions);//Add new options

        newOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 1; i < 13; i++)
        {
            newOptions.Add(new TMP_Dropdown.OptionData(i.ToString()));

        }

        MonthStart.ClearOptions();//Clear old options
        MonthStart.AddOptions(newOptions);//Add new options

        MonthEnd.ClearOptions();//Clear old options
        MonthEnd.AddOptions(newOptions);//Add new options


        newOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 1990; i < 2026; i++)
        {
            newOptions.Add(new TMP_Dropdown.OptionData(i.ToString()));

        }

        Yeartart.ClearOptions();//Clear old options
        Yeartart.AddOptions(newOptions);//Add new options

        YearEnd.ClearOptions();//Clear old options
        YearEnd.AddOptions(newOptions);//Add new options
    }

    #endregion

    #region INPUT FIELDS

    public static void PopulateDropDownWithEnum(TMP_Dropdown dropdown, Enum targetEnum)//You can populate any dropdown with any enum with this method
    {
        Type enumType = targetEnum.GetType();//Type of enum(FormatPresetType in my example)
        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < Enum.GetNames(enumType).Length; i++)//Populate new Options
        {
            newOptions.Add(new TMP_Dropdown.OptionData(Enum.GetName(enumType, i)));
        }

        dropdown.ClearOptions();//Clear old options
        dropdown.AddOptions(newOptions);//Add new options
    }

    void GetDataFromInput()
    {
        Challenge = new CreateChallengeDto();
        Challenge.ChallengeName = ChallengeName.text;
        Challenge.ChallengeDescription = ChallengeDescription.text;

        Challenge.StartDate = DateTime.Now.ToUniversalTime().Subtract(new DateTime(int.Parse(Yeartart.captionText.text), int.Parse(MonthStart.captionText.text), int.Parse(DayStart.captionText.text), 10, 1, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        Challenge.EndDate = DateTime.Now.ToUniversalTime().Subtract(new DateTime(int.Parse(YearEnd.captionText.text), int.Parse(MonthEnd.captionText.text), int.Parse(DayEnd.captionText.text), 10, 1, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        if (!string.IsNullOrEmpty(GameID.text))
        {
            Challenge.GameID = int.Parse(GameID.text);
        }

        if (!string.IsNullOrEmpty(GameID.text))
        {
            Challenge.Wager = int.Parse(Wager.text);
        }


        //Challenge.Target = int.Parse(Target.text);
        Challenge.Currency = (VERIFIED_CURRENCY)(Currency.value);
        Challenge.ChallengeCategory = (CHALLENGE_CATEGORIES)ChallengeCategory.value;
        Debug.Log(JsonUtility.ToJson(Challenge));
    }
    #endregion

    #region Button calls
    public async void SubmitChallegeData()
    {
        GetDataFromInput();
        ChallengeSDK SKD = new ChallengeSDK(m_url);

        await SKD.CreateChallenge(Challenge, jwtToken, (error, response) =>
        {
            ResultPanel.SetActive(true);

            if (error != null)
            {
                //FAILED
                Debug.LogError($"Request failed: {error.Message}");
                ResultText.text = error.Message;
            }
            else
            {
                //SUCCESS
                Debug.Log($"Request succeeded: {response.Text}");
                ResultText.text = response.Text;

            }
        });
    }

    public async void GetChallengeData()
    {
        ChallengeSDK SKD = new ChallengeSDK(m_url);


        await SKD.GetChallengeData(jwtToken, (error, response) =>
        {

            ResultPanel.SetActive(true);

            if (error != null)
            {
                //FAILED
                Debug.LogError($"Request failed: {error.Message}");
                ResultText.text = error.Message;
            }
            else
            {
                //SUCCESS
                Debug.Log($"Request succeeded: {response.Text}");
                ResultText.text = response.Text;

            }
        });
    }

    #endregion
}
