using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class UpdateQuizCommand : ICommand
{

    private readonly CreateQuizViewViewModel _viewModel;

    public UpdateQuizCommand(CreateQuizViewViewModel viewModel)
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
        _viewModel.UpdateQuiz();
    }

    public event EventHandler? CanExecuteChanged;
}