using System.ComponentModel.DataAnnotations;

namespace Library.API.Security;

public sealed class ApiKeyOptions
{
    public const string SectionName = "Authentication";

    [Required]
    [MinLength(32)]
    public string ApiKey { get; set; } = string.Empty;
}
