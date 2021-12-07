using System;
using System.Collections.Generic;
using System.Linq;
using TspRouting.WebUI.Entities;

namespace TspRouting.WebUI.Class
{
    public class GeneticOperations
    {
        #region RandomizedPopulation
        public Population RandomizedPopulation(int[] nodes, int populationSize)
        {
            List<Route> routes = new List<Route> { new Route(nodes, Utilities.DistanceMatrix) };

            for (int i = 0; i < populationSize - 1;)
            {
                Route routeNew = Shuffle(nodes);

                if (!routes.Exists(x => x.TotalCost == routeNew.TotalCost))
                {
                    routes.Add(routeNew);

                    i++;
                }
            }

            return new Population(routes);
        }
        #endregion

        #region Shuffle
        public Route Shuffle(int[] nodes)
        {
            int[] newNodes = (int[])nodes.Clone();

            for (int i = 0; i < Utilities.NodeCount; i++)
            {
                int rnd = Utilities.Random.Next(0, i);
                int tmpNode = newNodes[rnd];
                newNodes[rnd] = newNodes[i];
                newNodes[i] = tmpNode;
            }

            return new Route(newNodes, Utilities.DistanceMatrix);
        }
        #endregion

        #region Evolve
        public Population Evolve(Population population, GaEnvironments environments)
        {
            List<Route> routes = new List<Route>(Elite(population, environments.Elitism).Routes);

            for (int i = 0; i < environments.PopulationSize - environments.Elitism;)
            {
                int parent1 = TournamentSelection(population, environments.PopulationSize, environments.RandomIntegerCount);
                int parent2 = TournamentSelection(population, environments.PopulationSize, environments.RandomIntegerCount);

                while (parent1 == parent2 && environments.PopulationSize > 3)
                {
                    parent2 = TournamentSelection(population, environments.PopulationSize, environments.RandomIntegerCount);
                }

                Route childRoute = OrderCrossover(population.Routes[parent1].NodeNumber, population.Routes[parent2].NodeNumber);

                if (Utilities.Random.NextDouble() < environments.MutationRate)
                {
                    childRoute = Mutate(childRoute);
                }

                if (Utilities.Random.NextDouble() < environments.TwoOptRate)
                {
                    childRoute = TwoOpt(childRoute);
                }

                if (!routes.Exists(x => x.TotalCost == childRoute.TotalCost))
                {
                    routes.Add(childRoute);

                    i++;
                }
            }
            return new Population(routes);
        }
        #endregion

        #region Elite
        Population Elite(Population population, int elitism)
        {
            return new Population(population.Routes.OrderBy(x => x.TotalCost).Take(elitism).ToList());
        }
        #endregion

        #region Selection
        int TournamentSelection(Population population, int populationSize, int randomNumberCount)
        {
            int[] r = Utilities.CreateDifferentRandomIntegers(0, populationSize - 1, randomNumberCount);

            int selectedIndex = r[0];

            for (var i = 1; i < r.Length; i++)
            {
                if (population.Routes[r[i]].TotalCost < population.Routes[selectedIndex].TotalCost)
                {
                    selectedIndex = r[i];
                }
            }

            return selectedIndex;
        }
        #endregion

        #region Crossovers
        int[] Order1Crossover(int[] parent1, int[] parent2)
        {
            int[] rands = Utilities.CreateTwoDifferentRandomIntegers(0, Utilities.NodeCount - 1);
            Array.Sort(rands);

            //Mid part
            int[] part1 = new int[rands[1] - rands[0] + 1];
            Array.Copy(parent1, rands[0], part1, 0, rands[1] - rands[0] + 1);

            //Combine Start with Mid
            int[] part2 = new int[rands[1] + 1];
            Array.Copy(parent2.Except(part1).Take(rands[0]).ToArray(), part2, rands[0]);
            Array.Copy(part1, 0, part2, rands[0], rands[1] - rands[0] + 1);

            //Combine all
            int[] child = new int[Utilities.NodeCount];
            Array.Copy(part2, child, rands[1] + 1);
            Array.Copy(parent2.Except(part2).ToArray(), 0, child, rands[1] + 1, Utilities.NodeCount - rands[1] - 1);

            return child;
        }

        Route OrderCrossover(int[] parent1, int[] parent2)
        {
            int[] rnd = Utilities.CreateTwoDifferentRandomIntegers(0, Utilities.NodeCount - 1);
            Array.Sort(rnd);

            int[] parent1MidPart = new int[rnd[1] - rnd[0] + 1];
            Array.Copy(parent1, rnd[0], parent1MidPart, 0, rnd[1] - rnd[0] + 1);

            int[] parent2Remaining = parent2.Skip(rnd[1] + 1).Except(parent1MidPart)
                .Concat(parent2.Take(rnd[1] + 1).Except(parent1MidPart)).ToArray();

            int[] parent1MidAndLast = parent1MidPart.Concat(parent2Remaining.Take(Utilities.NodeCount - rnd[1] - 1)).ToArray();

            int[] childNew = parent2Remaining.Skip(Utilities.NodeCount - rnd[1] - 1).Concat(parent1MidAndLast).ToArray();

            return new Route(childNew, Utilities.DistanceMatrix);
        }

        int[] ErCrossover(int[] parent1, int[] parent2)
        {
            int[] CopyArrayByRollingFirstAndLast(int[] parentArray)
            {
                int[] newInts = new int[Utilities.NodeCount + 2];
                newInts[0] = parentArray[Utilities.NodeCount - 1];
                newInts[Utilities.NodeCount + 1] = parentArray[0];
                Array.Copy(parentArray, 0, newInts, 1, Utilities.NodeCount);
                return newInts;
            }

            int[] tempParent1 = CopyArrayByRollingFirstAndLast(parent1);
            int[] tempParent2 = CopyArrayByRollingFirstAndLast(parent2);

            List<ErNeighbor> erNeighbors = new List<ErNeighbor>();

            for (int indexP1 = 1; indexP1 < Utilities.NodeCount + 1; indexP1++)
            {
                ErNeighbor erNeighbor = new ErNeighbor(tempParent1[indexP1]);

                erNeighbor.Neighbors.Add(tempParent1[indexP1 - 1]);
                erNeighbor.Neighbors.Add(tempParent1[indexP1 + 1]);

                int indexP2 = Array.IndexOf(parent2, tempParent1[indexP1]);
                erNeighbor.AddNeighborIfNotExistInFirst2Index(tempParent2[indexP2]);
                erNeighbor.AddNeighborIfNotExistInFirst2Index(tempParent2[indexP2 + 2]);

                erNeighbors.Add(erNeighbor);
            }

            int[] result = new int[Utilities.NodeCount];

            ErNeighbor neighbor = erNeighbors.First();

            int i = 0;

            while (i < Utilities.NodeCount)
            {
                result[i] = neighbor.IndexValue;

                foreach (var erNeighbor in erNeighbors)
                {
                    erNeighbor.DeleteNeighbor(neighbor.IndexValue);
                }

                ErNeighbor tempErNeighbor;

                if (neighbor.NeighborCount > 0)
                {
                    List<ErNeighbor> subErNeighbors = erNeighbors.Where(x => neighbor.Neighbors.Contains(x.IndexValue))
                        .OrderBy(x => x.NeighborCount).ThenByDescending(x => x.CommonCount).ToList();

                    tempErNeighbor = subErNeighbors.First();

                    List<ErNeighbor> subBestErNeighbors = subErNeighbors.Where(x =>
                        x.NeighborCount == tempErNeighbor.NeighborCount && x.CommonCount == tempErNeighbor.CommonCount).ToList();

                    if (subBestErNeighbors.Count > 1)
                    {
                        tempErNeighbor = subBestErNeighbors.OrderBy(s => Utilities.Random.NextDouble()).First();
                    }

                    erNeighbors.Remove(neighbor);
                }
                else
                {
                    erNeighbors.Remove(neighbor);

                    tempErNeighbor = erNeighbors.OrderBy(s => Utilities.Random.NextDouble()).FirstOrDefault();
                }

                neighbor = tempErNeighbor;

                i++;
            }

            return result;
        }

        int[] ModifiedOrderCrossover(int[] parent1, int[] parent2)
        {
            int rand = Utilities.Random.Next(0, Utilities.NodeCount - 1);

            int[] parent2LastPart = parent2.Skip(rand + 1).ToArray();

            int[] child = (int[])parent1.Clone();

            int counter = 0;

            for (var i = 0; i < Utilities.NodeCount; i++)
            {
                if (parent2LastPart.Contains(child[i]))
                {
                    child[i] = parent2LastPart[counter];

                    counter++;
                }
            }

            return child;
        }
        #endregion

        #region Mutate
        Route Mutate(Route route)
        {
            int[] rands = Utilities.CreateTwoDifferentRandomIntegers(0, Utilities.NodeCount);

            int tmpNode = route.NodeNumber[rands[0]];
            route.NodeNumber[rands[0]] = route.NodeNumber[rands[1]];
            route.NodeNumber[rands[1]] = tmpNode;

            route.TotalCost = Utilities.CalculateTotalDistance(route.NodeNumber);

            return route;
        }
        #endregion

        #region TwoOpt
        public Route TwoOpt(Route route)
        {
            #region TwoOptSwap
            int[] TwoOptSwap(int[] myNodes, int i, int k)
            {
                int[] swappedNodes = new int[Utilities.NodeCount];

                Array.Copy(myNodes, swappedNodes, Utilities.NodeCount);
                Array.Reverse(swappedNodes, i, k - i + 1);

                return swappedNodes;
            }
            #endregion

            for (int i = 0; i < Utilities.NodeCount - 1; i++)
            {
                for (int k = i + 1; k < Utilities.NodeCount; k++)
                {
                    Route newRoute = new Route(TwoOptSwap(route.NodeNumber, i, k), Utilities.DistanceMatrix);

                    if (newRoute.TotalCost < route.TotalCost)
                    {
                        route = newRoute;
                    }
                }
            }

            return route;
        }
        #endregion

        #region CheckAndSetEnvironments
        public void CheckAndSetEnvironments(GaEnvironments environments)
        {
            if (Utilities.NodeCount < 9 && Utilities.Factorial(Utilities.NodeCount - Utilities.NodeCount / 2) <
                environments.PopulationSize)
            {
                environments.PopulationSize = Utilities.Factorial(Utilities.NodeCount / 2);
            }

            if (environments.IterationNumber > environments.PopulationSize * 5)
            {
                environments.IterationNumber = environments.PopulationSize * 5;
            }

            while (environments.Elitism > 1 && environments.Elitism * 5 >= environments.PopulationSize)
            {
                environments.Elitism = environments.Elitism / 2;
            }

            if (environments.RandomIntegerCount * 2 > environments.PopulationSize)
            {
                if (environments.PopulationSize < 4)
                {
                    environments.RandomIntegerCount = 1;
                }
                else
                {
                    environments.RandomIntegerCount = 2;
                }
            }
        }

        #endregion
    }
}
