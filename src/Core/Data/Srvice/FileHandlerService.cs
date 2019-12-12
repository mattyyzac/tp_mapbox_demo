using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Data.Srvice
{
    public class FileHandlerService
    {
        public static IEnumerable<Scenery> XlsHandler()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                // remember, it's bad for unit test, bad smells: too much dependency there
                var data = new List<Scenery>();
                var xls = Path.Combine(AppSettings.ContentRootPath, "sceneries.xlsx");
                using var stream = File.Open(xls, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                do
                {
                    while (reader.Read())
                    {
                        // in the excel file, 0 is name column, 4 is coordinates column
                        if (reader.GetString(0) != null && reader.GetString(4) != null)
                        {
                            var name = Convert.ToString(reader.GetString(0));
                            var coordinatesString = Convert.ToString(reader.GetString(4));
                            var spliter = coordinatesString.Split(",");
                            if (spliter.Length == 2)
                            {
                                double.TryParse(spliter[0], out var y); // latitude
                                double.TryParse(spliter[1], out var x); // longitude

                                if (x != 0 && y != 0)
                                {
                                    data.Add(new Scenery
                                    {
                                        Name = name,
                                        Coordinates = new Coordinates
                                        {
                                            Longitude = x,
                                            Latidue = y
                                        }
                                    });
                                }
                            }
                        }
                    }
                } while (reader.NextResult());

                return data;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
