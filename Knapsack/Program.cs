using System;
using System.Collections.Generic;

namespace Knapsack
{
    //class for Knapsack
    public class Knapsack
    {
        public float Capacity;
        public List<Item> Items;

        public Knapsack(float capacity)
        {
            Capacity = capacity;
            Items = new List<Item>();
        }
    }

    //class for Items to be put into the knapsack
    public class Item
    {
        public string Name;
        public float Profit;
        public float Weight;

        //used for Greedy Algorithm
        public float ProfitPerUnitWeight => Profit / Weight;

        public Item(string name, float profit, float weight)
        {
            Name = name;
            Profit = profit;
            Weight = weight;
        }
    }


    //Bruteforce Algorithm
    public class BruteForceAlgorithm
    {
        public static Tuple<List<Item>, float> For01Knapsack(Item[] items, float capacity)
        {
            float maxProfit = 0;
            List<Item> maxItems = new List<Item>();

            int n = items.Length;
            int numSubsets = (int)Math.Pow(2, n);

            for (int i = 0; i < numSubsets; i++)
            {
                List<Item> subset = new List<Item>();

                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        subset.Add(items[j]);
                    }
                }

                float totalWeight = 0;
                float totalProfit = 0;

                foreach (Item item in subset)
                {
                    totalWeight += item.Weight;
                    totalProfit += item.Profit;
                }

                if (totalWeight <= capacity && totalProfit > maxProfit)
                {
                    maxProfit = totalProfit;
                    maxItems = subset;
                }
            }

            return new Tuple<List<Item>, float>(maxItems, maxProfit);
        }

        public static Tuple<List<Item>, float> ForFractionalKnapsack(Item[] items, float capacity)
        {
            int n = items.Length;
            int numSubsets = (int)Math.Pow(2, n);

            float maxProfit = 0;
            List<Item> maxItems = new List<Item>();

            for (int i = 0; i < numSubsets; i++)
            {
                List<Item> subset = new List<Item>();

                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        subset.Add(items[j]);
                    }
                }

                float totalWeight = 0;
                float totalProfit = 0;

                foreach (Item item in subset)
                {
                    totalWeight += item.Weight;
                    totalProfit += item.Profit;
                }

                if (totalWeight <= capacity && totalProfit > maxProfit)
                {
                    maxProfit = totalProfit;
                    maxItems = subset;
                }
                else if (totalWeight > capacity)
                {
                    // calculate the remaining capacity and fractional profit as well as the fractional weight of the item used
                    float remainingCapacity = capacity - (totalWeight - subset[subset.Count - 1].Weight);
                    float fractionalProfit = subset[subset.Count - 1].Profit * (remainingCapacity / subset[subset.Count - 1].Weight);
                    float fractionalWeight = subset[subset.Count - 1].Weight * (remainingCapacity / subset[subset.Count - 1].Weight);

                    // check if the profit is higher 
                    if (totalProfit - subset[subset.Count - 1].Profit + fractionalProfit > maxProfit)
                    {
                        // update the maximum profit and selected items
                        maxProfit = totalProfit - subset[subset.Count - 1].Profit + fractionalProfit;
                        maxItems = subset;
                        // update the profit of the last item along with its weight in the selected items list
                        Item lastItem = maxItems[maxItems.Count - 1];
                        Item newItem = new Item(lastItem.Name, fractionalProfit, fractionalWeight);
                        maxItems[maxItems.Count - 1] = newItem;

                    }
                }
            }

            return new Tuple<List<Item>, float>(maxItems, maxProfit);
        }


    }


    //Greedy Algorithm
    public class GreedyAlgorithm
    {
        public static Tuple<List<Item>, float> GreedyFractionalKnapsack(Item[] items, float capacity)
        {
            int n = items.Length;

            // Sort items by profit per unit weight in descending order
            Array.Sort(items, (x, y) => y.ProfitPerUnitWeight.CompareTo(x.ProfitPerUnitWeight));

            float totalProfit = 0;
            float totalWeight = 0;
            List<Item> selectedItems = new List<Item>();

            for (int i = 0; i < n; i++)
            {
                if (totalWeight + items[i].Weight <= capacity)
                {
                    totalWeight += items[i].Weight;
                    totalProfit += items[i].Profit;
                    selectedItems.Add(items[i]);
                }
                else
                {
                    float remainingCapacity = capacity - totalWeight;
                    float fractionalProfit = items[i].ProfitPerUnitWeight * remainingCapacity;
                    totalWeight += remainingCapacity;
                    totalProfit += fractionalProfit;
                    selectedItems.Add(new Item(items[i].Name, fractionalProfit, remainingCapacity));
                    break;
                }
            }

            return new Tuple<List<Item>, float>(selectedItems, totalProfit);
        }

    }



    //Dynamic Programming
    public class DynamicProgramming
    {
        public static Tuple<List<Item>, float> For01Knapsack(Item[] items, float capacity)
        {
            int n = items.Length;

            // create a 2D array to store the maximum profit for each subproblem, and can later be used for common subproblems
            float[,] maxProfitTable = new float[n + 1, (int)capacity + 1];

            // build the maxProfitTable 2d matrix bottom-up
            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j <= capacity; j++)
                {
                    if (items[i - 1].Weight <= j)
                    {
                        // calculate the maximum profit by either including or excluding the current item
                        maxProfitTable[i, j] = Math.Max(maxProfitTable[i - 1, j], maxProfitTable[i - 1, j - (int)items[i - 1].Weight] + items[i - 1].Profit);
                    }
                    else
                    {
                        // if the current item weight exceeds the remaining capacity, do not include it
                        maxProfitTable[i, j] = maxProfitTable[i - 1, j];
                    }
                }
            }

            // here we can retrieve the selected items by backtracking through the maxProfitTable table
            List<Item> selectedItems = new List<Item>();
            int rowIndex = n;
            int colIndex = (int)capacity;

            while (rowIndex > 0 && colIndex > 0)
            {
                // check if the current item was included in the optimal solution
                if (maxProfitTable[rowIndex, colIndex] != maxProfitTable[rowIndex - 1, colIndex])
                {
                    selectedItems.Add(items[rowIndex - 1]);
                    colIndex -= (int)items[rowIndex - 1].Weight;
                }

                rowIndex--;
            }

            //reverse the selected items
            //easy while testing
            selectedItems.Reverse();

            // return the solution - contains the items as well as the maxprofit
            return new Tuple<List<Item>, float>(selectedItems, maxProfitTable[n, (int)capacity]);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //main function
        }
    }
}
