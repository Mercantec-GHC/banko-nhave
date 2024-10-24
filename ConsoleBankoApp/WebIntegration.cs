using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ConsoleBankoApp
{
    internal class WebIntegration
    {
        // Bruger Selenium til at scrape data om pladerne fra siden https://mercantech.github.io/Banko/
        public static void BuildPlatesFromWeb(Game game, string id, int numberOfPlates)
        {
            Console.Clear();
            Console.WriteLine($"Grabbing {numberOfPlates} plates from 'https://mercantech.github.io/Banko/' with an id of {id}\n");

            // Opretter en ChromeDriverService og skjuler dens CommandPrompt.
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            // Opretter ChromeOptions og tilføjer "headless" til den for at skjule browser vinduet.
            var options = new ChromeOptions();
            options.AddArgument("headless");

            // Starter Chrome Driveren.
            IWebDriver driver = new ChromeDriver(service, options);

            // Navigerer til siden.
            driver.Navigate().GoToUrl("https://mercantech.github.io/Banko/");

            // Sætter driveren til at vente 500 millisekunder for at sikre at siden er blevet fuldt indlæst.
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            var textBox = driver.FindElement(By.Id("tekstboks"));
            var submitButton = driver.FindElement(By.Id("knap"));

            for (int i = 0; i < numberOfPlates; i++)
            {
                string plateID = $"{id}{i+1}";

                textBox.Clear();
                textBox.SendKeys(plateID);
                submitButton.Click();

                IWebElement table = driver.FindElement(By.Id("p1"));

                int[][] bankoArray = CreateBankoArray();

                string[] result = Regex.Split(table.Text, @"\s+");

                for (int j = 0; j < result.Length; j++)
                {
                    int number = int.Parse(result[j]);
                    int row = (j == 0 ? 0 : j / 5);
                    int col = GetCollumnFromRange(number);
                    bankoArray[row][col] = number;
                }

                Plate plate = new Plate(plateID).LoadPlate(bankoArray);

                Console.WriteLine($"Build plate with id: {plateID}");
                plate.PrintPlate();

                game.addPlate(plate);
            }

            driver.Quit();

            Console.WriteLine("\x1b[3J");
            Console.Clear();
        }

        // Ny kode til hurtigt at generere det array som skal indeholde en plades rækker,
        // hvor alle felte oprindeligt indeholder taldet '0'.
        private static int[][] CreateBankoArray()
        {
            int[][] array = [
                Enumerable.Repeat(0, 9).ToArray(),
                Enumerable.Repeat(0, 9).ToArray(),
                Enumerable.Repeat(0, 9).ToArray()
            ];

            return array;
        }

        // Simpel funktion til at finde den søjle et tal skal indsættes i.
        private static int GetCollumnFromRange(int input)
        {
            int col = 0;

            if (input >= 80) col = 8;
            else if (input >= 70) col = 7;
            else if (input >= 60) col = 6;
            else if (input >= 50) col = 5;
            else if (input >= 40) col = 4;
            else if (input >= 30) col = 3;
            else if (input >= 20) col = 2;
            else if (input >= 10) col = 1;

            return col;
        }
    }
}
