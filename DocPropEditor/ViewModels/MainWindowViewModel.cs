using DocPropEditor.Infrastructure.Command;
using DocPropEditor.Models;
using DocPropEditor.Services;
using System;
using System.Windows.Input;

namespace DocPropEditor.ViewModels
{
    internal class MainWindowViewModel : BaseVm
    {
        private readonly IFileService _fileService;

        public ICommand OpenFIleCommand { get; set; }
        public ICommand EditCommand { get; set; }

        private bool _isChoosed;
        public bool IsChoosed
        {
            get { return _isChoosed; }
            set
            {
                _isChoosed = value; OnPropertyChanged();
            }
        }


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

        private string? _lastModifiedBy;

        public string LastModifiedBy
        {
            get { return _lastModifiedBy; }
            set { _lastModifiedBy = value; OnPropertyChanged(); }
        }

        private string _lastModifiedByIsVisible;

        public string LastModifiedByIsVisible
        {
            get { return _lastModifiedByIsVisible; }
            set { _lastModifiedByIsVisible = value; OnPropertyChanged();}
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
            EditCommand = new Command(EditFile, () => IsChoosed);
            OpenFIleCommand = new Command(Onload);

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            _fileService.ClearTempFolder();
        }

        private void Onload(object obj)
        {

            var docprop = _fileService.OpenArchiveAndGetData();
            Creator = docprop.Creator;
            LastModifiedBy = docprop.LastModifiedBy;
            TotalTime = docprop.TotalTime;
            CreateDate = docprop.CreationDate;
            ModifiedDate = docprop.ModifiedDate;

            LastModifiedByIsVisible = LastModifiedBy != "" && LastModifiedBy != Creator ? "Visible" : "Collapsed";

            IsChoosed = true;
            OnPropertyChanged(nameof(IsChoosed));
            OnPropertyChanged(nameof(LastModifiedByIsVisible));

        }

        private void EditFile(object obj)
        {
            DocProperties docProperties = new DocProperties
            {
                CreationDate = CreateDate,
                LastModifiedBy = LastModifiedBy,
                Creator = Creator,
                TotalTime = TotalTime,
                ModifiedDate = ModifiedDate
            };

            _fileService.SaveFileAndArchive(docProperties);

        }


    }

}

