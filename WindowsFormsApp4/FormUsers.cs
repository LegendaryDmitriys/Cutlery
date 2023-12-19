using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class FormUsers : Form
    {
        private connection con = new connection();
        
        public FormUsers()
        {
            InitializeComponent();
            ApplyColorScheme();
        }
        
        
        // Применение цветовой схемы
        private void ApplyColorScheme()
        {
            this.BackColor = Color.White;
            label3.BackColor = Color.FromArgb(118, 227, 131);
            label3.ForeColor = Color.White;
            panel1.BackColor = Color.FromArgb(118, 227, 131);
            deleteProductButton.BackColor = Color.FromArgb(73, 140, 81); 
            deleteProductButton.ForeColor = Color.White;
            addProductButton.BackColor = Color.FromArgb(73, 140, 81); 
            addProductButton.ForeColor = Color.White;
        }
        
        // Загрузка формы
        private void FormUsers_Load(object sender, EventArgs e)
        {
            label1.Text = DataBank.Text;

            // Ограничение действий для незарегистрированных пользователей
            if (DataBank.Text == "")
            {
                deleteProductButton.Enabled = false;
                addProductButton.Enabled = false;
                deleteProductButton.Visible = false;
                addProductButton.Visible = false;
                dataGridView1.CellDoubleClick -= dataGridView1_CellDoubleClick;
                searchTextBox.Enabled = false;
                searchTextBox.Visible = false;
                searchLabel.Enabled = false;
                searchLabel.Visible = false;
                filterLabel.Enabled = false;
                filterLabel.Visible = false;
                filterManufactorComboBox.Enabled = false;
                filterManufactorComboBox.Visible = false;
            }
            
            // Ограничение действий для пользователей
            if (DataBank.Text2 == "1")
            {
                deleteProductButton.Enabled = false;
                deleteProductButton.Visible = false;
                addProductButton.Enabled = false;
                addProductButton.Visible = false;
            }
            
            // Ограничение действий для менеджеров
            else if (DataBank.Text2 == "2")
            {
                deleteProductButton.Enabled = false;
                deleteProductButton.Visible = false;
                addProductButton.Enabled = false;
                addProductButton.Visible = false;
                dataGridView1.CellDoubleClick -= dataGridView1_CellDoubleClick;
            }
            
            
            LoadDataIntoDataGridView();
            FillComboBox();
            filterManufactorComboBox.Items.Add("Все производители");
            dataGridView1.Refresh();
        }

        
        
        // Обработка кнопку "Удалить"
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string productArticleNumber = dataGridView1.CurrentRow.Cells["Артикул"].Value.ToString();
                DeleteProduct(productArticleNumber);
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        
        
        // Загрузка из базы-данных в GridView
        private void LoadDataIntoDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            string query = "SELECT * FROM product"; 
            MySqlDataAdapter ms_data = new MySqlDataAdapter(query, connection.strProvider);
            DataTable table = new DataTable();
            ms_data.Fill(table);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.HeaderText = "Фото"; 
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns.Add(imageColumn);
            
            dataGridView1.Columns.Add("Наименование товара", "Наименование товара");
            dataGridView1.Columns.Add("Описание товара", "Описание товара");
            dataGridView1.Columns.Add("Производитель", "Производитель");
            dataGridView1.Columns.Add("Цена", "Цена");
            dataGridView1.Columns.Add("Артикул", "Артикул");
            dataGridView1.Columns.Add("Количество на складе", "Количество на складе");
            
            foreach (DataRow row in table.Rows)
            {
                string fileName = row["ProductPhoto"].ToString();
                
                Image image = LoadImageFromFileName(fileName);
                
                dataGridView1.Rows.Add(image, row["ProductName"], row["ProductDescription"], row["ProductManufacturer"], row["ProductCost"], row["ProductArticleNumber"], row["ProductQuantityInStock"]);

            }
            
            int totalRecords = GetTotalRecordsCount();
            UpadateDataInfoLabel(dataGridView1.Rows.Count, totalRecords);
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

        
        // Поиск по атрибутам в GridView
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();

            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                return;

            var values = searchTextBox.Text.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                foreach (string value in values)
                {
                    var row = dataGridView1.Rows[i];

                    if (row.Cells["Наименование товара"].Value.ToString().Contains(value) ||
                        row.Cells["Описание товара"].Value.ToString().Contains(value) ||
                        row.Cells["Производитель"].Value.ToString().Contains(value) ||
                        row.Cells["Цена"].Value.ToString().Contains(value) ||
                        row.Cells["Количество на складе"].Value.ToString().Contains(value))
                    {
                        row.Selected = true;                        
                    }
                }
            }
            int totalRecords = GetTotalRecordsCount();
            
            UpadateDataInfoLabel(dataGridView1.Rows.Count, totalRecords);
        }

        
        
        private void filterManufactorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDataByManufacturer();
        }
        
        // Фильтрация по производителя GridView
        private void LoadDataFromQuery(string query)
        {
            con.Open();

            MySqlDataAdapter ms_data = new MySqlDataAdapter(query, connection.strProvider);
            DataTable table = new DataTable();

            ms_data.Fill(table);

            con.Close();
            
            dataGridView1.Rows.Clear();

            foreach (DataRow row in table.Rows)
            {
                string fileName = row["ProductPhoto"].ToString();
                Image image = LoadImageFromFileName(fileName);

                dataGridView1.Rows.Add(image, row["ProductName"], row["ProductDescription"], row["ProductManufacturer"], row["ProductCost"], row["ProductQuantityInStock"]);
            }
        }
        
        // Метод фильтрации
        private void FilterDataByManufacturer()
        {
            string selectedManufacturer = filterManufactorComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedManufacturer) || selectedManufacturer == "Все производители")
            {
                LoadDataIntoDataGridView();
            }
            else
            {
                string query = $"SELECT * FROM product WHERE ProductManufacturer = '{selectedManufacturer}'";
                LoadDataFromQuery(query);
            }
            
            int totalRecords = GetTotalRecordsCount();


            UpadateDataInfoLabel(dataGridView1.Rows.Count, totalRecords);
        }
        
        
        // Заполнение списка производителей
        private void FillComboBox()
        {
            con.Open();
            
            string query = "SELECT DISTINCT ProductManufacturer FROM product;";
            MySqlDataAdapter ms_data = new MySqlDataAdapter(query, connection.strProvider);
            DataTable table = new DataTable();  

            ms_data.Fill(table);

            con.Close();

            filterManufactorComboBox.Items.Clear();

            foreach (DataRow row in table.Rows)
            {
                filterManufactorComboBox.Items.Add(row["ProductManufacturer"].ToString());

            }
            
        }
        
        
        // Обновление счетчика записей
        private void UpadateDataInfoLabel( int displayedCount, int totalRecords)
        {
            dataInfoLable.Text = $"{displayedCount-1} из {totalRecords}";  
        }
        
        
        // Получение общего количества записей
        private int GetTotalRecordsCount()
        {
            con.Open();

            string query = "SELECT COUNT(*) FROM product";
            MySqlCommand cmd = new MySqlCommand(query, con.GetConnection());

            int totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            return totalRecords;
        }

        
        // Метод удаления продукта
        private void DeleteProduct(string ProductArticleNumber )
        {
           
            if (!IsProductInOrder(ProductArticleNumber))
            {
              
                string deleteProductQuery = $"DELETE FROM product WHERE ProductArticleNumber  = '{ProductArticleNumber }'";
                con.ExecuteQuery(deleteProductQuery);
                
                DeleteAdditionalProducts(ProductArticleNumber );

                MessageBox.Show("Товар успешно удален.");
                LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("Нельзя удалить товар, который присутствует в заказе.");
            }
        }

        
        
        // Удаление с условием - 1
        private void DeleteAdditionalProducts(string productArticleNumber)
        {
            con.Open();

            string query = $"DELETE FROM product WHERE ProductArticleNumber = '{productArticleNumber}' AND NOT EXISTS (SELECT 1 FROM orderproduct WHERE ProductArticleNumber = '{productArticleNumber}')";
            MySqlCommand cmd = new MySqlCommand(query, con.GetConnection());
            
            con.Close();
        }

        // Удаление с условием - 2
        private bool IsProductInOrder(string ProductArticleNumber)
        {
            con.Open();
            string query = $"SELECT COUNT(*) FROM orderproduct WHERE ProductArticleNumber = '{ProductArticleNumber }'";
            using (MySqlCommand cmd = new MySqlCommand(query, con.GetConnection()))
            {
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                    
                return count > 0;
            }
        }
        
        
        // Показ формы добавления товара
        private void OpenAddProductForm()
        {
            FormAddProduct addProductForm = new FormAddProduct();
            addProductForm.ShowDialog();
            LoadDataIntoDataGridView();
        }
        // Показ формы редактирования товара
        private void OpenEditProductForm(string ProductArticleNumber)
        {
            FormEditProduct editProductForm = new FormEditProduct(ProductArticleNumber);
            editProductForm.ShowDialog();
            LoadDataIntoDataGridView(); 
        }

        
        // Кнопка добавления товара
        private void addProductButton_Click(object sender, EventArgs e)
        {
            OpenAddProductForm();
        }

        
        
        // Метод для открытия формы редактировая
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string ProductArticleNumber = dataGridView1.Rows[e.RowIndex].Cells["Артикул"].Value.ToString();
                
                OpenEditProductForm(ProductArticleNumber);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        // Запрет на удаление из DataGrid 
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }
    }
}
