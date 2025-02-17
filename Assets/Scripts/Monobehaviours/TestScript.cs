using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{



    public TextMeshProUGUI m_result_text;
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
    public string m_url;


    public CreateChallengeDto Challenge;

    ChallengeSDK _sdk;
    string jwtToken = "your_jwt_token_here";

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

        ////ChallengeSDK _sdk = new ChallengeSDK(m_url);
        ////string jwtToken = "your_jwt_token_here";

        ////SubmitChallegeData();
        //GetChallengeData();
    }

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



    public async void SubmitChallegeData()
    {
        GetDataFromInput();
        _sdk = new ChallengeSDK(m_url);
        await _sdk.CreateChallenge(Challenge, jwtToken);
    }



    public async void GetChallengeData()
    {
        _sdk = new ChallengeSDK(m_url);
        await _sdk.GetChallengeData(jwtToken);
    }

    void GetDataFromInput()
    {
        Challenge = new CreateChallengeDto();
        Challenge.ChallengeName = ChallengeName.text;
        Challenge.ChallengeDescription = ChallengeDescription.text;




        Challenge.StartDate = DateTime.Now.ToUniversalTime().Subtract(new DateTime(int.Parse(Yeartart.captionText.text), int.Parse(MonthStart.captionText.text), int.Parse(DayStart.captionText.text), 10, 1, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        Challenge.EndDate = DateTime.Now.ToUniversalTime().Subtract(new DateTime(int.Parse(YearEnd.captionText.text), int.Parse(MonthEnd.captionText.text), int.Parse(DayEnd.captionText.text), 10, 1, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        if (GameID.text == "")
        {
            GameID.text = "0";
        }
        Challenge.GameID = int.Parse(GameID.text);
        if (Wager.text == "")
        {
            Wager.text = "0";
        }
        Challenge.Wager = int.Parse(Wager.text);
        //Challenge.Target = int.Parse(Target.text);
        Challenge.Currency = (VERIFIED_CURRENCY)(Currency.value);
        Challenge.ChallengeCategory = (CHALLENGE_CATEGORIES)ChallengeCategory.value;


        Debug.Log(JsonUtility.ToJson(Challenge));


    }
}
