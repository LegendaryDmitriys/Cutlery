using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class FormEditProduct : Form
    {
        private connection con = new connection();
        private string ProductArticleNumber;
        public FormEditProduct(string ProductArticleNumber)
        {
            InitializeComponent();

            this.ProductArticleNumber = ProductArticleNumber;
            Console.WriteLine(ProductArticleNumber);
            LoadProductData();
            LoadCategories();
            
            imgPictureBox.DoubleClick += imgPictureBox_DoubleClick;

            imgPictureBox.Width = 300;
            imgPictureBox.Height = 200;
            
            ApplyColorScheme();

            // Если роль пользователя - user, делаем поля только для чтения
            
            if (DataBank.Text2 == "1")
            {
                nameTextBox.ReadOnly = true;
                descriptionTextBox.ReadOnly = true;
                costTextBox.ReadOnly = true;
                manufacterTextBox.ReadOnly = true;
                quantityTextBox.ReadOnly = true;
                unitTextBox.ReadOnly = true;
                categoryComboBox.Enabled = false;
                
            }
            
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
        
        
        // Загрузка данных о продукте
        private void LoadProductData()
        {
            
            con.Open();

            string query = $"SELECT * FROM product WHERE ProductArticleNumber = '{ProductArticleNumber}'";
            MySqlCommand cmd = new MySqlCommand(query, con.GetConnection());
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                nameTextBox.Text = reader["ProductName"].ToString();
                descriptionTextBox.Text = reader["ProductDescription"].ToString();
                costTextBox.Text = reader["ProductCost"].ToString();
                manufacterTextBox.Text = reader["ProductManufacturer"].ToString();
                quantityTextBox.Text = reader["ProductQuantityInStock"].ToString();
                imgPictureBox.Text = reader["ProductPhoto"].ToString();
                unitTextBox.Text = reader["ProductUnit"].ToString();
                
                string fileName = reader["ProductPhoto"].ToString();
                Image productImage = LoadImageFromFileName(fileName);
                imgPictureBox.Image = productImage;
                
            }

            con.Close();
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
        
        
        // Загрузка изображения по имени файла
        private Image LoadImageFromFileName(string fileName)
        {
            string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);
            string notImages = Path.Combine(Application.StartupPath, "Images", "picture.png");
            
            if (File.Exists(imagePath))
            {
                return Image.FromFile(imagePath);
            }
            else
            {
                return Image.FromFile(notImages);
            }
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
        
        
        // Сохранение изображения в базу данных
        private void SaveImageToDatabase()
        {
            if (imgPictureBox.Image != null)
            {
                con.Open();
        
                string fileName = Guid.NewGuid().ToString() + ".png";

                string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);
                
                imgPictureBox.Image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                
                string updateQuery = $"UPDATE product SET ProductPhoto = '{fileName}' WHERE ProductArticleNumber = '{ProductArticleNumber}'";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, con.GetConnection());
                updateCommand.ExecuteNonQuery();

                con.Close();
            }
        }

        // Обработчик нажатия на кнопку "Сохранить"
        private void saveButton_Click(object sender, EventArgs e)
        {
            con.Open();
            
            string updateQuery = $"UPDATE product SET ProductName = '{nameTextBox.Text}', ProductDescription = '{descriptionTextBox.Text}', ProductCost = {costTextBox.Text}, ProductManufacturer = '{manufacterTextBox.Text}', ProductQuantityInStock = {quantityTextBox.Text}, ProductUnit = '{unitTextBox.Text}' WHERE ProductArticleNumber = '{ProductArticleNumber}'";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, con.GetConnection());
            updateCommand.ExecuteNonQuery();
            
            if (categoryComboBox.SelectedItem != null)
            {
                string updateCategoryQuery = $"UPDATE product SET ProductCategory = '{categoryComboBox.SelectedItem.ToString()}' WHERE ProductArticleNumber = '{ProductArticleNumber}'";
                MySqlCommand updateCategoryCommand = new MySqlCommand(updateCategoryQuery, con.GetConnection());
                updateCategoryCommand.ExecuteNonQuery();
            }
            
            if (imgPictureBox.Image != null)
            {
                SaveImageToDatabase();
            }

            con.Close();
    
            MessageBox.Show("Изменения сохранены успешно.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            LoadProductData();
        }
    }
    
}