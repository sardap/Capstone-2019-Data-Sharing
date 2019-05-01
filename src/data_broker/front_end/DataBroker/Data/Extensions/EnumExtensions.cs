using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBroker.Data.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDisplayName(this Enum genericEnum)
		{

			var genericEnumType = genericEnum.GetType();
			var memberInfo = genericEnumType.GetMember(genericEnum.ToString());

			if (memberInfo != null && memberInfo.Length > 0)
			{
				dynamic _Attribs = memberInfo[0]
					.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);

				if (_Attribs != null && _Attribs.Length > 0)
				{
					return ((System.ComponentModel.DataAnnotations.DisplayAttribute)_Attribs[0]).Name;
				}
			}

			return genericEnum.ToString();
		}
	}
}
