using BuildQueueLab.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildQueueLab
{
	class Program
	{
		const string RulesFile = @"Data\Rules.xml";

		static void Main(string[] args)
		{
			try
			{
				Rules rules = Rules.LoadFromFile(RulesFile);
				//Print(rules);

				Planet planet = new Planet(rules);
				planet.Name = "Planet Nein";
				planet.GrowthRate = 0.05m;
				planet.PopulationCap = 1_000_000;
				planet.Population = 10_000;

				// Installations
				var mine = rules.PlanetaryInstallations.Single(pi => pi.Name == "Mine");
				var factory = rules.PlanetaryInstallations.Single(pi => pi.Name == "Factory");
				var bootcamp = rules.PlanetaryInstallations.Single(pi => pi.Name == "Bootcamp");
				planet.Installations.Add(mine, 5);
				planet.Installations.Add(factory, 5);
				planet.Installations.Add(bootcamp, 1);

				foreach (int year in Enumerable.Range(2400, 20))
				{
					Console.WriteLine("\nYear " + year);
					Print(planet);
					planet.Update(rules);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}

		static void Print(Rules rules)
		{
			Console.WriteLine("Resources");
			foreach (var resource in rules.Resources.Items)
			{
				Console.WriteLine($"  {resource.Name} {resource.Versor}");
			}

			Console.WriteLine("\nPlanetary installations");
			foreach (var pi in rules.PlanetaryInstallations)
			{
				Console.WriteLine($"  {pi.Name}");
				Console.WriteLine($"    Construct: {pi.ConstructionCost}");
				Console.WriteLine($"    Operate:   {pi.OperationCost}");
				Console.WriteLine($"    Produces:  {pi.ProductionInput} -> {pi.ProductionOutput}");
			}
		}

		static void Print(Planet planet)
		{
			Console.WriteLine(planet.Name);
			Console.WriteLine($"  Population: {planet.Population}");
			Console.WriteLine($"  Resources:  {planet.AvailableResources}");
		}
	}
}
