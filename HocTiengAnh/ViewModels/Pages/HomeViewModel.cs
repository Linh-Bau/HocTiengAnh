// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using HocTiengAnh.Helpers;
using HocTiengAnh.Models;
using HocTiengAnh.Services;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System.IO;
using Wpf.Ui.Controls;

namespace HocTiengAnh.ViewModels.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ConfigureService _configureService;
        private readonly LogService _logService;
        private readonly WordsService _wordsService;
        private readonly WordSpeakerService _wordSpeakerService;

        [ObservableProperty]
        private string _data=null!;

        [ObservableProperty]
        private int _total=0;

        [ObservableProperty]
        private int _learned=0;

        [ObservableProperty]
        private int _failed = 0;

        [ObservableProperty]
        private Visibility visibility = Visibility.Hidden;

        public QuestionModel Question { get; }

        public HomeViewModel(ConfigureService configureService,
            LogService logService,
            WordsService wordsService,
            WordSpeakerService wordSpeakerService)
        {
            this._configureService = configureService;
            this._logService = logService;
            this._wordsService = wordsService;
            _wordSpeakerService = wordSpeakerService;
            Question = new QuestionModel();
            InitBindingData();
        }

        [RelayCommand]
        private void UnHiding()
        {
            Visibility = Visibility.Visible;
        }

        bool isLearning = false;

        [RelayCommand]
        private void PlaySound()
        {
            if(!isLearning)
            {
                return;
            }
            if(!_wordSpeakerService.HasSoundFile(Question.Word))
            {
                return;
            }
            _wordSpeakerService.PlaySound(Question.Word);
        }

        [RelayCommand]
        private void StartLeaning()
        {
            isLearning = _wordsService.NextQuestion(Question);
            Visibility = Visibility.Hidden;
        }

        [RelayCommand]
        private async Task AnswerClick(string answerID)
        {
            if(!isLearning)
            {
                _logService.Debug("isLearning == false");
                return;
            }
            if(!_wordsService.CheckAnswer(Question, int.Parse(answerID), false))
            {
                Failed++;
            }
            await Task.Delay(1000);
            _wordsService.NextQuestion(Question);
            Visibility = Visibility.Hidden;
        }


        [RelayCommand]
        private async Task ButtonClick(string answerID)
        {
            if(!isLearning)
            {
                _logService.Debug("isLearning == false");
                return;
            }
            if(_wordsService.CheckAnswer(Question, int.Parse(answerID), true))
            {
                Learned++;
            }
            else
            {
                Failed++;
            }
            await Task.Delay(1000);
            _wordsService.NextQuestion(Question);
            Visibility = Visibility.Hidden;
        }



        [RelayCommand]
        private void SelectDatFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "words files (*.dat)|*.dat";
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            var file = dialog.FileName;
            if(!_wordsService.LoadFromFile(file))
            {
                return;
            }
            // if load success change config
            _configureService.AppConfig.CurrentDataFile = file;
            InitBindingData();
        }

        [RelayCommand]
        private void SelectTxtFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            // convert data
            string dataFilePath = dialog.FileName;
            var words = ConvertTextFileToListObject(dataFilePath);
            if (words == null)
            {
                return;
            }
            // save to file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string saveFileDir = Directory.GetCurrentDirectory() + "\\Data";
            if (!Directory.Exists(saveFileDir))
            {
                Directory.CreateDirectory(saveFileDir);
            }
            saveFileDialog.InitialDirectory = saveFileDir;
            saveFileDialog.FileName = Path.GetFileName(dataFilePath).Replace(".txt", ".dat");
            if(saveFileDialog.ShowDialog() != true)
            {
                return;
            }
            FileHelper.SaveObjectAsJson(saveFileDialog.FileName, words);
        }

        private List<EngWord> ConvertTextFileToListObject(string filepath)
        {
            try
            {
                var list = new List<EngWord>();
                string[] lines = File.ReadAllLines(filepath);
                foreach(string line in lines)
                {
                    if(string.IsNullOrEmpty(line)) continue;
                    try
                    {
                        list.Add(StringHelper.ConvertStringToEngWord(line));
                    }
                    catch(ArgumentException argEx)
                    {
                        _logService.Error(argEx.Message);
                    }
                }
                return list;
            }
            catch(Exception ex) {
                _logService.Error(ex.ToString());
                return null!;
            }
        }
   
    
        private void InitBindingData()
        {
            if (!string.IsNullOrEmpty(_configureService.AppConfig.CurrentDataFile))
            {
                Data = Path.GetFileName(_configureService.AppConfig.CurrentDataFile);
                Total = _wordsService.Words.Count;
                Learned = _wordsService.Words.Where(w => w.IsLeaned).Count();
                Failed = 0;
            }
        }
    }
}
