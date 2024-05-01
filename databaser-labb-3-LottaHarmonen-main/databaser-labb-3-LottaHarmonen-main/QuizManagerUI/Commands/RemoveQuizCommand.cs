using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class RemoveQuizCommand : ICommand
{
    private readonly CreateQuizViewViewModel _viewModel;

    public RemoveQuizCommand(CreateQuizViewViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (sender, e) => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
    }

    public bool CanExecute(object? parameter)
    {
        return !string.IsNullOrEmpty(_viewModel.InputQuizDescription)
               && !string.IsNullOrEmpty(_viewModel.InputQuizTitle);

        //SELECTED QUIZ INSTEAD?
    }

    public void Execute(object? parameter)
    {
        _viewModel.RemoveQuiz();
    }

    public event EventHandler? CanExecuteChanged;
}