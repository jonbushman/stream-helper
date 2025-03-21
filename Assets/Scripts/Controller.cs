using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]private List<Item> _items;
    private List<string> _directories;
    private string _saveFileName = "save data";


    private void Start()
    {
        int targetWidth = 1366;
        int targetHeight = 768;

        float targetAspect = (float)targetWidth / targetHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        if (Mathf.Abs(currentAspect - targetAspect) > 0.01f)
        {
            Screen.SetResolution(targetWidth, targetHeight, false);
        }

        Load();
    }

    public void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        _directories = new List<string>();
        for (var i = 0; i < _items.Count; i++)
        {
            _directories.Add(_items[i].GetDirectory());
        }

        //serialize and save
        var filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        var dataToStore = JsonConvert.SerializeObject(_directories, Formatting.Indented);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
    }

    private void Load()
    {
        var filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
        _directories = new List<string>();
        if (File.Exists(filePath))
        {
            string dataToLoad = "";
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
            _directories = JsonConvert.DeserializeObject<List<string>>(dataToLoad);

            for (var i = 0; i < _items.Count; i++)
            {
                if (string.IsNullOrEmpty(_directories[i]))
                {
                    _items[i].SetDirectory("no directory");
                }
                else
                {
                    _items[i].SetDirectory(_directories[i]);
                }
                _items[i].UpdateDirectoryLabel();
            }
        }

    }
}
