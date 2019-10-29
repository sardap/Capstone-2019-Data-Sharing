using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBroker.Models
{
	public class UserTokenLinking
	{
		[Key]
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		[Column(TypeName = "varchar(100)")]
		public string PolicyCreationToken { get; set; }
		[Column(TypeName = "varchar(100)")]
		public string PolicyBlockchainLocation { get; set; }
	}
}