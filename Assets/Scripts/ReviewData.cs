using System.Collections.Generic;


[System.Serializable]
public class DateReview
{
    public string char1Name;
    public string char2Name;
    public float char1Stars;
    public float char2Stars;
    public string char1Comment;
    public string char2Comment;
    public bool isSuccess;
}


[System.Serializable]
public class ReviewDatabase
{
    public List<DateReview> allPastDates = new List<DateReview>();
}