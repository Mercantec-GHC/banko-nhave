using Newtonsoft.Json;

namespace ConsoleBankoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialiserer Spillet.
            Game game = new Game();

            // Spørger spilleren hvad deres grund id skal være og giver dem muligheden for at lave det om.
            string? plateID = null;
            while (plateID == null)
            {
                Console.Clear();
                Console.Write("Please enter a base id for the plates you want: ");
                string? id = Console.ReadLine();
                if (id != null && id.Length > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Is {id} correct? (Y/n)");
                    string? confirm = Console.ReadLine();
                    if (confirm != null && confirm.ToLower() == "y") plateID = id;
                }
            }

            // Spørger spilleren hvor mange plade de vil have og giver dem muligheden for at lave det om.
            int plateAmount = 0;
            while (plateAmount <= 0)
            {
                Console.Clear();
                Console.Write("Please enter the amount of plates you want: ");
                string? amount = Console.ReadLine();
                int amountOut = 0;
                if (int.TryParse(amount, out amountOut) && amountOut > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Is {amountOut} correct? (Y/n)");
                    string? confirm = Console.ReadLine();
                    if (confirm != null && confirm.ToLower() == "y") plateAmount = amountOut;
                }
            }

            // Kalder Selenium web integrationen og beder den generere de plader som skal bruges.
            WebIntegration.BuildPlatesFromWeb(game, plateID, plateAmount);

            // Når alt er klart, startes spillet.
            game.run();
        }

        // Midlertidig kode til at læse plader ind fra et JSON dokument.
        private static void LoadPlatesFromJSON(Game game, string jsonPath)
        {
            using (StreamReader r = new StreamReader(jsonPath))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    int[][] rows = new int[item.rows.Count][];

                    for (int row = 0; row < item.rows.Count; row++)
                    {
                        int[] rowItems = new int[item.rows[row].Count];
                        for (int col = 0; col < item.rows[row].Count; col++)
                        {
                            int number = int.Parse(item.rows[row][col].ToString());
                            rowItems[col] = number;
                        }
                        rows[row] = rowItems;
                    }
                    game.addPlate(new Plate(item.name.ToString()).LoadPlate(rows));
                }
            }
        }
    }
}
