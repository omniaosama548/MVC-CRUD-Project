using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterViewModel
	{
        [Required(ErrorMessage ="FName is Required")]
        public string FName { get; set; }
		[Required(ErrorMessage = "LName is Required")]
		public string LName { get; set; }
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage ="Invaild Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "ConfirmPassword is Required")]
		[Compare("Password",ErrorMessage ="Password doesn't Match")]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
