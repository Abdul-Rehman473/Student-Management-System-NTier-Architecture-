using Microsoft.AspNetCore.Identity;

namespace SMS_Identity;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? PhoneNo { get; set; }
    public int? Semester { get; set; }
    public double? CGPA { get; set; }
    public bool IsStudent { get; set; }
} 