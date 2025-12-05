using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;

namespace WPFZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFZooManager.Properties.Settings.AnimalZooDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAllAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void listZoos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
        }

        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = @"SELECT a.Id, a.Name
                         FROM Animal a
                         INNER JOIN ZooAnimal za ON a.Id = za.AnimalId
                         WHERE za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable animalTable = new DataTable();
                sqlDataAdapter.Fill(animalTable);

                listAssociatedAnimals.DisplayMemberPath = "Name";
                listAssociatedAnimals.SelectedValuePath = "Id";
                listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void ShowAllAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    listAllAnimals.DisplayMemberPath = "Name";
                    listAllAnimals.SelectedValuePath = "Id";
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString()) ;
            }
        }
        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }
        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }
        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }
        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo set Location = @Location where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }
        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal set Name = @Name where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }
        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zooId = listZoos.SelectedValue;
                var animalId = listAssociatedAnimals.SelectedValue;
                MessageBox.Show(zooId.ToString());
                MessageBox.Show(animalId.ToString());

                if (zooId == null || animalId == null)
                {
                    MessageBox.Show("Сначала выберите зоопарк слева и животное в Associated Animals.");
                    return;
                }

                string query = "DELETE FROM ZooAnimal WHERE ZooId = @ZooId AND AnimalId = @AnimalId";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", zooId);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", animalId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();

                ShowAssociatedAnimals();
            }
        }
    }
}
