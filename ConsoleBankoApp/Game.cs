namespace ConsoleBankoApp
{
    class Game()
    {
        private List<Plate> allPlates = new List<Plate>();
        private List<int> numbersPicked = new List<int>();

        // Tilføjer en plade til dette spil.
        public void addPlate(Plate plate)
        {
            this.allPlates.Add(plate);
        }

        // Dette er koden som starter selve spillet.
        public void run()
        {
            // "\x1b[3J" bliver brugt til at rydde hele consollen da Console.Clear() kun kan rydde et hvis antal linjer.
            Console.WriteLine("\x1b[3J");
            Console.Clear();

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

                // Kontrollerer om er banko på en plade eller en anden spiller, hvis der bliver skrevet "u".
                if ((input != null && input.ToLower() == "u") || HasBanko(banko)) banko++;
            }
        }

        // Tilføjer et nummer til listen af trukkede numre og afleverer en besked baseret på det input den har fået.
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
            else if (numberIn.ToLower() == "u")
            {
                return "Too bad, someone else had a Banko!";
            }
            else
            {
                return "Please write a number instead!";
            }
        }

        // Brugt til at printe beskeden fra AddNumber, men kun hvis den ikke er null.
        private void PrintMessage(string? message)
        {
            if (message != null) Console.WriteLine(message);
        }

        // Printer listen med trukkede numre, i den rækkefølge de blev trukket.
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

        // Kode til at printe all pladerne i spillet.
        private void PrintPlates()
        {
            for (int i = 0; i < this.allPlates.Count; i++)
            {
                Plate plate = this.allPlates[i];
                Console.WriteLine($"ID: {plate.GetID()}");
                plate.PrintPlate(this.numbersPicked);
            }
        }

        // Kontrollerer alle pladerne for banko, baseret på det antal rækker der spilles om.
        private bool HasBanko(int rowsNeeded)
        {
            bool banko = false;

            for (int i = 0; i < this.allPlates.Count; i++)
            {
                Plate plate = this.allPlates[i];
                bool plateHasBanko = plate.hasBanko(this.numbersPicked, rowsNeeded);
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
