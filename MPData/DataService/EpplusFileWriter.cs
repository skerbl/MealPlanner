using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace MPData
{
    public class EpplusFileWriter : IFileWriter
    {
        private string _templatePath;

        string IFileWriter.TemplatePath
        {
            get => _templatePath;
            set 
            { 
                _templatePath = value;
                _templateFile = new FileInfo(Path.Combine(_templatePath, _templateFileName));
            }
        }


        private string _templateFileName = "MenüplanVorlage.xlsm";
        private FileInfo _templateFile;
        private Dictionary<string, List<string>> _cellLocations;

        public EpplusFileWriter()
        {
            _cellLocations = new Dictionary<string, List<string>>();
            InitializeCellLocations();
        }

        public void WriteToFile(Dictionary<string, Meal> meals, string fromDate, string toDate, string fileName)
        {
            FileInfo newFile = new FileInfo(@fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
            }

            using (ExcelPackage package = new ExcelPackage(newFile, _templateFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells["C18"].Value = fromDate;
                worksheet.Cells["C20"].Value = toDate;

                foreach (KeyValuePair<string, Meal> meal in meals)
                {
                    WriteMeal(worksheet, meal.Value, _cellLocations[meal.Key]);
                }

                worksheet.PrinterSettings.PrintArea = worksheet.Cells["B2:L90"];
                worksheet.Select();
                package.Save();
            }
        }

        private void WriteMeal(ExcelWorksheet worksheet, Meal meal, List<string> cellLocations)
        {
            worksheet.Cells[cellLocations[0]].Value = meal.Starter;
            worksheet.Cells[cellLocations[1]].Value = meal.MainDish;
            worksheet.Cells[cellLocations[2]].Value = meal.SideDish;
        }

        private void WriteTestCells(ExcelWorksheet worksheet)
        {
            worksheet.Cells["A1"].Value = "test";
            worksheet.Cells["C3"].Value = "test";
            worksheet.Cells["E5"].Value = "test";
            worksheet.Cells["G7"].Value = "test";
        }

        private void InitializeCellLocations()
        {
            _cellLocations.Add("Monday_1", new List<string> { "C31", "C33", "C35" });
            _cellLocations.Add("Monday_2", new List<string> { "H31", "H33", "H35" });
            _cellLocations.Add("Tuesday_1", new List<string> { "C39", "C41", "C43" });
            _cellLocations.Add("Tuesday_2", new List<string> { "H39", "H41", "H43" });
            _cellLocations.Add("Wednesday_1", new List<string> { "C47", "C49", "C51" });
            _cellLocations.Add("Wednesday_2", new List<string> { "H47", "H49", "H51" });
            _cellLocations.Add("Thursday_1", new List<string> { "C55", "C57", "C59" });
            _cellLocations.Add("Thursday_2", new List<string> { "H55", "H57", "H59" });
            _cellLocations.Add("Friday_1", new List<string> { "C63", "C65", "C67" });
            _cellLocations.Add("Friday_2", new List<string> { "H63", "H65", "H67" });
            _cellLocations.Add("Saturday", new List<string> { "C71", "C73", "C75" });
        }
    }
}
