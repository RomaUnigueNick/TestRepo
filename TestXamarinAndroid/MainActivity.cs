using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Android.Content.Res;

namespace TestXamarinAndroid
{
    [Activity(Label = "TestXamarinAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        AutoCompleteTextView textViewCountries;
        List<string> listCountriesTitles;
        int[] cidCountries;

        AutoCompleteTextView textViewCities;
        List<string> listCitiesTitles;
        long[] cidCities;

        AutoCompleteTextView textViewUnivercities;
        List<string> listUnivercitiesTitles;
        int[] idUniversitets;

        RootObjectCountry root;

        string savedNamePerson;
        string savedSurnamePerson;
        string savedTitleCountry;
        string savedTitleCity;
        string savedTitleUnivercity;
        
        EditText nameView;
        EditText surnameView;
        Button bFillBlank;
        protected override void OnCreate(Bundle bundle)//создаем и инициализируем необходимые переменные для 
                  //функционирования графического интерфейса пользователя
        {
            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.Main);

                nameView = FindViewById<EditText>(Resource.Id.name);
                nameView.Hint = "Имя";
                nameView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                nameView.AfterTextChanged += NameView_AfterTextChanged;

                surnameView = FindViewById<EditText>(Resource.Id.surname);
                surnameView.Hint = "Фамилия";
                surnameView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                surnameView.Enabled = false;
                surnameView.AfterTextChanged += SurnameView_AfterTextChanged;

                bFillBlank = FindViewById<Button>(Resource.Id.fillinBtn);
                bFillBlank.Enabled = false;
                bFillBlank.Click += BFillBlank_Click;

                WebRequest wr = WebRequest.Create("https://api.vk.com/method/database.getCountries?need_all=1&lang=0&count=229");
                WebResponse webResponse = wr.GetResponse();
                Stream stream = webResponse.GetResponseStream();

                root = (RootObjectCountry)new DataContractJsonSerializer(typeof(RootObjectCountry)).ReadObject(stream);
                listCountriesTitles = root.response.Select(r => r.title).ToList();
                cidCountries = root.response.Select(r => r.cid).ToArray();

                textViewCountries = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_country);
                var adapterCountry = new ArrayAdapter<String>(this, Resource.Layout.list_view, listCountriesTitles);
                textViewCountries.Hint = "Название страны";
                textViewCountries.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewCountries.Enabled = false;
                textViewCountries.ItemClick += TextViewCountries_ItemClick;
                textViewCountries.Adapter = adapterCountry;

                textViewCities = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_city);
                textViewCities.Hint = "Название города";
                textViewCities.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewCities.Enabled = false;

                textViewUnivercities = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_univercity);
                textViewUnivercities.Hint = "Название университета";
                textViewUnivercities.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewUnivercities.Enabled = false;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private void SurnameView_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)//запоминаем данные ввода
                                                                                                          //и включаем след поле
        {
            try
            {
                savedSurnamePerson = surnameView.Text;
                textViewCountries.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private void NameView_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)//запоминаем данные ввода
                                                                                                       //и включаем след поле
        {
            try
            {
                savedNamePerson = nameView.Text;
                surnameView.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private void BGetBack_Click(object sender, EventArgs e)//создаем вьюшку бланка и всё необходимое для ее 
            //функционирования, инициализуем переменные полей сохраненными данными
        {
            try
            {
                SetContentView(Resource.Layout.Main);

                nameView = FindViewById<EditText>(Resource.Id.name);
                nameView.Hint = "Имя";
                nameView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                nameView.AfterTextChanged += NameView_AfterTextChanged;
                nameView.Text = savedNamePerson;

                surnameView = FindViewById<EditText>(Resource.Id.surname);
                surnameView.Hint = "Фамилия";
                surnameView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                surnameView.AfterTextChanged += SurnameView_AfterTextChanged;
                surnameView.Text = savedSurnamePerson;

                textViewCountries = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_country);
                var adapterCountry = new ArrayAdapter<String>(this, Resource.Layout.list_view, listCountriesTitles);
                textViewCountries.Hint = "Название страны";
                textViewCountries.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewCountries.ItemClick += TextViewCountries_ItemClick;
                textViewCountries.Adapter = adapterCountry;
                textViewCountries.Text = savedTitleCountry;

                textViewCities = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_city);
                var adapterCity = new ArrayAdapter<String>(this, Resource.Layout.list_view, listCitiesTitles);
                textViewCities.Hint = "Название города";
                textViewCities.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewCities.ItemClick += TextViewCities_ItemClick;
                textViewCities.Adapter = adapterCity;
                textViewCities.Text = savedTitleCity;

                textViewUnivercities = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_univercity);
                var adapterUnivercity = new ArrayAdapter<String>(this, Resource.Layout.list_view, listUnivercitiesTitles);
                textViewUnivercities.Hint = "Название университета";
                textViewUnivercities.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Black));
                textViewUnivercities.ItemClick += TextViewUnivercities_ItemClick;
                textViewUnivercities.Adapter = adapterUnivercity;
                textViewUnivercities.Text = savedTitleUnivercity;

                bFillBlank = FindViewById<Button>(Resource.Id.fillinBtn);
                bFillBlank.Click += BFillBlank_Click;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private void BFillBlank_Click(object sender, EventArgs e)//создаем вьюшку просмотра и всё необходимое для ее 
                                                                 //функционирования
        {
            try
            {
                SetContentView(Resource.Layout.printInfo);

                Button bGetBack = FindViewById<Button>(Resource.Id.getbackBtn);
                bGetBack.Click += BGetBack_Click;

                TextView nameInfo = FindViewById<TextView>(Resource.Id.nameInfo);
                nameInfo.Text = "Имя: " + savedNamePerson;

                TextView surnameInfo = FindViewById<TextView>(Resource.Id.surnameInfo);
                surnameInfo.Text = "Фамилия: " + savedSurnamePerson;

                TextView countryInfo = FindViewById<TextView>(Resource.Id.countryInfo);
                countryInfo.Text = "Страна: " + savedTitleCountry;

                TextView cityInfo = FindViewById<TextView>(Resource.Id.cityInfo);
                cityInfo.Text = "Город: " + savedTitleCity;

                TextView univercityInfo = FindViewById<TextView>(Resource.Id.univercityInfo);
                univercityInfo.Text = "ВУЗ: " + savedTitleUnivercity;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private async void TextViewCountries_ItemClick(object sender, AdapterView.ItemClickEventArgs e)//асинхронный запрос для городов
        {
            try
            {
                int index = listCountriesTitles.FindIndex(x => x == textViewCountries.Text);

                var client = new HttpClient();
                var content = await client.GetStringAsync("https://api.vk.com/method/database.getCities?country_id="
                    + cidCountries[index] + "&need_all=1&lang=0&count=1000");
                var json = JObject.Parse(content).SelectTokens("$.response[*]");
                var cities = json.Select(n => n.ToObject<City>()).ToList();
                listCitiesTitles = cities.Select(r => r.title).ToList();
                cidCities = cities.Select(r => r.cid).ToArray();
                savedTitleCountry = textViewCountries.Text;

                var adapterCity = new ArrayAdapter<String>(this, Resource.Layout.list_view, listCitiesTitles);
                textViewCities.ItemClick += TextViewCities_ItemClick;
                textViewCities.Adapter = adapterCity;
                textViewCities.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        private async void TextViewCities_ItemClick(object sender, AdapterView.ItemClickEventArgs e)//асинхронный запрос для вузов
        {
            try
            {
                int indexCountry = listCountriesTitles.FindIndex(x => x == textViewCountries.Text);
                long indexCity = listCitiesTitles.FindIndex(x => x == textViewCities.Text);

                var client = new HttpClient();
                var content = await client.GetStringAsync("https://api.vk.com/method/database.getUniversities?country_id="
                    + cidCountries[indexCountry] + "&city_id=" + cidCities[indexCity] + "&count=1000");

                var json = JObject.Parse(content).SelectTokens("$.response[1:]");

                var universitets = json.Select(n => n.ToObject<University>()).ToList();

                listUnivercitiesTitles = universitets.Select(x => x.title).ToList();
                idUniversitets = universitets.Select(x => x.id).ToArray();
                savedTitleCity = textViewCities.Text;

                var adapterUnivercity = new ArrayAdapter<String>(this, Resource.Layout.list_view, listUnivercitiesTitles);
                textViewUnivercities.ItemClick += TextViewUnivercities_ItemClick;
                textViewUnivercities.Adapter = adapterUnivercity;
                textViewUnivercities.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
            
        }

        private void TextViewUnivercities_ItemClick(object sender, AdapterView.ItemClickEventArgs e)//запоминаем данные ввода
                                                                                                    //и включаем след поле
        {
            try
            {
                savedTitleUnivercity = textViewUnivercities.Text;
                bFillBlank.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        //классы для удобной работы с возвращенными апи вк данными

        public class University
        {
            public int id { get; set; }
            public string title { get; set; }
        }

        public class City
        {
            public long cid { get; set; }
            public string title { get; set; }
            public string region { get; set; }
        }

        public class RootObjectCities
        {
            public List<City> response { get; set; }
        }

        public class Country
        {
            public int cid { get; set; }
            public string title { get; set; }
        }

        public class RootObjectCountry
        {
            public List<Country> response { get; set; }
        }
    }
}

