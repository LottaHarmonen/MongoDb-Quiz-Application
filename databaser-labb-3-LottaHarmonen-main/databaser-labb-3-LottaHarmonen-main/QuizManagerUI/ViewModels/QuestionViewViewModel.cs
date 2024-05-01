using DataAccess.Services;
using DTOs;
using QuizManagerUI.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataAccess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QuizManagerUI.ViewModels;

public class QuestionViewViewModel : INotifyPropertyChanged
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

    private ObservableCollection<QuestionRecord> _questions;
    public ObservableCollection<QuestionRecord> Questions
    {
        get { return _questions; }
        set
        {
            SetField(ref _questions, value); 
            OnPropertyChanged(nameof(Questions));
        }
    }

    //---------------------SELECTED PROPERTIES FOR NEW QUESTION------------------------------------

    private string _selectedCategory;

    public string SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            if (_selectedCategory != value)
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        
    }

    private string _inputQuestionText = "Question";

    public string InputQuestionText
    {
        get { return _inputQuestionText; }
        set { SetField(ref _inputQuestionText, value); }

    }

    private string _inputAnswerOptionA;

    public string InputAnswerOptionA
    {
        get { return _inputAnswerOptionA; }
        set { SetField(ref _inputAnswerOptionA, value); }
    }
    private string _inputAnswerOptionB;

    public string InputAnswerOptionB
    {
        get { return _inputAnswerOptionB; }
        set { SetField(ref _inputAnswerOptionB, value); }
    }
    

    private string _inputAnswerOptionC;

    public string InputAnswerOptionC
    {
        get { return _inputAnswerOptionC; }
        set { SetField(ref _inputAnswerOptionC, value); }
    }

    private bool _isCorrectOptionA;

    public bool IsCorrectOptionA
    {
        get { return _isCorrectOptionA; }
        set { SetField(ref _isCorrectOptionA, value); }
    }

    private bool _isCorrectOptionB;

    public bool IsCorrectOptionB
    {
        get { return _isCorrectOptionB; }
        set { SetField(ref _isCorrectOptionB, value); }
    }

    private bool _isCorrectOptionC;

    public bool IsCorrectOptionC
    {
        get { return _isCorrectOptionC; }
        set { SetField(ref _isCorrectOptionC, value); }
    }


    //---------------------SELECTED PROPERTIES FOR NEW QUESTION END------------------------------------





    //--------------------SELECTED QUESTION IN THE LISTVIEW----------------------------------------


    private QuestionRecord _selectedQuestionListView;

    public QuestionRecord SelectedQuestionListView
    {
        get { return _selectedQuestionListView; }
        set
        {
            if (SetField(ref _selectedQuestionListView, value))
            {
                
                if (value != null)
                {


                    SelectedCategory = value.Category;
                    InputQuestionText = value.QuestionText;
                    InputAnswerOptionA = value.AnswerOptions.ElementAtOrDefault(0) ?? string.Empty;
                    InputAnswerOptionB = value.AnswerOptions.ElementAtOrDefault(1) ?? string.Empty;
                    InputAnswerOptionC = value.AnswerOptions.ElementAtOrDefault(2) ?? string.Empty;

                    IsCorrectOptionA = false;
                    IsCorrectOptionB = false;
                    IsCorrectOptionC = false;
                    switch (value.CorrectAnswerIndex)
                    {
                        case 0:
                            IsCorrectOptionA = true;
                            break;
                        case 1:
                            IsCorrectOptionB = true;
                            break;
                        case 2:
                            IsCorrectOptionC = true;
                            break;
                    }
                }
            }
        }
    }



    //--------------------SELECTED QUESTION IN THE LISTVIEW END----------------------------------------







 //---------------------------SORT BY CATEGORY--------------------------------------------------------

    private string _selectedCategoryForSorting;

    public string SelectedCategoryForSorting
    {
        get { return _selectedCategoryForSorting; }
        set
        {
            if (SetField(ref _selectedCategoryForSorting, value))
            {
                
                GetQuestionsByCategory(_selectedCategoryForSorting); 
            }

        }

    }

    public void GetQuestionsByCategory(string selectedCategoryForSorting)
    {

        Questions = new ObservableCollection<QuestionRecord>(
            _mongoDbService.GetQuestionsByCategory(selectedCategoryForSorting));
    }

    //---------------------------SORT BY CATEGORY END--------------------------------------------------








    public ICommand AddNewQuestionCommand { get; }
    public ICommand UpdateQuestionCommand { get; }
    public ICommand RemoveQuestionCommand { get; }

    public QuestionViewViewModel(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;

        Categories = new ObservableCollection<CategoryRecord>(GetAllCategoriesFromDatabase());
        Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());


        AddNewQuestionCommand = new AddNewQuestionCommand(this);
        UpdateQuestionCommand = new UpdateQuestionCommand(this);
        RemoveQuestionCommand = new RemoveQuestionCommand(this);
    }

    private List<QuizRecord> GetAllQuizzesFromDatabase()
    {
        return _mongoDbService.GetAllQuizzes();
    }

    private List<CategoryRecord> GetAllCategoriesFromDatabase()
    {
        return _mongoDbService.GetAllCategories();
    }

    private List<QuestionRecord> GetAllQuestionsFromDatabase()
    {
        return _mongoDbService.GetAllQuestions();
    }

    public void AddNewQuestion()
    {
        var category = new CategoryRecord(SelectedCategory);
       

        var newQuestion = new Question()
        {
            QuestionText = InputQuestionText,
            AnswerOptions = new List<string>
            {
                InputAnswerOptionA,
                InputAnswerOptionB,
                InputAnswerOptionC
            },
            Category = category.categoryName
        };

        if (IsCorrectOptionA)
        {
            newQuestion.CorrectAnswerIndex = 0;
        }
        else if (IsCorrectOptionB)
        {
            newQuestion.CorrectAnswerIndex = 1;
        }
        else if (IsCorrectOptionC)
        {
            newQuestion.CorrectAnswerIndex = 2;
        }

        _mongoDbService.AddQuestion(newQuestion);
        Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());
    }


    public void UpdateQuestion()
    {
        string id = SelectedQuestionListView.Id;
        string questiontext = InputQuestionText;
        List<string> answerOptions = new List<string>()
        {
            InputAnswerOptionA,
            InputAnswerOptionB,
            InputAnswerOptionC,
        };
        string category = SelectedCategory;
        int correctAnswerIndex = 0;
        if (IsCorrectOptionA)
        {
            correctAnswerIndex = 0;
        }
        else if (IsCorrectOptionB)
        {
            correctAnswerIndex = 1;
        }
        else if (IsCorrectOptionC)
        {
            correctAnswerIndex = 2;
        }


        _mongoDbService.UpdateQuestion(new QuestionRecord(id, questiontext, answerOptions, correctAnswerIndex, category));


        Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());
    }

    public void RemoveQuestion()
    {
        string id = _selectedQuestionListView.Id;
        _mongoDbService.RemoveQuestion(id);
        Questions = new ObservableCollection<QuestionRecord>(GetAllQuestionsFromDatabase());

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