using Newtonsoft.Json;

namespace ConsoleBankoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            //game.addPlate(new Plate("Bob").BuildPlate().PrintPlateTest());
            //game.addPlate(new Plate("Kurt").BuildPlate().PrintPlateTest());
            //game.addPlate(new Plate("Ib").BuildPlate().PrintPlateTest());

            //game.addPlate(new Plate("nhave").LoadPlate([
            //    [3,16,20,0,0,0,0,76,81],
            //    [0,0,24,32,44,0,65,77,0],
            //    [5,19,29,0,47,59,0,0,0]
            //]));
            //game.addPlate(new Plate("nhave1").LoadPlate([
            //    [5,0,24,30,41,51,0,0,0],
            //    [0,12,26,0,0,53,0,74,84],
            //    [0,14,27,0,0,56,66,75,0]
            //]));
            //game.addPlate(new Plate("nhave2").LoadPlate([
            //    [0,10,0,34,41,52,63,0,0],
            //    [8,13,25,36,0,0,0,74,0],
            //    [0,0,0,39,49,59,67,0,89]
            //]));
            //game.addPlate(new Plate("nhave3").LoadPlate([
            //    [0,11,20,31,43,0,64,0,0],
            //    [6,17,0,0,0,56,66,77,0],
            //    [0,18,22,0,0,0,68,79,90]
            //]));
            //game.addPlate(new Plate("nhave4").LoadPlate([
            //    [0,10,23,0,0,0,62,71,81],
            //    [6,0,0,0,43,0,64,75,84],
            //    [8,19,0,38,0,59,0,77,0]
            //]));

            //LoadPlatesFromJSON(game, "C:/Users/nhave/Code/c#/banko-nhave/ConsoleBankoApp/plates.json");

            WebIntegration.BuildPlatesFromWeb(game, "nhave", 100);

            game.run();
        }

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
