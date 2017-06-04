using Concepts.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BuildQueueLab.UI.ViewModels
{
	class PlanetVm : ViewModelBase
	{
		private Rules _rules;
		private Planet _planet;

		public string Name => _planet.Name;

		public int Population => _planet.Population;

		public int PopulationCapacity => _planet.PopulationCap;

		public decimal PopulationCapPct => Math.Round(100m * Population / PopulationCapacity, 1);

		public ResourceAmountVectorWrapper AvailableResources { get; private set; }

		public ConstructionQueueVm BuildQueue { get; private set; }

		public IEnumerable<InstallationStackWrapper> Installations { get; private set; }

		public IEnumerable<DistrictWrapper> Districts { get; private set; }

		public ICommand EnqueueInstallationCommand { get; private set; }

		public IEnumerable<string> EnqueueInstallationParams { get; private set; }

		// ctor
		public PlanetVm(Planet planet, Rules rules)
		{
			_rules = rules;
			_planet = planet;

			var next = _planet.GetProductionPreview(rules);
			AvailableResources = new ResourceAmountVectorWrapper(_planet.AvailableResources, next);

			BuildQueue = new ConstructionQueueVm(_planet.BuildQueue);

			Installations = _planet.Installations.Items.Select(stack => new InstallationStackWrapper(stack)).ToList();

			Districts = _planet.Districts.Select((d, n) => new DistrictWrapper(d, n + 1)).ToList();

			EnqueueInstallationCommand = new RelayCommand<string>(DoEnqueueInstallation, CanEnqueueInstallation);

			EnqueueInstallationParams = _rules.PlanetaryInstallations
				.Select(pi => pi.Name)
				.ToList();
		}

		private bool CanEnqueueInstallation(string name)
		{
			var design = _rules.PlanetaryInstallations.SingleOrDefault(pi => pi.Name == name);
			return design != null;
		}

		private void DoEnqueueInstallation(string name)
		{
			var design = _rules.PlanetaryInstallations.Single(pi => pi.Name == name);
			_planet.BuildQueue.Enqueue(design, 1);

			BuildQueue = new ConstructionQueueVm(_planet.BuildQueue);
			RaisePropertyChanged(() => BuildQueue);
		}
	}

	class ResourceAmountVectorWrapper : ObservableObject
	{
		public IEnumerable<ResourceAmountWrapper> Items { get; private set; }

		// ctor
		public ResourceAmountVectorWrapper(ResourceAmountVector current, Projection next)
		{	
			Items = current.Resources
				.Select(resource => new ResourceAmountWrapper(resource, current, next))
				.ToList();
		}
	}

	class ResourceAmountWrapper : ObservableObject
	{
		public string Name { get; private set; }

		public int CurrentAmount { get; private set; }

		public int NextAmount { get; private set; }

		public int ProducedAmount { get; private set; }

		public int ConsumedAmount { get; private set; }

		public int WastedAmount { get; private set; }

		// ctor
		public ResourceAmountWrapper(Resource resource, ResourceAmountVector current, Projection next)
		{
			Name = resource.Name;
			CurrentAmount = current[resource];
			NextAmount = next.Available[resource];
			ProducedAmount = next.Produced[resource];
			ConsumedAmount = next.Consumed[resource];
			WastedAmount = next.Wasted[resource];
		}
	}

	class InstallationStackWrapper : ObservableObject
	{
		public int Count { get; private set; }

		public string Name { get; private set; }

		// ctor
		public InstallationStackWrapper(PlanetaryInstallationStack stack)
		{
			Count = stack.Count;
			Name = stack.Installation.Name;
		}
	}

	class DistrictWrapper : ObservableObject
	{
		public int Id { get; private set; }

		public string[] Flags { get; private set; }

		public DistrictWrapper(District district, int id)
		{
			Id = id;
			Flags = district.Types.Select(dt => dt.Name).ToArray();
		}
	}
}
