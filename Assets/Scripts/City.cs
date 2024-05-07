using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    public int money;
    private int TotalCost = 0; // Суммарная стоимость всех зданий
    private int TotalIncome=0; //Суммарная стоимость доходов зданий
    public int curPopulation;
    public int curJobs;
    public int curFood;
    public int maxPopulation;
    public int maxJobs;
    public int incomePerJob;

    public TextMeshProUGUI statsText;

    private List<BuildingPreset> buildings = new List<BuildingPreset>();

    //Параметры времени в игре

    private float gameTime; // Общее время с начала игры

    private float uiUpdateInterval = 0.5f; // Интервал обновления UI в секундах
    private float timeSinceLastUIUpdate = 0f; // Время с последнего обновления UI
    private float UpdateEconomiсsInterval=30f; //Период обновления экономики в игре
    private float timeSinceLastEconomicsUpdate = 0f;// Счётчик времени с последнего обновления экономики

    private bool isPaused = false; // Состояние паузы

    public static City inst;

    void Awake()
    {
        inst = this;
        isPaused = true; // Изначально игра на паузе
    }

    public void OnPlaceBuilding(BuildingPreset building)
    {
        maxPopulation += building.population;
        maxJobs += building.jobs;
        buildings.Add(building);
        // Увеличиваем общую стоимость при добавлении здания
        TotalCost += building.costPerTurn;
    }
    public void TogglePause() // Переключение состояния паузы
    {
        isPaused = !isPaused; // Изменение значения переменной
    }

   void UpdateGame()
    {
        float deltaTime = Time.deltaTime;

        if (!isPaused)
        {
            gameTime += deltaTime; // Увеличиваем общее время в игре

            // // Частое обновление игровых расчётов (каждый кадр)
            // CalculateMoney(); 
            // CalculatePopulation(); 
            // CalculateJobs();
            // CalculateFood();
            CalculateJobs();
            CalculateFood();
            CalculatePopulation(); 
            // Периодическое обновление экономики в каждые 30 секунд
            timeSinceLastEconomicsUpdate += deltaTime;
            if (timeSinceLastEconomicsUpdate >= UpdateEconomiсsInterval)
            {
                // Обновляем экономику
                CalculateMoney(); 
                CalculateIncome();
                timeSinceLastEconomicsUpdate = 0f; // Сбрасываем счётчик обновления экономики
            }

        }

        // Обновление UI непрерывное
        timeSinceLastUIUpdate += deltaTime;
        if (timeSinceLastUIUpdate >= uiUpdateInterval)
        {
            // Преобразуем время в часы, минуты, секунды
            int hours = (int)(gameTime / 3600);
            int minutes = (int)((gameTime % 3600) / 60);
            int seconds = (int)(gameTime % 60);

            // Добавляем индикатор паузы в выводимый текст
            string pauseIndicator = isPaused ? "   [Paused]" : ""; // Показать, что игра на паузе

            // Обновляем текст, включая общее время и индикатор паузы
            statsText.text = $"Time: {hours:D2}:{minutes:D2}:{seconds:D2}   \nMoney: ${money}   \nBuildingsCost: ${TotalCost}  BuildingsIncome: ${TotalIncome-TotalCose}       Pop: {curPopulation} / {maxPopulation}   Jobs: {curJobs} / {maxJobs}   Food: {curFood}         {pauseIndicator}";

            timeSinceLastUIUpdate = 0f; // Сбрасываем таймер обновления UI
        }
    }
    void CalculateMoney()
    {
        TotalIncome= curJobs * incomePerJob;
        money +=  TotalIncome;
       
        // foreach(BuildingPreset building in buildings)
        //     money -= building.costPerTurn;
        money-=TotalCost;
    }
    
    void CalculateIncome()
    {
        TotalIncome+=curJobs * incomePerJob;
    }

    void CalculatePopulation()
    {
        maxPopulation = 0;

        foreach(BuildingPreset building in buildings)
            maxPopulation += building.population;

        if(curFood >= curPopulation && curPopulation < maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        }
        else if(curFood < curPopulation)
        {
            curPopulation = curFood;
        }
    }

    void CalculateJobs()
    {
        curJobs = 0;
        maxJobs = 0;

        foreach(BuildingPreset building in buildings)
            maxJobs += building.jobs;

        curJobs = Mathf.Min(curPopulation, maxJobs);
    }

    void CalculateFood()
    {
        curFood = 0;

        foreach(BuildingPreset building in buildings)
            curFood += building.food;
    }


    void Update()
    {
        UpdateGame();
    }
}