using System.Collections;
using System.Collections.Generic;
using TinyJson;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class HighTimeScoreController : MonoBehaviour
{
    public GameObject scrollView;
    public TextMeshProUGUI playerHighTimeScore;

    // Start is called before the first frame update
    void Start()
    {
        this.showPlayerHighScore();
        StartCoroutine(printAllHighScores());
    }

    private void showPlayerHighScore()
    {
        int highTimeScore = PlayerPrefs.GetInt("highTimeScore");
        this.playerHighTimeScore.text = "You (" + PlayerPrefs.GetString("username") + "): " + highTimeScore.ToString();
    }

    private IEnumerator printAllHighScores()
    {
        string url = "http://sharebox.altervista.org/hightimescore.php";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Web error");
        }

        string json = www.downloadHandler.text;
        fillScores(json.FromJson<List<HighScore>>());

        yield return json;
    }

    private void fillScores(List<HighScore> scores)
    {
        foreach (HighScore hs in scores)
        {
            GameObject scorePlayer = Instantiate(generateText(hs.name + ": " + hs.score, Color.white)) as GameObject;
            scorePlayer.transform.SetParent(scrollView.transform, false);
        }
    }

    private GameObject generateText(string content, Color color)
    {
        if (color == null)
        {
            color = Color.white;
        }
        if (content == null)
        {
            content = "";
        }

        GameObject newText = new GameObject(content.Replace(" ", "-"), typeof(RectTransform));
        var newTextComp = newText.AddComponent<TextMeshProUGUI>();
        //newText.AddComponent<CanvasRenderer>();

        //Text newText = transform.gameObject.AddComponent<Text>();
        newTextComp.text = content;
        newTextComp.color = color;
        newTextComp.fontSize = 50;

        return newText;
    }
}
