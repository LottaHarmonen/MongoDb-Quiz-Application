using DataAccess.Services;
using DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuizManagerUI.Commands;

namespace QuizManagerUI.ViewModels;

public class CreateCategoriesViewViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly MongoDbService _mongoDbService;

    private ObservableCollection<CategoryRecord> _categories;
    public ObservableCollection<CategoryRecord> Categories
    {
        get { return _categories; }
        set
        {
            if (_categories != value)
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }

    private ObservableCollection<CategoryRecord> _selectedCategory;
    public ObservableCollection<CategoryRecord> SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }


    private string _userInputCategory = "Category name";
    public string UserInputCategory
    {
        get { return _userInputCategory; }
        set
        {
            if (_userInputCategory != value)
            {
                _userInputCategory = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }

    public ICommand AddNewCategoryCommand { get; set; }
    public CreateCategoriesViewViewModel(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;

        Categories = new ObservableCollection<CategoryRecord>(GetAllCategoriesFromDatabase());

        AddNewCategoryCommand = new AddNewCategoryCommand(this);
    }



    private List<CategoryRecord> GetAllCategoriesFromDatabase()
    {
        return _mongoDbService.GetAllCategories();
    }

    public void AddNewCategory()
    {


        var newCategoryRecord = new CategoryRecord(UserInputCategory);

        _mongoDbService.AddCategory(newCategoryRecord);


        Categories = new ObservableCollection<CategoryRecord>(GetAllCategoriesFromDatabase());
    }

    public void DeleteCategory()
    {

    }





























    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}