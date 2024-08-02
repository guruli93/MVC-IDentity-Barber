using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dai_2022.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
    }
}
