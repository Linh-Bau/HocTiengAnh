using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HocTiengAnh.Models
{
    public partial class QuestionModel : ObservableObject
    {
        public static SolidColorBrush DefaultBG = Brushes.LightGray;

        [ObservableProperty]
        private string word = "Chưa có từ nào được chọn!";

        [ObservableProperty]
        private string ipa = "";

        [ObservableProperty]
        private string answerA = "";

        [ObservableProperty]
        private SolidColorBrush answerABackgroundColor = DefaultBG;

        [ObservableProperty]
        private string answerB = "";

        [ObservableProperty]
        private SolidColorBrush answerBBackgroundColor = DefaultBG;

        [ObservableProperty]
        private string answerC = "";

        [ObservableProperty]
        private SolidColorBrush answerCBackgroundColor = DefaultBG;

        [ObservableProperty]
        private string answerD = "";

        [ObservableProperty]
        private SolidColorBrush answerDBackgroundColor = DefaultBG;
    }


    public static class QuestionModelExtension
    {
        public static void Clear(this QuestionModel model)
        {
            model.Word = "";
            model.Ipa = "";
            model.AnswerA = "";
            model.AnswerB = "";
            model.AnswerC = "";
            model.AnswerD = "";
            //

            model.AnswerABackgroundColor = QuestionModel.DefaultBG;
            model.AnswerBBackgroundColor = QuestionModel.DefaultBG;
            model.AnswerCBackgroundColor = QuestionModel.DefaultBG;
            model.AnswerDBackgroundColor = QuestionModel.DefaultBG;
        }

        public static int GetCorrectIndex(this QuestionModel model, string correctAnswer)
        {
            if(model.AnswerA == correctAnswer)
            {
                return 0;
            }
            if (model.AnswerB == correctAnswer)
            {
                return 1;
            }
            if (model.AnswerC == correctAnswer)
            {
                return 2;
            }
            if (model.AnswerD == correctAnswer)
            {
                return 3;
            }
            return -1;
        }

        public static void ShowAnwser(this QuestionModel model, int correct, int choose)
        {
            if (correct == choose)
            {
                model.SetColor(correct, Brushes.Green);
            }
            else
            {
                model.SetColor(correct, Brushes.Green);
                model.SetColor(choose, Brushes.Red);
            }
        }

        private static void SetColor(this QuestionModel model, int id, SolidColorBrush color)
        {
            switch (id)
            {
                case 0:
                    model.AnswerABackgroundColor = color;
                    break;

                case 1:
                    model.AnswerBBackgroundColor = color;
                    break;

                case 2:
                    model.AnswerCBackgroundColor = color;
                    break;
                case 3:
                    model.AnswerDBackgroundColor = color;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
