using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public Text resultText;

    private void Start()
    {
        DisplayResults();
    }

    private void DisplayResults()
    {
        resultText.text = $"Stemming öncesi benzersiz kelime sayisi: {RegexAramasi.uniqueWordCountBeforeStemming}\n";
        resultText.text += $"Stemming sonrasi benzersiz kelime sayisi: {RegexAramasi.uniqueWordCountAfterStemming}\n\n";
        resultText.text += "Stemm kelimeler:\n";
        foreach (var word in RegexAramasi.stemmedResults)
        {
            resultText.text += word + "\t";
        }
    }
}