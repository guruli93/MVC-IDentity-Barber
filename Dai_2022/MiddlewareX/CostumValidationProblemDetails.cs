using Microsoft.AspNetCore.Mvc;

namespace Dai;

public class CostumValidationProblemDetails:ProblemDetails
{ 
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}