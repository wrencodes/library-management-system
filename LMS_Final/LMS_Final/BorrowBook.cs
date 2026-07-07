using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class BorrowBook : Form
    {
        private string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        private string _bookID;

        public BorrowBook()
        {
            InitializeComponent();
            LoadStudentsToCombo();
        }

        public BorrowBook(string bookID,
                          string title,
                          string author,
                          string genre,
                          string year,
                          byte[] coverImageBytes) : this()
        {
            _bookID = bookID;

            TitleTXBX.Text = title;
            AuthorTXTBX.Text = author;
            GenreTXTBX.Text = genre;
            YearTXTBX.Text = year;

            BorrowDateTXTBX.Text = DateTime.Now.ToShortDateString();
            DueDateTXTBX.Text = DateTime.Now.AddDays(7).ToShortDateString();

            if (coverImageBytes != null && coverImageBytes.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(coverImageBytes))
                {
                    pictureBox3.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox3.Image = null;
            }

            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
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
                MessageBox.Show("LoadStudentsToCombo failed\n\n" + ex.Message,
                                "SQL ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private int GetStudentPoints(SqlConnection con, int studentID)
        {
            string query = "SELECT Points FROM StudentPoints WHERE StudentID = @StudentID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    string insert = "INSERT INTO StudentPoints (StudentID, Points) VALUES (@StudentID, 1)";
                    using (SqlCommand ins = new SqlCommand(insert, con))
                    {
                        ins.Parameters.AddWithValue("@StudentID", studentID);
                        ins.ExecuteNonQuery();
                    }
                    return 1;
                }

                return Convert.ToInt32(result);
            }
        }

        private void Borrow_Click(object sender, EventArgs e)
        {
            if (SelectCMBBX.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a student first.",
                        "No student selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_bookID))
            {
                MessageBox.Show("No book selected. Please reopen this form.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int studentID = Convert.ToInt32(SelectCMBBX.SelectedValue);
            string bookId = _bookID;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string overdueQuery = @"
                SELECT 
                bb.BookID,
                b.Title,
                bb.DueDate
                FROM BorrowedBooks bb
                INNER JOIN Books b ON bb.BookID = b.BookID
                WHERE bb.StudentID = @StudentID
                AND bb.Status = 'Approved'
                AND bb.ReturnDate IS NULL
                AND bb.DueDate < @Today";

                using (SqlCommand cmd = new SqlCommand(overdueQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@Today", DateTime.Now.Date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            string message = "This student has overdue book(s):\n\n";

                            while (reader.Read())
                            {
                                string obBookId = reader["BookID"].ToString();
                                string obTitle = reader["Title"].ToString();
                                DateTime due = (DateTime)reader["DueDate"];

                                message += "• " + obTitle +
                                           " – Due: " + due.ToString("yyyy-MM-dd") + "\n";
                            }

                            MessageBox.Show(
                                message,
                                "Overdue Books",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }


                int points = GetStudentPoints(con, studentID);
                if (points <= 0)
                {
                    MessageBox.Show(
                        "This student has no points left.",
                        "No Points", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string dupQuery = @"
            SELECT COUNT(*) 
            FROM BorrowedBooks
            WHERE StudentID = @StudentID
              AND BookID = @BookID
              AND ReturnDate IS NULL
              AND Status IN ('Pending', 'Approved')";

                using (SqlCommand cmd = new SqlCommand(dupQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@BookID", bookId);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("This student already borrowed this book.",
                                        "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string qtyQuery = "SELECT Quantity FROM Books WHERE BookID = @BookID";
                using (SqlCommand cmd = new SqlCommand(qtyQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    object result = cmd.ExecuteScalar();

                    if (result == null || Convert.ToInt32(result) <= 0)
                    {
                        MessageBox.Show("Book unavailable.",
                                        "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                DateTime borrowDate = DateTime.Now.Date;
                DateTime dueDate = borrowDate.AddDays(7);

                string insertQuery = @"
            INSERT INTO BorrowedBooks (StudentID, BookID, Status, BorrowDate, DueDate)
            VALUES (@StudentID, @BookID, 'Approved', @BorrowDate, @DueDate)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE StudentPoints SET Points = Points - 1 WHERE StudentID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", studentID);
                    cmd.ExecuteNonQuery();
                }

                int remainingPoints = points - 1;

                MessageBox.Show(
                    "Book borrowed successfully!\n\n" +
                    "Remaining points for this student: " + remainingPoints,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void SelectCMBBX_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }
        private void BorrowBook_Load(object sender, EventArgs e)
        {
       
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }
    }
}
