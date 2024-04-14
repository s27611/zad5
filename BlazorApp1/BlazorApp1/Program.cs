using BlazorApp1.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using AnimalShelter;





namespace AnimalShelter
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Weight { get; set; }
        public string FurColor { get; set; }
    }

    public class Visit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public int AnimalId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public static class AnimalData
    {
        public static List<Animal> Animals { get; } = new List<Animal>
        {
            new Animal { Id = 1, Name = "Burek", Category = "Pies", Weight = 10.5, FurColor = "Brązowy" },
            new Animal { Id = 2, Name = "Mruczek", Category = "Kot", Weight = 5.2, FurColor = "Czarny" },
            new Animal { Id = 3, Name = "Reksio", Category = "Pies", Weight = 8.0, FurColor = "Biały" },
            new Animal { Id = 4, Name = "Filemon", Category = "Kot", Weight = 6.8, FurColor = "Szary" },
            new Animal { Id = 5, Name = "Rufus", Category = "Pies", Weight = 12.3, FurColor = "Brązowy" },
            new Animal { Id = 6, Name = "Luna", Category = "Pies", Weight = 6.0, FurColor = "Czarny" },
            new Animal { Id = 7, Name = "Fiona", Category = "Kot", Weight = 4.5, FurColor = "Biały" },
            new Animal { Id = 8, Name = "Max", Category = "Pies", Weight = 9.8, FurColor = "Szary" }
        };

        public static List<Visit> Visits { get; } = new List<Visit>();
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class AnimalController
    {
        [HttpGet("/api/animals")]
        public void GetAnimals(HttpContext context)
        {
            var animals = AnimalData.Animals;
            var json = JsonConvert.SerializeObject(animals);
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(json);
        }

        [HttpGet("/api/animals/{id}")]
        public void GetAnimal(HttpContext context, int id)
        {
            var animal = AnimalData.Animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var json = JsonConvert.SerializeObject(animal);
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(json);
        }

        [HttpPost("/api/animals")]
        public void AddAnimal(HttpContext context)
        {
            var requestBody = context.Request.Body;
            var requestBodyString = new System.IO.StreamReader(requestBody).ReadToEnd();
            var newAnimal = JsonConvert.DeserializeObject<Animal>(requestBodyString);
            AnimalData.Animals.Add(newAnimal);
            context.Response.StatusCode = 201;
        }

        [HttpPut("/api/animals/{id}")]
        public void UpdateAnimal(HttpContext context, int id)
        {
            var animalToUpdate = AnimalData.Animals.FirstOrDefault(a => a.Id == id);
            if (animalToUpdate == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var requestBody = context.Request.Body;
            var requestBodyString = new System.IO.StreamReader(requestBody).ReadToEnd();
            var updatedAnimal = JsonConvert.DeserializeObject<Animal>(requestBodyString);
            animalToUpdate.Name = updatedAnimal.Name;
            animalToUpdate.Category = updatedAnimal.Category;
            animalToUpdate.Weight = updatedAnimal.Weight;
            animalToUpdate.FurColor = updatedAnimal.FurColor;
            context.Response.StatusCode = 204; 
        }

        [HttpDelete("/api/animals/{id}")]
        public void DeleteAnimal(HttpContext context, int id)
        {
            var animalToDelete = AnimalData.Animals.FirstOrDefault(a => a.Id == id);
            if (animalToDelete == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            AnimalData.Animals.Remove(animalToDelete);
            context.Response.StatusCode = 204;
        }
    }

    public class VisitController
    {
        [HttpGet("/api/visits/{animalId}")]
        public void GetVisits(HttpContext context, int animalId)
        {
            var visitsForAnimal = AnimalData.Visits.Where(v => v.AnimalId == animalId).ToList();
            var json = JsonConvert.SerializeObject(visitsForAnimal);
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(json);
        }

        [HttpPost("/api/visits")]
        public void AddVisit(HttpContext context)
        {
            var requestBody = context.Request.Body;
            var requestBodyString = new System.IO.StreamReader(requestBody).ReadToEnd();
            var newVisit = JsonConvert.DeserializeObject<Visit>(requestBodyString);
            AnimalData.Visits.Add(newVisit);
            context.Response.StatusCode = 201;
        }
    }
    

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000"); // Ustawienie numeru portu na 5000
                });
    }
}