using ConsoleApp2.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Dto;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("1 - Fetch Data (GET)");
                Console.WriteLine("2 - Add Data (POST)");
                Console.WriteLine("3 - Update Data (PUT)");
                Console.WriteLine("4- Delete Data (DELETE)");
                Console.Write("Choice (1/2/3/4): ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Which data type should be fetched? (pokemon/move/owner/country/review/reviewer/category): ");
                        string type = Console.ReadLine();
                        await FetchAndPrintData(type);
                        break;

                    case "2":
                        Console.Write("What type of data do you want to add? (pokemon/move/owner/country/review/reviewer/category): ");
                        string postType = Console.ReadLine();
                        await PostData(postType);
                        await PostData(postType);
                        break;

                    case "3":
                        Console.Write("What type of data will be updated? (pokemon/move/owner/country/review/reviewer/category): ");
                        string putType = Console.ReadLine();
                        await PutData(putType);
                        break;

                    case "4":
                        Console.Write("What type of data will be deleted? (pokemon/move/owner/country/review/reviewer/category): ");
                        string deleteType = Console.ReadLine();
                        await DeleteData(deleteType);
                        break;


                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static async Task FetchAndPrintData(string type)
        {
            string baseUrl = "https://localhost:7295/api/";
            string fullUrl = baseUrl + type;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            try
            {
                HttpResponseMessage response = await client.GetAsync(fullUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    switch (type)
                    {
                        case "pokemon":
                            var pokemons = JsonConvert.DeserializeObject<List<PokemonDto>>(responseBody);
                            foreach (var p in pokemons)
                                Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, BirthDate: {p.BirthDate}");
                            break;

                        case "owner":
                            var owners = JsonConvert.DeserializeObject<List<OwnerDto>>(responseBody);
                            foreach (var o in owners)
                                Console.WriteLine($"ID: {o.Id}, FirstName: {o.FirstName}, LastName: {o.LastName}");
                            break;

                        case "move":
                            var moves = JsonConvert.DeserializeObject<List<MoveDto>>(responseBody);
                            foreach (var m in moves)
                                Console.WriteLine($"ID: {m.Id}, Name: {m.Name}, Type: {m.Type}");
                            break;

                        case "country":
                            var countries = JsonConvert.DeserializeObject<List<CountryDto>>(responseBody);
                            foreach (var c in countries)
                                Console.WriteLine($"ID: {c.Id}, Name: {c.Name}");
                            break;
                        case "category":
                            var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(responseBody);
                            foreach (var cat in categories)
                                Console.WriteLine($"ID: {cat.Id}, Name: {cat.Name}");
                            break;

                        case "reviewer":
                            var reviewers = JsonConvert.DeserializeObject<List<ReviewerDto>>(responseBody);
                            foreach (var rev in reviewers)
                                Console.WriteLine($"ID: {rev.Id}, FirstName: {rev.FirstName}, LastName: {rev.LastName}");
                            break;

                        case "review":
                            var reviews = JsonConvert.DeserializeObject<List<ReviewDto>>(responseBody);
                            foreach (var r in reviews)
                                Console.WriteLine($"ID: {r.Id}, Title: {r.Title}, Rating: {r.Rating}");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine($"API request failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        static async Task PutData(string type)
        {
            switch (type)
            {
                case "review":
                    await PutReview();
                    break;
                case "pokemon":
                    await PutPokemon();
                    break;
                case "move":
                    await PutMove();
                    break;
                case "owner":
                    await PutOwner();
                    break;
                case "country":
                    await PutCountry();
                    break;
                case "reviewer":
                    await PutReviewer();
                    break;
                case "category":
                    await PutCategory();
                    break;
                default:
                    Console.WriteLine("There is no PUT support for this type yet.");
                    break;
            }
        }

        static async Task DeleteData(string type)
        {
            Console.Write("ID to be deleted: ");
            int id = int.Parse(Console.ReadLine());

            string url = $"https://localhost:7295/api/{type}/{id}";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{type} successfully deleted.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        static async Task PostData(string type)
        {
            switch (type)
            {
                case "move":
                    await PostMove();
                    break;
                case "owner":
                    await PostOwner();
                    break;
                case "country":
                    await PostCountry();
                    break;
                 case "pokemon":
                    await PostPokemon();
                    break;
                case "category":
                    await PostCategory();
                    break;
                case "reviewer":
                    await PostReviewer();
                    break;
                case "review":
                    await PostReview();
                    break;

                default:
                    Console.WriteLine("Invalid POST type.");
                    break;
            }
        }


        static async Task PutReview()
        {
            Console.Write("Review ID to be updated: ");
            int reviewId = int.Parse(Console.ReadLine());
            Console.Write("New Title: ");
            string title = Console.ReadLine();
            Console.Write("New Text: ");
            string text = Console.ReadLine();
            Console.Write("New Rating (1–5): ");
            int rating = int.Parse(Console.ReadLine());

            var updatedReview = new ReviewDto
            {
                Id = reviewId,
                Title = title,
                Text = text,
                Rating = rating
            };

            string url = $"https://localhost:7295/api/review/{reviewId}";
            await SendPutRequest(url, updatedReview, "Review");
        }

        static async Task PutReviewer()
        {
            Console.Write("Reviewer ID to be updated: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New name: ");
            string firstName = Console.ReadLine();
            Console.Write("New last name: ");
            string lastName = Console.ReadLine();

            var reviewer = new ReviewerDto { Id = id, FirstName = firstName, LastName = lastName };
            string url = $"https://localhost:7295/api/reviewer/{id}";
            await SendPutRequest(url, reviewer, "Reviewer");
        }

        static async Task PutCategory()
        {
            Console.Write("Category ID to be updated: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Name: ");
            string name = Console.ReadLine();

            var category = new CategoryDto { Id = id, Name = name };
            string url = $"https://localhost:7295/api/category/{id}";
            await SendPutRequest(url, category, "Category");
        }

        static async Task PutCountry()
        {
            Console.Write("Country ID to be updated: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New name: ");
            string name = Console.ReadLine();

            var country = new CountryDto { Id = id, Name = name };
            string url = $"https://localhost:7295/api/country/{id}";
            await SendPutRequest(url, country, "Country");
        }

        static async Task PutOwner()
        {
            Console.Write("Owner ID to be updated: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New name: ");
            string firstName = Console.ReadLine();
            Console.Write("New lastname: ");
            string lastName = Console.ReadLine();
            Console.Write("New Gym: ");
            string gym = Console.ReadLine();

            var owner = new OwnerDto { Id = id, FirstName = firstName, LastName = lastName, Gym = gym };
            string url = $"https://localhost:7295/api/owner/{id}";
            await SendPutRequest(url, owner, "Owner");
        }

        static async Task PutMove()
        {
            Console.Write("Move ID to be updated: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("New Name: ");
            string name = Console.ReadLine();

            Console.Write("New type: ");
            string type = Console.ReadLine();

            Console.Write("New Power: ");
            int power = int.Parse(Console.ReadLine());

            Console.Write("New accuracy: ");
            int accuracy = int.Parse(Console.ReadLine());

            Console.Write("Pokemon ID: ");
            int pokemonId = int.Parse(Console.ReadLine());  

            var move = new MoveDto
            {
                Id = id,
                Name = name,
                Type = type,
                Power = power,
                Accuracy = accuracy,
                PokemonId = pokemonId
            };

            string url = $"https://localhost:7295/api/move/{id}";
            await SendPutRequest(url, move, "Move");
        }


        static async Task PutPokemon()
        {
            Console.Write("Pokemon ID to be updated: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New name: ");
            string name = Console.ReadLine();
            Console.Write("New birthdate (yyyy-MM-dd): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            var pokemon = new PokemonDto { Id = id, Name = name, BirthDate = birthDate };
            string url = $"https://localhost:7295/api/pokemon/{id}";
            await SendPutRequest(url, pokemon, "Pokemon");
        }

        static async Task SendPutRequest<T>(string url, T data, string entityName)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            using var client = new HttpClient(handler);

            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine($" {entityName} successfully updated.");
                else
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        static async Task PostCategory()
        {
            Console.Write("Category name: ");
            string name = Console.ReadLine();

            var category = new CategoryDto { Name = name };
            string url = "https://localhost:7295/api/category";

            await SendPostRequest(url, category, "Category");
        }

        static async Task PostReviewer()
        {
            Console.Write("Reviewer name: ");
            string firstName = Console.ReadLine();
            Console.Write("Reviewer last name: ");
            string lastName = Console.ReadLine();
            var reviewer = new ReviewerDto
            {
                FirstName = firstName,
                LastName = lastName
            };
            string url = "https://localhost:7295/api/reviewer";
            await SendPostRequest(url, reviewer, "Reviewer");
        }

        static async Task PostReview()
        {
            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Text: ");
            string text = Console.ReadLine();

            Console.Write("Rating (1–5): ");
            int rating = int.Parse(Console.ReadLine());

            Console.Write("Reviewer ID: ");
            int reviewerId = int.Parse(Console.ReadLine());

            Console.Write("Pokemon ID: ");
            int pokeId = int.Parse(Console.ReadLine());

            var review = new ReviewDto
            {
                Title = title,
                Text = text,
                Rating = rating
            };

            string url = $"https://localhost:7295/api/review?reviewerId={reviewerId}&pokeId={pokeId}";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            try
            {
                string jsonData = JsonConvert.SerializeObject(review);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Review successfully added.");
                else
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }


        static async Task SendPostRequest<T>(string url, T data, string entityName)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            using var client = new HttpClient(handler);
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{entityName} successfully added.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }


        static async Task PostOwner()
        {
            
            Console.Write("Owner name: ");
            string firstName = Console.ReadLine();

            Console.Write("Owner last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Gym name: ");
            string gym = Console.ReadLine();

            Console.Write("Country ID: ");
            int countryId = int.Parse(Console.ReadLine());

            
            var newOwner = new OwnerDto
            {
                FirstName = firstName,
                LastName = lastName,
                Gym = gym              
            };

            string url = $"https://localhost:7295/api/owner?countryId={countryId}";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            try
            {
                string jsonData = JsonConvert.SerializeObject(newOwner);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Owner successfully added.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }


        static async Task PostCountry()
        {
            Console.Write("Country name: ");
            string name = Console.ReadLine();
            var newCountry = new CountryDto
            {
                Name = name
            };
            string url = "https://localhost:7295/api/country";
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            using var client = new HttpClient(handler);
            try
            {
                string jsonData = JsonConvert.SerializeObject(newCountry);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Country successfully added.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        static async Task PostPokemon()
        {
            Console.Write("Pokemon name: ");
            string name = Console.ReadLine();
            Console.Write("Birthdate (yyyy-MM-dd): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Owner ID: ");
            int ownerId = int.Parse(Console.ReadLine());
            var newPokemon = new PokemonDto
            {
                Name = name,
                BirthDate = birthDate,
                Moves = new List<MoveDto>()
            };

            Console.Write("Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            string url = $"https://localhost:7295/api/pokemon?ownerId={ownerId}&catId={categoryId}";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            using var client = new HttpClient(handler);
            try
            {
                string jsonData = JsonConvert.SerializeObject(newPokemon);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Pokemon successfully added.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }


        static async Task PostMove()
        {
            // Kullanıcıdan veri al
            Console.Write("Move name: ");
            string name = Console.ReadLine();

            Console.Write("Move type: ");
            string type = Console.ReadLine();

            Console.Write("Power: ");
            int power = int.Parse(Console.ReadLine());

            Console.Write("Accuracy: ");
            int accuracy = int.Parse(Console.ReadLine());

            Console.Write("Pokemon ID: ");
            int pokemonId = int.Parse(Console.ReadLine());

            var newMove = new MoveDto
            {
                Name = name,
                Type = type,
                Power = power,
                Accuracy = accuracy,
                PokemonId = pokemonId
            };

            string url = "https://localhost:7295/api/move";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            try
            {
                string jsonData = JsonConvert.SerializeObject(newMove);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Move successfully added.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

    }
}
