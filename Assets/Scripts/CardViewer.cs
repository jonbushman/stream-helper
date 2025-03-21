//need to fix duplicate named cards within units and within upgrades

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class CardViewer : MonoBehaviour
{
    public TMP_Dropdown UnitDropDown;
    public TMP_Dropdown UpgradeDropDown;
    public TMP_Dropdown CommandDropDown;
    public TMP_Dropdown BattleDropDown;
    public TMP_Dropdown CounterpartDropDown;
    public TMP_Dropdown FlawDropDown;

    public UnityEngine.UI.Image ImageContainer;

    private Dictionary<string, CardData> _cardData;

    private void Start()
    {
        LoadCardsJS();

        var cardNameDict = GetCardNames("unit");
        foreach (var cardName in cardNameDict)
        {
            UnitDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
        cardNameDict = GetCardNames("upgrade");
        foreach (var cardName in cardNameDict)
        {
            UpgradeDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
        cardNameDict = GetCardNames("command");
        foreach (var cardName in cardNameDict)
        {
            CommandDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
        cardNameDict = GetCardNames("battle");
        foreach (var cardName in cardNameDict)
        {
            BattleDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
        cardNameDict = GetCardNames("counterpart");
        foreach (var cardName in cardNameDict)
        {
            CounterpartDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
        cardNameDict = GetCardNames("flaw");
        foreach (var cardName in cardNameDict)
        {
            FlawDropDown.options.Add(new TMP_Dropdown.OptionData(cardName.Item2));
        }
    }

    private void OnEnable()
    {
        UnitDropDown.onValueChanged.AddListener(UnitChange);
        UpgradeDropDown.onValueChanged.AddListener(UpgradeChange);
        CommandDropDown.onValueChanged.AddListener(CommandChange);
        BattleDropDown.onValueChanged.AddListener(BattleChange);
        CounterpartDropDown.onValueChanged.AddListener(CounterpartChange);
        FlawDropDown.onValueChanged.AddListener(FlawChange);
    }

    private void OnDisable()
    {
        UnitDropDown.onValueChanged?.RemoveListener(UnitChange);
        UpgradeDropDown.onValueChanged?.RemoveListener(UpgradeChange);
        CommandDropDown.onValueChanged?.RemoveListener(CommandChange);
        BattleDropDown.onValueChanged?.RemoveListener(BattleChange);
        CounterpartDropDown.onValueChanged?.RemoveListener(CounterpartChange);
        FlawDropDown.onValueChanged?.RemoveListener(FlawChange);
    }

    private void LoadCardsJS()
    {
        //string dataToLoad = "";
        //using (FileStream stream = new FileStream("Assets/Resources/cards.json", FileMode.Open))
        //{
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        dataToLoad = reader.ReadToEnd();
        //    }
        //}
        //_cardData = JsonConvert.DeserializeObject<Dictionary<string, CardData>>(dataToLoad);

        TextAsset jsonFile = Resources.Load<TextAsset>("cards");

        if (jsonFile == null)
        {
            Debug.LogError("cards.json not found in Resources!");
        }
        else
        {
            string dataToLoad = jsonFile.text;
            _cardData = JsonConvert.DeserializeObject<Dictionary<string, CardData>>(dataToLoad);
        }
    }

    private List<KeyValuePair<string,CardData>> FilterByCardType(string cardType)
    {
        return _cardData.Where(x => x.Value.cardType == cardType).ToList();
    }

    private List<(string,string)> GetCardNames(string cardType)
    {
        var kvps = _cardData.Where(x => x.Value.cardType == cardType).ToList();
        var cardNameDict = new List<(string, string)>();

        foreach (var kvp in kvps)
        {
            cardNameDict.Add((kvp.Key, kvp.Value.cardName));
        }

        cardNameDict.Sort((x, y) => string.Compare(x.Item2, y.Item2, true));

        return cardNameDict;
    }

    #region DropDownChanges
    private void UnitChange(int index)
    {
        var cardName = UnitDropDown.options[index].text;
        CardSelected(index, "unit");

        //UpgradeDropDown.value = 0;
        //CommandDropDown.value = 0;
        //BattleDropDown.value = 0;
        //CounterpartDropDown.value = 0;
        //FlawDropDown.value = 0;
    }
    private void UpgradeChange(int index)
    {
        var cardName = UpgradeDropDown.options[index].text;
        CardSelected(index, "upgrade");

        //UnitDropDown.value = 0;
        //CommandDropDown.value = 0;
        //BattleDropDown.value = 0;
        //CounterpartDropDown.value = 0;
        //FlawDropDown.value = 0;
    }
    private void CommandChange(int index)
    {
        var cardName = CommandDropDown.options[index].text;
        CardSelected(index, "command");

        //UnitDropDown.value = 0;
        //UpgradeDropDown.value = 0;
        //BattleDropDown.value = 0;
        //CounterpartDropDown.value = 0;
        //FlawDropDown.value = 0;
    }
    private void BattleChange(int index)
    {
        var cardName = BattleDropDown.options[index].text;
        CardSelected(index, "battle");

        //UnitDropDown.value = 0;
        //UpgradeDropDown.value = 0;
        //CommandDropDown.value = 0;
        //CounterpartDropDown.value = 0;
        //FlawDropDown.value = 0;
    }
    private void CounterpartChange(int index)
    {
        var cardName = CounterpartDropDown.options[index].text;
        CardSelected(index, "counterpart");

        //UnitDropDown.value = 0;
        //UpgradeDropDown.value = 0;
        //CommandDropDown.value = 0;
        //BattleDropDown.value = 0;
        //FlawDropDown.value = 0;
    }
    private void FlawChange(int index)
    {
        var cardName = FlawDropDown.options[index].text;
        CardSelected(index, "flaw");

        //UnitDropDown.value = 0;
        //UpgradeDropDown.value = 0;
        //CommandDropDown.value = 0;
        //BattleDropDown.value = 0;
        //CounterpartDropDown.value = 0;
    }
    #endregion
    
    public void CardSelected(int index, string cardType)
    {
        var filteredDict = GetCardNames(cardType);
        if (filteredDict.Count > 0)
        {
            var cardID = filteredDict[index - 1].Item1;
        
            var imageName = _cardData[cardID].imageName.Replace(".jpeg", ".png").Replace(".jpg", ".png").Replace(".webp", ".png").Replace(".png", "");
            var imagePath = cardType + "/" + imageName;
            Debug.Log(imagePath);
            var image = Resources.Load<Sprite>(imagePath);
            if (image == null)
            {
                Debug.LogWarning($"Image not found at path: {imagePath}");
            }
            else
            {
                Debug.Log($"Loaded image: {image.name}");
            }


            ImageContainer.sprite = image;
            ImageContainer.preserveAspect = true;
        }
    }
    
}

[System.Serializable]
public class CardData
{
    public string id;
    public string cardType;
    public string defense;
    public List<string> surges;
    public int wounds;
    public int courage;
    public int speed;
    public string cardSubtype;
    public string cardName;
    public bool isUnique;
    public string rank;
    public int cost;
    public string faction;
    public string imageName;
    public bool isStormTide;
    public List<string> keywords;
    public List<string> upgradeBar;
    public List<HistoryData> history;
    public List<string> products;

}

public class HistoryData
{
    public string data;
    public string description;
}