using System;
using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
	public class RegisterViewModel
	{
		[Display(Name ="Email Address")]
		[Required(ErrorMessage = "Please do not leave the email field blank")]
		public string EmailAddress { get; set; }
		[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please re-enter your password to confirm")]
        [DataType(DataType.Password)]
		[Compare("Password",
			ErrorMessage = "The confirmed password does not match. " +
			"Please make sure both passwords are the same")]
        public string ConfirmPassword { get; set; }
	}
}

