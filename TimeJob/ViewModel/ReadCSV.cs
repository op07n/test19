﻿using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace TimeJob.ViewModel
{
  public class ReadCsv
  {
    public DataTable readCSV;

    public ReadCsv(string fileName, bool firstRowContainsFieldNames = true)
    {
      readCSV = GenerateDataTable(fileName, firstRowContainsFieldNames);
    }

    private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
    {
      DataTable result = new DataTable();

      if (fileName == "")
      {
        return result;
      }

      string delimiters = ",";
      string extension = Path.GetExtension(fileName);

      if (extension != null && extension.ToLower() == "txt")
        delimiters = "\t";
      else if (extension != null && extension.ToLower() == "csv")
        delimiters = ",";

      using (TextFieldParser tfp = new TextFieldParser(fileName))
      {
        tfp.SetDelimiters(delimiters);

        // Get The Column Names
        if (!tfp.EndOfData)
        {
          string[] fields = tfp.ReadFields();

          for (int i = 0; i < fields.Count(); i++)
          {
            if (firstRowContainsFieldNames)
              result.Columns.Add(fields[i]);
            else
              result.Columns.Add("Col" + i);
          }

          // If first line is data then add it
          if (!firstRowContainsFieldNames)
            result.Rows.Add(fields);
        }

        // Get Remaining Rows from the CSV
        while (!tfp.EndOfData)
          result.Rows.Add(tfp.ReadFields());
      }

      return result;
    }
  }
}
