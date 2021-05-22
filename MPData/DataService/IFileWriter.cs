using System.Collections.Generic;

namespace MPData
{
    public interface IFileWriter
    {
        string TemplatePath { get; set; }
        void WriteToFile(Dictionary<string, Meal> meals, string fromDate, string toDate, string fileName);
    }
}
