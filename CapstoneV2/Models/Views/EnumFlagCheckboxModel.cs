﻿using CapstoneV2.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneV2.Models.Views
{
	public class EnumFlagCheckboxModel
	{
		public int EnumValue { get; set; }
		public string DisplayValue { get; set; }
		public bool IsChecked { get; set; }
	}
}
