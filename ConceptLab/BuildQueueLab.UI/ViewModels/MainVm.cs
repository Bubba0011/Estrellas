using Concepts.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BuildQueueLab.UI.ViewModels
{
	class MainVm : ViewModelBase
	{
		const string RulesFile = @"Data\Rules.xml";

		private Rules _rules;
		private Planet _planet;

		public PlanetVm CurrentPlanet { get; private set; }

		public ICommand UpdateCommand { get; private set; }

		// ctor
		private MainVm()
		{
			UpdateCommand = new RelayCommand(DoUpdate);

			_rules = Rules.LoadFromFile(RulesFile);

			_planet = CreatePlanet(_rules);
			CurrentPlanet = new PlanetVm(_planet, _rules);
		}

		internal static ViewModelBase CreateInstance()
		{
			try
			{
				return new MainVm();
			}
			catch (Exception ex)
			{
				return new ErrorVm(ex);
			}			
		}

		private void DoUpdate()
		{
			_planet.Update(_rules);
			CurrentPlanet = new PlanetVm(_planet, _rules);
			RaisePropertyChanged(() => CurrentPlanet);
		}

		private Planet CreatePlanet(Rules rules)
		{
			Planet planet = new Planet(rules, 10_000);
			planet.Name = "Planet Nein";
			planet.GrowthRate = 0.15m;
			planet.PopulationCap = 1_000_000;

			// Installations
			var mine = rules.PlanetaryInstallations.Single(pi => pi.Name == "Mine");
			var factory = rules.PlanetaryInstallations.Single(pi => pi.Name == "Factory");
			var bootcamp = rules.PlanetaryInstallations.Single(pi => pi.Name == "Bootcamp");
			var farm = rules.PlanetaryInstallations.Single(pi => pi.Name == "Farm");

			planet.Installations.Add(farm, 2);

			// Build queue
			planet.BuildQueue.Enqueue(farm, 5);
			planet.BuildQueue.Enqueue(mine, 5);
			planet.BuildQueue.Enqueue(factory, 5);
			planet.BuildQueue.Enqueue(farm, 5);
			planet.BuildQueue.Enqueue(mine, 10);
			planet.BuildQueue.Enqueue(factory, 10);
			planet.BuildQueue.Enqueue(farm, 5);
			planet.BuildQueue.Enqueue(mine, 15);
			planet.BuildQueue.Enqueue(factory, 15);
			planet.BuildQueue.Enqueue(farm, 5);

			return planet;
		}
	}
}
