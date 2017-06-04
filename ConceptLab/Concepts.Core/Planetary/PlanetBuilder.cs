using System;

namespace Concepts.Core
{
	public static class PlanetBuilder
	{
		public static Planet Build(Rules rules, int population, int districtCount)
		{
			Random rnd = new Random(123456);

			Planet planet = new Planet(rules, population);

			for (int i = 0; i < districtCount; i++)
			{
				District district = new District();

				foreach (DistrictType type in rules.DistrictTypes.Items)
				{
					if (rnd.Next(0, 100) < type.Pct)
					{
						district.Types.Add(type);
					}
				}

				planet.Districts.Add(district);
			}

			return planet;
		}
	}
}
