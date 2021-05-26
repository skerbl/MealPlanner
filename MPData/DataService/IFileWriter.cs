using System.Collections.Generic;

namespace MPData
{
    public interface IFileWriter
    {
        string TemplatePath { get; set; }
        void WriteToFile(List<Meal> mealList1, List<Meal> mealList2, string fromDate, string toDate, string fileName);
    }
}
