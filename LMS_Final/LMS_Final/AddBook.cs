using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class AddBook : Form
    {
        string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        public AddBook()
        {
            InitializeComponent();
            LoadBooks();
        }
        private void LoadBooks(string searchTerm = "")
        {
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add("BookID", "BookID");
                dataGridView1.Columns["BookID"].Visible = false;

                DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
                imgCol.HeaderText = "Cover";
                imgCol.Name = "CoverImage";
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridView1.Columns.Add(imgCol);

                dataGridView1.Columns.Add("Title", "Title");
                dataGridView1.Columns.Add("Author", "Author");
                dataGridView1.Columns.Add("Genre", "Genre");
                dataGridView1.Columns.Add("Quantity", "Quantity");
                dataGridView1.Columns.Add("PublishingYear", "Year");
            }

            dataGridView1.Rows.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    SELECT BookID, Title, Author, Genre, Quantity, PublishingYear, CoverImage
                    FROM Books
                    WHERE (@search = '' 
                           OR Title LIKE @search 
                           OR Author LIKE @search 
                           OR Genre LIKE @search)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@search",
                        string.IsNullOrWhiteSpace(searchTerm) ? "" : "%" + searchTerm + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Image img = null;
                            if (reader["CoverImage"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["CoverImage"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    img = Image.FromStream(ms);
                                }
                            }

                            dataGridView1.Rows.Add(
                                reader["BookID"],
                                img,
                                reader["Title"],
                                reader["Author"],
                                reader["Genre"],
                                reader["Quantity"],
                                reader["PublishingYear"]
                            );
                        }
                    }
                }
            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private void Genre_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }
        private void ClearFields()
        {
            TitleTXTBX.Text = "";
            AuthorTXTBX.Text = "";
            GenreTXTBX.Text = "";
            QuantityTXTBX.Text = "";
            PublishingYearTXTBX.Text = "";
            Picture.Image = null;
        }
        // REMOVE BOOK BUTTON :3
        private void Remove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to remove.");
                return;
            }

            int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM Books WHERE BookID = @BookID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BookID", bookId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book removed successfully!");
                            ClearFields();
                            LoadBooks();
                        }
                        else
                        {
                            MessageBox.Show("No book found with that ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        // ADD BOOK BUTTON :3
        private void Add_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO Books 
                                    (Title, Author, Genre, Quantity, PublishingYear, CoverImage) 
                                     VALUES 
                                    (@Title, @Author, @Genre, @Quantity, @PublishingYear, @CoverImage)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", TitleTXTBX.Text.Trim());
                        cmd.Parameters.AddWithValue("@Author", AuthorTXTBX.Text.Trim());
                        cmd.Parameters.AddWithValue("@Genre", GenreTXTBX.Text.Trim());

                        if (!int.TryParse(QuantityTXTBX.Text.Trim(), out int quantity))
                        {
                            MessageBox.Show("Please enter a valid number for Quantity.");
                            return;
                        }
                        cmd.Parameters.AddWithValue("@Quantity", quantity);

                        if (!int.TryParse(PublishingYearTXTBX.Text.Trim(), out int year))
                        {
                            MessageBox.Show("Please enter a valid number for Publishing Year.");
                            return;
                        }
                        cmd.Parameters.AddWithValue("@PublishingYear", year);

                        if (Picture.Image != null)
                        {
                            byte[] imgBytes = ImageToByteArray(Picture.Image);
                            cmd.Parameters.AddWithValue("@CoverImage", imgBytes);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CoverImage", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Book Added Successfully!");
                ClearFields();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // UPDATE BOOK BUTTON :3
        private void Update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to update.");
                return;
            }

            int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookID"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"UPDATE Books 
                                     SET Title = @Title, Author = @Author, Genre = @Genre, 
                                         Quantity = @Quantity, PublishingYear = @PublishingYear, CoverImage = @CoverImage 
                                     WHERE BookID = @BookID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", TitleTXTBX.Text.Trim());
                        cmd.Parameters.AddWithValue("@Author", AuthorTXTBX.Text.Trim());
                        cmd.Parameters.AddWithValue("@Genre", GenreTXTBX.Text.Trim());

                        if (int.TryParse(QuantityTXTBX.Text.Trim(), out int qty))
                            cmd.Parameters.AddWithValue("@Quantity", qty);
                        else
                            cmd.Parameters.AddWithValue("@Quantity", DBNull.Value);

                        if (int.TryParse(PublishingYearTXTBX.Text.Trim(), out int year))
                            cmd.Parameters.AddWithValue("@PublishingYear", year);
                        else
                            cmd.Parameters.AddWithValue("@PublishingYear", DBNull.Value);

                        if (Picture.Image != null)
                        {
                            byte[] imgBytes = ImageToByteArray(Picture.Image);
                            cmd.Parameters.AddWithValue("@CoverImage", imgBytes);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CoverImage", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@BookID", bookId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book updated successfully!");
                            ClearFields();
                            LoadBooks();
                        }
                        else
                        {
                            MessageBox.Show("No book found with that ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        // UPLOAD BOOK BUTTON :3
        private void Upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Book Cover";
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Picture.Image = Image.FromFile(ofd.FileName);
                Picture.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            TitleTXTBX.Text = row.Cells["Title"].Value?.ToString();
            AuthorTXTBX.Text = row.Cells["Author"].Value?.ToString();
            GenreTXTBX.Text = row.Cells["Genre"].Value?.ToString();
            QuantityTXTBX.Text = row.Cells["Quantity"].Value?.ToString();
            PublishingYearTXTBX.Text = row.Cells["PublishingYear"].Value?.ToString();

            if (row.Cells["CoverImage"].Value != null && row.Cells["CoverImage"].Value is Image)
            {
                Picture.Image = (Image)row.Cells["CoverImage"].Value;
            }
            else
            {
                Picture.Image = null;
            }
        }
        private void SearchTXTBX_TextChanged(object sender, EventArgs e)
        {
            LoadBooks(SearchTXTBX.Text.Trim());
        }
    }
}
