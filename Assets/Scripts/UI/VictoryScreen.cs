using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class VictoryScreen : MonoBehaviour
{   [SerializeField] private Timer _timeManager;
    [SerializeField] private GameObject  _Victory_screen;
    [SerializeField] private GameObject _Victory_screen_text;
    [SerializeField] private ScoreManager _scoreManager;
    private string _winnerMessage;
    private bool b1=true;
    private void Update()
    {
        if (_timeManager.GetTimeString().Equals("0") && b1)
        {   
            b1=false;
            _Victory_screen.SetActive(true);
            int[] array=new int [4];
            
            array[0]=_scoreManager.PlayerFourScoreArea.CurrentScore;
            array[1]=_scoreManager.PlayerFourScoreArea.CurrentScore;
            array[2]=_scoreManager.PlayerFourScoreArea.CurrentScore;
            array[3]=_scoreManager.PlayerFourScoreArea.CurrentScore;

            int winner=0;
            int count=1;
            for (int i = 0; i < array.Length; i++)
            {
                    if(array[i]>winner){
                        winner=array[i];
                        count=i;
                    }
            }
            _winnerMessage="!!! Player"+count+" is Won !!!";
            _Victory_screen_text.GetComponent<TextMeshProUGUI>().text=_winnerMessage;
        } 
    }
    private string GetMessage(){
        return _winnerMessage;
    }
    public void deneme(){
        SceneManager.LoadScene(0);
    }

}
