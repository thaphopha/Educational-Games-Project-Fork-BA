using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    [Header("Settings")]
    [SerializeField] float BoneFishStartTimer;
    [SerializeField] float BoneFishProgressTimer;
    [Header("Objects")]
    [SerializeField] GameObject BoneFish;
    [SerializeField] GameObject BoneFishBar;
    [SerializeField] GameObject Rose;
    [SerializeField] GameObject RoseBar;
    [Header("Data")]
    [SerializeField] int BoneFishBarProgress;
    [SerializeField] int RoseBarProgress;
    [SerializeField] Transform FishManager;
    [SerializeField] QuestionnaireEventManager questionnaireEventManager;
    [SerializeField] QuestionnaireManager questionnaireManager;
    [SerializeField] public Diver diver;

    private bool ending;

    void Start()
    {
        questionnaireEventManager = FindObjectOfType<QuestionnaireEventManager>();
        questionnaireManager = FindObjectOfType<QuestionnaireManager>();
        if (diver == null)
        {
            diver = FindObjectOfType<Diver>();
        }
        Invoke("StartBoneFish", BoneFishStartTimer);
        ending = false;
    }

    void StartBoneFish()
    {
        BoneFish.SetActive(true);
        BoneFishBar.SetActive(true);
        BoneFishProgressor();
    }

    private void Update()
    {
        if (ending)
        {
            questionnaireManager.WriteJsonToFile();
            CheckPlayerNotInNetForGoodEnding();
        }
    }

    void BoneFishProgressor()
    {
        StartCoroutine(MakeProgressBoneFishCoroutine());
        Invoke("BoneFishProgressor", BoneFishProgressTimer);
    }

    IEnumerator MakeProgressBoneFishCoroutine()
    {
        if (BoneFishBarProgress >= 21)
        {
            foreach (Transform child in FishManager)
            {
                if (child.TryGetComponent(out FishPlayerInput playerInput))
                {
                    if (!NarratorAudioManager.instance.growDeathBoolean)
                    {
                        NarratorAudioManager.instance.growDeathBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.growDeathClip);
                    }
                    child.GetComponent<FishEating>().FishDying();
                }
            }
        }
        else
        {
            BoneFishBarProgress++;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(0.025f);
                BoneFish.transform.position += new Vector3(4f, 0, 0);
                BoneFishBar.transform.localScale += new Vector3(0.04f, 0, 0);
            }
        }
    }

    public void MakeProgressRose(int amount)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                StartCoroutine(MakeProgressRoseCoroutine());
            }
        }
    }

    IEnumerator MakeProgressRoseCoroutine()
    {
        if (RoseBarProgress == 15)
        {
            diver.draw();
            questionnaireEventManager.TriggerQuestionEvent("1");
        }
        if (RoseBarProgress >= 22)
        {
            ending = true;
        }
        else
        {
            if (RoseBarProgress < 22)
            {
                RoseBarProgress++;
                for (int i = 0; i < 10; i++)
                {
                    yield return new WaitForSeconds(0.025f);
                    Rose.transform.position += new Vector3(4f, 0, 0);
                    RoseBar.transform.localScale += new Vector3(0.04f, 0, 0);
                }
            }
        }
    }

    void CheckPlayerNotInNetForGoodEnding()
    {
        if (!GameObject.Find("FishNetManager").GetComponent<FishnetManager>().playerInsideNet)
        {
            ending = false;
            RoseBarProgress = -10;
            BlackScreenManager.instance.MakeBlackInScreen();
            Invoke("LoadMateScene", 2f);
        }
    }

    void LoadMateScene()
    {
        if (!NarratorAudioManager.instance.mateEndingBoolean)
        {
            NarratorAudioManager.instance.mateEndingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.mateEndingClip, true);
        }

        SceneManager.LoadScene("GameOver_Mate");
    }

}
