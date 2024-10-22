using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleBankoApp
{
    class Game
    {
        private List<Plate> allPlates = new List<Plate>();
        private List<int> numbersPicked = new List<int>();

        public Game() { }

        public void addPlate(Plate plate)
        {
            this.allPlates.Add(plate);
        }

        public void run()
        {
            //Console.Clear();

            Console.WriteLine("Game Started.\n");

            PrintPlates();

            int banko = 1;
            while (banko <= 3)
            {
                Console.WriteLine($"Playing for {banko} row{(banko > 1 ? "s" : "")}: ");
                Console.Write("Write the number pulled: ");
                string? input = Console.ReadLine();

                Console.WriteLine("\x1b[3J");
                Console.Clear();

                string? msg = AddNumber(input);
                PrintPlates();
                PrintMessage(msg);
                PrintPickedNumbers();
                if (HasBanko(banko)) banko++;
            }
        }

        private string? AddNumber(string? numberIn)
        {
            if (numberIn == null || numberIn == string.Empty) return null;

            int numberOut = 0;
            if (int.TryParse(numberIn, out numberOut))
            {
                if (this.numbersPicked.Contains(numberOut))
                {
                    return "Please write a number between not already picked!";
                }
                else if (numberOut > 0 && numberOut < 91)
                {
                    this.numbersPicked.Add(numberOut);
                    return $"Added number: {numberOut}";
                }
                else
                {
                    return "Please write a number between 1 & 90!";
                }
            }
            else
            {
                return "Please write a number instead!";
            }
        }

        private void PrintMessage(string? message)
        {
            if (message != null) Console.WriteLine(message);
        }

        private void PrintPickedNumbers()
        {
            if (this.numbersPicked.Count > 0)
            {
                Console.WriteLine($"Numbers picked: {this.numbersPicked.Count}\n");
                for (int i = 0; i < this.numbersPicked.Count; i++)
                {
                    int number = this.numbersPicked[i];
                    Console.Write($"{number} ");
                }
                Console.WriteLine("\n");
            }
        }

        private void PrintPlates()
        {
            for (int i = 0; i < this.allPlates.Count; i++)
            {
                Plate plate = this.allPlates[i];
                Console.WriteLine($"ID: {plate.GetID()}");
                plate.PrintPlate(this.numbersPicked);
            }
        }

        private bool HasBanko(int rowsNeeded)
        {
            bool banko = false;

            for (int i = 0; i < this.allPlates.Count; i++)
            {
                Plate plate = this.allPlates[i];
                bool plateHasBanko = plate.hasBanko(this.numbersPicked, rowsNeeded);
                //Console.WriteLine($"{plate.GetID()}: {plateHasBanko}");
                if (plateHasBanko)
                {
                    banko = true;
                    Console.WriteLine($"\nBANKO!\nOn plate: {plate.GetID()}");
                }
            }
            Console.WriteLine();

            return banko;
        }
    }
}
