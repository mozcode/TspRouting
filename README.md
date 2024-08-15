# TSP Routing with Google Maps API

This repository contains a project focused on solving the Traveling Salesman Problem (TSP) using real-time data provided by the Google Maps API. The project includes both the core routing algorithms and a web interface for interacting with the routing system.

## Project Structure

The project is organized into the following directories:

- **GoogleApi/**: Contains the core logic for interacting with the Google Maps API, including functions to fetch real-time data such as distances and travel times.
- **TspRouting.WebUI/**: The web-based user interface for visualizing and interacting with the routing system. This component allows users to input locations, specify constraints, and view the optimized routes.

## Features

- **Real-Time Routing**: Utilizes Google Maps API to retrieve up-to-date information on distances and travel times.
- **TSP Solver**: Implements algorithms to solve the TSP problem, optimizing the route for a single salesman.
- **Web Interface**: A user-friendly web interface for inputting data and viewing results.
- **Scalability**: Designed to handle a large number of locations efficiently.

## Getting Started

### Prerequisites

To run this project, you will need the following:

- .NET Core SDK 6.0 or higher
- A valid Google Maps API key
- Visual Studio or another C# IDE

