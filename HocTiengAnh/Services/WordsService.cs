using HocTiengAnh.Helpers;
using HocTiengAnh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Services
{
    public class WordsService
    {
        LogService logService;
        OnlineDictionaryService onlineDictionaryService;
        WordSpeakerService wordSpeakerService;

        public List<EngWord> Words
        {
            get;
            private set;
        }

        public WordsService(LogService logService, OnlineDictionaryService onlineDictionaryService, WordSpeakerService wordSpeakerService) 
        {
            this.logService = logService;
            this.onlineDictionaryService = onlineDictionaryService;
            this.wordSpeakerService = wordSpeakerService;
            Words = new List<EngWord>();
        }

        public bool LoadFromFile(string filePath)
        {
            try
            {
                logService.Debug($"load words file from path: {filePath}");
                string context = File.ReadAllText(filePath);
                var words = JsonConvert.DeserializeObject<List<EngWord>>(context) ?? throw new Exception("null");
                Words.Clear();
                Words = words;
                logService.Debug($"{words.Count} loaded!");
                return true;
            }
            catch (Exception ex)
            {
                logService.Error(ex.ToString());
                return false;
            }
        }

        public void SaveToFile(string filePath)
        {
            try
            {
                logService.Debug($"save words to file path: {filePath}");
                string context = JsonConvert.SerializeObject(Words, Formatting.Indented);
                File.WriteAllText(filePath, context);
            }
            catch (Exception ex)
            {
                logService.Error(ex.ToString());
            }
        }

        public bool NextQuestion(QuestionModel question)
        {
            if(Words is null || Words.Count == 0)
            {
                logService.Error("Words is null or 0");
                return false;
            }
            question.Clear();
            if (Words.Where(w=>!w.IsLeaned).Count() == 0)
            {
                question.Word = "Bạn đã hoàn thành nhóm từ vựng này!";
                logService.Error("Great, you have leaned all works");
                return false;
            }
            // tạo câu hỏi
            var unlearnedWord = RandomHelper.GetRandomObject<EngWord>(Words.Where(w => !w.IsLeaned).ToList(), 1);
            var list = RandomHelper.GetRandomObject<EngWord>(Words, 3);
            list.AddRange(unlearnedWord);
            question.Word = unlearnedWord[0].Word;
            var answers = list.Select(w => w.Meaning).ToList();
            RandomHelper.ShuffleList(answers);
            question.AnswerA = answers[0];
            question.AnswerB = answers[1];
            question.AnswerC = answers[2];
            question.AnswerD = answers[3];
            if (string.IsNullOrEmpty(unlearnedWord[0].Ipa))
            {
                unlearnedWord[0].Ipa = onlineDictionaryService.GetIPA(unlearnedWord[0].Word);
            }
            question.Ipa = unlearnedWord[0].Ipa;
            // speak
            if (!wordSpeakerService.HasSoundFile(unlearnedWord[0].Word))
            {
                if(onlineDictionaryService.DownloadSound(unlearnedWord[0].Word))
                {
                    wordSpeakerService.PlaySound(unlearnedWord[0].Word);
                }
            }else
            {
                wordSpeakerService.PlaySound(unlearnedWord[0].Word);
            }
            return true;
        }



        public bool CheckAnswer(QuestionModel question, int choose, bool isDefinitely)
        {
            // find word
            var word = Words.Find(w => w.Word == question.Word);
            if(word == null)
            {
                throw new ArgumentException($"{question.Word} not found!");
            }
            if(choose<0 || choose > 3)
            {
                throw new ArgumentException($"answer id: {choose} is out of range 0-3");
            }
            int correct = question.GetCorrectIndex(word.Meaning);
            if (correct==-1)
            {
                logService.Error($"cannot get correct index");
                return false;
            }
            question.ShowAnwser(correct, choose);
            if(isDefinitely) word.IsLeaned = true;
            return correct == choose;
        }
    }
}
