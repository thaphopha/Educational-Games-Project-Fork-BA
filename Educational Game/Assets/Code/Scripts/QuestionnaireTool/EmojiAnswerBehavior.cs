using UnityEngine;
using Utilities;
public class EmojiAnswerBehavior : MonoBehaviour
{
    [SerializeField] private QuestionnaireManager questionnaireManager;
    [SerializeField] private string questionID;
    [SerializeField] private string answerID;
    [SerializeField] private AudioClip narratorAudioClip;
    [SerializeField] private AnswerContainer answersContainer;
    [SerializeField] private float spacing = 5.0f;

    public string QuestionID
    {
        get { return questionID; }
        set { questionID = value; }
    }
    // getter and setter for answerID
    public string AnswerID
    {
        get { return answerID; }
        set { answerID = value; }
    }

    // getter and setter for narratorAudioClip
    public AudioClip NarratorAudioClip
    {
        get { return narratorAudioClip; }
        set { narratorAudioClip = value; }
    }

    // getter and setter for answersContainer
    public AnswerContainer AnswersContainer
    {
        get { return answersContainer; }
        set { answersContainer = value; }
    }

    private void Awake()
    {
        if (questionnaireManager == null)
        {
            questionnaireManager = FindObjectOfType<QuestionnaireManager>();
            if (questionnaireManager == null)
            {
                Debug.LogError("QuestionnaireManager cannot be found in the scene.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NarratorAudioManager.instance.PlayNarratorClip(narratorAudioClip);
            questionnaireManager.RecordAnswer(questionID, answerID);
            answersContainer.RemoveAnswers();
            Destroy(gameObject);
        }
    }
}
