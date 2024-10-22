using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public Plate BuildRandomPlate()
        {
            Console.WriteLine($"Plate ID: {this.id}");

            List<List<int>> allNumbers = this.BuildColNumbers();

            this.rows = this.buildRows(allNumbers);

            return this;
        }

        public string GetID()
        {
            return this.id;
        }

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
                    //Console.Write(number + " ");
                }
                //Console.WriteLine();
                allNumbers.Add(colNumbers);
            }
            //Console.WriteLine();

            return allNumbers;
        }

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

        public Plate PrintPlate()
        {
            return PrintPlate(new List<int>());
        }

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

        public bool hasBanko(List<int> numbersPicked, int rowsNeeded)
        {
            int bankoCount = 0;
            for (int row = 0; row < this.rows.Length; row++)
            {
                for (int col = 0; col < this.rows[row].Length; col++)
                {
                    int number = this.rows[row][col];

                    if (number > 0 && !numbersPicked.Contains(number)) break;
                    else if (col+1 == this.rows[row].Length) bankoCount++;
                }
            }
            return bankoCount >= rowsNeeded;
        }

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