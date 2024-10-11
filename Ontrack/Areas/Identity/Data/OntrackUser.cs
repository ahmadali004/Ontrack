using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ontrack.Areas.Identity.Data;

// Add profile data for application users by adding properties to the _userManager class
public class OntrackUser : IdentityUser
{
    [PersonalData]
    public string ? FirstName { get; set; }

    [PersonalData]
    public string ? LastName { get; set; }


    public string ? UserType { get; set; }

}

