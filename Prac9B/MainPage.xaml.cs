using System.Security.Cryptography;
using System.Xml.Linq;

namespace Prac9B
{
    public partial class MainPage : ContentPage
    {
        private string selectedImagePath;

        public MainPage()
        {
            InitializeComponent();
           
            LoadSavedData();
        }

        private void LoadSavedData()
        {
           
            // Второй параметр - значение по умолчанию, если данные не найдены

            ecName.Text = Preferences.Default.Get("name", "");
            ecFamily.Text = Preferences.Default.Get("family", "");
            ecDopName.Text = Preferences.Default.Get("dopName", "");
            ecRas.Text = Preferences.Default.Get("ras", "");
            ecWorld.Text = Preferences.Default.Get("world", "");
            ecFracia.Text = Preferences.Default.Get("fractia", "");
            ecSpeshial.Text = Preferences.Default.Get("speshial", "");

           
            double savedAge = Preferences.Default.Get("age", 18.0);
            stAge.Value = savedAge;
            lblAge.Text = savedAge.ToString();

            
            swPsayker.On = Preferences.Default.Get("psayker", false);

            
            string savedGod = Preferences.Default.Get("god", "");
            if (!string.IsNullOrEmpty(savedGod))
            {
                pickerGod.SelectedItem = savedGod;
            }

            string savedLegion = Preferences.Default.Get("legion", "");
            if (!string.IsNullOrEmpty(savedLegion))
            {
                pickerLegion.SelectedItem = savedLegion;
            }

            selectedImagePath = Preferences.Default.Get("photoPath", "");
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                selectedImage.Source = ImageSource.FromFile(selectedImagePath);
            }
        }

        private void SaveData()
        {
           
            // Первый параметр - ключ, второй - значение

            Preferences.Default.Set("name", ecName.Text ?? "");
            Preferences.Default.Set("family", ecFamily.Text ?? "");
            Preferences.Default.Set("dopName", ecDopName.Text ?? "");
            Preferences.Default.Set("ras", ecRas.Text ?? "");
            Preferences.Default.Set("world", ecWorld.Text ?? "");
            Preferences.Default.Set("fractia", ecFracia.Text ?? "");
            Preferences.Default.Set("speshial", ecSpeshial.Text ?? "");
            Preferences.Default.Set("age", stAge.Value);
            Preferences.Default.Set("psayker", swPsayker.On);
            Preferences.Default.Set("god", pickerGod.SelectedItem?.ToString() ?? "");
            Preferences.Default.Set("legion", pickerLegion.SelectedItem?.ToString() ?? "");
            Preferences.Default.Set("photoPath", selectedImagePath ?? "");
        }

        private void stAge_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblAge.Text = e.NewValue.ToString();
            
            SaveData();
        }

        
        

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            SaveData();
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            SaveData();
        }

        private async void btnChaos_Click(object sender, System.EventArgs e)
        {
            
            SaveData();

            string name = !string.IsNullOrEmpty(ecName.Text) ? ecName.Text : "Неизвестно";
            string family = !string.IsNullOrEmpty(ecFamily.Text) ? ecFamily.Text : "Неизвестно";
            string dopName = !string.IsNullOrEmpty(ecDopName.Text) ? ecDopName.Text : "Неизвестно";
            string ras = !string.IsNullOrEmpty(ecRas.Text) ? ecRas.Text : "Неизвестно";
            string world = !string.IsNullOrEmpty(ecWorld.Text) ? ecWorld.Text : "Неизвестно";
            string fractia = !string.IsNullOrEmpty(ecFracia.Text) ? ecFracia.Text : "Неизвестно";
            string age = lblAge.Text ?? "18";
            string speshial = !string.IsNullOrEmpty(ecSpeshial.Text) ? ecSpeshial.Text : "Неизвестно";

            string psayker = swPsayker.On ? "Да" : "Нет";

            string god = !string.IsNullOrEmpty(pickerGod.SelectedItem?.ToString()) ?
                        pickerGod.SelectedItem.ToString() : "Не выбран";

            string legion = !string.IsNullOrEmpty(pickerLegion.SelectedItem?.ToString()) ?
                           pickerLegion.SelectedItem.ToString() : "Не выбран";

            string photoInfo = !string.IsNullOrEmpty(selectedImagePath) ?
                         $"Фото: {Path.GetFileName(selectedImagePath)}" : "Фото не выбрано";

            string message = $"Заявка на вступление в ряды Хаоса:\n\n" +
                        $"Имя: {name}\n" +
                        $"Фамилия: {family}\n" +
                        $"Титул или прозвище: {dopName}\n" +
                        $"Раса: {ras}\n" +
                        $"Родной мир: {world}\n" +
                        $"Прошлая фракция: {fractia}\n" +
                        $"Возраст: {age}\n" +
                        $"Специализация: {speshial}\n" +
                        $"Псайкер: {psayker}\n" +
                        $"Бог хаоса: {god}\n" +
                        $"Легион: {legion}\n" +
                        $"{photoInfo}";

            await DisplayAlert("Заявка подана!", message, "Да здравствует Хаос!");
        }

        private async void OnPickPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите фото адепта Хаоса",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    selectedImagePath = result.FullPath;
                    selectedImage.Source = ImageSource.FromFile(selectedImagePath);
                    
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось выбрать фото: {ex.Message}", "OK");
            }
        }
    }
}
