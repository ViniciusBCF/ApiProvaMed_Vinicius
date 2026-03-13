using ApiMedTest.Service.Results.Enums;

namespace ApiMedTest.Service.Results
{
    public class Result
    {
        public bool IsValid { get; set; } = true;
        public StatusCodeResultEnum StatusCode { get; set; }
        public string[]? Message { get; set; }
    }
}
