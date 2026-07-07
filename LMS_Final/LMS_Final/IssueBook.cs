using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class IssueBook : Form
    {
        private int adminID;
        private string firstName;

        string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        public IssueBook()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks(string searchTerm = "")
        {
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    SELECT BookID, Title, Author, Genre, PublishingYear, CoverImage 
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
                            string bookID = reader["BookID"].ToString();
                            string bookTitle = reader["Title"].ToString();
                            string author = reader["Author"].ToString();
                            string genre = reader["Genre"].ToString();
                            int year = Convert.ToInt32(reader["PublishingYear"]);
                            byte[] coverImageBytes = reader["CoverImage"] != DBNull.Value
                                ? (byte[])reader["CoverImage"]
                                : null;

                            Panel panel = new Panel
                            {
                                Width = 150,
                                Height = 240,
                                Margin = new Padding(10),
                                BorderStyle = BorderStyle.FixedSingle,
                                BackColor = Color.WhiteSmoke
                            };

                            PictureBox pic = new PictureBox
                            {
                                Width = 140,
                                Height = 160,
                                Top = 5,
                                Left = 5,
                                SizeMode = PictureBoxSizeMode.Zoom,
                                BorderStyle = BorderStyle.FixedSingle
                            };

                            if (coverImageBytes != null)
                            {
                                using (MemoryStream ms = new MemoryStream(coverImageBytes))
                                {
                                    pic.Image = Image.FromStream(ms);
                                }
                            }
                            else
                            {
                                pic.BackColor = Color.LightGray;
                            }

                            Label lblTitle = new Label
                            {
                                Text = bookTitle,
                                AutoSize = false,
                                Width = 140,
                                Height = 25,
                                Top = pic.Bottom + 5,
                                Left = 5,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Arial", 9, FontStyle.Bold),
                                ForeColor = Color.Black
                            };

                            Button btnBorrow = new Button
                            {
                                Text = "Borrow",
                                Width = 90,
                                Height = 28,
                                Left = (panel.Width - 90) / 2,
                                Top = lblTitle.Bottom + 3,
                                BackColor = Color.FromArgb(154, 63, 63),
                                ForeColor = Color.White,
                                FlatStyle = FlatStyle.Flat,
                                Font = new Font("Arial", 8, FontStyle.Bold)
                            };
                            btnBorrow.FlatAppearance.BorderSize = 0;

                            btnBorrow.Click += (s, e) =>
                            {
                                BorrowBook borrowbook = new BorrowBook(
                                    bookID,
                                    bookTitle,
                                    author,
                                    genre,
                                    year.ToString(),
                                    coverImageBytes
                                );
                                borrowbook.Show();
                                this.Hide();
                            };

                            panel.Controls.Add(pic);
                            panel.Controls.Add(lblTitle);
                            panel.Controls.Add(btnBorrow);

                            flowLayoutPanel1.Controls.Add(panel);
                        }
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadBooks(textBox1.Text.Trim());
        }
    }
}
