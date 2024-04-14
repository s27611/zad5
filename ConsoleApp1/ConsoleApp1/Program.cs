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
    public Animal Animal { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/animals")]
public class AnimalController : ControllerBase
{
    private static List<Animal> _animals = new List<Animal>();

    [HttpGet]
    public ActionResult<List<Animal>> GetAnimals()
    {
        return _animals;
    }

    [HttpGet("{id}")]
    public ActionResult<Animal> GetAnimal(int id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return NotFound();
        return animal;
    }

    [HttpPost]
    public ActionResult<Animal> AddAnimal(Animal animal)
    {
        _animals.Add(animal);
        return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}")]
    public ActionResult<Animal> UpdateAnimal(int id, Animal updatedAnimal)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return NotFound();
        animal.Name = updatedAnimal.Name;
        animal.Category = updatedAnimal.Category;
        animal.Weight = updatedAnimal.Weight;
        animal.FurColor = updatedAnimal.FurColor;
        return animal;
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteAnimal(int id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return NotFound();
        _animals.Remove(animal);
        return NoContent();
    }
}

[ApiController]
[Route("api/visits")]
public class VisitController : ControllerBase
{
    private static List<Visit> _visits = new List<Visit>();

    [HttpGet("{animalId}")]
    public ActionResult<List<Visit>> GetVisits(int animalId)
    {
        var visits = _visits.Where(v => v.Animal.Id == animalId).ToList();
        return visits;
    }

    [HttpPost]
    public ActionResult<Visit> AddVisit(Visit visit)
    {
        _visits.Add(visit);
        return CreatedAtAction(nameof(GetVisits), new { animalId = visit.Animal.Id }, visit);
    }
}
