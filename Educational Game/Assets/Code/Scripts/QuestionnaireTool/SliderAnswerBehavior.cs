using UnityEngine;
using UnityEngine.UI;

public class SliderAnswerBehavior : MonoBehaviour
{
    [SerializeField] private QuestionnaireManager questionnaireManager;
    [SerializeField] private string questionID;
    [SerializeField] private string answerID;
    [SerializeField] private AudioClip narratorAudioClip;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider sliderInstance;

    public string QuestionID
    {
        get { return questionID; }
        set { questionID = value; }
    }
    public string AnswerID
    {
        get { return answerID; }
        set { answerID = value; }
    }

    public AudioClip NarratorAudioClip
    {
        get { return narratorAudioClip; }
        set { narratorAudioClip = value; }
    }

    private void Awake()
    {
        if (questionnaireManager == null)
        {
            questionnaireManager = FindObjectOfType<QuestionnaireManager>();
            if (questionnaireManager == null)
            {
                Debug.LogError("QuestionnaireManager not found in the scene.");
            }
        }
    }

    public void OnConfirmButtonClick()
    {
        ConfirmAnswer();
    }
    public void ConfirmAnswer()
    {
        float sliderValue = sliderInstance.value;
        string sliderValueString = sliderValue.ToString("F2"); // Convert float to string with 2 decimal places

        questionnaireManager.RecordAnswer(questionID, sliderValueString);

        NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioClip);
        RemoveAnswer();
    }

    public void RemoveAnswer(float delay = 0f)
    {
        gameObject.SetActive(false);
    }
}
