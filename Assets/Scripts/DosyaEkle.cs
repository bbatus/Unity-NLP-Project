using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DosyaEkle : MonoBehaviour
{
    public Button selectFileButton;      // Dosya seçim butonu
    public Text selectedFileName;       // Seçilen dosyanın ismini gösterecek Text bileşeni
    public Dropdown fileDropdown;       // Masaüstündeki "Texts" klasöründeki txt dosyalarını listelemek için bir Dropdown bileşeni

    private string folderPath;          // "Texts" klasörünün yolu
    private string selectedFilePath = string.Empty; // Seçilen dosyanın tam yolu

    private void Start()
    {
        // Masaüstündeki "Texts" klasörünün yolunu alıyoruz.
        folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Texts");

        // Dosya seçim butonuna işlevsellik ekliyoruz.
        selectFileButton.onClick.AddListener(PopulateFileList);
    }

    private void PopulateFileList()
    {
        // "Texts" klasöründeki tüm txt dosyalarını listeliyoruz.
        string[] files = Directory.GetFiles(folderPath, "*.txt");
        fileDropdown.ClearOptions(); // Eski seçenekleri temizliyoruz.

        List<string> fileNames = new List<string>();

        foreach (string file in files)
        {
            fileNames.Add(Path.GetFileName(file));
        }

        fileDropdown.AddOptions(fileNames); // Dosya isimlerini Dropdown'a ekliyoruz.
        fileDropdown.onValueChanged.AddListener(delegate { FileSelected(fileDropdown); }); // Dosya seçildiğinde tetiklenecek fonksiyonu belirtiyoruz.
    }

    private void FileSelected(Dropdown dropdown)
    {
        selectedFilePath = Path.Combine(folderPath, dropdown.options[dropdown.value].text);
        selectedFileName.text = dropdown.options[dropdown.value].text; // Seçilen dosyanın ismini gösteriyoruz.
    }

    public string GetSelectedFilePath()
    {
        return selectedFilePath; // Eğer başka bir scriptten dosya yoluna erişmek isterseniz bu fonksiyonu kullanabilirsiniz.
    }

    public string GetProcessedContent()
    {
        if (string.IsNullOrEmpty(selectedFilePath))
        {
            Debug.LogWarning("Dosya seçilmedi!");
            return string.Empty;
        }
        string content = File.ReadAllText(selectedFilePath);

        // Noktalama işaretlerini kaldırma:
        content = RemovePunctuation(content);

        content = CorrectSpellingMistakes(content);

        // Daha ileri aşamalarda hatalı yazımları düzeltme işlemini burada ekleyebilirsiniz.

        return content;
    }
    private string RemovePunctuation(string input)
    {
        // Bu fonksiyon metindeki tüm noktalama işaretlerini kaldırır.
        return Regex.Replace(input, @"[^\w\s]", "");
    }

    private Dictionary<string, string> spellingCorrections = new Dictionary<string, string>
{
    // Örnek: "kelime" yanlış yazılmışsa "doğruKelime" ile değiştirilir.
    // Bu örnekleri kendi yazım yanlışlarınızla doldurabilirsiniz.
    { "kelime", "doğruKelime" },
    { "yanlis", "doğru" },
    { "Bri", "Bir" },
    { "ormnd", "ormanda" },
    { "tavsan", "tavşan" },
    { "yanlız", "yalnız" }

    // ... Diğer yazım yanlışları ve düzeltmeleri ekleyebilirsiniz.
};

    private string CorrectSpellingMistakes(string input)
    {
        foreach (var mistake in spellingCorrections)
        {
            input = input.Replace(mistake.Key, mistake.Value);
        }
        return input;
    }
}