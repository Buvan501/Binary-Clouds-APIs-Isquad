 #region CustomerDetailsByMobileNO
 [WebMethod]
 [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json, XmlSerializeString = false)]
 public void CheckCustomerByMobile(string mobileNumber)
 {
     JavaScriptSerializer ser = new JavaScriptSerializer();
     try
     {
         string mobileStr = Convert.ToString(mobileNumber);

         if (string.IsNullOrEmpty(mobileStr))
         {
             HttpContext.Current.Response.ContentType = "application/json";
             HttpContext.Current.Response.Write(ser.Serialize(new
             {
                 Status = false,
                 Message = "Mobile number is required",
                 Details = new List<object>()
             }));
             return;
         }

         Database db = JSSM.Entity.DBConnection.dbCon;
         DataSet dsCustomer = null;

         // Call existing stored procedure with PK_CustomerID = 0
         DbCommand cmd = db.GetStoredProcCommand("usp_Select_Customer");
         db.AddInParameter(cmd, "@PK_CustomerID", DbType.Int32, 0);
         dsCustomer = db.ExecuteDataSet(cmd);

         object responseObj;

         if (dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
         {
             var customerList = new List<object>();

             foreach (DataRow dr in dsCustomer.Tables[0].Rows)
             {
                 string customerMobile = Convert.ToString(dr["CustomerMobileNo"]);

                 // Exact match check
                 if (customerMobile == mobileStr)
                 {
                     string customerName = Convert.ToString(dr["CustomerName"]);
                     string customerAddress = Convert.ToString(dr["CustomerAddress"]);

                     customerList.Add(new
                     {
                         CustomerName = customerName,
                         CustomerAddress = customerAddress,
                         CustomerMobileNo = customerMobile
                     });
                 }
             }

             if (customerList.Count > 0)
             {
                 responseObj = new
                 {
                     Status = true,
                     Message = "Customer found",
                     Details = customerList
                 };
             }
             else
             {
                 responseObj = new
                 {
                     Status = false,
                     Message = "Customer does not exist",
                     Details = new List<object>()
                 };
             }
         }
         else
         {
             responseObj = new
             {
                 Status = false,
                 Message = "No customers found",
                 Details = new List<object>()
             };
         }

         HttpContext.Current.Response.ContentType = "application/json";
         HttpContext.Current.Response.Write(ser.Serialize(responseObj));
     }
     catch (Exception ex)
     {
         HttpContext.Current.Response.ContentType = "application/json";
         HttpContext.Current.Response.Write(ser.Serialize(new
         {
             Status = false,
             Message = ex.Message,
             Details = new List<object>()
         }));
     }
 }
 #endregion