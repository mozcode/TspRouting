using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ExcelDataReader;
using GoogleApi;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TspRouting.WebUI.Entities;
using TspRouting.WebUI.Models;

namespace TspRouting.WebUI.Class
{
    public class Utilities
    {
        public static Random Random { get; set; }

        public static double[,] DistanceMatrix { get; set; }

        public static int NodeCount { get; set; }

        #region GetNodeList from txt file
        /// <summary>
        /// Reads from the text file. Starts to read when see "1" in first character of the line. Ends reading if first character of the number not increase properly.
        /// </summary>
        /// <param name="filePath">Path of the text file. </param>
        /// <returns></returns>
        public static List<Node> GetNodeListFromTextFile(string filePath = "")
        {
            if (filePath == "")
            {
                filePath = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!, "Files")).FirstOrDefault();
            }

            List<Node> nodes = new List<Node>();

            string[] lines = File.ReadAllLines(filePath);

            if (lines != null)
            {
                int number = 0;

                NumberFormatInfo numberFormat = new NumberFormatInfo { NegativeSign = "-", NumberDecimalSeparator = "." };
                NumberStyles numberStyles = NumberStyles.AllowTrailingSign | NumberStyles.Float | NumberStyles.AllowDecimalPoint;

                foreach (string line in lines)
                {
                    try
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Any())
                        {
                            if (parts[0] == "EOF")
                            {
                                break;
                            }

                            if (Int32.TryParse(parts[0], out int _))
                            {
                                if (number + 1 == Convert.ToInt32(parts[0]))
                                {
                                    nodes.Add(new Node(number,
                                        Double.Parse(parts[1], numberStyles, numberFormat),
                                        Double.Parse(parts[2], numberStyles, numberFormat)));

                                    number++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                //If nodes count is more than 1, than accept it and add first node value as new last node value.
                //Else do not accept and clear nodes
                if (number > 1)
                {
                    //Add first node to end cause of back to starting point
                    //nodes.Add(new Node(nodes.Count, nodes.First().X, nodes.First().Y));
                }
                else
                {
                    nodes.Clear();
                }
            }

            return nodes;
        }
        #endregion

        #region GetNodeList from file

        public static List<Node> GetNodeListFromFile(FileStream fileStream)
        {
            List<Node> nodes = new List<Node>();

            if (fileStream == null)
            {
                return nodes;
            }

            string[] lines = new string[] { };

            if (fileStream.Name.EndsWith(".txt"))
            {
                lines = File.ReadAllLines(fileStream.Name);
            }
            else if (fileStream.Name.EndsWith(".xls") || fileStream.Name.EndsWith(".xlsx"))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(fileStream))
                {
                    List<string> fileContent = new List<string>();

                    while (reader.Read()) //Each row of the file
                    {
                        fileContent.Add(String.Concat(reader.GetValue(0), " ", reader.GetValue(1), " ", reader.GetValue(2)));
                    }

                    lines = fileContent.ToArray();
                }
            }

            if (lines != null)
            {
                int number = 0;

                NumberFormatInfo numberFormat = new NumberFormatInfo { NegativeSign = "-", NumberDecimalSeparator = "." };
                NumberStyles numberStyles = NumberStyles.AllowTrailingSign | NumberStyles.Float | NumberStyles.AllowDecimalPoint;

                foreach (string line in lines)
                {
                    string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Any())
                    {
                        if (parts[0] == "EOF")
                        {
                            break;
                        }

                        if (Int32.TryParse(parts[0], out int _))
                        {
                            if (number + 1 == Convert.ToInt32(parts[0]))
                            {
                                nodes.Add(new Node(number,
                                    Double.Parse(parts[1], numberStyles, numberFormat),
                                    Double.Parse(parts[2], numberStyles, numberFormat)));

                                number++;
                            }
                        }
                    }
                }

                //If nodes count is more than 1, than accept it and add first node value as new last node value.
                //Else do not accept and clear nodes
                if (number > 1)
                {
                    //Add first node to end cause of back to starting point
                    //nodes.Add(new Node(nodes.Count, nodes.First().X, nodes.First().Y));
                }
                else
                {
                    nodes.Clear();
                }
            }

            return nodes;
        }
        #endregion
        
        #region GetFileName
        public static string GetFileName(IFormFile coordinateFile, IWebHostEnvironment webHostEnvironment)
        {
            string fileName;

            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "CoordinateFiles\\");

            if (coordinateFile != null)
            {
                fileName = Path.Combine(filePath, coordinateFile.FileName);

                //Delete if same named file exist
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                //Save file
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    coordinateFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
            else
            {
                fileName = Path.Combine(filePath, "default.xlsx");
            }

            return fileName;
        }
        #endregion

        #region Create random integer
        public static int[] CreateTwoDifferentRandomIntegers(int startIndex, int endIndex)
        {
            int[] r = new int[2];

            r[0] = Random.Next(startIndex, endIndex);

            do
            {
                r[1] = Random.Next(startIndex, endIndex);
            } while (r[0] == r[1]);

            return r;
        }

        public static int[] CreateDifferentRandomIntegers(int startIndex, int endIndex, int arrayLength)
        {
            #region HasDuplicate Function
            bool HasDuplicate(int[] numbers)
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                    for (int j = i + 1; j < numbers.Length; j++)
                    {
                        if (numbers[i] == numbers[j])
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            #endregion

            int[] r = new int[arrayLength];

            r[0] = Random.Next(startIndex, endIndex);

            do
            {
                for (int i = 1; i < arrayLength; i++)
                {
                    r[i] = Random.Next(startIndex, endIndex);
                }
            } while (HasDuplicate(r));

            return r;
        }
        #endregion

        #region GetGoogleDistanceMatrix
        public static double[,] GetGoogleDistanceMatrix(List<Node> nodes)
        {
            Location[] locations = (from n in nodes select new Location(n.Lat, n.Lng)).ToArray();

            double[,] distanceMatrix = new double[Utilities.NodeCount, Utilities.NodeCount];

            for (int i = 0; i < locations.Length; i++)
            {
                var request = new DistanceMatrixRequest
                {
                    Key = "AIzaSyAo-B0aWdLTIkk49iS8W7WjTL5nTkfIa_c",
                    OriginsRaw = locations[i].Latitude.ToString().Replace(',', '.') + "," + locations[i].Longitude.ToString().Replace(',', '.'),
                    Destinations = locations
                };

                var response = GoogleMaps.DistanceMatrix.Query(request);

                foreach (var row in response.Rows)
                {
                    int j = 0;
                    foreach (var element in row.Elements)
                    {
                        distanceMatrix[i, j] = element.Distance.Value;

                        j++;
                    }
                }
            }

            return distanceMatrix;
        }
        #endregion

        #region GetEuclideanDistanceMatrix
        public static double[,] GetEuclideanDistanceMatrix(List<Node> nodes, int nodeCount)
        {
            double[,] distanceMatrix = new double[nodes.Count, nodes.Count];

            for (var i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    distanceMatrix[i, j] = Math.Sqrt(Math.Pow((nodes[i].Lat - nodes[j].Lat), 2) + Math.Pow((nodes[i].Lng - nodes[j].Lng), 2));
                }
            }

            return distanceMatrix;
        }
        #endregion

        #region CalculateTotalDistance
        public static double CalculateTotalDistance(int[] nodes)
        {
            double totalDistance = 0;

            int i = 0;

            for (; i < nodes.Length - 1; i++)
            {
                totalDistance += DistanceMatrix[nodes[i], nodes[i + 1]];
            }

            //Add first and last node distance to complete circle
            totalDistance += DistanceMatrix[nodes[i], nodes[0]];

            return totalDistance;
        }
        #endregion

        #region CalculateFactorial
        public static int Factorial(int number)
        {
            int result = number;

            if (number > 1)
            {
                for (int i = number - 1; i > 1; i--)
                {
                    result = result * i;
                }
            }
            else
            {
                result = 1;
            }

            return result;
        }
        #endregion
    }
}
