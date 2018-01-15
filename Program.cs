using System;


/// <summary>
/// Example output of first version:
/// https://imgur.com/E9xeaYa
/// Latest improvements remove redundancies and reduce the running time of the algorithm. 
///
/// Command line args given:
/// "35" "4 3 6 5 3 7 9 1 10" "4 6 2 4 6 3 7 3 9"
/// 
/// Error checking might have worked more nicely within the KnapsackSolver function,
/// but it'll do for now...
/// </summary>


namespace Knapsack
{
    class Program
    {


        /// <summary>
        /// compare two integers for the maximum between them
        /// </summary>
        /// <param name="firstInt">the first integer passed in</param>
        /// <param name="secondInt">the second integer passed in</param>
        /// <returns></returns>
        static int Maximum(int firstInt, int secondInt)
        {
            if (firstInt > secondInt)
            {
                return firstInt;
            }
            return secondInt;
        }


        /// <summary>
        /// This is optional, but it should return the items that were actually used in the ideal knapsack.
        /// Includes index, weight, and value, as well as final weight and value
        /// 
        /// Based on:
        /// https://beckernick.github.io/dynamic-programming-knapsack/
        /// </summary>
        /// <param name="littleKnapsack">the n x m array that stores max capacity of a knapsack
        /// n = weight, m = num of items</param>
        /// <param name="numItems">the integer nunmber of items that were in the knapsack</param>
        /// <param name="remainingWeight">the max knapsack weight to test
        /// begins with the maximum knapsack weight and reduces as items are chosen</param>
        /// <param name="itemWeights">the weight of items, array</param>
        /// <param name="itemValues">the value of items, array</param>
        /// <returns></returns>
        static string IdealKnapsackString(int[,] littleKnapsack, int numItems, 
            int remainingWeight, int[] itemWeights, int[] itemValues)
        {
            // this part will make a string of indices of items that fit the knapsack
            int anItemsWeight = 0;

            string chosenItemString = "";
            int weightCounter = 0;
            int valueCounter = 0;

            for (int downCownter = numItems; downCownter > 0; downCownter--)
            {
                // if the jth item (index downcounter) is in the knapsack,
                // then the weight should change between the little knapsack's
                // j and j-1 values for the same weight.
                // Attach relevant info to the string of chosen items
                // Attached in reverse order to reflect reverse iteration
                // add values to weight and value counters

                // Old version: Remove the item, label the item as chosen, and continue;
                //no -> otherwise, give a dummy label//

                if (littleKnapsack[remainingWeight, downCownter] != littleKnapsack[remainingWeight, downCownter - 1])
                {
                    if (downCownter == 1) //final string
                    {
                        chosenItemString = $"{(downCownter - 1).ToString()} - Weight: {itemWeights[downCownter - 1]}\tValue: {itemValues[downCownter - 1]}"
                        + chosenItemString;
                    }
                    else //the final item might not be when downCounter is 1; that's the only thing
                    {
                        chosenItemString = $"\n{(downCownter - 1).ToString()} - Weight: {itemWeights[downCownter - 1]}\tValue: {itemValues[downCownter - 1]}"
                        + chosenItemString;
                    }
                    anItemsWeight = itemWeights[downCownter - 1];
                    weightCounter += anItemsWeight;
                    valueCounter += itemValues[downCownter - 1];

                    
                    remainingWeight = remainingWeight - anItemsWeight;
                }
            }

            
            //original code looks nicer, but uses an extra loop
            //for (int index = 0; index < chosenItems.Length; index++)
            //{
            //    if (chosenItems[index] == "Chosen")
            //    {
            //        chosenItemString += $"{index.ToString()} - Weight: {itemWeights[index]}\tValue: {itemValues[index]}\n";
            //        weightCounter += itemWeights[index];
            //        valueCounter += itemValues[index];
            //    }
            //}
            chosenItemString += $"\nTotal Weight: {weightCounter}\tTotal Value: {valueCounter}";
            return chosenItemString;
        }


        /// <summary>
        /// Book: Algorithms by Dasgupta/Padadimitriou/Vazirani
        /// Implements pseudocode algorithm from book pg. 182:
        /// """ Initialize all K(0, j) = 0 and all K(w, 0) = 0
        /// for j = 1 to n:
        ///     for w = 1 to W:
        ///         if w_j > w: K(w, j) = K(w, j − 1)
        ///         else: K(w, j) = max{K(w, j − 1); K(w − w_j, j − 1) + v_j}
        /// return K(W, n)"""
        /// 
        /// Idea: make a bunch of smaller subproblems; divide and conquer
        /// Alternative to book's "Initialize all K(0, j) = 0 and all K(w, 0) = 0":
        /// check if w or j are 0; if yes, return 0
        /// w_j = the weight of the jth item
        /// 
        /// j, substitute with itemNumberCounter
        /// w, substitute with weightIncrementer
        /// K, substitute with LittleKnapsack
        /// 
        /// LittleKnapsack tries to determine the largest value
        /// for the items 0 through j with max weight w
        /// 
        /// conditional explanations given in comments
        /// 
        /// note: the jth object in an array is at index j - 1
        /// </summary>
        /// <param name="maxKnapsackWeight">the maximum weight the knapsack can hold</param>
        /// <param name="itemWeights">ordered array of integers with weights of each item</param>
        /// <param name="itemValues">ordered array of integers with values of each item</param>
        /// <param name="numItems">the integer number of items to choose from</param>
        /// <returns></returns>
        static int KnapsackSolver(int maxKnapsackWeight, int[] itemWeights, int[] itemValues, int numItems)
        {
            int weightIncrementer;
            int itemNumberCounter;
            int[,] littleKnapsack = new int[maxKnapsackWeight + 1, numItems + 1];

            for (itemNumberCounter = 0; itemNumberCounter <= numItems; itemNumberCounter++)
            {
                for (weightIncrementer = 0; weightIncrementer <= maxKnapsackWeight; weightIncrementer++)
                {
                    // zero cases: if 0 items or no weight allowed,
                    // max value is 0 (0 items, 0 value)
                    if (weightIncrementer == 0 || itemNumberCounter == 0)
                    {
                        littleKnapsack[weightIncrementer, itemNumberCounter] = 0;
                    }

                    // if w_j > w: K(w, j) = K(w, j − 1)
                    // Explanation:
                    // if the weight of the jth object is larger than target,
                    // ignore the jth object; max value is that of the j-1
                    // objects preceding
                    else if (itemWeights[itemNumberCounter - 1] > weightIncrementer)
                    {
                        littleKnapsack[weightIncrementer, itemNumberCounter] =
                            littleKnapsack[weightIncrementer, 
                            itemNumberCounter - 1];
                    }

                    // else: K(w, j) = max{ K(w, j − 1), K(w − w_j, j − 1) + v_j}
                    // Explanation:
                    // if adding the value of the jth element is larger than the
                    // maximum value of the j-1 items before it while still max
                    // target weight, the second option is chosen, else first

                    else
                    {
                        littleKnapsack[weightIncrementer, itemNumberCounter] =
                            Maximum(littleKnapsack[weightIncrementer, itemNumberCounter - 1],
                            littleKnapsack[weightIncrementer - itemWeights[itemNumberCounter - 1],
                            itemNumberCounter - 1] + itemValues[itemNumberCounter - 1]);
                    }
                }
            }

            string solution = IdealKnapsackString(littleKnapsack, numItems, 
                maxKnapsackWeight, itemWeights, itemValues);
            Console.WriteLine(solution);


            return littleKnapsack[maxKnapsackWeight, numItems];
        }

        /// <summary>
        /// The main program. Receives input from either cmdline
        /// or user (if no arguments passed in). After rudimentary
        /// error checking (making sure that user input has an equal #
        /// of items in each to-be array), passes to the KnapsackSolver
        /// method
        /// 
        /// </summary>
        /// <param name="passedInArguments">0, 3, or 4 arguments required:
        /// - if 0, run a prompt for args
        /// The 3/4:
        /// 1. maximum weight of the knapsack
        /// 2. an array of item weights, space separated
        /// 3. an array of item values, space separated
        /// 4. (optional) the number of items
        /// 
        /// examples of acceptable args input:
        /// [20, 8 2 9 3 5 1, 3 4 3 6 1 2]
        /// [20, 8 2 9 3 5 1, 3 4 3 6 1 2, 6]
        /// (nothing, in which case a prompt will run)
        /// </param>
        static void Main(string[] passedInArguments)
        {
            bool running = true;
            while (running)
            {
                if (passedInArguments.Length == 0)
                {

                    //this part could have been refactored
                    int maxWeight = 0;
                    Console.WriteLine("maximum integer weight of the bag?");

                    string maxWeightStr = Console.ReadLine();
                    if (maxWeightStr.Length == 0)
                    {
                        //fail
                        Console.WriteLine("need valid integer weight");
                        Console.ReadLine();
                        Environment.Exit(39);
                    }
                    else
                    {
                        maxWeight = Convert.ToInt32(maxWeightStr);
                    }
                    Console.WriteLine(
                        "list space demarcated item weights, ",
                        "example '3 4 2 8 10' without the surrounding ''");
                    string[] itemWeightsString = Console.ReadLine().Split();

                    Console.WriteLine(
                        "list space demarcated item values, ",
                        "example '3 4 2 8 10' without the surrounding ''");
                    string[] itemValuesString = Console.ReadLine().Split();


                    int itemNumbers = itemWeightsString.Length;

                    if (itemValuesString.Length == itemNumbers)
                    {
                        //continue
                        int[] itemWeights = Array.ConvertAll(itemWeightsString, int.Parse);
                        int[] itemValues = Array.ConvertAll(itemValuesString, int.Parse);
                        int knapVal =
                            KnapsackSolver(maxWeight, itemWeights, itemValues, itemNumbers);
                        //Console.Write(knapVal);
                        Console.ReadLine();
                    }
                    else
                    {
                        //fail
                        Console.WriteLine("number of item weights, ",
                            itemWeightsString.Length,
                                "not equal to number of item values, ",
                                itemValuesString.Length);
                        Console.ReadLine();
                    }

                }
                else if (passedInArguments.Length == 3)
                {
                    string[] itemWeightsString = passedInArguments[1].Split();
                    string[] itemValuesString = passedInArguments[2].Split();

                    if (itemWeightsString.Length == itemValuesString.Length)
                    {
                        //continue
                        int[] itemWeights = Array.ConvertAll(itemWeightsString,
                            int.Parse);
                        int[] itemValues = Array.ConvertAll(itemValuesString,
                            int.Parse);
                        int knapVal =
                            KnapsackSolver(Convert.ToInt32(passedInArguments[0]), itemWeights,
                            itemValues, itemValues.Length);
                        //Console.Write(knapVal);
                        Console.ReadLine();
                    }
                    else
                    {
                        //fail
                        Console.WriteLine("number of item weights, ",
                            itemWeightsString.Length,
                                "not equal to number of item values, ",
                                itemValuesString.Length);
                        Console.ReadLine();
                    }
                }
                else if (passedInArguments.Length == 4)
                {
                    string[] itemWeightsString = passedInArguments[1].Split();
                    string[] itemValuesString = passedInArguments[2].Split();

                    if (itemWeightsString.Length == itemValuesString.Length)
                    {
                        int itemCount = Convert.ToInt32(passedInArguments[3]);
                        if (itemWeightsString.Length == itemCount)
                        {
                            //continue
                            int[] itemWeights = Array.ConvertAll(itemWeightsString,
                                int.Parse);
                            int[] itemValues = Array.ConvertAll(itemValuesString,
                                int.Parse);
                            int maxWeight = Convert.ToInt32(passedInArguments[0]);

                            int knapVal = KnapsackSolver(maxWeight, itemWeights,
                                itemValues, itemCount);
                            //Console.Write(knapVal); //this is now redundant
                            Console.ReadLine();
                        }
                        else
                        {
                            //fail
                            Console.WriteLine("number of items in array, ",
                                itemWeightsString.Length,
                                "not equal to number of items stated, ", itemCount);
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        //fail
                        Console.WriteLine("number of item weights, ",
                            itemWeightsString.Length,
                                "not equal to number of item values, ",
                                itemValuesString.Length);
                        Console.ReadLine();
                    }
                }
                else
                {
                    //number of args is not 0, 3, or 4
                    //fail
                    Console.WriteLine("need 0, 3, or 4 args");
                    Console.ReadLine();
                }


                //checks if user wants to run again
                //if yes, user needs to provide all new info
                Console.WriteLine("run again? y = yes (case sensitive), all others no");
                string rerun = Console.ReadLine();
                if (rerun != "y")
                {
                    running = false;
                }
                else
                {
                    //clear values (if necessary)
                    if (passedInArguments.Length > 0)
                    {
                        passedInArguments = new string[0];
                    }
                    
                }
            }
        }
    }
}
