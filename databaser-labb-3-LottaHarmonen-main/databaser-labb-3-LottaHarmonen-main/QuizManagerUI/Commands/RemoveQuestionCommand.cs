using QuizManagerUI.ViewModels;
using System.Windows.Input;

namespace QuizManagerUI.Commands;

public class RemoveQuestionCommand : ICommand
{
    private readonly QuestionViewViewModel _viewModel;

    public RemoveQuestionCommand(QuestionViewViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (sender, e) => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
    }

    public bool CanExecute(object? parameter)
    {
        return !string.IsNullOrEmpty(_viewModel.SelectedCategory)
               && !string.IsNullOrEmpty(_viewModel.InputQuestionText)
               && !string.IsNullOrEmpty(_viewModel.InputAnswerOptionA)
               && !string.IsNullOrEmpty(_viewModel.InputAnswerOptionB)
               && !string.IsNullOrEmpty(_viewModel.InputAnswerOptionC)
               && ((_viewModel.IsCorrectOptionA ? 1 : 0) +
                   (_viewModel.IsCorrectOptionB ? 1 : 0) +
                   (_viewModel.IsCorrectOptionC ? 1 : 0)) == 1;
    }

    public void Execute(object? parameter)
    {
        _viewModel.RemoveQuestion();
    }

    public event EventHandler? CanExecuteChanged;
}