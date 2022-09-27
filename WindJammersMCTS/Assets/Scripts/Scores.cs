using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Scores : MonoBehaviour
{
    public static Scores instance;

    public GameObject score;
    public int PlayerScore = 0;
    public int EnnemyScore = 0;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Scores dans la scene");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        score.GetComponent<TMPro.TextMeshProUGUI>().text = EnnemyScore.ToString() + '-' + PlayerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        score.GetComponent<TMPro.TextMeshProUGUI>().text = EnnemyScore.ToString() + '-' + PlayerScore.ToString();
    }
}
