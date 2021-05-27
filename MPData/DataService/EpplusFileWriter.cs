using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace MPData
{
    public class EpplusFileWriter : IFileWriter
    {
        #region Private Members

        private readonly string _templateFileName = "MenüplanVorlage.xlsm";
        private string _templatePath;
        private FileInfo _templateFile;
        private List<List<string>> _cellList1;
        private List<List<string>> _cellList2;

        #endregion

        #region Public Properties

        string IFileWriter.TemplatePath
        {
            get => _templatePath;
            set
            {
                _templatePath = value;
                _templateFile = new FileInfo(Path.Combine(_templatePath, _templateFileName));
            }
        }

        #endregion

        #region Constructor

        public EpplusFileWriter()
        {
            _cellList1 = new List<List<string>>();
            _cellList2 = new List<List<string>>();
            InitializeCellLocations();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the data coming from the ViewModels to a .xlsx file.
        /// </summary>
        /// <param name="mealList1">The first meal list</param>
        /// <param name="mealList2">The second meal list</param>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <param name="fileName">The name of the file</param>
        public void WriteToFile(List<Meal> mealList1, List<Meal> mealList2, string fromDate, string toDate, string fileName)
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

                for (int i = 0; i < mealList1.Count; i++)
                {
                    WriteMeal(worksheet, mealList1[i], _cellList1[i]);
                }

                for (int i = 0; i < mealList2.Count; i++)
                {
                    WriteMeal(worksheet, mealList2[i], _cellList2[i]);
                }

                worksheet.PrinterSettings.PrintArea = worksheet.Cells["B2:L90"];
                worksheet.Select();
                package.Save();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes a single meal to the specified cell locations
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="meal">The meal</param>
        /// <param name="cellLocations">A list of cells</param>
        private void WriteMeal(ExcelWorksheet worksheet, Meal meal, List<string> cellLocations)
        {
            worksheet.Cells[cellLocations[0]].Value = meal.Starter;
            worksheet.Cells[cellLocations[1]].Value = meal.MainDish;
            worksheet.Cells[cellLocations[2]].Value = meal.SideDish;
        }

        /// <summary>
        /// Sets up the cell locations for all meals in both lists
        /// </summary>
        private void InitializeCellLocations()
        {
            _cellList1.Add(new List<string> { "C31", "C33", "C35" });
            _cellList1.Add(new List<string> { "C39", "C41", "C43" });
            _cellList1.Add(new List<string> { "C47", "C49", "C51" });
            _cellList1.Add(new List<string> { "C55", "C57", "C59" });
            _cellList1.Add(new List<string> { "C63", "C65", "C67" });
            _cellList1.Add(new List<string> { "C71", "C73", "C75" });

            _cellList2.Add(new List<string> { "H31", "H33", "H35" });
            _cellList2.Add(new List<string> { "H39", "H41", "H43" });
            _cellList2.Add(new List<string> { "H47", "H49", "H51" });
            _cellList2.Add(new List<string> { "H55", "H57", "H59" });
            _cellList2.Add(new List<string> { "H63", "H65", "H67" });
        } 

        #endregion
    }
}
