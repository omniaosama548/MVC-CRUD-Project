using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Password is Reqired")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
		[Required(ErrorMessage = "Password is Reqired")]
		[DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password doesn't match")]
		public string ConfirmNewPassword { get; set; }
    }
}
