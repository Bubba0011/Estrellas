﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BuildQueueLab.Concepts
{
	class Rules
	{
		public readonly ResourceRegistry Resources = new ResourceRegistry();			

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
				rules.Resources.AddResource(name, transitory, refined);
			}

			// Resource versors
			foreach (var resource in rules.Resources.Items)
			{
				resource.Versor = rules.Resources.GetResourceVersor(resource);
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
