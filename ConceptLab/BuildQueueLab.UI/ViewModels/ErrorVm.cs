using GalaSoft.MvvmLight;
using System;

namespace BuildQueueLab.UI.ViewModels
{
	class ErrorVm : ViewModelBase
	{
		public string Error { get; private set; }

		public ErrorVm(Exception ex)
		{
			Error = ex.ToString();
		}
	}
}
