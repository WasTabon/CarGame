using System.Collections.Generic;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    public static WalletController Instance { get; private set; }

    private const string MoneyKey = "Money";
    private const string LevelKey = "ShopLevel";
    private const string PurchasedObjectsKey = "PurchasedObjects";

    [SerializeField] private int defaultMoney = 100;

    // Деньги с автосохранением
    private int _money;
    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            PlayerPrefs.SetInt(MoneyKey, _money);
            PlayerPrefs.Save();
        }
    }

    // Уровень магазина
    public int CurrentLevel { get; private set; }

    // Список уровней магазина
    [System.Serializable]
    public class ShopLevel
    {
        public int price;
        public GameObject[] objectsToDisable;
    }

    public List<ShopLevel> shopLevels = new List<ShopLevel>();

    private void Awake()
    {
        Instance = this;
        
        LoadData();
    }

    private void LoadData()
    {
        Money = PlayerPrefs.GetInt(MoneyKey, defaultMoney);
        CurrentLevel = PlayerPrefs.GetInt(LevelKey, 0);

        // Загружаем отключенные объекты
        string purchasedData = PlayerPrefs.GetString(PurchasedObjectsKey, "");
        if (!string.IsNullOrEmpty(purchasedData))
        {
            string[] objectNames = purchasedData.Split(';');
            foreach (string objName in objectNames)
            {
                if (string.IsNullOrEmpty(objName)) continue;
                GameObject obj = GameObject.Find(objName);
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    public void TryPurchaseCurrentLevel()
    {
        if (CurrentLevel >= shopLevels.Count) return; // Нет уровней
        ShopLevel level = shopLevels[CurrentLevel];

        if (Money >= level.price)
        {
            // Снимаем деньги
            Money -= level.price;

            // Отключаем объекты
            foreach (var obj in level.objectsToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    SaveDisabledObject(obj);
                }
            }

            // Переход на следующий уровень
            CurrentLevel++;
            PlayerPrefs.SetInt(LevelKey, CurrentLevel);
            PlayerPrefs.Save();
            
        }
        else
        {
            Debug.Log("Недостаточно денег!");
        }
    }

    private void SaveDisabledObject(GameObject obj)
    {
        string purchasedData = PlayerPrefs.GetString(PurchasedObjectsKey, "");
        if (!purchasedData.Contains(obj.name))
        {
            purchasedData += obj.name + ";";
            PlayerPrefs.SetString(PurchasedObjectsKey, purchasedData);
            PlayerPrefs.Save();
        }
    }
}
