using System.Security.Cryptography;
using System.Text;

namespace ConsoleBankoApp
{
    class Plate
    {
        private Random rand;
        private int[][] rows;
        private string id;

        public Plate(string id)
        {
            this.id = id;
            this.rand = new Random(GetSeedFromString(id));
        }

        public Plate LoadPlate(int[][] rows)
        {
            this.rows = rows;

            return this;
        }

        // Bygger en tilfældig plade, dog mangler der sortering af hver søjle.
        // Denne kode er skrevet, fordi jeg havde misfortået starten af selve opgaven.
        // Men jeg lader den ligge her for at vise den kode jeg har skrevet til det.
        public Plate BuildRandomPlate()
        {
            Console.WriteLine($"Plate ID: {this.id}");

            List<List<int>> allNumbers = this.BuildColNumbers();

            this.rows = this.buildRows(allNumbers);

            return this;
        }

        // Getter funktion til at få pladen id uden at kunne ændre den.
        public string GetID()
        {
            return this.id;
        }

        // Kode brugt til at generere lister med de numre som kan være i hver søjle.
        private List<List<int>> BuildColNumbers()
        {
            int[][] rowContents = [[1, 9], [10, 19], [20, 29], [30, 39], [40, 49], [50, 59], [60, 69], [70, 79], [80, 90]];
            List<List<int>> allNumbers = new List<List<int>>();

            for (int col = 0; col < rowContents.Length; col++)
            {
                List<int> colNumbers = new List<int>();
                for (int number = rowContents[col][0]; number <= rowContents[col][1]; number++)
                {
                    colNumbers.Add(number);
                }
                allNumbers.Add(colNumbers);
            }

            return allNumbers;
        }

        // Bygger rækkerne og henter tal fra listerne fået fra funktionen BuildColNumbers()
        private int[][] buildRows(List<List<int>> allNumbers)
        {
            int[][] rows = new int[3][];

            for (int rowNumber = 0; rowNumber < rows.Length; rowNumber++)
            {
                List<int> indexes = new List<int> { 0, 1, 3, 4, 5, 6, 7, 8 };
                int[] row = [0, 0, 0, 0, 0, 0, 0, 0, 0];

                for (int slotToFill = 0; slotToFill < 5; slotToFill++)
                {
                    int slotIndex = this.rand.Next(indexes.Count);
                    int slot = indexes[slotIndex];

                    List<int> slotNumbers = allNumbers[slot];

                    int colIndex = this.rand.Next(slotNumbers.Count);

                    row[slot] = slotNumbers[colIndex];

                    slotNumbers.RemoveAt(colIndex);
                    indexes.RemoveAt(slotIndex);
                }

                rows[rowNumber] = row;
            }

            return rows;
        }

        // Simpel funktion til at printe pladen til konsollen.
        // Kalder PrintPlate(numbersPicked) med en tom liste.
        public Plate PrintPlate()
        {
            return PrintPlate(new List<int>());
        }

        // Kode til at printe pladen til konsollen med XX i stedet for de numre der er blevet trukket.
        public Plate PrintPlate(List<int> numbersPicked)
        {
            for (int row = 0; row < this.rows.Length; row++)
            {
                for (int col = 0; col < this.rows[row].Length; col++)
                {
                    int number = this.rows[row][col];
                    string numberStr = number.ToString();
                    if (numberStr.Length < 2) numberStr = "0" + numberStr;

                    if (numbersPicked.Contains(number)) numberStr = "XX";

                    Console.Write($"{numberStr} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return this;
        }

        // Bruger en liste med numre trukket og rækker nødvendigt, og kontrollere om der er Banko på denne plade.
        public bool hasBanko(List<int> numbersPicked, int rowsNeeded)
        {
            int bankoCount = 0;
            for (int row = 0; row < this.rows.Length; row++)
            {
                for (int col = 0; col < this.rows[row].Length; col++)
                {
                    int number = this.rows[row][col];

                    if (number > 0 && !numbersPicked.Contains(number)) break;
                    else if (col + 1 == this.rows[row].Length) bankoCount++;
                }
            }
            return bankoCount >= rowsNeeded;
        }

        // Tidlig kode til at generere et seed til denne plades Random.
        public static int GetSeedFromString(string value)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return BitConverter.ToInt32(hashBytes, 0);
            }
        }
    }
}