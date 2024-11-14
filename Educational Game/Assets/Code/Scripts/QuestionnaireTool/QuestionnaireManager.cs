using UnityEngine;
using System;
using System.Collections.Generic;
using Utilities;
using AnswerTypes;
using System.IO;
using System.Linq;


public class QuestionnaireManager : MonoBehaviour
{
    [SerializeField] private List<QuestionSO> questions;
    [Header("Answer Behavior Prefabs")]
    [SerializeField] private GameObject emojiAnswerPrefab; // Assign EmojiAnswerBehavior prefab
    [SerializeField] private GameObject sliderAnswerPrefab;
    [Header("Answer Container Prefab")]
    private List<ResultsData> questionnaireResults = new List<ResultsData>();

    private Dictionary<AnswerType, Type> answerTypeMappings;

    [System.Serializable]
    public class ResultsData
    {
        public string questionID;
        public string questionText;
        public string answerID;
        public string answerType;
        public string answerOptions;
        public string selectedAnswer;
    }

    [System.Serializable]
    public class ResultsDataList
    {
        public List<ResultsData> answers;
    }

    private void Awake()
    {
        answerTypeMappings = new Dictionary<AnswerType, Type>
        {
            { AnswerType.Emoji, typeof(EmojiAnswerBehavior) },
            { AnswerType.Slider, typeof(SliderAnswerBehavior) }
        };
    }
    public void TriggerQuestion(string questionID)
    {
        QuestionSO questionData = questions.Find(q => q.questionID == questionID);

        if (questionData != null)
        {
            NarratorAudioManager.instance.PlayNarratorClip(questionData.narratorAudio, onClipFinishAction: () =>
            {
                InstantiateAnswer(questionData);
            });
        }
        else
        {
            Debug.LogWarning($"Question Data not found for question ID: {questionID}. Check that right question ID was provided.");
        }
    }

    // two cases, if GetAnswerPrefab() returns null, only need to set active, if not null, call instantiate and populate with questiondata
    private void InstantiateAnswer(QuestionSO questionData)
    {
        if (questionData.answerType == AnswerType.Emoji)
        {
            if (emojiAnswerPrefab == null)
            {
                //create empty container
                GameObject answerContainerInstance = new GameObject("EmojiAnswerContainer");
                answerContainerInstance.AddComponent<AnswerContainer>();
                //add answercontainer component
                AnswerContainer answerContainer = answerContainerInstance.GetComponent<AnswerContainer>();
                answerContainer.SetPosition();

                //for each answer option, instantiate prefab, answer id, response audio and populate with question data
                foreach (var answerOption in questionData.answerOptions)
                {
                    GameObject answerInstance = Instantiate(answerOption.answerPrefab, answerContainerInstance.transform);
                    // use setter from emojianswerbehavior to set answerid
                    answerInstance.GetComponent<EmojiAnswerBehavior>().QuestionID = questionData.questionID;
                    answerInstance.GetComponent<EmojiAnswerBehavior>().AnswerID = answerOption.answerID;
                    answerInstance.GetComponent<EmojiAnswerBehavior>().NarratorAudioClip = answerOption.responseAudio;
                    answerInstance.GetComponent<EmojiAnswerBehavior>().AnswersContainer = answerContainer;
                }
                answerContainer.SetPositionOfChildren();
            }
            else
            {
                emojiAnswerPrefab.SetActive(true);
                emojiAnswerPrefab.GetComponent<AnswerContainer>().SetPosition();
                emojiAnswerPrefab.GetComponent<AnswerContainer>().SetPositionOfChildren();
            }
        }
        else if (questionData.answerType == AnswerType.Slider)
        {
            if (sliderAnswerPrefab == null)
            {
                // UI hard to create with code, so better to create in advance
                Debug.Log("Please craete a slider prefab and assign it in the inspector");
            }
            else
            {
                sliderAnswerPrefab.GetComponent<SliderAnswerBehavior>().QuestionID = questionData.questionID;
                sliderAnswerPrefab.GetComponent<SliderAnswerBehavior>().AnswerID = questionData.answerOptions[0].answerID;
                sliderAnswerPrefab.GetComponent<SliderAnswerBehavior>().NarratorAudioClip = questionData.answerOptions[0].responseAudio;
                sliderAnswerPrefab.SetActive(true);
            }
        }
    }

    public void RecordAnswer(string questionID, string answerID)
    {
        QuestionSO questionData = questions.Find(q => q.questionID == questionID);
        if (questionData != null)
        {
            string answerType = questionData.answerType.ToString();
            string questionText = questionData.questionText;
            string answerOptions = string.Join(",", questionData.answerOptions.ConvertAll(option => option.answerID));
            string selectedAnswer = answerID;

            ResultsData ResultsData = new ResultsData
            {
                questionID = questionID,
                questionText = questionText,
                answerID = answerID,
                answerType = answerType,
                answerOptions = answerOptions,
                selectedAnswer = selectedAnswer,
            };

            questionnaireResults.Add(ResultsData);
        }
        else
        {
            Debug.LogWarning($"Question Data not found for question ID: {questionID}. Check that right question ID was provided.");
        }
    }
    public void WriteJsonToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"QuestionnaireResults_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json");
        Debug.Log($"Path to File: {filePath}");

        ResultsDataList resultsDataList = new ResultsDataList { answers = questionnaireResults };
        string resultsJson = JsonUtility.ToJson(resultsDataList, true);
        File.WriteAllText(filePath, resultsJson);
    }
}
