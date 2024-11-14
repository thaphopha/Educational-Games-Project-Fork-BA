using System.Collections.Generic;
using UnityEngine;
using AnswerTypes;

[System.Serializable]
public class AnswerOption
{
    public string answerID;
    public GameObject answerPrefab;
    public AudioClip responseAudio;
}

[CreateAssetMenu(fileName = "QuestionSO", menuName = "Question", order = 0)]
public class QuestionSO : ScriptableObject
{
    public string questionID;
    public string questionText;
    public AudioClip narratorAudio;
    public AnswerType answerType;
    public List<AnswerOption> answerOptions = new List<AnswerOption>();
}
