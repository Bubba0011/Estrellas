using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Concepts.Core
{
	public class Rules
	{
		public readonly ResourceRegistry Resources = new ResourceRegistry();

		public readonly DistrictTypeRegistry DistrictTypes = new DistrictTypeRegistry();

		public readonly List<PlanetaryInstallation> PlanetaryInstallations = new List<PlanetaryInstallation>();

		public static Rules LoadFromFile(string filename)
		{
			Rules rules = new Rules();

			var xRules = XDocument.Load(filename).Root;

			// Resources
			foreach (var xResource in xRules.XPathSelectElements("Resources/Resource"))
			{
				string name = (string)xResource.Attribute("name");
				bool transitory = (bool?)xResource.Attribute("is-transitory") ?? false;
				bool refined = (bool?)xResource.Attribute("is-refined") ?? false;
				decimal decay = (decimal?)xResource.Attribute("decay-rate") ?? 0;
				rules.Resources.AddResource(name, transitory, refined, decay);
			}

			// Resource versors
			foreach (var resource in rules.Resources.Items)
			{
				resource.Versor = rules.Resources.GetResourceVersor(resource);
			}

			// District types
			foreach (var xDistrictType in xRules.XPathSelectElements("Districts/Type"))
			{
				string name = (string)xDistrictType.Attribute("name");
				int pct = (int)xDistrictType.Attribute("pct");
				rules.DistrictTypes.Add(name, pct);
			}

			// PIs			
			foreach (var xPI in xRules.XPathSelectElements("PlanetaryInstallations/PI"))
			{
				var pi = new PlanetaryInstallation(xPI, rules);
				rules.PlanetaryInstallations.Add(pi);
			}

			return rules;
		}
	}
}
