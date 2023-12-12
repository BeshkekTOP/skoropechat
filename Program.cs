using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TypingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TypingTest typingTest = new TypingTest();
            typingTest.RunTest();

            Console.WriteLine("Желаете пройти тест ещё раз? (Y/N)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                typingTest.RunTest();
            }
        }
    }

    class TypingTest
    {
        private string playerName;
        private string textToType;
        private Stopwatch stopwatch;
        private List<Record> leaderboard;

        private const string LeaderboardFilePath = "leaderboard.json";

        public TypingTest()
        {
            leaderboard = LoadLeaderboard();
        }

        public void RunTest()
        {
            Console.WriteLine("Введите ваше имя:");
            playerName = Console.ReadLine();

            Console.WriteLine("Пожалуйста, напечатайте следующий текст:");

            textToType = "Медленно минуты уплывают в даль, встречи с ними ты уже не жди И хотя нам прошлого немного жаль лучшее, конечно, впереди. Скатертью, скатертью дальний путь стелется и упирается прямо в небосклон Каждому, каждому а лучшее верится катится, катится голубой вагон. Может мы обидели кого-то зря календарь закроет старый лист. К новым приключениям спешим, друзья, эй, прибавь-ка ходу, машинист! Скатертью, скатертью дальний путь стелется и упирается прямо в небосклон Каждому, каждому а лучшее верится катится, катится голубой вагон. Голубой вагон бежит, качается скорый поезд набирает ход, Ну, зачем же этот день кончается, пусть бы он тянулся целый год! Скатертью, скатертью дальний путь стелется и упирается прямо в небосклон Каждому, каждому а лучшее верится катится, катится голубой вагон. Скатертью, скатертью дальний путь стелется и упирается прямо в небосклон Каждому, каждому а лучшее верится катится, катится голубой вагон. Скатертью, скатертью дальний путь стелется и упирается прямо в небосклон Каждому, каждому а лучшее верится катится, катится Голубой вагон.";
            
            Console.WriteLine(textToType);

            Console.ReadLine();

            Console.WriteLine("Время началось! Начинайте печатать...");

            stopwatch = Stopwatch.StartNew();

            string typedText = Console.ReadLine();

            stopwatch.Stop();

            Console.WriteLine("\nВремя, затраченное на ввод: " + stopwatch.Elapsed);

            CalculateScore(typedText);

            DisplayLeaderboard();
            
            SaveLeaderboard();
        }

        private void CalculateScore(string typedText)
        {
            int charactersTyped = typedText.Length;
            double timeInSeconds = stopwatch.Elapsed.TotalSeconds;
            double charactersPerMinute = charactersTyped / timeInSeconds * 60;
            double charactersPerSecond = charactersTyped / timeInSeconds;

            Record record = new Record(playerName, charactersPerMinute, charactersPerSecond);

            leaderboard.Add(record);

            leaderboard = leaderboard.OrderByDescending(r => r.CharactersPerMinute).ToList();
        }

        private void DisplayLeaderboard()
        {
            Console.WriteLine("\nТаблица рекордов:");
            Console.WriteLine("Имя\t\tСимволов в минуту\tСимволов в секунду");
            foreach (Record record in leaderboard)
            {
                Console.WriteLine($"{record.PlayerName}\t\t{record.CharactersPerMinute}\t\t{record.CharactersPerSecond}");
            }
        }
        
        private void SaveLeaderboard()
        {
            string json = JsonConvert.SerializeObject(leaderboard, Formatting.Indented);
            File.WriteAllText(LeaderboardFilePath, json);
        }

        private List<Record> LoadLeaderboard()
        {
            if (File.Exists(LeaderboardFilePath))
            {
                string json = File.ReadAllText(LeaderboardFilePath);
                return JsonConvert.DeserializeObject<List<Record>>(json);
            }
            else
            {
                return new List<Record>();
            }
        }
    }

    class Record
    {
        public string PlayerName { get; }
        public double CharactersPerMinute { get; }
        public double CharactersPerSecond { get; }

        public Record(string playerName, double charactersPerMinute, double charactersPerSecond)
        {
            PlayerName = playerName;
            CharactersPerMinute = charactersPerMinute;
            CharactersPerSecond = charactersPerSecond;
        }
    }
}