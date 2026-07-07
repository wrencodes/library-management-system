using System;
using System.Windows.Forms;

namespace LMS_Final
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void AddBookBTN_Click(object sender, EventArgs e)
        {
            AddBook addbooks = new AddBook();
            addbooks.Show();
            this.Hide();
        }

        private void AddStudentBTN_Click(object sender, EventArgs e)
        {
            AddStudent addstudent = new AddStudent();
            addstudent.Show();
            this.Hide();
        }
        private void LogoutBTN_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
            MessageBox.Show("Logout successfully!");
        }

        private void IssueBookBTN_Click(object sender, EventArgs e)
        {
            IssueBook issuebook = new IssueBook();
            issuebook.Show();
            this.Hide();
        }

        private void ReturnBookBTN_Click(object sender, EventArgs e)
        {
            ReturnBook returnbook = new ReturnBook();
            returnbook.Show();
            this.Hide();
        }

        private void BorrowedBTN_Click(object sender, EventArgs e)
        {
            BorrowedBook borrowedbook = new BorrowedBook();
            borrowedbook.Show();
            this.Hide(); 
        }
    }
}
