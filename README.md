# ConsoleApp2 – .NET Console Client for Pokémon API

This is a .NET 8 console application that interacts with a RESTful Pokémon Web API built with ASP.NET Core. It allows users to perform full CRUD (Create, Read, Update, Delete) operations on various Pokémon-related entities via the command line.

## Features

- 📄 GET: Fetch all data for:
  - Pokémon
  - Moves
  - Owners
  - Countries
  - Categories
  - Reviewers
  - Reviews

- ➕ POST: Add new entries for all entities, including related foreign keys (e.g., ownerId, countryId).

- 🛠️ PUT: Update existing entries by ID for all entities.

- ❌ DELETE: Delete entries by ID for all entities.

- 🔐 HTTPS support via local development certificate.

## Technologies Used

- ✅ .NET 8 Console App
- ✅ Newtonsoft.Json for JSON serialization
- ✅ HttpClient for REST communication

## Requirements

- .NET 8 SDK
- A running instance of your ASP.NET Core Pokémon API (usually `https://localhost:7295`)

## How to Run

1. Make sure the API (`WebApplication1`) is running.
2. Run the console app:
   ```bash
   dotnet run
