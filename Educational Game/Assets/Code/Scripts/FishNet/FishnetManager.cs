using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishnetManager : MonoBehaviour
{
    [SerializeField] float startFirstNetTime;
    [SerializeField] float netSurfaceDuration;
    [SerializeField] float netCoolDown;

    [Header("")]
    [SerializeField] GameObject fishnetLeftSidePrefab;
    [SerializeField] GameObject fishnetRightSidePrefab;
    [SerializeField] GameObject fishnetBackPrefab;

    [SerializeField] GameObject fishBoatPrefab;

    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject BlackScreen;
    [SerializeField] QuestionnaireManager questionnaireManager;

    [SerializeField] QuestionnaireEventManager questionnaireEventManager;
    GameObject fishnet;
    GameObject fishBoat;

    float newTimer;
    bool timerSynchronized;
    bool spawnNewNets;

    bool createdNet = false;
    bool createdBoat = false;
    bool netEnd = false;

    public bool playerInsideNet = false;
    bool playerWasInNet = false;
    bool oneCaughtClipPerNet = false;

    int netNumber;


    void Start()
    {
        ChooseRandomNetNumber();
        CheckForPlayerFishInNet();
        newTimer = 0;
        timerSynchronized = false;
        spawnNewNets = false;
    }

    void Update()
    {
        if (spawnNewNets)
        {
            NetTimeManaging();
            CheckForSuccesfulEscapeAudio();
        }
        else
        {
            StartNetSpawning();
        }

        //netze k�nnen erst nach dem ersten Essen und dann nach dem Timer erscheinen
        if (NarratorAudioManager.instance.eatClipBoolean)
        {
            newTimer += Time.deltaTime;
        }
    }

    void StartNetSpawning()
    {
        if (startFirstNetTime < newTimer)
        {
            spawnNewNets = true;
            newTimer = netCoolDown;
            float seconds = 0;
            if (!NarratorAudioManager.instance.fishNetBoolean)
            {
                if (netNumber == 0 || netNumber == 1)
                {
                    seconds = 15f;
                }

                questionnaireEventManager.TriggerQuestionEvent("0");
                //StartCoroutine(DelayedPlayNarratorClip(seconds));
            }
        }
    }

    IEnumerator DelayedPlayNarratorClip(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NarratorAudioManager.instance.fishNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.fishNetClip, netRelated: true);
    }

    void ChooseRandomNetNumber()
    {
        //0 left side
        //1 right side
        //2 back
        netNumber = Random.Range(0, 3);
    }

    void NetTimeManaging()
    {
        if (!createdBoat && newTimer > netCoolDown)
        {
            createdBoat = true;
            StartCoroutine(CreateBoat());
        }

        if (createdNet)
        {
            CheckForCaughtPlayerAudio();
            if (fishnet.transform.position.y > 8.5)
            {
                if (!playerWasInNet && playerInsideNet)
                {
                    playerWasInNet = true;
                }

                if (!timerSynchronized)
                {
                    newTimer = 0;
                    timerSynchronized = true;
                }
            }
            if (timerSynchronized && newTimer > netSurfaceDuration)
            {
                PullNetOut();
                FishboatLeaving();
            }
        }
    }

    IEnumerator CreateBoat()
    {
        float maxTravelDistance = Random.Range(40f, 69f);

        //boat only spawns on side nets
        if (netNumber != 2)
        {
            fishBoat = Instantiate(fishBoatPrefab, new Vector3(-100, 21.5f, 0), Quaternion.identity, transform);
            FishingBoat fishingBoat = fishBoat.GetComponent<FishingBoat>();
            fishingBoat.maxTravelDistance = maxTravelDistance;

            if (netNumber == 1)
            {
                fishBoat.transform.position = new Vector3(-60, 21.5f, 0);
                fishingBoat.leftSideFactor = 1;
                fishBoat.transform.localScale = new Vector3(-fishBoat.transform.localScale.x, fishBoat.transform.localScale.y, fishBoat.transform.localScale.z);
            }
            else
            {
                //netnumber 0
                fishBoat.transform.position = new Vector3(60, 21.5f, 0);
                fishingBoat.leftSideFactor = -1;
            }

            yield return new WaitForSeconds(12f);
        }

        createdNet = true;
        CreateNet(maxTravelDistance);
    }

    void CreateNet(float maxTravelDistance)
    {
        switch (netNumber)
        {
            case 0:
                fishnet = Instantiate(fishnetLeftSidePrefab, new Vector3(60, Random.Range(-21f, -6f), 0), Quaternion.identity, transform);
                break;
            case 1:
                fishnet = Instantiate(fishnetRightSidePrefab, new Vector3(-60, Random.Range(-21f, -6f), 0), Quaternion.Euler(0, 0, 180), transform);
                break;
            case 2:
                fishnet = Instantiate(fishnetBackPrefab, new Vector3(Random.Range(-21f, 21f), Random.Range(-14f, -6f), 0), Quaternion.Euler(0, 0, -90), transform);
                break;
        }

        //same travel distance for side nets
        if (fishnet.TryGetComponent(out FishnetSide fishNetSide))
        {
            fishNetSide.maxTravelDistance = maxTravelDistance;
        }

    }

    void PullNetOut()
    {
        if (fishnet.transform.position.y < 30)
        {
            fishnet.transform.Translate(0, 2 * Time.deltaTime, 0, Space.World);

            if (!NarratorAudioManager.instance.pullOutBoolean && playerInsideNet)
            {
                NarratorAudioManager.instance.pullOutBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.pullOutClip, netRelated: true);
            }
        }
        else if (!netEnd)
        {
            netEnd = true;

            if (playerInsideNet)
            {
                StartCoroutine(LoadGameOverCatechedScene());
            }
            Invoke("ResetAll", 3f);
        }
    }

    IEnumerator LoadGameOverCatechedScene()
    {
        if (!NarratorAudioManager.instance.catchedEndingBoolean)
        {
            NarratorAudioManager.instance.catchedEndingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.catchedEndingClip, true, netRelated: true);
        }

        Instantiate(BlackScreen, Canvas.transform.position, Quaternion.identity, Canvas.transform);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver_Catched");
    }

    void FishboatLeaving()
    {
        if (netNumber == 0)
        {
            fishBoat.transform.Translate(-5f * Time.deltaTime, 0, 0);
        }
        else if (netNumber == 1)
        {
            fishBoat.transform.Translate(5f * Time.deltaTime, 0, 0);
        }
    }

    void ResetAll()
    {
        fishnet.transform.GetComponent<Fishnet>().DestroyAllFish();
        if (spawnNewNets)
        {
            Destroy(fishBoat);
        }
        Destroy(fishnet);
        ChooseRandomNetNumber();

        createdNet = false;
        createdBoat = false;

        newTimer = 0;
        timerSynchronized = false;

        netEnd = false;
        playerWasInNet = false;
        oneCaughtClipPerNet = false;
    }

    public void DisableNewNetsForEasterEgg()
    {
        spawnNewNets = false;
        newTimer = -1000;
    }

    public GameObject GetFishBoat()
    {
        return fishBoat;
    }

    public GameObject GetFishnet()
    {
        return fishnet;
    }

    void CheckForPlayerFishInNet()
    {
        if (fishnet != null)
        {
            playerInsideNet = fishnet.transform.GetChild(0).GetComponent<Fishnet>().TryGetPlayerFish();
            if (FindFirstObjectByType<FishPlayerInput>() != null)
            {
                Transform playerTransform = FindFirstObjectByType<FishPlayerInput>().transform;
                //pr�ft ob sich der spieler oberhalb des netzes befindet z.B bei einem Sprung
                if (playerTransform.position.y > 15.5 && playerTransform.position.x < fishnet.transform.position.x + 16 && playerTransform.position.x > fishnet.transform.position.x - 16)
                {
                    playerInsideNet = true;
                }
            }
        }
        Invoke("CheckForPlayerFishInNet", 2f);
    }

    void CheckForSuccesfulEscapeAudio()
    {
        if (!NarratorAudioManager.instance.escapedNetBoolean && !playerInsideNet && playerWasInNet)
        {
            NarratorAudioManager.instance.escapedNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.escapedNetClip, netRelated: true);
        }
    }

    void CheckForCaughtPlayerAudio()
    {
        if (!oneCaughtClipPerNet && playerInsideNet)
        {
            if (!NarratorAudioManager.instance.caughtFirstBoolean)
            {
                NarratorAudioManager.instance.caughtFirstBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.caughtFirstClip, netRelated: true);
            }
            else if (!NarratorAudioManager.instance.caughtSecoundBoolean)
            {
                NarratorAudioManager.instance.caughtSecoundBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.caughtSecoundClip, netRelated: true);
            }
            else if (!NarratorAudioManager.instance.inNetBoolean)
            {
                NarratorAudioManager.instance.inNetBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.inNetClip, netRelated: true);
            }
            oneCaughtClipPerNet = true;
        }
    }
}
