﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Users.Models;

namespace Users.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var names = new List<string>();
            var role = await _roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (user != null && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
