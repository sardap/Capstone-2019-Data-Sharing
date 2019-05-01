using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataBroker.Models;
using DataBroker.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using DataBroker.Data.Extensions;
using DataBroker.Models.Views;

namespace DataBroker.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		public RegisterModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[Required]
			[DataType(DataType.Text)]
			[Display(Name = "Gender")]
			public string Gender { get; set; }

			[Required]
			[DataType(DataType.Date)]
			[Display(Name = "Date of birth")]
			public DateTime Birthday { get; set; }

			[Required]
			[DataType(DataType.Text)]
			[Display(Name = "Firstname")]
			public string Firstname { get; set; }

			[Required]
			[DataType(DataType.Text)]
			[Display(Name = "Lastname")]
			public string Lastname { get; set; }

			[DataType(DataType.PhoneNumber)]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }

			[Display(Name = "Ethnicities", Description = "Select one or more")]
			public IList<EnumFlagCheckboxModel> Ethnicities { get; set; } 
				= Enum.GetValues(typeof(Ethnicity)).Cast<Ethnicity>()
					.Select(z => 
						new EnumFlagCheckboxModel
						{
							EnumValue = (int)z,
							DisplayValue = z.GetDisplayName()
						}
					)
                    .OrderBy(z => z.DisplayValue)
                    .ToList();

            [Display(Name = "Medical Conditions")]
            public IList<MedicalCondition> MedicalConditions { get; set; } = new List<MedicalCondition>();
		}

		public void OnGet(string returnUrl = null)
		{
			Input = new InputModel();
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");
			if (ModelState.IsValid)
			{
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Firstname = Input.Firstname,
                    Lastname = Input.Lastname,
                    Birthday = Input.Birthday,
                    PhoneNumber = Input.PhoneNumber,
                    Gender = Input.Gender.ToLower(),
                    Ethnicities = Input.Ethnicities.Where(z => z.IsChecked).Select(z => (Ethnicity)z.EnumValue).Aggregate((e, j) => e | j),
                    MedicalConditions = Input.MedicalConditions.ToList()
				};

				var result = await _userManager.CreateAsync(user, Input.Password);
				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { userId = user.Id, code = code },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
