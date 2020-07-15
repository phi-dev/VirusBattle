using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject namePanel;
    public GameObject messagePanel;
    public TextMeshProUGUI messagePanelTitle;
    public TextMeshProUGUI messagePanelDetails;

    // Start is called before the first frame update
    void Start()
    {
        /* se non è stato preimpostato un nome in precedenza */
        if (!PlayerPrefs.HasKey("username"))
        {
            /* se è connesso a internet, allora mostra il menu per impostare il nome */
            if(Application.internetReachability != NetworkReachability.NotReachable)
                this.showNamePanel();
        } else
        {
            if (PlayerPrefs.GetInt("hasPlayed") == 0)
            {
                this.showMessagepanelPanel("Welcome back " + PlayerPrefs.GetString("username"), "Login successful");
                PlayerPrefs.SetInt("hasPlayed", 1);
            }
        }

        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 50);
            PlayerPrefs.SetFloat("effectsVolume", 50);
        }
    }

    private void showNamePanel()
    {
        this.namePanel.SetActive(true);
    }

    private void showMessagepanelPanel(string title,string message)
    {
        messagePanel.SetActive(true);
        messagePanelTitle.text = title;
        messagePanelDetails.text = message;
        StartCoroutine(closeMessagePanel());
    }

    private IEnumerator closeMessagePanel()
    {
        yield return new WaitForSeconds(5);
        messagePanel.SetActive(false);
    }

}
