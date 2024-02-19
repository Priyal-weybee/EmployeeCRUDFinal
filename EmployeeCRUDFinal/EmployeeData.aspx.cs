using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeCRUDFinal
{
    public partial class EmployeeData : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCountryDropDown(); // Call method to bind countries to dropdownlist

                display(); // Call method to bind gridview
            }


        }

        //protected void Insert_Click(object sender, EventArgs e)
        //{
        //    SqlConnection con = new SqlConnection(cs);
        //    SqlCommand cmd = new SqlCommand("insert into Employee values('" + Name.Text + "','" + Gender.SelectedItem.Text + "','" + CountryDropDownList1.SelectedItem.Text + "','" + StateDropDownList1.SelectedItem.Text + "')", con);

        //    string CountryName = CountryDropDownList1.SelectedItem.Text;
        //    string StateName = StateDropDownList1.SelectedItem.Text;
        //    int countryId = GetCountryIdByName(CountryName);
        //    int stateId = GetStateIdByName(StateName);


        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //    Name.Text = "";
        //    CountryDropDownList1.SelectedItem.Text = "";

        //    StateDropDownList1.SelectedItem.Text = "";
        //    display();
        //}
        protected void Insert_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string name = Name.Text;
            string gender = Gender.SelectedItem.Text;
            string countryName = CountryDropDownList1.SelectedItem.Text;
            string stateName = StateDropDownList1.SelectedItem.Text;

            int countryId = GetCountryIdByName(countryName);
            int stateId = GetStateIdByName(stateName);

            SqlCommand cmd = new SqlCommand("INSERT INTO Employee (Name, Gender, CountryId, StateId) VALUES (@Name, @Gender, @CountryId, @StateId)", con);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@CountryId", countryId);
            cmd.Parameters.AddWithValue("@StateId", stateId);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Name.Text = "";
            CountryDropDownList1.ClearSelection();
            StateDropDownList1.ClearSelection();

            display();
        }
        public void display()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("select EmployeeId,Name,Gender,e.CountryId,CountryName,e.StateId,StateName from Employee e join Country c on e.CountryId=c.CountryId join State s on e.StateId=s.StateId;", con);

                SqlConnection conn = new SqlConnection(cs);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

        }
        private int GetCountryIdByName(string countryName)
        {
            int countryId = 0;
            // Assuming you have a connection string in your web.config


            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT CountryId FROM Country WHERE CountryName = @CountryName", con);
                cmd.Parameters.AddWithValue("@CountryName", countryName);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    countryId = Convert.ToInt32(result);
                }
            }
            return countryId;
        }

        // Method to get StateId from States table based on state name
        private int GetStateIdByName(string stateName)
        {
            int stateId = 0;
            // Assuming you have a connection string in your web.config
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT StateId FROM State WHERE StateName = @StateName", con);
                cmd.Parameters.AddWithValue("@StateName", stateName);
                con.Open();
                object v = cmd.ExecuteScalar();
                object result = v;
                if (result != null)
                {
                    stateId = Convert.ToInt32(result);
                }
            }
            return stateId;
        }


        private void PopulateCountryDropDown()
        {
            // Define SQL query to fetch country names from your database
            string query = "SELECT CountryName FROM Country";

            // Establish connection and command objects
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                // Open connection, fill data into DataTable, and bind to the DropDownList
                con.Open();
                da.Fill(dt);
                con.Close();

                // Bind the DataTable to the Country DropDownList
                CountryDropDownList1.DataSource = dt;
                CountryDropDownList1.DataTextField = "CountryName";
                CountryDropDownList1.DataBind();

                // Add a default item to the Country DropDownList
                CountryDropDownList1.Items.Insert(0, new ListItem("-- Select Country --", ""));
                PopulateStateDropDown();
            }
        }
        private void PopulateStateDropDown()
        {
            // Clear existing items from State DropDownList
            StateDropDownList1.Items.Clear();

            // Get the selected country
            string selectedCountry = CountryDropDownList1.SelectedValue;

            if (!string.IsNullOrEmpty(selectedCountry))
            {
                // Define SQL query to fetch states based on the selected country
                string query = "SELECT StateName FROM State WHERE CountryId = (SELECT CountryId FROM Country WHERE CountryName = @CountryName)";

                // Establish connection and command objects
                using (SqlConnection con = new SqlConnection(cs.ToString()))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CountryName", selectedCountry);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    // Open connection, fill data into DataTable, and bind to the DropDownList
                    con.Open();
                    da.Fill(dt);
                    con.Close();

                    // Bind the DataTable to the State DropDownList
                    StateDropDownList1.DataSource = dt;
                    StateDropDownList1.DataTextField = "StateName";
                    StateDropDownList1.DataBind();

                    // Add a default item to the State DropDownList
                    StateDropDownList1.Items.Insert(0, new ListItem("-- Select State --", ""));
                }
            }
        }
        protected void CountryDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateStateDropDown(); // Call method to populate state dropdownlist based on selected country
        }
        //protected void EditButton_Click(object sender, GridViewEditEventArgs e)
        //{
        //   Insert.Enabled = false;
        //    using (SqlConnection con = new SqlConnection(cs.ToString()))
        //    {

        //        int EmployeeId = Convert.ToInt32(GridView1.DataKeys[e.NewEditIndex].Values[0]);
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("Select Name,Gender,CountryId,StateId From Employee WHERE EmployeeId=" + EmployeeId, con);
        //        cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                Name.Text = reader["Name"].ToString();
        //                Gender.SelectedValue = reader["Gender"].ToString();
        //                CountryDropDownList1.SelectedValue = reader["CountryId"].ToString();
        //                PopulateStateDropDown();
        //                StateDropDownList1.SelectedValue = reader["StateId"].ToString();

        //            }
        //            else
        //            {

        //            }
        //        }



        //    }




        //}
        protected void EditButton_Click(object sender, GridViewEditEventArgs e)
        {
            
            Insert.Enabled = false;
            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                int employeeId = Convert.ToInt32(GridView1.DataKeys[e.NewEditIndex].Value);

                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT Employee.Name, Employee.Gender, Country.CountryName, State.StateName
                                           FROM Employee
                                           INNER JOIN Country ON Employee.CountryId = Country.CountryId
                                           INNER JOIN State ON Employee.StateId = State.StateId
                                           WHERE Employee.EmployeeId = @EmployeeId", con);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Name.Text = reader["Name"].ToString();
                        Gender.SelectedValue = reader["Gender"].ToString();
                        // Set the country dropdown list value
                        string countryName = reader["CountryName"].ToString();
                        ListItem countryItem = CountryDropDownList1.Items.FindByText(countryName);
                        if (countryItem != null)
                        {
                            CountryDropDownList1.SelectedValue = countryItem.Value;
                            // Populate state dropdown list based on the selected country
                            PopulateStateDropDown();
                        }

                        // Set the state dropdown list value
                        string stateName = reader["StateName"].ToString();
                        ListItem stateItem = StateDropDownList1.Items.FindByText(stateName);
                        if (stateItem != null)
                        {
                            StateDropDownList1.SelectedValue = stateItem.Value;
                        }
                    }
                    else
                    {
                        // Handle the case where no data is found
                    }
                }
            }
        }

        protected void DeleteRowButton_Click(Object sender, GridViewDeleteEventArgs e)
        {

            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                //var EmployeeId1 = GridView1.Rows[e.RowIndex].Cells[1];
                //int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
                //int EmployeeId = int.Parse(GridView1.Rows[e.RowIndex].FindControl("EmployeeId").ToString());
                int EmployeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE EmployeeId=" + EmployeeId, con);
                cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                display();

            }

        }
      
        protected void RowCancelButton_Click(Object sender, GridViewCancelEditEventArgs e)
        {
            CountryDropDownList1.ClearSelection();
            StateDropDownList1.ClearSelection();
            Insert.Enabled = true;
            GridView1.EditIndex = -1;
            display();
        }
        //protected void RowUpdateButton_CLick(Object sender,GridViewUpdateEventArgs e)
        //{
        //    Insert.Enabled= false;
        //   // Insert_Click.Enabled = false;
        //    //Label  id = GridView1.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        //    SqlConnection con = new SqlConnection(cs);
        //    int EmployeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        //    SqlCommand cmd = new SqlCommand("Update Employee set Name='" + Name.Text + "',Gender='" + Gender.SelectedItem.Text + "',CountryId='" + CountryDropDownList1.SelectedItem.Text + "',StateId='" + StateDropDownList1.SelectedItem.Text + "'  WHERE EmployeeId=" + EmployeeId, con);
        //    //bool isActive = isActive.Checked ? true : false;
        //    string CountryName = CountryDropDownList1.SelectedItem.Text;
        //    string StateName = StateDropDownList1.SelectedItem.Text;
        //    int countryId = GetCountryIdByName(CountryName);r


        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //    Name.Text = "";

        //    display();


        //}
        protected void RowUpdateButton_CLick(object sender, GridViewUpdateEventArgs e)
        {
          
            
            Insert.Enabled = false;

            int employeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

            string countryName = CountryDropDownList1.SelectedItem.Text;
            string stateName = StateDropDownList1.SelectedItem.Text;

            // Fetch country and state IDs based on their names
            int countryId = GetCountryIdByName(countryName);
            int stateId = GetStateIdByName(stateName);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Employee SET Name = @Name, Gender = @Gender, CountryId = @CountryId, StateId = @StateId WHERE EmployeeId = @EmployeeId", con);
                cmd.Parameters.AddWithValue("@Name", Name.Text);
                cmd.Parameters.AddWithValue("@Gender", Gender.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.Parameters.AddWithValue("@StateId", stateId);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Name.Text = "";
            display();
        }
        }
}
