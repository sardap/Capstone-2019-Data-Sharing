using DataBroker.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBroker.Models.Views
{
	public class EnumFlagCheckboxModel
	{
		public int EnumValue { get; set; }
		public string DisplayValue { get; set; }
		public bool IsChecked { get; set; }
	}
}
