using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenu : MonoBehaviour
{
    public GameObject Menu;

    private void Start()
    {
        //Menu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Show");
            Time.timeScale = (Time.timeScale + 1) % 2;
            Menu.gameObject.SetActive(!Menu.gameObject.activeSelf);
        }
    }

    public void Disconnect()
    {
        
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Launcher");
        //Menu.gameObject.SetActive(!Menu.gameObject.activeSelf);




        Debug.Log("Exiting");
        Resources.ReCreate();
        //PhotonNetwork.Disconnect();
    }
}
