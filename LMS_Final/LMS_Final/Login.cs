using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LMS_Final
{
    public partial class Login : Form
    {
        string connectionString = @"Data Source=DESKTOP-A8QTH9B\SQLEXPRESS;Initial Catalog=LibraryManagementSystem;Integrated Security=True;";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            PasswordTXTBX.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            PasswordTXTBX.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTXTBX.Text.Trim();
            string password = PasswordTXTBX.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter Username and Password.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Admins WHERE Username = @u AND Password = @p";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}