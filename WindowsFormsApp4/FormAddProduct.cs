using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class FormAddProduct : Form
    {
        private connection con = new connection();
        
        public FormAddProduct()
        {
            InitializeComponent();
            ApplyColorScheme();
            LoadCategories();
            imgPictureBox.DoubleClick += imgPictureBox_DoubleClick;
            imgPictureBox.Width = 300;
            imgPictureBox.Height = 200;
        }
        
        
        // Применение цветовой схемы
        private void ApplyColorScheme()
        {
            this.BackColor = Color.White;
            titleLable.BackColor = Color.FromArgb(118, 227, 131);
            titleLable.ForeColor = Color.White;
            panel1.BackColor = Color.FromArgb(118, 227, 131);
            saveButton.BackColor = Color.FromArgb(73, 140, 81); 
            saveButton.ForeColor = Color.White;

        }

        
        // Загрузка категорий товаров
        private void LoadCategories()
        {
            con.Open();
            
            categoryComboBox.Items.Clear();
            
            string query = "SELECT DISTINCT ProductCategory FROM product";
            MySqlCommand cmd = new MySqlCommand(query, con.GetConnection());
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categoryComboBox.Items.Add(reader["ProductCategory"].ToString());
            }

            con.Close();
        }

        
        // Обработчик двойного клика по изображению для выбора файла
        private void imgPictureBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения PNG (*.png)|*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                Image selectedImage = Image.FromFile(selectedImagePath);
                imgPictureBox.Image = selectedImage;
            }
        }
        
        // Сохранение изображения и данных о товаре в базу данных
        private void SaveImageAndProductToDatabase()
        {
            if (imgPictureBox.Image != null)
            {
                con.Open();

                try
                {
                    if (decimal.TryParse(costTextBox.Text, out decimal cost) && cost >= 0 &&
                        int.TryParse(quantityTextBox.Text, out int quantity) && quantity >= 0)
                    {

                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);
                        imgPictureBox.Image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                        
                        string insertQuery = $"INSERT INTO product (ProductName, ProductDescription, ProductCost, ProductManufacturer, ProductQuantityInStock, ProductUnit, ProductCategory, ProductArticleNumber, ProductPhoto) VALUES ('{nameTextBox.Text}', '{descriptionTextBox.Text}', {cost}, '{manufacterTextBox.Text}', {quantity}, '{unitTextBox.Text}', '{categoryComboBox.SelectedItem?.ToString()}', '{articleNumberTextBox.Text}', '{fileName}')";

                        MySqlCommand insertCommand = new MySqlCommand(insertQuery, con.GetConnection());
                        insertCommand.ExecuteNonQuery();

                        MessageBox.Show("Товар успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные значения для стоимости и количества товара.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        
        // Обработчик нажатия на кнопку "Сохранить"
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveImageAndProductToDatabase();
        }
    }
}
