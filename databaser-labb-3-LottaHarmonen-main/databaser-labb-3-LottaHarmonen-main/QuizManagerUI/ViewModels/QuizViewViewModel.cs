using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataAccess.Entities;
using DataAccess.Services;
using DTOs;

namespace QuizManagerUI.ViewModels;

public class QuizViewViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly MongoDbService _mongoDbService;

    private ObservableCollection<QuizRecord> _quizzes;
    public ObservableCollection<QuizRecord> Quizzes
    {
        get { return _quizzes; }
        set
        {
            if (_quizzes != value)
            {
                _quizzes = value;
                OnPropertyChanged(nameof(Quizzes));
            }
        }
    }



    //------------------------------------SHOW QUESTIONS CHOSEN QUIZ--------------------------------------------------------------

    private QuizRecord _selectedQuiz;

    public QuizRecord SelectedQuiz
    {
        get { return _selectedQuiz; }
        set
        {
            if (SetField(ref _selectedQuiz, value))
            {
                SelectedQuizQuestions =
                    new ObservableCollection<QuestionRecord>(_mongoDbService.GetQuestionsForQuiz(SelectedQuiz.id));
            }
        }
    }


    private ObservableCollection<QuestionRecord> _selectedQuizQuestions;
    public ObservableCollection<QuestionRecord> SelectedQuizQuestions
    {
        get { return _selectedQuizQuestions; }
        set
        {
            if (_selectedQuizQuestions != value)
            {
                _selectedQuizQuestions = value;
                OnPropertyChanged();
            }
        }
    }

    //------------------------------------SHOW QUESTIONS CHOSEN QUIZ END--------------------------------------------------------------



    public QuizViewViewModel(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;

        Quizzes = new ObservableCollection<QuizRecord>(GetAllQuizzesFromDatabase());
    }

    private List<QuizRecord> GetAllQuizzesFromDatabase()
    {
        return _mongoDbService.GetAllQuizzes();
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