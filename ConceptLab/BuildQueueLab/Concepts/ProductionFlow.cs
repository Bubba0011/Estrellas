namespace BuildQueueLab.Concepts
{
	class ProductionFlow
	{
		public readonly ResourceAmountVector Input;

		public readonly ResourceAmountVector Output;

		public ResourceAmountVector NetFlow
		{
			get { return Output - Input; }
		}

		// ctor
		public ProductionFlow(ResourceAmountVector input, ResourceAmountVector output)
		{
			Input = input;
			Output = output;
		}

		public ProductionFlow(ResourceAmountVector output)
		{
			Input = output * 0;
			Output = output;
		}
	}
}
