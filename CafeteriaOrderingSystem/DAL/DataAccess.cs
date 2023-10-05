using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using CafeteriaOrderingSystem.BLL;
using System.Data;
using Dapper;

namespace CafeteriaOrderingSystem.DAL
{
    class DataAccess
    {
        readonly string ConnectionString;
        public DataAccess()
        {
            
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CafeteriaDB"].ConnectionString;
        }

        #region AddNewUser
        public bool AddNewUser(string username, string password, string usertype)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction sqlTran = connection.BeginTransaction();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@usertype", usertype);

                    command.CommandText =
                       "Insert into users (username, U_password, usertype) values (@username, @password, @usertype)";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
           } 
        }
        #endregion

        #region TestInsertUser
        public bool TestInsertUser(string username, string U_password, string usertype)
        {
            using (IDbConnection conn = new MySqlConnection(Helper.CnnVal("CafeteriaDB")))
            {
                try
                {
                    List<UsersBLL> user = new List<UsersBLL>();
                    user.Add(new UsersBLL { Username = username, Usertype = usertype, U_password = U_password});
                    conn.Execute("cafeteriaposdb_Insert_Users @username, @usertype, @U_password", user);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            
        }

        #endregion

        #region CheckLogin
        public bool CheckLogin(UsersBLL l)
        {
            bool isSucess = false;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction sqlTran = connection.BeginTransaction();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    command.Parameters.AddWithValue("@username", l.Username);
                    command.Parameters.AddWithValue("@U_password", l.U_password);

                    command.CommandText =
                       "SELECT * FROM users WHERE Username = @username AND U_password = @U_password;";
                    command.ExecuteNonQuery();
                    sqlTran.Commit();
                    isSucess = true;
                    
                    
                    
                    
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    isSucess = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return isSucess;
        }
        #endregion

        #region RetreiveExpenseDetails
        public ArrayList RetreiveExpenseDetails(int saleID, int userID)
        {
            ArrayList usersList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID, Expens_name, Expens FROM expenses where SaleID = '" + saleID + "' AND UserID = '"+ userID +"';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string expense_name = reader.GetString(1);
                        decimal ex = reader.GetDecimal(2);

                        usersList.Add(new ExpensesBLL() { ID = ID, Expense_name = expense_name ,Ex = ex});
                    }
                }
                reader.Close();

                return usersList;
            }
        }
        #endregion

        #region RetreiveAllUsersFromDatabase
        public ArrayList RetreiveAllUsersFromDatabase()
        {
            ArrayList usersList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE Usertype = 'كاشير';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string password = reader.GetString(2);
                        string usertype = reader.GetString(3);

                        usersList.Add(new UsersBLL() { ID = ID, Username = username, U_password = password, Usertype = usertype });
                    }
                }
                reader.Close();

                return usersList;
            }
        }
        #endregion

        #region RetreiveUsersDetails
        public UsersBLL RetreiveUsersDetails()
        {
            UsersBLL usersDetails = new UsersBLL();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM users;", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usersDetails.ID = reader.GetInt32(0);
                        usersDetails.Username = reader.GetString(1);
                        usersDetails.U_password = reader.GetString(2);
                        usersDetails.Usertype = reader.GetString(3);
                    }
                }
                reader.Close();

                return usersDetails;
            }
        }
        #endregion

        #region ReturnUsetype
        public string ReturnUsetype(string username ,string pass)
        {
            string usertype = "";
            

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT Usertype FROM users where Username = '" + username + "' AND U_password = '"+ pass +"';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usertype = reader.GetString(0);
                    }
                }
                reader.Close();

                return usertype;
            }
        }
        #endregion

        #region UpdateUser
        public bool UpdateUser(int ID, string username, string password, string usertype)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlTransaction sqlTran = connection.BeginTransaction();

                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@usertype", usertype);

                    command.CommandText =
                       "Update users set username = @username, U_password = @password, usertype = @usertype where ID = @ID";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }
        #endregion

        #region DeleteUser
        public bool DeleteUser(int ID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("Delete from users where ID = '" + ID + "'", connection);
                    connection.Open();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        connection.Close();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region ReturnUsernameID
        public int ReturnUsernameID(string username)
        {
            int id = 0;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID FROM users where Username = '" + username + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                reader.Close();

                return id;
            }
        }
        #endregion

        #region ReturnUsername
        public string ReturnUsername(int ID)
        {
            string ProductDetails = "";

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT Username FROM users where ID = '" + ID + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetails = reader.GetString(0);
                    }
                }
                reader.Close();

                return ProductDetails;
            }
        }
        #endregion

        #region ReturnUserID
        public int ReturnUserID(string username, string pass)
        {
            int userid = 0;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID FROM users where Username = '" + username + "' AND U_password = '"+ pass +"';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userid = reader.GetInt32(0);
                    }
                }
                reader.Close();

                return userid;
            }
        }
        #endregion

        #region RetreiveSaleUsersberDay
        public ArrayList RetreiveSaleUsersberDay(int SaleID)
        {
            ArrayList usersList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT UserID FROM salesitem WHERE SaleID = '"+ SaleID +"';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);

                        usersList.Add(new SaleItemsBLL() { UserID = ID});
                    }
                }
                reader.Close();

                return usersList;
            }
        }
        #endregion

        #region AddNewCategoryToDatabase
        public bool AddNewCategoryToDatabase(string category_name, PictureBox category_pics)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction sqlTran = connection.BeginTransaction();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                MemoryStream ms = new MemoryStream();

                category_pics.Image.Save(ms, category_pics.Image.RawFormat);

                byte[] Category_Picture = ms.GetBuffer();

                ms.Close();

                try
                {
                    command.Parameters.AddWithValue("@category_name", category_name);
                    command.Parameters.AddWithValue("@category_pics", Category_Picture);

                    command.CommandText =
                       "Insert into category (category_name, category_pics) values (@category_name, @category_pics)";
                    command.ExecuteNonQuery();
                    
                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region RetreiveAllCategoriesFromDatabas
        public ArrayList RetreiveAllCategoriesFromDatabase()
        {
            ArrayList CategoriesList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM category;", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int CategoryID = reader.GetInt32(0);
                        string CategoryName = reader.GetString(1);
                        byte[] CategoryPicture = (byte[])reader[2];

                        CategoriesList.Add(new CategoryBLL() { ID = CategoryID, Category_name = CategoryName, Category_pics = CategoryPicture });
                    }
                }
                reader.Close();

                return CategoriesList;
            }
        }
        #endregion

        #region RetreiveCategoryDetails
        public CategoryBLL RetreiveCategoryDetails(int CategoryID)
        {
            CategoryBLL CategoryDetails = new CategoryBLL();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT Category_name, Category_pics FROM category where ID = '" + CategoryID + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryDetails.Category_name = reader.GetString(0);
                        CategoryDetails.Category_pics = (byte[])reader[1];
                    }
                }
                reader.Close();

                return CategoryDetails;
            }
        }
        #endregion

        #region DeleteCategory
        public bool DeleteCategory(int CategoryID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("Delete from category where ID = '" + CategoryID + "'", connection);
                    connection.Open();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        connection.Close();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region DeleteProductAccossiatedWithCategor
        public bool DeleteProductAccossiatedWithCategory(int ProductCategoryID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("Delete from products where ProductCategoryID = '" + ProductCategoryID + "'", connection);
                    connection.Open();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        connection.Close();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UpdateCategory
        public bool UpdateCategory(int CategoryID, string CategoryName, PictureBox CategoryPicture)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                
                MySqlTransaction sqlTran = connection.BeginTransaction();
                
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                MemoryStream ms = new MemoryStream();

                CategoryPicture.Image.Save(ms, CategoryPicture.Image.RawFormat);

                byte[] Category_Picture = ms.GetBuffer();

                ms.Close();

                try
                {
                    command.Parameters.AddWithValue("@ID", CategoryID);
                    command.Parameters.AddWithValue("@category_name", CategoryName);
                    command.Parameters.AddWithValue("@category_pics", Category_Picture);

                    command.CommandText =
                       "Update category set category_name = @category_name, category_pics=@category_pics where ID = @ID";
                    command.ExecuteNonQuery();
                    
                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region ReturnCategoryID
        public int ReturnCategoryID(string category_name)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID FROM category where Category_name = '" + category_name + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                int CategoryID = 0;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryID = reader.GetInt32(0);
                    }
                }
                reader.Close();

                return CategoryID;
            }
        }
        #endregion

        #region ReturnCategoryName
        public string ReturnCategoryName(int CategoryID)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT Category_name FROM category where ID = '" + CategoryID + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                string CategoryName = string.Empty;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryName = reader.GetString(0);
                    }
                }
                reader.Close();

                return CategoryName;
            }
        }
        #endregion

        #region AddNewProductToDatabase
        public bool AddNewProductToDatabase(string ProductName, decimal ProductPrice, int ProductCategoryID,  PictureBox ProductPicture)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                /*Start a local transaction*/
                MySqlTransaction sqlTran = connection.BeginTransaction();

                /*Enlist a command in the current transaction*/
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;
                MemoryStream ms = new MemoryStream();

                ProductPicture.Image.Save(ms, ProductPicture.Image.RawFormat);

                byte[] Category_Picture = ms.GetBuffer();

                ms.Close();

                try
                {
                    // Execute separate commands.
                    command.Parameters.AddWithValue("@ProductName", ProductName);
                    command.Parameters.AddWithValue("@ProductPrice", ProductPrice);
                    command.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductImage", Category_Picture);

                    command.CommandText =
                       "Insert into products (ProductName, ProductPrice, ProductCategoryID, ProductImage) values (@ProductName, @ProductPrice, @ProductCategoryID, @ProductImage)";
                    command.ExecuteNonQuery();

                    // Commit the transaction.
                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region RetreiveAllProducts
        public ArrayList RetreiveAllProducts()
        {
            ArrayList ProductsList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID, ProductName, ProductPrice, ProductCategoryID, ProductImage FROM products;", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string ProductName = reader.GetString(1);
                        decimal ProductPrice = reader.GetDecimal(2);
                        int ProductCategoryID = reader.GetInt32(3);
                        byte[] ProductPicture = (byte[])reader[4];

                        ProductsList.Add(new ProductBLL() { ID = ID , ProductName = ProductName, ProductPrice = ProductPrice, ProductCategoryID = ProductCategoryID, ProductPicture = ProductPicture});
                    }
                }
                reader.Close();

                return ProductsList;
            }
        }
        #endregion

        #region UpdateProduct
        public bool UpdateProduct(int ProductID, string ProductName, decimal ProductPrice, int ProductCategoryID, PictureBox ProductPicture)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlTransaction sqlTran = connection.BeginTransaction();

                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                MemoryStream ms = new MemoryStream();

                ProductPicture.Image.Save(ms, ProductPicture.Image.RawFormat);

                byte[] Product_Pic = ms.GetBuffer();

                try
                {
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    command.Parameters.AddWithValue("@ProductName", ProductName);
                    command.Parameters.AddWithValue("@ProductPrice", ProductPrice);
                    command.Parameters.AddWithValue("@ProductCategoryID", ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductImage", Product_Pic);

                    command.CommandText =
                       "Update products set ProductName = @ProductName, ProductPrice = @ProductPrice, ProductCategoryID = @ProductCategoryID, ProductImage=@ProductImage where ID = @ProductID";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }
        #endregion

        #region DeleteProduct
        public bool DeleteProduct(int ProductID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("Delete from products where ID = '" + ProductID + "'", connection);
                    connection.Open();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        connection.Close();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region RetreiveProductsFromCategory
        public ArrayList RetreiveProductsFromCategory(int CategoryID)
        {
            ArrayList ProductsList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ID, ProductName, ProductPrice, ProductImage FROM products where ProductCategoryID = '" + CategoryID + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ProductID = reader.GetInt32(0);
                        string ProductName = reader.GetString(1);
                        decimal ProductPrice = reader.GetDecimal(2);
                        byte[] ProductPicture = (byte[])reader[3];

                        ProductsList.Add(new ProductBLL() { ID = ProductID, ProductName = ProductName, ProductPrice = ProductPrice, ProductPicture = ProductPicture });
                    }
                }
                reader.Close();

                return ProductsList;
            }
        }
        #endregion

        #region RetreiveProductDetails
        public ProductBLL RetreiveProductDetails(int ProductID)
        {
            ProductBLL ProductDetails = new ProductBLL();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ProductName, ProductPrice, ProductCategoryID, ProductImage FROM products where ID = '" + ProductID + "';", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductDetails.ProductName = reader.GetString(0);
                        ProductDetails.ProductPrice = reader.GetDecimal(1);
                        ProductDetails.ProductCategoryID = reader.GetInt32(2);
                        ProductDetails.ProductPicture = (byte[])reader[3];
                    }
                }
                reader.Close();

                return ProductDetails;
            }
        }
        #endregion

        #region ReturnProductName
        //public string ReturnProductName(int ProductID)
        //{
        //    string ProductDetails = "";

        //    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        //    {
        //        MySqlCommand command = new MySqlCommand("SELECT ProductName FROM Products where ID = '" + ProductID + "';", connection);
        //        connection.Open();

        //        MySqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ProductDetails = reader.GetString(0);
        //            }
        //        }
        //        reader.Close();

        //        return ProductDetails;
        //    }
        //}
        #endregion

        #region ReturnProductPrice
        //public decimal ReturnProductPrice(int ProductID)
        //{
        //    decimal ProductDetails = 0;

        //    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        //    {
        //        MySqlCommand command = new MySqlCommand("SELECT ProductPrice FROM Products where ID = '" + ProductID + "';", connection);
        //        connection.Open();

        //        MySqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ProductDetails = reader.GetDecimal(0);
        //            }
        //        }
        //        reader.Close();

        //        return ProductDetails;
        //    }
        //}
        #endregion

        #region AddANewSale
        public bool AddANewSale()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction sqlTran = connection.BeginTransaction();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;
                

                try
                {
                    command.Parameters.AddWithValue("@SaleTime", DateTime.Now);

                    command.CommandText =
                       "Insert into sales (SaleTime) values (@SaleTime)";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region ReturnSaleID
        public int ReturnSaleID()
        {
            int SaleID = 1;
            try
            {

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT MAX(ID) FROM sales", connection);
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            SaleID = reader.GetInt32(0);
                        }


                    }
                    reader.Close();

                    return SaleID;

                }
            }
            catch (Exception)
            {
                return SaleID;
            }
        }
        #endregion

        #region ReturnDateTime
        public DateTime ReturnDateTime()
        {
            DateTime SaleTime = new DateTime();
            try
            {

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT MAX(SaleTime) FROM sales;", connection);
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            SaleTime = reader.GetDateTime(0);
                        }


                     }
                     reader.Close();

                    return SaleTime;

                }
            }
            catch (Exception)
            {
                return SaleTime;
            }
        }

        #endregion

        #region RecordASale
        public bool RecordASale(ArrayList ProductsList)
        {
            int SaleID = ReturnSaleID();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlTransaction sqlTran = connection.BeginTransaction();

                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {

                    foreach (SaleItemsBLL ProductDetail in ProductsList)
                    {
                        command.CommandText =
                        "Insert into salesitem (ProductQuantity, ProductTotal, SaleID, UserID, ProductID) values ('" + ProductDetail.Quantity + "', '" + ProductDetail.ProductTotal + "', '" + SaleID + "', '" + ProductDetail.UserID + "' , '"+ ProductDetail.ID +"')";
                        command.ExecuteNonQuery();
                    }



                    sqlTran.Commit();

                    connection.Close();
                    return true;
                }


                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;

                }
                finally
                {
                    connection.Close();

                }
            }
        }
        #endregion

        #region RetreiveAllSalesFromDatabase
        public ArrayList RetreiveAllSalesFromDatabase()
        {
            ArrayList SalesList = new ArrayList();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM sales;", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int CategoryID = reader.GetInt32(0);
                        DateTime DateOfSale = reader.GetDateTime(1);

                        SalesList.Add(new SalesBLL() { ID = CategoryID, DateOfSale = DateOfSale });
                    }
                }
                reader.Close();

                return SalesList;
            }
        }
        #endregion

        #region RetreiveSaleItems
        public List<SaleItemsBLL> RetreiveSaleItems(int SaleID, int userID)
        {
            List<SaleItemsBLL> ProductsList = new List<SaleItemsBLL>();
   
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("SELECT ProductName, ProductPrice, SUM(ProductQuantity), SUM(ProductTotal), ProductID FROM salesitem INNER JOIN products ON salesitem.ProductID = products.ID WHERE SaleID = '" + SaleID + "' AND userID = '" + userID + "' GROUP BY ProductID;", connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string ProductName = reader.GetString(0);
                        decimal ProductPrice = reader.GetDecimal(1);
                        int ProductQuantity = reader.GetInt32(2);
                        decimal ProductTotal = reader.GetDecimal(3);
                        int ID = reader.GetInt32(4);
                        ProductsList.Add(new SaleItemsBLL() { ProductName = ProductName, ProductPrice = ProductPrice, Quantity = ProductQuantity, ProductTotal = ProductTotal ,PIDs = ID});
                    }
                }
               
                reader.Close();
                return ProductsList;
            }
        }
        #endregion

        #region AddExpenses
        public bool AddExpenses(string expens_name,decimal expense,int saleID, int userID)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction sqlTran = connection.BeginTransaction();
                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    command.Parameters.AddWithValue("@expens_name", expens_name);
                    command.Parameters.AddWithValue("@expens", expense);
                    command.Parameters.AddWithValue("@saleID", saleID);
                    command.Parameters.AddWithValue("@userID", userID);

                    command.CommandText =
                       "Insert into expenses (expens_name, expens, saleID, userID) values (@expens_name, @expens, @saleID, @userID)";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region DeleteExpense
        public bool DeleteExpense(int ID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    MySqlCommand command = new MySqlCommand("Delete from expenses where ID = '" + ID + "'", connection);
                    connection.Open();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        connection.Close();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UpdateExpense
        public bool UpdateExpense(int ID, string expense_name, decimal expense)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlTransaction sqlTran = connection.BeginTransaction();

                MySqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@expens_name", expense_name);
                    command.Parameters.AddWithValue("@expens", expense);

                    command.CommandText =
                       "Update expenses set expens_name = @expens_name, expens=@expens where ID = @ID";
                    command.ExecuteNonQuery();

                    sqlTran.Commit();

                    connection.Close();

                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}
