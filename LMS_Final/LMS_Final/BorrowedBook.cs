using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class BorrowedBook : Form
    {
        private string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        public BorrowedBook()
        {
            InitializeComponent();
            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks(string searchTerm = "")
        {
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    SELECT 
                        b.BookID,
                        b.Title,
                        b.Author,
                        b.Genre,
                        b.PublishingYear,
                        b.CoverImage,
                        bb.BorrowDate,
                        bb.DueDate,
                        u.FirstName,
                        u.LastName
                    FROM BorrowedBooks bb
                    JOIN Books b ON bb.BookID = b.BookID
                    JOIN Users u ON bb.StudentID = u.StudentID
                    WHERE bb.ReturnDate IS NULL
                      AND (
                           @search = '' 
                           OR u.FirstName LIKE @search
                           OR u.LastName LIKE @search
                           OR (u.FirstName + ' ' + u.LastName) LIKE @search
                      )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (string.IsNullOrWhiteSpace(searchTerm))
                        cmd.Parameters.AddWithValue("@search", "");
                    else
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader["Title"].ToString();
                            string author = reader["Author"].ToString();
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();

                            DateTime borrowDate = reader["BorrowDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["BorrowDate"])
                                : DateTime.MinValue;

                            DateTime dueDate = reader["DueDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DueDate"])
                                : DateTime.MinValue;

                            int remainingDays = (dueDate.Date - DateTime.Now.Date).Days;

                            byte[] coverImageBytes = reader["CoverImage"] != DBNull.Value
                                ? (byte[])reader["CoverImage"]
                                : null;

                            Panel panel = new Panel
                            {
                                Width = 200,
                                Height = 290,
                                Margin = new Padding(10),
                                BorderStyle = BorderStyle.FixedSingle,
                                BackColor = Color.WhiteSmoke
                            };

                            PictureBox pic = new PictureBox
                            {
                                Width = 170,
                                Height = 160,
                                Top = 8,
                                Left = 12,
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
                                Text = title,
                                AutoSize = false,
                                Width = 170,
                                Height = 20,
                                Top = pic.Bottom + 5,
                                Left = 12,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                ForeColor = Color.Black
                            };

                            Label lblAuthor = new Label
                            {
                                Text = "by " + author,
                                AutoSize = false,
                                Width = 170,
                                Height = 18,
                                Top = lblTitle.Bottom,
                                Left = 12,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                                ForeColor = Color.DimGray
                            };

                            Label lblStudent = new Label
                            {
                                Text = firstName + " " + lastName,
                                AutoSize = false,
                                Width = 170,
                                Height = 18,
                                Top = lblAuthor.Bottom,
                                Left = 12,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                                ForeColor = Color.MediumSlateBlue
                            };

                            string statusText;
                            Color statusColor;

                            if (dueDate == DateTime.MinValue)
                            {
                                statusText = "No due date";
                                statusColor = Color.Gray;
                            }
                            else if (remainingDays < 0)
                            {
                                statusText = "⚠ Overdue! Please return";
                                statusColor = Color.Red;
                            }
                            else if (remainingDays == 0)
                            {
                                statusText = "Due today!";
                                statusColor = Color.OrangeRed;
                            }
                            else if (remainingDays <= 2)
                            {
                                statusText = "⏳ " + remainingDays + " day(s) left";
                                statusColor = Color.Orange;
                            }
                            else
                            {
                                statusText = remainingDays + " days left";
                                statusColor = Color.ForestGreen;
                            }

                            Label lblStatus = new Label
                            {
                                Text = statusText,
                                AutoSize = false,
                                Width = 170,
                                Height = 35,
                                Top = lblStudent.Bottom + 3,
                                Left = 12,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                                ForeColor = statusColor
                            };

                            string datesText = "";

                            if (borrowDate != DateTime.MinValue)
                                datesText += "Borrowed: " + borrowDate.ToString("MM/dd/yyyy");

                            if (dueDate != DateTime.MinValue)
                                datesText += "\nDue: " + dueDate.ToString("MM/dd/yyyy");

                            Label lblDates = new Label
                            {
                                Text = datesText,
                                AutoSize = false,
                                Width = 170,
                                Height = 30,
                                Top = lblStatus.Bottom,
                                Left = 12,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Font = new Font("Segoe UI", 8)
                            };

                            panel.Controls.Add(pic);
                            panel.Controls.Add(lblTitle);
                            panel.Controls.Add(lblAuthor);
                            panel.Controls.Add(lblStudent);
                            panel.Controls.Add(lblStatus);
                            panel.Controls.Add(lblDates);

                            flowLayoutPanel1.Controls.Add(panel);
                        }
                    }
                }
            }
        }

        private void BorrowedBook_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            LoadBorrowedBooks(textBox1.Text.Trim());
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }
    }
}
