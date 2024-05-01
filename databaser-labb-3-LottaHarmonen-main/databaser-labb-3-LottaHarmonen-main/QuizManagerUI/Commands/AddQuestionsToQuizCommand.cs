using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class AddQuestionsToQuizCommand : ICommand
{

    private readonly CreateQuizViewViewModel _viewModel;

    public AddQuestionsToQuizCommand(CreateQuizViewViewModel viewModel)
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
        _viewModel.AddQuestionToQuiz();
    }

    public event EventHandler? CanExecuteChanged;
}