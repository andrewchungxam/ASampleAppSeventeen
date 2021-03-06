﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
//using System.Windows.Input;

using ASampleApp.Data;
using ASampleApp.CosmosDB;

namespace ASampleApp
{
	public class FirstViewModel : BaseViewModel
	{

		string _firstLabel;
        string _firstEntryText;
        string _secondEntryText;

        public ICommand MyFavoriteCommand { get; set; }
		public ICommand MySecondFavoriteCommand { get; set; }


		public string FirstLabel
		{
			get { return _firstLabel;}
			set { SetProperty (ref _firstLabel, value);}
		}



		public string FirstEntryText
		{
			get { return _firstEntryText;}
			set { SetProperty (ref _firstEntryText, value);}
		}

		public string SecondEntryText
		{
			get { return _secondEntryText; }
			set { SetProperty(ref _secondEntryText, value); }
		}


		public FirstViewModel ()
		{
             MyFavoriteCommand = new Command(OnMyFavoriteAction);
//           MySecondFavoriteCommand = new Command(async () => await OnMySecondFavoriteCommand());

		}

        void OnMyFavoriteAction()
        {

            //ADD NEW DOG
            App.DogRep.AddNewDog(this.FirstEntryText, this.SecondEntryText);
			AddLastDogToCosmosDBAsync();

			//ADD DOG TO OBSERVABLE COLLECTION OF THE LISTVIEW
			var tempLastDog = App.DogRep.GetLastDog();
            App.MyDogListMVVMPage.MyViewModel._observableCollectionOfDogs.Add(tempLastDog);

            string _lastNameString = App.DogRep.GetLastDog().Name;

            string _lastNameStringAdd = System.String.Format("{0} added to the list!", _lastNameString);
            this.FirstLabel = _lastNameStringAdd;

            return;

        }

		private async void AddLastDogToCosmosDBAsync()
		{
			var myDog = App.DogRep.GetLastDog();
			var myCosmosDog = DogConverter.ConvertToCosmosDog(myDog);
            await CosmosDB.CosmosDBService.PostCosmosDogAsync(myCosmosDog);
		}

    }
}
