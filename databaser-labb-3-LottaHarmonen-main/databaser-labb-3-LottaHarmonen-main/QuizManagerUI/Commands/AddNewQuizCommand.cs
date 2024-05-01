using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class AddNewQuizCommand : ICommand
{

    private readonly CreateQuizViewViewModel _viewModel;

    public AddNewQuizCommand(CreateQuizViewViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (sender, e) => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
    }
    public bool CanExecute(object? parameter)
    {
        return !string.IsNullOrEmpty(_viewModel.InputQuizDescription)
               && !string.IsNullOrEmpty(_viewModel.InputQuizTitle);
    }

    public void Execute(object? parameter)
    {
        _viewModel.AddNewQuiz();
    }

    public event EventHandler? CanExecuteChanged;
}