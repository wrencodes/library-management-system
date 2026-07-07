using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LMS_Final
{
    public partial class AddStudent : Form
    {
        string connectionString =
            @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        public AddStudent()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT StudentID, FirstName, LastName, Program, YearLevel FROM Users";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            StudentIDTXTBX.Text = "";
            FirstNameTXTBX.Text = "";
            LastNameTXTBX.Text = "";
            ProgramTXTBX.Text = "";
            YearLevelTXTBX.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            StudentIDTXTBX.Text = row.Cells["StudentID"].Value.ToString();
            FirstNameTXTBX.Text = row.Cells["FirstName"].Value.ToString();
            LastNameTXTBX.Text = row.Cells["LastName"].Value.ToString();
            ProgramTXTBX.Text = row.Cells["Program"].Value.ToString();
            YearLevelTXTBX.Text = row.Cells["YearLevel"].Value.ToString();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dash = new Dashboard();
            dash.Show();
            this.Hide();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }
        // ADD STUDENT BUTTON :3
        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIDTXTBX.Text) ||
          string.IsNullOrWhiteSpace(FirstNameTXTBX.Text) ||
          string.IsNullOrWhiteSpace(LastNameTXTBX.Text) ||
          string.IsNullOrWhiteSpace(ProgramTXTBX.Text) ||
          string.IsNullOrWhiteSpace(YearLevelTXTBX.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE StudentID=@StudentID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@StudentID", StudentIDTXTBX.Text);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Student ID already exists!");
                    return;
                }

                string query = @"INSERT INTO Users (StudentID, FirstName, LastName, Program, YearLevel)
                                 VALUES (@StudentID, @FirstName, @LastName, @Program, @YearLevel)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", StudentIDTXTBX.Text);
                cmd.Parameters.AddWithValue("@FirstName", FirstNameTXTBX.Text);
                cmd.Parameters.AddWithValue("@LastName", LastNameTXTBX.Text);
                cmd.Parameters.AddWithValue("@Program", ProgramTXTBX.Text);
                cmd.Parameters.AddWithValue("@YearLevel", YearLevelTXTBX.Text);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Student added successfully!");
            LoadStudents();
            ClearFields();
        }
        // UPDATE STUDENT BUTTON
        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIDTXTBX.Text))
            {
                MessageBox.Show("Please select a student to update.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"UPDATE Users SET 
                                FirstName=@FirstName,
                                LastName=@LastName,
                                Program=@Program,
                                YearLevel=@YearLevel
                                WHERE StudentID=@StudentID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", StudentIDTXTBX.Text);
                cmd.Parameters.AddWithValue("@FirstName", FirstNameTXTBX.Text);
                cmd.Parameters.AddWithValue("@LastName", LastNameTXTBX.Text);
                cmd.Parameters.AddWithValue("@Program", ProgramTXTBX.Text);
                cmd.Parameters.AddWithValue("@YearLevel", YearLevelTXTBX.Text);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Student updated successfully!");
            LoadStudents();
            ClearFields();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIDTXTBX.Text))
            {
                MessageBox.Show("Please select a student to remove.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "DELETE FROM Users WHERE StudentID=@StudentID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", StudentIDTXTBX.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Student removed successfully!");
            LoadStudents();
            ClearFields();
        }

        private void SearchTXTBX_TextChanged(object sender, EventArgs e)
        {
            string id = SearchTXTBX.Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = string.IsNullOrEmpty(id)
                    ? "SELECT StudentID, FirstName, LastName, Program, YearLevel FROM Users"
                    : "SELECT StudentID, FirstName, LastName, Program, YearLevel FROM Users WHERE StudentID LIKE @id";

                SqlCommand cmd = new SqlCommand(query, con);

                if (!string.IsNullOrEmpty(id))
                    cmd.Parameters.AddWithValue("@id", "%" + id + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }
    }
}
