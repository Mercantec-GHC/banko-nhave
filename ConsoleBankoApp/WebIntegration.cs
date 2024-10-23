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
        public static void BuildPlatesFromWeb(Game game, string id, int numberOfPlates)
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://mercantech.github.io/Banko/");

            var title = driver.Title;

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            var textBox = driver.FindElement(By.Id("tekstboks"));
            var submitButton = driver.FindElement(By.Id("knap"));

            for (int i = 0; i < numberOfPlates; i++)
            {
                string plateID = $"{id}{i}";

                textBox.Clear();
                textBox.SendKeys(plateID);
                submitButton.Click();

                IWebElement table = driver.FindElement(By.Id("p1"));

                int[][] plate = CreateBankoArray();

                string[] result = Regex.Split(table.Text, @"\s+");

                for (int j = 0; j < result.Length; j++)
                {
                    int number = int.Parse(result[j]);
                    int row = (j == 0 ? 0 : j / 5);
                    int col = GetCollumnFromRange(number);
                    plate[row][col] = number;
                }

                game.addPlate(new Plate(plateID).LoadPlate(plate));
            }

            driver.Quit();
        }

        private static int[][] CreateBankoArray()
        {
            int[][] array = [
                Enumerable.Repeat(0, 9).ToArray(),
                Enumerable.Repeat(0, 9).ToArray(),
                Enumerable.Repeat(0, 9).ToArray()
            ];

            return array;
        }

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
