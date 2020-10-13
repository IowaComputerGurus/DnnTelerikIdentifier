using System.Collections.Generic;

namespace IowaComputerGurus.Dnn.TelerikIdentifier.Models
{
    public class TelerikAnalysisModel
    {
        public bool AnalysisComplete { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ExpectedAssemblies { get; set; } = new List<string>();
        public List<string> UnexpectedAssemblies { get; set; } = new List<string>();
        public List<AssemblyAnalysisError> AssemblyAnalysisErrors { get; set; } = new List<AssemblyAnalysisError>();
    }

    public class AssemblyAnalysisError
    {
        public string AssemblyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}