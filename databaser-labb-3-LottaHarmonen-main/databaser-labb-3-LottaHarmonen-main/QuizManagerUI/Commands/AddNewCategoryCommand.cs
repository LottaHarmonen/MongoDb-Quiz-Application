using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class AddNewCategoryCommand : ICommand
{

    private readonly CreateCategoriesViewViewModel _viewModel;

    public AddNewCategoryCommand(CreateCategoriesViewViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (sender, e) => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
    }

    public bool CanExecute(object? parameter)
    {
        return !string.IsNullOrEmpty(_viewModel.UserInputCategory);
    }

    public void Execute(object? parameter)
    {
        _viewModel.AddNewCategory();
    }

    public event EventHandler? CanExecuteChanged;

}