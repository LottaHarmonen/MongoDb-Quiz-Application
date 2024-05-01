using DataAccess.Services;
using QuizManagerUI.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var quizViewViewModel = new QuizViewViewModel(new MongoDbService());
            var createQuizViewModel = new CreateQuizViewViewModel(new MongoDbService());
            var questionViewViewModel = new QuestionViewViewModel(new MongoDbService());
            var createCategoriesViewViewModel = new CreateCategoriesViewViewModel(new MongoDbService());

            DataContext = new
            {
                QuizViewViewModel = quizViewViewModel,
                CreateQuizViewViewModel = createQuizViewModel,
                QuestionViewViewModel = questionViewViewModel,
                CreateCategoriesViewViewModel = createCategoriesViewViewModel
            };
        }
    }
}