using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Erika
{
    public partial class Form1 : Form
    {
        public static string connectionString = "Data Source=DESKTOP-2DKQGSL\\SQLEXPRESS;Initial Catalog=erika;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            totalDeductions();
            overalldeductions();
            deductedPersonnel();
            overallpersonnel();
            positions();
            totalperson();
            totalsalary();
            deduction();
            Personnel.Hide();
            Deductions.Hide();
            SetDarkModeForDataGridView(dataGridView1);
            SetDarkModeForDataGridView(dataGridView2);
            SetDarkModeForDataGridView(dataGridView3);
            SetDarkModeForDataGridView(dataGridView4);
            SetDarkModeForDataGridView(dataGridView6);

        }
        private void deduction() {
            string query = @"
                Select * From DeductionRecords";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView6.DataSource = dataTable;
                    foreach (DataGridViewColumn column in dataGridView6.Columns)
                    {
                        // Adjust the width as needed
                        column.Width = 150; // You can adjust this value according to your requirements
                    }

                }
            }
        }
        private void positions() {
            string query = @"
                Select * From Positions";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView4.DataSource = dataTable;
                   
                }
            }
        }
        private void overallpersonnel() {
            string query = @"
                Select * From Personnel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable;
                }
                foreach (DataGridViewColumn column in dataGridView3.Columns)
                {
                    // Adjust the width as needed
                    column.Width = 108; // You can adjust this value according to your requirements
                }
            }
        }
        private void overalldeductions() {
            string query = "SELECT SUM(deduction_amount) AS TotalDeductedAmount\r\nFROM DeductionRecords;\r\n"; // Your SELECT statement

            int totalPersonnels = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        totalPersonnels = Convert.ToInt32(command.ExecuteScalar());
                        label7.Text = totalPersonnels.ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        } 
        private void totalsalary() {
            string query = "SELECT \r\n    SUM(p.salary) - COALESCE((SELECT SUM(deduction_amount) FROM DeductionRecords), 0) AS OverallSalaryAfterDeductions\r\nFROM \r\n    Personnel p;\r\n"; // Your SELECT statement

            int totalPersonnels = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        totalPersonnels = Convert.ToInt32(command.ExecuteScalar());
                        label6.Text = totalPersonnels.ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
        private void totalperson() {
            {
              
                string query = "SELECT COUNT(*) FROM Personnel"; // Your SELECT statement

                int totalPersonnels = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            totalPersonnels = Convert.ToInt32(command.ExecuteScalar());
                            label5.Text = totalPersonnels.ToString();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }

                Console.WriteLine("Total Personnels: " + totalPersonnels);

                // Add further code here...
            }
        }
        private void SetDarkModeForDataGridView(DataGridView dataGridView)
        {
            // Set the background color of the DataGridView
            dataGridView.BackgroundColor = Color.FromArgb(37, 42, 64);

            // Set the foreground color (text color) of the DataGridView
            dataGridView.ForeColor = Color.White;

            // Set the header background color
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black; // Change to black

            // Set the header foreground color (text color)
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Set the selection background color
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 42, 64);

            // Set the selection foreground color
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;

            // Set the border color
            dataGridView.GridColor = Color.FromArgb(37, 42, 64);

            // Set the cell border style
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;

            // Set the side number color to white
            dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.White;

            // Optionally, set specific column styles
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.DefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
                column.DefaultCellStyle.ForeColor = Color.White;
                column.HeaderCell.Style.BackColor = Color.FromArgb(37, 42, 64);
                column.HeaderCell.Style.ForeColor = Color.White;
            }
            dataGridView.RowTemplate.Height = 40;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Center-align text in each column
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }
       
        private void totalDeductions() {
            string query = @"
                SELECT p.personnel_id, p.personnel_name, ps.position_name,
                       ISNULL(SUM(dr.deduction_amount), 0) AS TotalDeductions
                FROM Personnel p
                INNER JOIN Positions ps ON p.position_id = ps.position_id
                LEFT JOIN DeductionRecords dr ON p.personnel_id = dr.personnel_id
                GROUP BY p.personnel_id, p.personnel_name, ps.position_name order by TotalDeductions desc";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        // Adjust the width as needed
                        column.Width = 118; // You can adjust this value according to your requirements
                    }
                   
                }
            }
        }
        private void deductedPersonnel() {
            string query = @"
                --total deductions
SELECT 
    p.personnel_id,
    p.personnel_name,
    p.salary - ISNULL(SUM(dr.deduction_amount), 0) AS Deducted_salary
FROM 
    Personnel p
LEFT JOIN 
    DeductionRecords dr ON p.personnel_id = dr.personnel_id
GROUP BY 
    p.personnel_id, p.personnel_name, p.salary;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                }
            }
        }
        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dashboard.Show();
            Personnel.Hide();
            Deductions.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];

            textBox1.Text = selectedRow.Cells[0].Value?.ToString();
            textBox2.Text = selectedRow.Cells[1].Value?.ToString();
            textBox3.Text = selectedRow.Cells[2].Value?.ToString();
            textBox4.Text = selectedRow.Cells[3].Value?.ToString();
            textBox5.Text = selectedRow.Cells[4].Value?.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int personnel_id = int.Parse(textBox1.Text);
            int position_id = int.Parse(textBox2.Text);
            string personnel_name = textBox3.Text;
            decimal salary = decimal.Parse(textBox4.Text);
            string contact_number = textBox5.Text;
            string query = @"
        INSERT INTO Personnel (personnel_id, position_id, personnel_name, salary, contact_number)
        VALUES (@personnel_id, @position_id, @personnel_name, @salary, @contact_number)";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@personnel_id", personnel_id);
                    command.Parameters.AddWithValue("@position_id", position_id);
                    command.Parameters.AddWithValue("@personnel_name", personnel_name);
                    command.Parameters.AddWithValue("@salary", salary);
                    command.Parameters.AddWithValue("@contact_number", contact_number);

                    try
                    {
                        connection.Open();
                        // Execute the INSERT query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Personnel inserted successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert personnel.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int personnel_id = int.Parse(textBox1.Text);
            int position_id = int.Parse(textBox2.Text);
            string personnel_name = textBox3.Text;
            decimal salary = decimal.Parse(textBox4.Text);
            string contact_number = textBox5.Text;
            string query = @"
        UPDATE Personnel
        SET position_id = @position_id,
            personnel_name = @personnel_name,
            salary = @salary,
            contact_number = @contact_number
        WHERE personnel_id = @personnel_id";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@personnel_id", personnel_id);
                    command.Parameters.AddWithValue("@position_id", position_id);
                    command.Parameters.AddWithValue("@personnel_name", personnel_name);
                    command.Parameters.AddWithValue("@salary", salary);
                    command.Parameters.AddWithValue("@contact_number", contact_number);

                    try
                    {
                        connection.Open();
                        // Execute the UPDATE query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Personnel updated successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update personnel.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int personnel_id = int.Parse(textBox1.Text);
            string query = "DELETE FROM Personnel WHERE personnel_id = @personnel_id";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameter
                    command.Parameters.AddWithValue("@personnel_id", personnel_id);

                    try
                    {
                        connection.Open();
                        // Execute the DELETE query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Personnel deleted successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);

                        }
                        else
                        {
                            MessageBox.Show("No personnel found with the provided ID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox10.Text);
            int personnel_id = int.Parse(textBox9.Text);
            DateTime dateTime = DateTime.Now;
            decimal deduction_amount = decimal.Parse(textBox7.Text);
            string deduction_reason = textBox6.Text;

            // SQL INSERT statement with parameters
            string query = @"
        INSERT INTO DeductionRecords (deduction_id, personnel_id, deduction_date, deduction_amount, deduction_reason)
        VALUES (@deduction_id, @personnel_id, @deduction_date, @deduction_amount, @deduction_reason)";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@deduction_id", id);
                    command.Parameters.AddWithValue("@personnel_id", personnel_id);
                    command.Parameters.AddWithValue("@deduction_date", dateTime);
                    command.Parameters.AddWithValue("@deduction_amount", deduction_amount);
                    command.Parameters.AddWithValue("@deduction_reason", deduction_reason);

                    try
                    {
                        connection.Open();
                        // Execute the INSERT query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Deduction record inserted successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert deduction record.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView4.Rows.Count)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView6.Rows[e.RowIndex];

                // Assuming TextBox1 to TextBox5 are your TextBox controls
                textBox10.Text = selectedRow.Cells[0].Value?.ToString();
                textBox9.Text = selectedRow.Cells[1].Value?.ToString();
                textBox7.Text = selectedRow.Cells[3].Value?.ToString();
                textBox6.Text = selectedRow.Cells[4].Value?.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox10.Text);
            int personnel_id = int.Parse(textBox9.Text);
            DateTime dateTime = DateTime.Now;
            decimal deduction_amount = decimal.Parse(textBox7.Text);
            string deduction_reason = textBox6.Text;
            string query = @"
        UPDATE DeductionRecords
        SET personnel_id = @personnel_id,
            deduction_date = @deduction_date,
            deduction_amount = @deduction_amount,
            deduction_reason = @deduction_reason
        WHERE deduction_id = @deduction_id";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@deduction_id", id);
                    command.Parameters.AddWithValue("@personnel_id", personnel_id);
                    command.Parameters.AddWithValue("@deduction_date", dateTime);
                    command.Parameters.AddWithValue("@deduction_amount", deduction_amount);
                    command.Parameters.AddWithValue("@deduction_reason", deduction_reason);

                    try
                    {
                        connection.Open();
                        // Execute the UPDATE query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Deduction record updated successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update deduction record.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox10.Text);
            string query = @"
        DELETE FROM DeductionRecords
        WHERE deduction_id = @deduction_id";

            // Create and open connection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameter
                    command.Parameters.AddWithValue("@deduction_id", id);

                    try
                    {
                        connection.Open();
                        // Execute the DELETE query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Deduction record deleted successfully!");
                            totalDeductions();
                            overalldeductions();
                            deductedPersonnel();
                            overallpersonnel();
                            positions();
                            totalperson();
                            totalsalary();
                            deduction();
                            SetDarkModeForDataGridView(dataGridView1);
                            SetDarkModeForDataGridView(dataGridView2);
                            SetDarkModeForDataGridView(dataGridView3);
                            SetDarkModeForDataGridView(dataGridView4);
                            SetDarkModeForDataGridView(dataGridView6);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete deduction record.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox9.Clear();
            textBox10.Clear();
            textBox7.Clear();
            textBox6.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Deductions.Hide();
            Personnel.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {   
            Deductions.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
