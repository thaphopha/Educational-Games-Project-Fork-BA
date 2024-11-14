using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject BlackScreen;

    void Start()
    {
        Invoke("LoadMainMenuScene", 20f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenuScene();
        }
    }

    void LoadMainMenuScene()
    {
        StartCoroutine(LoadMainMenuSceneCoroutine());
    }

    IEnumerator LoadMainMenuSceneCoroutine()
    {
        Instantiate(BlackScreen, transform.position, Quaternion.identity, Canvas.transform);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
