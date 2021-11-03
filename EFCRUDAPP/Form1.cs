using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCRUDAPP
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (EFDBEntities db = new EFDBEntities())
            {
                if (model.CustomerID == 0) //insert operation
                {
                    db.Customer.Add(model);
                }
                else // updade opration
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }

            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted successfully");
        }
        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        void PopulateDataGridView()

        {

            dbvCustomer.AutoGenerateColumns = false;
            using (EFDBEntities db = new EFDBEntities())
            {
                dbvCustomer.DataSource = db.Customer.ToList<Customer>();
            }
        }

  
        private void dbvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dbvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dbvCustomer.CurrentRow.Cells["CustomerID"].Value);

                using (EFDBEntities db = new EFDBEntities())
                {
                    model = db.Customer.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Updade";
                btnDelete.Enabled = true;
            }
        }
    }
}
