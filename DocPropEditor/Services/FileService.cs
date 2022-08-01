using System;
using DocPropEditor.Models;
using System.IO;
using System.IO.Compression;
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
        public DirectoryInfo TempDirectoryPath { get; set; }
        public string FilesPathDirectory { get; set; } = "C:\\ArchiveTemp\\docProps";
        
        public string oldFilePath { get; set; }
        public string FileCore { get; set; }
        public string FileApp { get; set; }
        public string Creator { get; set; }
        public string Creationdate { get; set; }
        public string ModifiedDate { get; set; }
        public string TotalTime { get; set; }
        
        public DocProperties OpenArchiveAndGetData()
        {
            oldFilePath = ChooseFile();
            Directory.CreateDirectory("C:\\ArchiveTemp");

            ZipFile.ExtractToDirectory(oldFilePath, "C:\\ArchiveTemp");

            TempDirectoryPath = new DirectoryInfo("C:\\ArchiveTemp\\");


            var docProp = OpenFilesAndGetDocProperties();

            return docProp;

        }

        public DocProperties OpenFilesAndGetDocProperties()
        {
            StreamReader sr = new StreamReader(FilesPathDirectory + "\\app.xml");
            FileApp = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            var sr2 = new StreamReader(FilesPathDirectory + "\\core.xml");

            FileCore = sr2.ReadToEnd();

            Creator = Regex.Match(FileCore, @"<dc:creator>(.*)<.dc:creator>").Groups[1].ToString();

            Creationdate = Regex.Match(FileCore, @"<dcterms:created.*>(.*)<.dcterms:created>").Groups[1].ToString();

            ModifiedDate = Regex.Match(FileCore, @"<dcterms:modified.*>(.*)<.dcterms:modified>").Groups[1].ToString();

            TotalTime = Regex.Match(FileApp, @"<TotalTime>(.*)<.TotalTime>").Groups[1].ToString();



            DocProperties docProperties = new DocProperties
            {
                Creator = Creator,
                CreationDate = Creationdate,
                ModifiedDate = ModifiedDate,
                TotalTime = TotalTime
            };


            return docProperties;
        }

        public void SaveFileAndArchive(DocProperties docProperties)
        {

            StreamWriter sw = new StreamWriter(FilesPathDirectory + "\\core.xml", false);

            var newCoreFile = FileCore.Replace(Creator,
                    docProperties.Creator)
                .Replace(Creationdate,
                    docProperties.CreationDate)
                .Replace(ModifiedDate,
                    docProperties.ModifiedDate);

            sw.WriteLine(newCoreFile);

            sw.Close();
            sw.Dispose();

            StreamWriter sw2 = new StreamWriter(FilesPathDirectory + "\\app.xml", false);

            var newAppFile = FileApp.Replace(TotalTime, docProperties.TotalTime);
            sw2.WriteLine(newAppFile);
            sw2.Close();
            sw2.Dispose();

            ZipFile.CreateFromDirectory(TempDirectoryPath.FullName, oldFilePath.Replace(".docx", "Edited.docx") );

            TempDirectoryPath.Delete(true);

            MessageBox.Show("Успешно!");
        }


        public string ChooseFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            bool? result = dlg.ShowDialog();

            return dlg.FileName;
        }


    }


}
