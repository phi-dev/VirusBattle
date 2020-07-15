using System.Collections;
using System.Collections.Generic;
using TinyJson;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HighScoreController : MonoBehaviour
{

    public GameObject scrollView;
    public GameObject playerScoreEntry;
    public TextMeshProUGUI playerHighScore;

    // Start is called before the first frame update
    void Start()
    {
        this.showPlayerHighScore();
        StartCoroutine(printAllHighScores());
    }

    private void showPlayerHighScore()
    {
        int highScore = PlayerPrefs.GetInt("highScore");
        this.playerHighScore.text = "You (" +PlayerPrefs.GetString("username") + "): " + highScore.ToString();
    }

    private IEnumerator printAllHighScores()
    {
        string url = "http://sharebox.altervista.org/highscore.php";
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
        int position = 1;
        foreach(HighScore hs in scores)
        {
            //GameObject scorePlayer = Instantiate(generateText(hs.name + ": " + hs.score,Color.white)) as GameObject;
            GameObject scorePlayer = Instantiate(generateEntry(position,hs.name,hs.score)) as GameObject;
            scorePlayer.transform.SetParent(scrollView.transform, false);
            position++;
        }
    }

    private enum Child:int
    {
        POSITION,
        NAME,
        SCORE
    }

    private GameObject generateEntry(int position,string name,string score)
    {
        GameObject entry = Instantiate(this.playerScoreEntry);
        TextMeshProUGUI positionText = entry.transform.GetChild((int)Child.POSITION).GetComponent<TextMeshProUGUI>() ;
        TextMeshProUGUI nameText = entry.transform.GetChild((int)Child.NAME).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = entry.transform.GetChild((int)Child.SCORE).GetComponent<TextMeshProUGUI>();

        positionText.text = "   " + position.ToString() + "   ";
        nameText.text = name;
        scoreText.text = score;

        return entry;
    }

    private GameObject generateText(string content,Color color)
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
