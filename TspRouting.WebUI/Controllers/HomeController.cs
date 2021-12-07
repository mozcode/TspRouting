using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GoogleApi;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TspRouting.WebUI.Class;
using TspRouting.WebUI.Entities;
using TspRouting.WebUI.Models;

namespace TspRouting.WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region DI
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            GoogleMapsRouteViewModel routeViewModel = new GoogleMapsRouteViewModel();
            routeViewModel.GaEnvironments = new GaEnvironments();

            return View(routeViewModel);
        }

        [HttpPost]
        public IActionResult Index(GoogleMapsRouteViewModel routeViewModel)
        {
            #region Get Coordinates From File, Set and Check First Values
            Utilities.Random = new Random();

            routeViewModel.GaEnvironments ??= new GaEnvironments();

            using (FileStream fileStream = System.IO.File.Open(Utilities.GetFileName(routeViewModel.CoordinateFile, _webHostEnvironment), FileMode.Open, FileAccess.Read))
            {
                routeViewModel.Nodes = Utilities.GetNodeListFromFile(fileStream);
            }

            Utilities.NodeCount = routeViewModel.Nodes.Count;

            SetMapSettings(routeViewModel);

            Utilities.DistanceMatrix = Utilities.GetGoogleDistanceMatrix(routeViewModel.Nodes);
            #endregion

            EvolveWithGeneticAlgorithm(routeViewModel);

            return View(routeViewModel);
        }
        #endregion

        #region EvolveWithGeneticAlgorithm
        private void EvolveWithGeneticAlgorithm(GoogleMapsRouteViewModel routeViewModel)
        {
            int generation = 1;
            int tempGeneration = 1;
            bool better = true;
            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            GeneticOperations geneticOperations = new GeneticOperations();

            geneticOperations.CheckAndSetEnvironments(routeViewModel.GaEnvironments);

            Population population = geneticOperations.RandomizedPopulation(routeViewModel.Nodes.Select(x => x.No).ToArray(),
                routeViewModel.GaEnvironments.PopulationSize);

            routeViewModel.CostViewModels = new List<CostViewModel>();

            while (true)
            {
                if (tempGeneration * routeViewModel.GaEnvironments.IterationTerminationPercent / 100 < generation - tempGeneration && generation > 40)
                {
                    break;
                }

                #region Show route values
                if (better || generation == 1 || generation == routeViewModel.GaEnvironments.IterationNumber)
                {
                    if (generation == routeViewModel.GaEnvironments.IterationNumber)
                    {
                        break;
                    }

                    if (better)
                    {
                        better = false;
                    }
                }

                routeViewModel.CostViewModels.Add(new CostViewModel(generation - 1, population.MinimumCost));

                #endregion

                double tmpMaxFitness = population.MinimumCost;

                population = geneticOperations.Evolve(population, routeViewModel.GaEnvironments);

                if (population.MinimumCost < tmpMaxFitness)
                {
                    tempGeneration = generation;
                    better = true;
                }

                generation++;
            }

            watch.Stop();

            Route bestRoute = population.Routes.FirstOrDefault(x => x.TotalCost == population.MinimumCost);
            routeViewModel.Nodes = routeViewModel.Nodes.OrderBy(x => bestRoute.NodeNumber.ToList().FindIndex(i => i == x.No))
                .ToList();
            routeViewModel.Nodes.Add(routeViewModel.Nodes.FirstOrDefault());
            routeViewModel.DistanceMatrix = Utilities.DistanceMatrix;

            #region Set Statistical Results

            routeViewModel.Results = new Results
            {
                TotalCost = bestRoute.TotalCost,
                ExecutionTime = String.Format("min:sec:millis  {0:00}:{1:00}.{2}", watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds),
                LastBestIteration = tempGeneration,
                TotalIteration = generation
            };

            #endregion
        }
        #endregion

        #region MapSettings
        private void SetMapSettings(GoogleMapsRouteViewModel routeViewModel)
        {
            routeViewModel.MapCenter = new Node(0, routeViewModel.Nodes.Sum(x => x.Lat) / routeViewModel.Nodes.Count,
                routeViewModel.Nodes.Sum(x => x.Lng) / routeViewModel.Nodes.Count);

            double latDifference = routeViewModel.Nodes.Max(x => x.Lat) - routeViewModel.Nodes.Min(x => x.Lat);
            double lngDifference = routeViewModel.Nodes.Max(x => x.Lng) - routeViewModel.Nodes.Min(x => x.Lng);

            if (latDifference + lngDifference > 10)
            {
                routeViewModel.MapZoom = 4;
            }
            else if (latDifference + lngDifference > 1)
            {
                routeViewModel.MapZoom = 8;
            }
            else if (latDifference + lngDifference > 0.1)
            {
                routeViewModel.MapZoom = 12;
            }
            else if (latDifference + lngDifference > 0.01)
            {
                routeViewModel.MapZoom = 14;
            }
            else
            {
                routeViewModel.MapZoom = 16;
            }
        }
        #endregion
    }
}
