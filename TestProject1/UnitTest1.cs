using Knapsack;

namespace TestProject1
{
    [TestClass]
    public class KnapsackTests
    {
        [TestMethod]
        public void TestForBruteForce01Knapsack()
        {
            Item[] items = {
                //Item(name, profit, weight)
            new Item("Item1", 10, 2),
            new Item("Item2", 5, 3),
            new Item("Item3", 15, 5),
            new Item("Item4", 7, 7),
            new Item("Item5", 6, 1),
            new Item("Item6", 18, 4),
            new Item("Item7", 3, 1)
            };
            float capacity = 15;

            Tuple<List<Item>, float> result = BruteForceAlgorithm.For01Knapsack(items, capacity);

            float expectedTotalProfit = 54;
            List<Item> expectedItems = new List<Item>
            {   
                items[0],
                items[2],
                items[5],
                items[4],
                items[1]
            };

            float actualTotalProfit = 0;
            foreach (Item item in result.Item1)
            {
                actualTotalProfit += item.Profit;
            }

            Assert.AreEqual(expectedTotalProfit, actualTotalProfit);
            CollectionAssert.AreEquivalent(expectedItems, result.Item1);


        }



        [TestMethod]
        public void TestForDynamic01Knapsack()
        {
            Item[] items = {
                //Item(name, profit, weight)
                new Item("Item1", 10, 2),
                new Item("Item2", 5, 3),
                new Item("Item3", 15, 5),
                new Item("Item4", 7, 7),
                new Item("Item5", 6, 1),
                new Item("Item6", 18, 4),
                new Item("Item7", 3, 1)
            };
            float capacity = 15;

            Tuple<List<Item>, float> result = DynamicProgramming.For01Knapsack(items, capacity);

            float expectedTotalProfit = 54;
            List<Item> expectedItems = new List<Item>
            {
                items[0],
                items[2],
                items[5],
                items[4],
                items[1]
            };

            float actualTotalProfit = 0;
            foreach (Item item in result.Item1)
            {
                actualTotalProfit += item.Profit;
            }

            Assert.AreEqual(expectedTotalProfit, actualTotalProfit);
            CollectionAssert.AreEquivalent(expectedItems, result.Item1);
        }



        [TestMethod]
        public void TestForGreedyFractionalKnapsack()
        {
            Item[] items = {
                //Item(name, profit, weight)
                new Item("Item1", 60, 10),
                new Item("Item2", 100, 20),
                new Item("Item3", 120, 30),
            };
            float capacity = 50;

            Tuple<List<Item>, float> result = GreedyAlgorithm.GreedyFractionalKnapsack(items, capacity);

            float expectedTotalProfit = 240;
            List<Item> expectedItems = new List<Item>
            {
                new Item("Item1", 60, 10),
                new Item("Item2", 100, 20),
                new Item("Item3", 80, 20),
            };

            float actualTotalProfit = 0;
            foreach (Item item in result.Item1)
            {
                actualTotalProfit += item.Profit;
            }

            Assert.AreEqual(expectedTotalProfit, actualTotalProfit);

            var expectedItemList = expectedItems.Select(i => new { i.Name, i.Profit, i.Weight }).ToList();
            var actualItemList = result.Item1.Select(i => new { i.Name, i.Profit, i.Weight }).ToList();

            CollectionAssert.AreEqual(expectedItemList, actualItemList);

        }



        [TestMethod]
        public void TestForBruteForceFractionalKnapsack()
        {
            Item[] items = {
                // Item(name, profit, weight)
                new Item("Item1", 60, 10),
                new Item("Item2", 110, 20),
                new Item("Item3", 120, 30),
            };
            float capacity = 50;

            Tuple<List<Item>, float> result = BruteForceAlgorithm.ForFractionalKnapsack(items, capacity);

            float expectedTotalProfit = 250;
            List<Item> expectedItems = new List<Item>
            {
                new Item("Item1", 60, 10),
                new Item("Item2", 110, 20),
                new Item("Item3", 80, 20),
            };

            float actualTotalProfit = 0;
            foreach (Item item in result.Item1)
            {
                actualTotalProfit += item.Profit;
            }

            Assert.AreEqual(expectedTotalProfit, actualTotalProfit);

            var expectedItemList = expectedItems.Select(i => new { i.Name, i.Profit, i.Weight }).ToList();
            var actualItemList = result.Item1.Select(i => new { i.Name, i.Profit, i.Weight }).ToList();

            CollectionAssert.AreEqual(expectedItemList, actualItemList);
        }
    }
}
