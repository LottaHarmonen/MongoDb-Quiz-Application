using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class RemoveQuestionFromQuizCommand : ICommand
{
    private readonly CreateQuizViewViewModel _viewModel;

    public RemoveQuestionFromQuizCommand(CreateQuizViewViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (sender, e) => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _viewModel.DeleteQuestionFromQuiz();
    }

    public event EventHandler? CanExecuteChanged;
}