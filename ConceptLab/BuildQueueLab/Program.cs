﻿using Concepts.Core;
using System;
using System.Linq;

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

				Planet planet = new Planet(rules, 10_000);
				planet.Name = "Planet Nein";
				planet.GrowthRate = 0.05m;
				planet.PopulationCap = 1_000_000;

				// Installations
				var mine = rules.PlanetaryInstallations.Single(pi => pi.Name == "Mine");
				var factory = rules.PlanetaryInstallations.Single(pi => pi.Name == "Factory");
				var bootcamp = rules.PlanetaryInstallations.Single(pi => pi.Name == "Bootcamp");
				
				planet.BuildQueue.Enqueue(mine, 5);
				planet.BuildQueue.Enqueue(factory, 5);
				planet.BuildQueue.Enqueue(mine, 5);
				planet.BuildQueue.Enqueue(factory, 5);
				planet.BuildQueue.Enqueue(mine, 5);
				planet.BuildQueue.Enqueue(factory, 5);

				foreach (int year in Enumerable.Range(2400, 15))
				{
					Console.WriteLine("\nYear " + year);
					Print(planet, rules);
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

		static void Print(Planet planet, Rules rules)
		{
			Console.WriteLine(planet.Name);
			Console.WriteLine($"  Population: {planet.Population}");
			Console.WriteLine($"  Resources:  {planet.AvailableResources} -> {planet.GetProductionPreview(rules)}");

			Console.WriteLine("  Installations:");
			foreach (var pi in planet.Installations.Items)
			{
				Console.WriteLine($"    {pi.Installation.Name}: {pi.Count}");
			}

			Console.WriteLine("  Build queue:");
			foreach (var order in planet.BuildQueue.Items)
			{
				Console.WriteLine($"    {order.Construction.Name} ({Math.Round(100 * order.ProgressPct, 0)}%) ({order.RemainingResources})");
			}
		}
	}
}
