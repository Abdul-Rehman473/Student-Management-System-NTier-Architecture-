using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NTier_Final.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? PhoneNo { get; set; }
    public int? Semester { get; set; }
    public double? CGPA { get; set; }
    public bool IsStudent { get; set; }
}

