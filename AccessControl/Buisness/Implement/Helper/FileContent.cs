using System;
using System.IO;
using System.Net.Http;

namespace AccessControl.Buisness.Implements.Helper
{
    public class FileContent : MultipartFormDataContent
    {
        public FileContent(string filePath, string apiParamName)
        {
            try
            {
                var filestream = File.Open(filePath, FileMode.Open);
                var filename = Path.GetFileName(filePath);

                Add(new StreamContent(filestream), apiParamName, filename);
            }
            catch(IOException ioException)
            {
                throw ioException;
            }
            catch(ArgumentNullException argNullException)
            {
                throw argNullException;
            }
            catch(ArgumentException argException)
            {
                throw argException;
            }
        }
    }
}
