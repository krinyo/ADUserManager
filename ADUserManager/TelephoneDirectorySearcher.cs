using System.IO;

namespace ADUserManager
{
    internal class TelephoneDirectorySearcher
    {
        private string? _internalNumber;
        private string? _mobileNumber;
        //private string _query;
        private string _filePath;
        public TelephoneDirectorySearcher(string path) 
        {
            _filePath = path;
        }
        /*public void Search(string query) 
        {
            using (var package = new ExcelPackage(new FileInfo(_filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                
                int rowCount = worksheet.Dimension.Rows;
                int columnCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row ++) //
                {
                    string? fullName = worksheet.Cells[row, 4].Value?.ToString();
                    if (fullName != null && fullName.Equals(query, StringComparison.OrdinalIgnoreCase)) 
                    {
                        _internalNumber = worksheet.Cells[row, 6].Value?.ToString();
                        _mobileNumber = worksheet.Cells[row,7].Value?.ToString();
                        break;
                    }

                }
            }
            
        }*/
        public void Search(string query)
        {
            if (query == null)
                return;
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    string fullName = values[3];
                    if (fullName.Trim().ToLower().Contains(query.Trim().ToLower()))
                    {
                        _internalNumber = values[5];
                        _mobileNumber = values[6];
                        break;
                    }
                }
            }
        }
        public void CopyBaseFile() 
        {
            string sourceFilePath = _filePath;
            string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _filePath.Split('\\')[3]);

            File.Copy(sourceFilePath, destinationFilePath, true);
            _filePath = destinationFilePath;
        }

        public override string ToString()
        {
            return string.Format("Внутренний номер {0} \nМобильный номер {1}", this._internalNumber, this._mobileNumber);
        }
    }
}
