using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocPropEditor.ViewModels
{
    internal class MainWindowViewModel : BaseVm
    {
        public ICommand editCommand { get; set; }

        private string _creator;

        public string Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
                OnPropertyChanged();
            }
        }


        private string _totalTime;

        public string TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }

        private string _createDate;

        public string CreateDate
        {
            get { return _createDate; }
            set
            {
                _createDate = value;
                OnPropertyChanged();
            }
        }

        private string _modifiedDate;

        public string ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            
        }

    }

}

