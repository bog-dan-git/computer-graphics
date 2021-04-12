using CommandLine;

namespace ComputerGraphics.ConsoleApp
{
    public class Options
    {
        [Option('s', "source", Required=true, HelpText = "Input file name")]
        public string FileName { get; set; }
            
        [Option('g', "goal-format", Required = true, HelpText = "Output image format")]
        public string Format { get; set; }
        
        [Option('o', "output", Required = false, HelpText = "Output file name. Default to input file name (with changed extension)")]
        public string Output { get; set; }
    }
}