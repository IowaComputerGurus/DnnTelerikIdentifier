using System.Collections.Generic;

namespace IowaComputerGurus.Dnn.TelerikIdentifier.Models
{
    public class TelerikAnalysisModel
    {
        public bool AnalysisComplete { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ExpectedAssemblies { get; set; } = new List<string>();
        public List<string> UnexpectedAssemblies { get; set; } = new List<string>();
    }
}