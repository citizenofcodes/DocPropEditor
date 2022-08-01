using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DocPropEditor.Infrastructure.Command;
using DocPropEditor.Models;
using DocPropEditor.Services;

namespace DocPropEditor.ViewModels
{
    internal class MainWindowViewModel : BaseVm
    {
        private readonly IFileService _fileService;

        public ICommand OpenFIleCommand { get; set; }
        public ICommand EditCommand { get; set; }
        
        

        private string? _creator;

        public string? Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
                OnPropertyChanged();
            }
        }


        private string? _totalTime;

        public string? TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }

        private string? _createDate;

        public string? CreateDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                OnPropertyChanged();
            }
        }

        private string? _modifiedDate;

        public string? ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;
            EditCommand = new Command(EditFile);
            OpenFIleCommand = new Command(Onload);
        }

        private void Onload(object obj)
        {

            var docprop = _fileService.OpenArchiveAndGetData();
            Creator = docprop.Creator;
            TotalTime = docprop.TotalTime;
            CreateDate = docprop.CreationDate;
            ModifiedDate = docprop.ModifiedDate;

        }

        private void EditFile(object obj)
        {
            DocProperties docProperties = new DocProperties
            {
                CreationDate = CreateDate,
                Creator = Creator,
                TotalTime = TotalTime,
                ModifiedDate = ModifiedDate
            };

            _fileService.SaveFileAndArchive(docProperties);

        }


    }

}

