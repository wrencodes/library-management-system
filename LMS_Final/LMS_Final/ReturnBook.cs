using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class ReturnBook : Form
    {
        private string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        private int _selectedStudentId;

        public ReturnBook()
        {
            InitializeComponent();
            LoadStudentsToCombo();
        }

        private void LoadStudentsToCombo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                        SELECT 
                            StudentID,
                            FirstName + ' ' + LastName AS FullName
                        FROM Users
                        ORDER BY FullName";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    SelectCMBBX.DataSource = dt;
                    SelectCMBBX.DisplayMember = "FullName";
                    SelectCMBBX.ValueMember = "StudentID";

                    if (dt.Rows.Count > 0)
                        SelectCMBBX.SelectedIndex = 0;
                    else
                        SelectCMBBX.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load students.\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBorrowedBooksForStudent(int studentId)
        {
            TitleCMBBX.DataSource = null;
            TitleCMBBX.Items.Clear();

            AuthorTXTBX.Text = "";
            GenreTXTBX.Text = "";
            PublishingYearTXTBX.Text = "";
            BorrowDateTXTBX.Text = "";
            DueDateTXTBX.Text = "";
            pictureBox3.Image = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
                    SELECT b.BookID, b.Title
                    FROM BorrowedBooks bb
                    JOIN Books b ON bb.BookID = b.BookID
                    WHERE bb.StudentID = @StudentID
                      AND bb.ReturnDate IS NULL";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        TitleCMBBX.DataSource = dt;
                        TitleCMBBX.DisplayMember = "Title";
                        TitleCMBBX.ValueMember = "BookID";
                        // wag na mag-set ng SelectedIndex dito
                    }
                    else
                    {
                        TitleCMBBX.DataSource = null;
                        TitleCMBBX.Items.Clear();
                    }

                }
            }
        }

        private void SelectCMBBX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectCMBBX.SelectedItem is DataRowView row)
            {
                _selectedStudentId = Convert.ToInt32(row["StudentID"]);
                LoadBorrowedBooksForStudent(_selectedStudentId);
            }
        }

        private void TitleCMBBX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectedStudentId <= 0)
                return;

            if (!(TitleCMBBX.SelectedItem is DataRowView row))
                return;

            int bookID = Convert.ToInt32(row["BookID"]);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
            SELECT b.Author, b.Genre, b.PublishingYear,
                   bb.BorrowDate, bb.DueDate, b.CoverImage
            FROM BorrowedBooks bb
            JOIN Books b ON bb.BookID = b.BookID
            WHERE bb.StudentID = @StudentID
              AND bb.BookID = @BookID
              AND bb.ReturnDate IS NULL";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", _selectedStudentId);
                    cmd.Parameters.AddWithValue("@BookID", bookID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AuthorTXTBX.Text = reader["Author"] != DBNull.Value
                                ? reader["Author"].ToString()
                                : "";

                            GenreTXTBX.Text = reader["Genre"] != DBNull.Value
                                ? reader["Genre"].ToString()
                                : "";

                            PublishingYearTXTBX.Text = reader["PublishingYear"] != DBNull.Value
                                ? reader["PublishingYear"].ToString()
                                : "";

                            if (reader["BorrowDate"] != DBNull.Value)
                                BorrowDateTXTBX.Text = Convert.ToDateTime(reader["BorrowDate"]).ToShortDateString();
                            else
                                BorrowDateTXTBX.Text = "";

                            if (reader["DueDate"] != DBNull.Value)
                                DueDateTXTBX.Text = Convert.ToDateTime(reader["DueDate"]).ToShortDateString();
                            else
                                DueDateTXTBX.Text = "";

                            if (reader["CoverImage"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["CoverImage"];
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    pictureBox3.Image = Image.FromStream(ms);
                                }
                                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            else
                            {
                                pictureBox3.Image = null;
                            }
                        }
                    }
                }
            }
        }


        private void Return_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId <= 0)
            {
                MessageBox.Show("Please select a student.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(TitleCMBBX.SelectedItem is DataRowView row))
            {
                MessageBox.Show("Please select a book to return.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentID = _selectedStudentId;
            int bookID = Convert.ToInt32(row["BookID"]);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                DateTime? dueDate = null;
                string dueQuery = @"
                    SELECT DueDate
                    FROM BorrowedBooks
                    WHERE StudentID = @StudentID
                      AND BookID = @BookID
                      AND ReturnDate IS NULL";

                using (SqlCommand cmd = new SqlCommand(dueQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@BookID", bookID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        dueDate = Convert.ToDateTime(result);
                }

                DateTime now = DateTime.Now;

                string updateBorrow = @"
                    UPDATE BorrowedBooks
                    SET ReturnDate = @ReturnDate
                    WHERE StudentID = @StudentID
                      AND BookID = @BookID
                      AND ReturnDate IS NULL";

                using (SqlCommand updateCmd = new SqlCommand(updateBorrow, con))
                {
                    updateCmd.Parameters.AddWithValue("@ReturnDate", now);
                    updateCmd.Parameters.AddWithValue("@StudentID", studentID);
                    updateCmd.Parameters.AddWithValue("@BookID", bookID);
                    updateCmd.ExecuteNonQuery();
                }

                string updateBook = @"UPDATE Books SET Quantity = Quantity + 1 WHERE BookID = @BookID";

                using (SqlCommand incBook = new SqlCommand(updateBook, con))
                {
                    incBook.Parameters.AddWithValue("@BookID", bookID);
                    incBook.ExecuteNonQuery();
                }

                bool early = dueDate.HasValue && now.Date < dueDate.Value.Date;
                int addPoints = early ? 2 : 1;

                string pointsQuery = @"
                    IF EXISTS (SELECT 1 FROM StudentPoints WHERE StudentID = @StudentID)
                        UPDATE StudentPoints 
                        SET Points = Points + @Add
                        WHERE StudentID = @StudentID;
                    ELSE
                        INSERT INTO StudentPoints (StudentID, Points)
                        VALUES (@StudentID, @Add);";

                using (SqlCommand cmd = new SqlCommand(pointsQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@Add", addPoints);
                    cmd.ExecuteNonQuery();
                }

                string msg = early
                    ? "Book returned successfully!\n\nPoints added: 2 (1 refund + 1 early return bonus)."
                    : "Book returned successfully!\n\nPoints added: 1 (refund).";

                MessageBox.Show(msg,
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadBorrowedBooksForStudent(studentID);

                AuthorTXTBX.Text = "";
                GenreTXTBX.Text = "";
                PublishingYearTXTBX.Text = "";
                BorrowDateTXTBX.Text = "";
                DueDateTXTBX.Text = "";
                pictureBox3.Image = null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void ReturnBook_Load(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e) { }
    }
}
