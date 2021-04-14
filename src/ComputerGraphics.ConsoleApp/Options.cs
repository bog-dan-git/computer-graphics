using CommandLine;

namespace ComputerGraphics.ConsoleApp
{
    public class Options
    {
        [Option('s', "source", Required=true, HelpText = "Input file name")]
        public string FileName { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "Output file name")]
        public string Output { get; set; }
    }
}