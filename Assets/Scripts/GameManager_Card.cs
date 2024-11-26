using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
//casisingletonxd
using UnityEngine.Networking;
public class GameManager : MonoBehaviour
{
    public Sprite backcardSprite;
    private bool firstGuess, secondGuess;
    public CardSO[] cardPool;
    public GameObject card;
    public GameObject cardField;
  
    private List<GameObject> cards = new List<GameObject>();
    private List<Button> buttons = new List<Button>();


    private int index;
    private Card firstchoise;
    private Card secondchoise;
    private bool evaluating;

    private int matches;
    private int totalMatches;

   
    [SerializeField] GameObject _winPanel;

    [SerializeField] private AudioSource main;
    [SerializeField] private AudioSource audioSourceone;
    [SerializeField] private AudioSource audioSourcetwo;
    [SerializeField] private AudioClip correctAudio;
    [SerializeField] private AudioClip wrongAudio;

    //PanelAnimation
    [SerializeField] private GridLayoutGroup _panelCards;

    private bool isGameRunning = true;
    public TextMeshProUGUI _textTimer;
    float timer = 0;
    public TextMeshProUGUI _finalTimeText;

    //newwwwwwwww
    [HeaderAttribute(" Time ID")]
    public int bestCardTime;
    public int user_id = 0; // ID del usuario
    private string scoreUrl = "http://localhost/insert_cardgame.php";


    /// <summary> new2 no se q es sumaryxd
  // private string loginUrl = "http://localhost/UserLogin2.php";
    /// </summary>
    /// 

  
    public void ReceiveID(string id)
    {
        if (int.TryParse(id, out int parsedId)) // Asegurarnos de que el ID es válido
        {
            user_id = parsedId;
            Debug.Log("Received user ID: " + user_id);
        }
        else
        {
            Debug.LogError("Invalid user ID received: " + id);
        }
    }


    public class GameTime
    {
        public int user_id;
        public int best_card_time; 
    }

    [SerializeField] private GameTime gameTime;

    public void InsertScore()
    {
        StartCoroutine(InsertScoreCoroutine());
    }

    private IEnumerator InsertScoreCoroutine()
    {
        string jsonString = JsonUtility.ToJson(gameTime);
        UnityWebRequest request = new UnityWebRequest(scoreUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al insertar el mejor tiempo: " + request.error);
        }
        else
        {

            string responseText = request.downloadHandler.text;
            ServerResponseCard response = JsonUtility.FromJson<ServerResponseCard>(responseText);

            if (response.message == "Card time inserted successfully")
            {
                Debug.Log("tiempo agregao");
            }
            else
            {
                Debug.LogError("puntaje carta fallao: " + response.message);
            }
        }
    }
    public void SetUserID()
    {
        gameTime.user_id = user_id;
        Debug.Log("User ID set to: " + gameTime.user_id);
    }

    public void SetBestCardTime(int time)
    {
       
        gameTime.best_card_time = time;
        Debug.Log("Best card time successfully set to: " + gameTime.best_card_time);
    }



    void CurrentTime()
    {
        if (isGameRunning)
        {
            timer += Time.deltaTime;
        }
        
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        _textTimer.text = minutes + ":" + seconds;

    }
    private void Update()
    {
        CurrentTime();

    }
    void Start()
    {
        ////////
        

        ///////
        gameTime = new GameTime();
        gameTime.user_id = 0; // Asigna un valor inicial si lo tienes.
        gameTime.best_card_time = 0;
        ///////////
        _winPanel.SetActive(false);
        totalMatches = cardPool.Length;
        for (int i = 0; i < cardPool.Length; i++)
        {
            for (int l = 0; l < 2; l++)
            {
                GameObject go = Instantiate(card, cardField.transform, false);
                go.GetComponent<Card>().Initialize(cardPool[i].index, cardPool[i].sprite, backcardSprite);
                go.gameObject.name = i.ToString();
                cards.Add(go);
            }
        }
        List<GameObject> cardscopy = new List<GameObject>();
        List<GameObject> displaycards = new List<GameObject>();

        for (int i = 0; i < cards.Count; i++)
        {
            cardscopy.Add(cards[i]);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            int x = UnityEngine.Random.Range(0, cardscopy.Count);
            displaycards.Add(cardscopy[x]);
            cardscopy.RemoveAt(x);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i] = displaycards[i];
            cards[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            Button btn = cards[i].gameObject.GetComponent<Button>();
            buttons.Add(btn);
        }
        AddListeners();
        
    }
    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        if (evaluating)
        {
            return;
        }
        if (index < 2)
        {
            index++;
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>().Flip();
            if (!firstGuess)
            {
                firstGuess = true;
                firstchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                audioSourceone.Play();
            }
            else if (!secondGuess)
            {
                secondGuess = true;
                secondchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                audioSourcetwo.Play();
            }
        }
        if (index == 2)
        {
            evaluating = true;
            //waitaudio
            StartCoroutine(EvaluateCards());
        }
    }

    private IEnumerator EvaluateCards()
    {
        yield return new WaitForSeconds(1.5f);
        firstGuess = secondGuess = false;
        if (firstchoise.Index() == secondchoise.Index() && firstchoise.GetInstanceID() != secondchoise.GetInstanceID())
        {
            if (!firstchoise.IsPared() && !secondchoise.IsPared())
            {
                firstchoise.SetPair();
                secondchoise.SetPair();
                matches++;
                firstchoise.btn.interactable = false;
                secondchoise.btn.interactable = false;
                main.PlayOneShot(correctAudio);
            }
        }
        else
        {
            firstchoise.Flip();
            secondchoise.Flip();
            main.PlayOneShot(wrongAudio);
        }
        index = 0;
        evaluating = false;

        if (matches == totalMatches)
        {
            //wincondition
            isGameRunning = false;
            print("Win");
            ShowWinPanel();
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            _finalTimeText.text = "Your time: " + minutes + ":" + seconds;
            bestCardTime = Mathf.CeilToInt(timer);

            //StartCoroutine(FetchUserID());
            SetBestCardTime(bestCardTime);
            SetUserID();
            InsertScore();
        }
    }
 
    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void ShowWinPanel()
    {
        Transform winTransform = _winPanel.transform;
        _winPanel.SetActive(true);

    }
}
[System.Serializable]
public class ServerResponseCard
{
    public string message;
}

/*public class ServerResponse
{
    public int user_id;
    public string message;
}*/
/*private IEnumerator FetchUserID()
 {

         UnityWebRequest request = UnityWebRequest.Get(loginUrl);
         yield return request.SendWebRequest();

         if (request.result == UnityWebRequest.Result.Success)
         {
             var jsonResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
             if (jsonResponse.user_id > 0)
             {
                 user_id = jsonResponse.user_id;

                 Debug.Log("User ID fetched and saved: " + user_id);
             }
         }
         else
         {
             Debug.LogError("Error fetching user ID: " + request.error);
         }

 }*/
