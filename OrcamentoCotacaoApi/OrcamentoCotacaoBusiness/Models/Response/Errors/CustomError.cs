using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response.Errors
{
    public class CustomError
    {
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
