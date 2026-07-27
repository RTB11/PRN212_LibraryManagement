using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementProject.Model;

public partial class BorrowDetail : INotifyPropertyChanged
{
    private bool _isSelected;

    [NotMapped]
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}