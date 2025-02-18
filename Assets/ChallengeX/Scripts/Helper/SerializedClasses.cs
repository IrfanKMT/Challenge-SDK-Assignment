#region SERIALIZED CLASS

using System;

[System.Serializable]
public class CreateChallengeDto
{
    public string ChallengeName;
    public string ChallengeDescription;
    public int StartDate;
    public int EndDate;
    public int GameID;
    public int MaxParticipants;
    public int Wager;
    public int Target;
    public int ChallengeCreator;
    public bool AllowSideBets;
    public int SideBetsWager;
    public string Unit;
    public bool IsPrivate;
    public string Currency;
    public string ChallengeCategory;
    public string NFTMedia;
    public string Media;
    public int ActualStartDate;
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