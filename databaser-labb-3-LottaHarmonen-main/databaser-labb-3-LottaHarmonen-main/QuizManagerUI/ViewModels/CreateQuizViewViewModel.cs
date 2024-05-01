using DataAccess.Services;
using DTOs;
using QuizManagerUI.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataAccess.Entities;
using MongoDB.Bson;

namespace QuizManagerUI.ViewModels;

public class CreateQuizViewViewModel : INotifyPropertyChanged
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


    private ObservableCollection<QuestionRecord> _questions;
    public ObservableCollection<QuestionRecord> Questions
    {
        get { return _questions; }
        set
        {
            if (_questions != value)
            {
                _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }
    }

    public ICommand AddNewQuizCommand { get; }
    public ICommand UpdateQuizCommand { get; }
    public ICommand RemoveQuizCommand { get; }

    public ICommand AddQuestionsCommand { get; }

    public ICommand RemoveQuestionCommand { get; }

    public CreateQuizViewViewModel(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
        Quizzes = new ObservableCollection<QuizRecord>(GetAllQuizzesFromDatabase());
        Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());

        AddNewQuizCommand = new AddNewQuizCommand(this);
        UpdateQuizCommand = new UpdateQuizCommand(this);
        RemoveQuizCommand = new RemoveQuizCommand(this);
        AddQuestionsCommand = new AddQuestionsToQuizCommand(this);
        RemoveQuestionCommand = new RemoveQuestionFromQuizCommand(this);
    }

    private List<QuizRecord> GetAllQuizzesFromDatabase()
    {
        return _mongoDbService.GetAllQuizzes();
    }

    private List<QuestionRecord> GetAllQuestionsFromDatabase()
    {
        return _mongoDbService.GetAllQuestions();
    }


    //---------------------SELECTED PROPERTIES FOR NEW QUIZ---------------------------------------

    private string _inputQuizTitle = "Title";

    public string InputQuizTitle
    {
        get { return _inputQuizTitle; }
        set { SetField(ref _inputQuizTitle, value); }

    }

   

    private string _inputQuizDescription = "Description";

    public string InputQuizDescription
    {
        get { return _inputQuizDescription; }
        set { SetField(ref _inputQuizDescription, value); }

    }

    //---------------------SELECTED PROPERTIES FOR NEW QUIZ END------------------------------------





    //------------------FILTER QUESTIONS BY TITLE------------------------------------------------------

    private string _searchText;

    public string SearchText
    {
        get { return _searchText; }
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(SearchText);
                FilterQuestionsByTitle(_searchText);
            }
        }
    }


    private void FilterQuestionsByTitle(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());
        }
        else
        {
            Questions = new ObservableCollection<QuestionRecord>( _mongoDbService.GetQuestionsByTitle(searchText));
        }
    }



    //------------------FILTER QUESTIONS BY TITLE END-------------------------------------------------

    //------------------SELECTED QUESTIONS TO BE REMOVED------------------------------------------------


    private QuestionRecord _selectedQuestionToRemove;

    public QuestionRecord SelectedQuestionToRemove
    {
        get { return _selectedQuestionForQuiz; }
        set { SetField(ref _selectedQuestionForQuiz, value); }

    }

    public void DeleteQuestionFromQuiz()
    {
        //which Quiz is chosen 
        string id = SelectedQuizListView.id;
        string quizDescription = InputQuizDescription;
        string quizTitle = InputQuizTitle;
        var newQuizRecord = new QuizRecord(id, quizTitle, quizDescription);

        //which question is chosen 
        string id2 = SelectedQuestionToRemove.Id;
        string questionText = SelectedQuestionToRemove.QuestionText;
        var answerOptions = SelectedQuestionToRemove.AnswerOptions;
        var correctAnswerIndex = SelectedQuestionToRemove.CorrectAnswerIndex;
        string category = SelectedQuestionToRemove.Category;
        var newQuestionRecord = new QuestionRecord(id2, questionText, answerOptions, correctAnswerIndex, category);

        _mongoDbService.RemoveQuestionFromQuiz(newQuizRecord, newQuestionRecord);

        SelectedQuizQuestions = new ObservableCollection<QuestionRecord>(_mongoDbService.GetQuestionsForQuiz(_selectedQuizListView.id));
    }


    //------------------SELECTED QUESTIONS TO BE REMOVED END--------------------------------------


    //--------------------SELECTED QUESTIONS FOR QUIZ------------------------------------------

    //selectedQuestionForQuiz

    private QuestionRecord _selectedQuestionForQuiz;

    public QuestionRecord SelectedQuestionForQuiz
    {
        get { return _selectedQuestionForQuiz; }
        set { SetField(ref _selectedQuestionForQuiz, value); }

    }


    public void AddQuestionToQuiz()
    {
        if (_selectedQuizListView == null)
        {
            return;
        }
        //which Quiz is chosen 
        string id = SelectedQuizListView.id;
        string quizDescription = InputQuizDescription;
        string quizTitle = InputQuizTitle;
        var newQuizRecord = new QuizRecord(id, quizTitle, quizDescription);

        //which question is chosen 
        string id2 = SelectedQuestionForQuiz.Id;
        string questionText = SelectedQuestionForQuiz.QuestionText;
        var answerOptions = SelectedQuestionForQuiz.AnswerOptions;
        var correctAnswerIndex = SelectedQuestionForQuiz.CorrectAnswerIndex;
        string category = SelectedQuestionForQuiz.Category;
        var newQuestionRecord = new QuestionRecord(id2, questionText, answerOptions, correctAnswerIndex, category);

        _mongoDbService.AddQuestionToQuiz(newQuizRecord, newQuestionRecord);
        SelectedQuizQuestions = new ObservableCollection<QuestionRecord>(_mongoDbService.GetQuestionsForQuiz(_selectedQuizListView.id));

    }


    //--------------------SELECTED QUESTIONS FOR QUIZ END---------------------------------------




    private QuizRecord _selectedQuizListView;

    public QuizRecord SelectedQuizListView
    {
        get { return _selectedQuizListView; }
        set
        {
            if (SetField(ref _selectedQuizListView, value))
            {

                if (value != null)
                {
                    string selectedQuizId = value.id;
                    InputQuizTitle = value.quizTitle;
                    InputQuizDescription = value.quizDescription;

                    SelectedQuizQuestions = new ObservableCollection<QuestionRecord>(_mongoDbService.GetQuestionsForQuiz(_selectedQuizListView.id));

                }
            }
        }
    }



    private ObservableCollection<QuestionRecord> _selectedQuizQuestions;
    public ObservableCollection<QuestionRecord> SelectedQuizQuestions
    {
        get { return _selectedQuizQuestions; }
        set
        {
            SetField(ref _selectedQuizQuestions, value);
            OnPropertyChanged(nameof(_selectedQuizQuestions));
        }
    }

    //-------------------------------ADD NEW QUIZ----------------------------------------

    public void AddNewQuiz()
    {
        var newQuiz = new Quiz()
        {
            QuizDescription = InputQuizDescription,
            QuizTitle = InputQuizTitle
        };
        _mongoDbService.AddQuiz(newQuiz);
        Quizzes = new ObservableCollection<QuizRecord>(GetAllQuizzesFromDatabase());
    }

    //-------------------------------ADD NEW QUIZ END----------------------------------------








    //-------------------------------UPDATE QUIZ --------------------------------------------

    public void UpdateQuiz()
    {
        string id = SelectedQuizListView.id;
        string quizDescription = InputQuizDescription;
        string quizTitle = InputQuizTitle;
        var newQuizRecord = new QuizRecord(id, quizTitle, quizDescription);

        _mongoDbService.UpdateQuiz(newQuizRecord);
        Quizzes = new ObservableCollection<QuizRecord>(GetAllQuizzesFromDatabase());
    }

    //-------------------------------UPDATE QUIZ END----------------------------------------










    //-------------------------------DELETE QUIZ ------------------------------------------

    public void RemoveQuiz()
    {
        string id = _selectedQuizListView.id;

        _mongoDbService.RemoveQuiz(id);
        Quizzes = new ObservableCollection<QuizRecord>(GetAllQuizzesFromDatabase());
    }

    //-------------------------------DELETE QUIZ END----------------------------------------








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