using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace lms.elegalhubs.com.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly string apiBaseUrl;
        private readonly string projectID;
        private readonly IConfiguration _Configure;
        public IndexModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            projectID = _Configure.GetValue<string>("ProjectID");
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            [Required]
            public string PhoneNumber { get; set; }
            [Required]
            public string FullName { get; set; }
            public string Address { get; set; }
        }

        private async Task LoadAsync(Users user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fullName =  _userManager.Users.Where(p => p.UserName == userName).First().FullName;
            var address =  _userManager.Users.Where(p => p.UserName == userName).First().Address;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
                Address = address
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != user.PhoneNumber)
                user.PhoneNumber = Input.PhoneNumber;
            if (Input.FullName != user.FullName)
                user.FullName = Input.FullName;
            if (Input.Address != user.Address)
                user.Address = Input.Address;

            var setResult = await _userManager.UpdateAsync(user);
            if (!setResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent user1 = new StringContent(JsonConvert.SerializeObject(new User
                    {
                        UserId = user.Id,
                        ProjectId = projectID,//admin
                        UserName = user.UserName,
                        MobileNumber = user.PhoneNumber,
                        EmailAddress = user.Email,
                        Name = user.FullName

                    }), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiBaseUrl + "/Users", user1))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            User retuser = JsonConvert.DeserializeObject<User>(apiResponse);

                            StringContent user2 = new StringContent(JsonConvert.SerializeObject(new User
                            {
                                UserId = user.Id,
                                ProjectId = projectID,//admin
                                UserName = user.UserName,
                                MobileNumber = user.PhoneNumber,
                                EmailAddress = user.Email,
                                Name = user.FullName,
                                Id = retuser.Id
                            }), Encoding.UTF8, "application/json");

                            using (var response2 = await httpClient.PutAsync(apiBaseUrl + "/Users/" + retuser.Id, user2))
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                }
                            }
                        }
                    }
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
