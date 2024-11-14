using UnityEngine;

public class FishEating : MonoBehaviour
{
    [SerializeField] Collider2D ignoreCollider;
    [SerializeField] private GameObject eatArrow;

    [Header("Eating")]
    public int foodCount;
    public bool canEat;
    [SerializeField] float EatCooldown;

    [Header("Hunger")]
    [SerializeField] int hunger;
    [SerializeField] float hungerTimerPlayer;
    [SerializeField] float hungerTimerAI;


    private void Awake()
    {
        canEat = true;
        if (TryGetComponent(out FishPlayerInput playerInput))
        {
            Invoke("ReduceHunger", hungerTimerPlayer);
        }
        else if (TryGetComponent(out FishAiInput fishAIInput))
        {
            Invoke("ReduceHunger", hungerTimerAI);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CircleCollider2D circleCollider2D = collision as CircleCollider2D;
        if (circleCollider2D != null)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out Eatable eatable))
        {
            if (eatable != GetComponent<Eatable>())
            {
                Eat(eatable);
            }
        }
    }

    void Eat(Eatable eatable)
    {
        // Restrictions
        if (!canEat)
        {
            return;
        }
        if (foodCount < eatable.requiredEatFactor)
        {
            return;
        }

        // Eat
        canEat = false;

        foodCount += eatable.foodValue;
        IncreaseHunger(eatable.foodValue);

        Destroy(eatable.gameObject);
        Invoke("ResetEat", EatCooldown);

        FishGrow.instance.Grow(this.gameObject);


        // If Player is Eaten give Player Controll over the Fish
        if (eatable.TryGetComponent(out FishPlayerInput playerInput))
        {
            gameObject.AddComponent<FishPlayerInput>();
            gameObject.GetComponent<FishPlayerInput>().enabled = false;
            if (!NarratorAudioManager.instance.eatenClipBoolean)
            {
                NarratorAudioManager.instance.eatenClipBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.eatenClip);
				Invoke("GetNewPlayerFish", 9f);
            }
            else
            {
                Invoke("GetNewPlayerFish", 3f);
            }
        }

        // Eat Audio
        if (TryGetComponent(out FishAudio fishAudio))
        {
            fishAudio.PlayEatAudio();
        }

        // Player Specifics
        if (TryGetComponent(out FishPlayerInput player))
        {
            // Narrator
            if (!NarratorAudioManager.instance.eatClipBoolean)
            {
                NarratorAudioManager.instance.eatClipBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.eatClip);
			}

            // ProgressBar
            if (eatable.foodValue >= 2)
            {
                ProgressBarManager.instance.MakeProgressRose(2);
            }
            else
            {
                ProgressBarManager.instance.MakeProgressRose(1);
            }
        }
    }

    void ResetEat()
    {
        canEat = true;
    }

    void IncreaseHunger(int factor)
    {
        hunger += factor * 2;
        if (hunger > 10)
        {
            hunger = 10;
        }
        else if (hunger < 0)
        {
            hunger = 0;
        }

        // Player Specifics
        if (TryGetComponent(out FishPlayerInput playerInput))
        {
            SetEmotes();
            AvatarFrameManager.instance.UpdateHungerBar(hunger);
        }
    }

    void ReduceHunger()
    {
        hunger--;

        // Starving 
        if (hunger < 1)
        {
            hunger = 0;
            if (TryGetComponent(out FishPlayerInput Input))
            {
                SetEmotes();
                // Narrator
                if (!NarratorAudioManager.instance.hungerDeathBoolean)
                {
                    NarratorAudioManager.instance.hungerDeathBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.hungerDeathClip);
				}
            }
            FishDying();
        }

        // Player Specifics
        if (TryGetComponent(out FishPlayerInput playerInput))
        {
            SetEmotes();
            AvatarFrameManager.instance.UpdateHungerBar(hunger);
            Invoke("ReduceHunger", hungerTimerPlayer);
        }
        // Ai Specifics
        else if (TryGetComponent(out FishAiInput fishAIInput))
        {
            Invoke("ReduceHunger", hungerTimerAI);
        }

    }

    public void FishDying()
    {
        bool player = false;

        // Player Specifics
        if (TryGetComponent<FishPlayerInput>(out FishPlayerInput playerInput))
        {
            Destroy(GetComponent<FishPlayerInput>());
            player = true;

            // Narrator
            if (!NarratorAudioManager.instance.skelletFishBoolean)
            {
                NarratorAudioManager.instance.skelletFishBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.skelletFishClip);
			}
        }
        // Ai Specifics
        else if (TryGetComponent<FishAiInput>(out FishAiInput aiInput))
        {
            Destroy(GetComponent<FishAiInput>());
        }

        // Component Managing
        Destroy(GetComponent<FishMovement>());
        Destroy(GetComponent<FishAudio>());
        Destroy(GetComponent<FishGravity>());
        Destroy(GetComponent<CircleCollider2D>());

        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        FishDead fishDead = gameObject.AddComponent<FishDead>();
        fishDead.player = player;

        Destroy(this);
    }

    void SetEmotes()
    {
        if (hunger > 6)
        {
            EmoteManager.instance.SetHappynessStatus(0);
            EmoteManager.instance.SetHungryStatus(0);
        }
        else if (hunger > 4)
        {
            EmoteManager.instance.SetHappynessStatus(0);
            EmoteManager.instance.SetHungryStatus(1);
        }
        else if (hunger > 0)
        {
            EmoteManager.instance.SetHappynessStatus(1);
            EmoteManager.instance.SetHungryStatus(2);
        }
        else
        {
            EmoteManager.instance.SetHappynessStatus(4);
        }
    }

    void GetNewPlayerFish()
    {
        Instantiate(eatArrow, transform);
        Destroy(gameObject.GetComponent<FishAiInput>());
        GetComponent<FishPlayerInput>().enabled = true;

        // Cameras
        gameObject.layer = 3;
        CameraScript.instance.target = this.transform;
        AvatarCameraScript.instance.target = this.transform;

        CameraScript.instance.ChangeZoom(gameObject.transform.localScale.x);
        AvatarCameraScript.instance.ChangeZoom(gameObject.transform.localScale.x);
    }
}
