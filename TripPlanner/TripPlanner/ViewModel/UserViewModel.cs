using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TripPlanner.ViewModel
{
    public class UserViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please enter a valid email")]
        [JsonProperty("Email")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
        [MinLength(7, ErrorMessage = "Password must be at least 7 characters")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*#?&])[A-Za-z\\d$@$!%*#?&]{7,}$", ErrorMessage = "Password must contain lower case and upper case letters, at least one digit and a special character")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please repeat your password")]
        [JsonProperty("ConfirmPassword")]
        public string RepeatPassword { get; set; }
    }
}
