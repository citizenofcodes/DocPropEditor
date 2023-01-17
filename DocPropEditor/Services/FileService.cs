using DocPropEditor.Models;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace DocPropEditor.Services
{
    internal interface IFileService
    {
        DocProperties OpenArchiveAndGetData();
        void SaveFileAndArchive(DocProperties docProperties);
    }


    internal class FileService : IFileService
    {
        private DirectoryInfo TempDirectoryPath { get; set; }
        private string FilesPathDirectory { get; set; } = "C:\\ArchiveTemp\\docProps";

        private string oldFilePath { get; set; }
        private string FileCore { get; set; }
        private string FileApp { get; set; }
        private string Creator { get; set; }
        private string LastModifiedBy { get; set; }
        private string Creationdate { get; set; }
        private string ModifiedDate { get; set; }
        private string TotalTime { get; set; }
        private string TotalTimeHtml { get; set; }

        public DocProperties OpenArchiveAndGetData()
        {
            oldFilePath = ChooseFile();

            Directory.CreateDirectory("C:\\ArchiveTemp");

            ZipFile.ExtractToDirectory(oldFilePath, "C:\\ArchiveTemp");

            var docProp = OpenFilesAndGetDocProperties();

            return docProp;
        }

        public DocProperties OpenFilesAndGetDocProperties()
        {
            using var sr = new StreamReader(FilesPathDirectory + "\\app.xml");
            FileApp = sr.ReadToEnd();
            sr.Close();

            var sr2 = new StreamReader(FilesPathDirectory + "\\core.xml");

            FileCore = sr2.ReadToEnd();

            sr2.Close();

            Creator = Regex.Match(FileCore, @"<dc:creator>(.*)<.dc:creator>").Groups[1].ToString();

            LastModifiedBy = Regex.Match(FileCore, @"ModifiedBy>(.*)<.cp:lastModifiedBy>").Groups[1].ToString();

            Creationdate = Regex.Match(FileCore, @"<dcterms:created.*>(.*)<.dcterms:created>").Groups[1].ToString();

            ModifiedDate = Regex.Match(FileCore, @"<dcterms:modified.*>(.*)<.dcterms:modified>").Groups[1].ToString();

            TotalTime = Regex.Match(FileApp, @"<TotalTime>(.*)<.TotalTime>").Groups[1].ToString();

            TotalTimeHtml = Regex.Match(FileApp, @"<TotalTime>(.*)<.TotalTime>").ToString();



            DocProperties docProperties = new DocProperties
            {
                Creator = Creator,
                LastModifiedBy = LastModifiedBy,
                CreationDate = Creationdate,
                ModifiedDate = ModifiedDate,
                TotalTime = TotalTime
            };



            return docProperties;
        }

        public void SaveFileAndArchive(DocProperties docProperties)
        {
            string newFileCore;
            string newFileApp;
            using var sw = new StreamWriter(FilesPathDirectory + "\\core.xml", false);
            newFileCore = FileCore.Replace(Creator,
                    docProperties.Creator)
                .Replace(Creationdate,
                    docProperties.CreationDate)
                .Replace(ModifiedDate,
                    docProperties.ModifiedDate);

            if (LastModifiedBy != "")
                newFileCore = newFileCore.Replace(LastModifiedBy, docProperties.LastModifiedBy);

            sw.WriteLine(newFileCore);

            sw.Close();

            var sw2 = new StreamWriter(FilesPathDirectory + "\\app.xml", false);
            newFileApp = FileApp.Replace(TotalTimeHtml , $"<TotalTime>{docProperties.TotalTime}</TotalTime>");
            sw2.WriteLine(newFileApp);
            sw2.Close();

            ZipFile.CreateFromDirectory(TempDirectoryPath.FullName, oldFilePath.Replace(".docx", $"{DateAndTime.Now.Second}.docx"));


            MessageBox.Show("Успешно!");

            TempDirectoryPath.Delete(true);
        }


        public string ChooseFile()
        {
            if (Directory.Exists("C:\\ArchiveTemp"))
                TempDirectoryPath.Delete(true);

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            bool? result = dlg.ShowDialog();

            return dlg.FileName;
        }

        public FileService()
        {
            TempDirectoryPath = new DirectoryInfo("C:\\ArchiveTemp\\");
        }
    }


}
