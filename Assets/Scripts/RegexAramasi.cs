using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;


public class RegexAramasi : MonoBehaviour
{
    public DosyaEkle dosyaEkle; // Önceden oluşturduğunuz FileSelector scriptini burada referans olarak alıyoruz.
    public InputField regexInput;     // Regex değerinin girileceği InputField bileşeni
    public Button searchButton;       // "Ara" butonu
    public Text resultText;           // Sonuçları göstermek için kullanılacak Text bileşeni
    public Button morfolojiAnalizButton;  // Morfoloji Analiz Getir butonu için
    public Text morfolojiResultText;      // Morfoloji analiz sonuçlarını göstermek için kullanılacak Text bileşeni
    public Button kokAlButton;  // "Kök Al" butonu için
    public string nextSceneName = "Scene_2";  // Geçiş yapılacak sahnenin adı

    private List<string> searchResults = new List<string>(); // Eşleşen sonuçları saklamak için bir liste

    public static List<string> stemmedResults = new List<string>();  // Kök alma sonuçlarını saklamak için
    public static int uniqueWordCountBeforeStemming;
    public static int uniqueWordCountAfterStemming;

    private void Start()
    {
        // "Ara" butonuna işlevsellik ekliyoruz.
        searchButton.onClick.AddListener(SearchUsingRegex);
        morfolojiAnalizButton.onClick.AddListener(PerformMorfolojiAnaliz);
        kokAlButton.onClick.AddListener(PerformKokAlma);
    }

    private void SearchUsingRegex()
    {
        string filePath = dosyaEkle.GetSelectedFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogWarning("Dosya seçilmedi!");
            return;
        }

        string content = File.ReadAllText(filePath);  // Dosya içeriğini okuyoruz.
        string regexPattern = regexInput.text;        // Kullanıcının girdiği Regex değerini alıyoruz.

        MatchCollection matches = Regex.Matches(content, regexPattern);

        searchResults.Clear(); // Eski sonuçları temizliyoruz.
        foreach (Match match in matches)
        {
            searchResults.Add(match.Value);
        }

        DisplayResults(); // Sonuçları ekranda gösteriyoruz.
    }

    private void DisplayResults()
    {
        resultText.text = string.Join("\t", searchResults); // Sonuçları yeni satırlarla birlikte ekranda gösteriyoruz.
    }

    private void PerformMorfolojiAnaliz()
    {
        string processedContent = dosyaEkle.GetProcessedContent();
        // Şu an için işlenmiş içeriği direkt olarak gösteriyoruz:
        morfolojiResultText.text = processedContent;
    }

    public void PerformKokAlma()
    {
        string content = dosyaEkle.GetProcessedContent();
        string[] words = content.Split(' ');

        HashSet<string> uniqueWords = new HashSet<string>(words);
        uniqueWordCountBeforeStemming = uniqueWords.Count;

        HashSet<string> stemmedWords = new HashSet<string>();
        foreach (var word in uniqueWords)
        {
            stemmedWords.Add(SimpleStemming(word));
        }
        uniqueWordCountAfterStemming = stemmedWords.Count;

        // Sonuçları static değişkende saklama:
        stemmedResults.Clear();
        foreach (var word in stemmedWords)
        {
            stemmedResults.Add(word);
        }

        // Sahne değiştirme:
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    private string SimpleStemming(string word)
    {
        if (word.Length > 2)
        {
            return word.Substring(0, word.Length - 2);
        }
        return word;
    }

}

