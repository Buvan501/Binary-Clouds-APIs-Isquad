[WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json, XmlSerializeString = false)]
    public void addjobcard(
     string customerName,
     string phoneNumber,
     string altphoneNumber,
     string customerAddress,
     string productReceivedDate,
     string mobileModel,
     string passwordMobile,
     string brandName,
     string color,
     string serialNumber,
     string complaint,
     string powerOn,
     string display,
     string touch,
     string camera,
     string deviceCondition,
     string accessories,
     string expectedDelivery,
     string expectedAmount,
     string advanceAmount,
     string technician,
     string jobStatus,
     string Createdby
 )
    {
        JavaScriptSerializer ser = new JavaScriptSerializer();
        this.Context.Response.ContentType = "application/json; charset=utf-8";
        try
        {
            decimal estimatedAmount = 0, advAmount = 0;
            if (!decimal.TryParse(expectedAmount, out estimatedAmount)) estimatedAmount = 0;
            if (!decimal.TryParse(advanceAmount, out advAmount)) advAmount = 0;
            if (string.IsNullOrWhiteSpace(jobStatus)) jobStatus = "pending";

            //// Format dates as dd/MM/yyyy for SQL style 103
            //DateTime dtProductReceived, dtExpectedDelivery;
            //if (!DateTime.TryParse(productReceivedDate, out dtProductReceived))
            //    dtProductReceived = DateTime.UtcNow;
            //if (!DateTime.TryParse(expectedDelivery, out dtExpectedDelivery))
            //    dtExpectedDelivery = DateTime.UtcNow;

            //string formattedProductReceivedDate = dtProductReceived.ToString("dd/MM/yyyy");
            //string formattedExpectedDelivery = dtExpectedDelivery.ToString("dd/MM/yyyy");

            Database oDb = JSSM.Entity.DBConnection.dbCon;

            // --- 1. Insert/Get Customer ID ---
            int customerID = 0;
            string customerCode = "";
            DbCommand cmdCustomer = oDb.GetStoredProcCommand("usp_Insert_Customer");
            oDb.AddOutParameter(cmdCustomer, "@PK_CustomerID", DbType.Int32, 4);
            oDb.AddInParameter(cmdCustomer, "@CustomerName", DbType.String, customerName);
            oDb.AddOutParameter(cmdCustomer, "@CustomerCode", DbType.String, 10); // OUTPUT
            oDb.AddInParameter(cmdCustomer, "@CustomerAddress", DbType.String, customerAddress);
            oDb.AddInParameter(cmdCustomer, "@CustomerMobileNo", DbType.String, phoneNumber);
            oDb.AddInParameter(cmdCustomer, "@FK_DistrictID", DbType.Int32, 1); // Default
            oDb.AddInParameter(cmdCustomer, "@City", DbType.String, "");
            oDb.AddInParameter(cmdCustomer, "@IsActive", DbType.Boolean, true);
            oDb.AddInParameter(cmdCustomer, "@IsDealer", DbType.Boolean, false);
            oDb.AddInParameter(cmdCustomer, "@CustomerTag", DbType.Boolean, false);
            oDb.AddInParameter(cmdCustomer, "@FK_CreatedBy", DbType.Int32, 4); // Default User
            oDb.AddInParameter(cmdCustomer, "@CustomerEmail", DbType.String, "");

            oDb.ExecuteNonQuery(cmdCustomer);
            customerID = Convert.ToInt32(oDb.GetParameterValue(cmdCustomer, "@PK_CustomerID"));
            object codeObj = oDb.GetParameterValue(cmdCustomer, "@CustomerCode");
            customerCode = codeObj != null ? codeObj.ToString() : null;

            // --- 2. Insert JobCard ---
            DbCommand cmd = oDb.GetStoredProcCommand("usp_Insert_JobCard");
            oDb.AddOutParameter(cmd, "@PK_JobCardID", DbType.Int32, 4);
            oDb.AddOutParameter(cmd, "@JobCardNo", DbType.String, 15);

            oDb.AddInParameter(cmd, "@JobCardDate", DbType.String, productReceivedDate);
            oDb.AddInParameter(cmd, "@ReferenceNo", DbType.String, ""); // No reference number
            oDb.AddInParameter(cmd, "@FK_CustomerID", DbType.Int32, customerID);
            oDb.AddInParameter(cmd, "@FK_ModelID", DbType.Int32, mobileModel); // 10772
            oDb.AddInParameter(cmd, "@ComplaintName", DbType.String, complaint);
            oDb.AddInParameter(cmd, "@EstimatedAmount", DbType.Decimal, estimatedAmount);
            oDb.AddInParameter(cmd, "@AdvanceAmount", DbType.Decimal, advAmount);
            oDb.AddInParameter(cmd, "@ExpectedDeliveryDate", DbType.String, expectedDelivery);
            oDb.AddInParameter(cmd, "@FK_StatusID", DbType.Int32, 1); // 1
            oDb.AddInParameter(cmd, "@Remarks", DbType.String, ""); // No remarks
            oDb.AddInParameter(cmd, "@FK_CreatedBy", DbType.Int32,Createdby); // Default user
            oDb.AddInParameter(cmd, "@IMEINo", DbType.String, serialNumber);
            oDb.AddInParameter(cmd, "@FK_ColorID", DbType.Int32, 1); // Default color
            oDb.AddInParameter(cmd, "@PowerOnID", DbType.Int32, 1); // Default PowerOn
            oDb.AddInParameter(cmd, "@DisplayID", DbType.Int32, 1); // Default Display
            oDb.AddInParameter(cmd, "@CameraID", DbType.Int32, 1); // Default Camera
            oDb.AddInParameter(cmd, "@DeviceConditionName", DbType.String, deviceCondition);
            oDb.AddInParameter(cmd, "@AccessoriesName", DbType.String, accessories);
            oDb.AddInParameter(cmd, "@ComplaintsRemarks", DbType.String, ""); // No extra remarks
            oDb.AddInParameter(cmd, "@TouchID", DbType.Int32, touch); // Default Touch
            oDb.AddInParameter(cmd, "@DataBackupID", DbType.Int32, 1); // Default DataBackup
            oDb.AddInParameter(cmd, "@IsConfirmEstimation", DbType.Boolean, false); // Default
            oDb.AddInParameter(cmd, "@IMEINoID", DbType.Int32, 1); // Default IMEINoID
            oDb.AddInParameter(cmd, "@DataBackupOptionID", DbType.Int32, 1); // Default DataBackupOption
            oDb.AddInParameter(cmd, "@DevicePattern", DbType.String, ""); // Default
            oDb.AddInParameter(cmd, "@DeviceID", DbType.String, ""); // Default
            oDb.AddInParameter(cmd, "@DevicePassword", DbType.String, passwordMobile);
            oDb.AddInParameter(cmd, "@FK_BranchID", DbType.Int32, 1); // Default Branch
            oDb.AddInParameter(cmd, "@FK_CompanyID", DbType.Int32, 1); // Default Company
            oDb.AddInParameter(cmd, "@LabourCharge", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@FK_TaxID", DbType.Int32, 0);
            oDb.AddInParameter(cmd, "@TaxPercentage", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@TotalTaxAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@CGSTPercent", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@SGSTPercent", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@IGSTPercent", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@CGSTAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@SGSTAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@IGSTAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@IsPartsUsed", DbType.Boolean, false);
            oDb.AddInParameter(cmd, "@TotalLabourCost", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@TotalPartsAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@OtherCharges", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@DiscountPercentage", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@InvoiceAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@InvoiceNo", DbType.String, null);
            oDb.AddInParameter(cmd, "@InvoiceDate", DbType.Date, DBNull.Value);
            oDb.AddInParameter(cmd, "@IsOG", DbType.Boolean, false);
            oDb.AddInParameter(cmd, "@Product", DbType.String, mobileModel);
            oDb.AddInParameter(cmd, "@CashAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@FK_CardModeBankID", DbType.Int32, 0);
            oDb.AddInParameter(cmd, "@CardAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@FK_UPIModeBankID", DbType.Int32, 0);
            oDb.AddInParameter(cmd, "@UPIAmount", DbType.Decimal, 0);
            oDb.AddInParameter(cmd, "@IsDealerFlag", DbType.Boolean, false);

            int jobCardID = oDb.ExecuteNonQuery(cmd) != 0 ? Convert.ToInt32(oDb.GetParameterValue(cmd, "@PK_JobCardID")) : 0;

            // --- 3. Return JSON response ---
            updatestatus invDetails = new updatestatus();
            invDetails.status = jobCardID != 0 ? "true" : "false";
            this.Context.Response.Write(ser.Serialize(invDetails));
        }
        catch (Exception ex)
        {
            var err = new { status = "false", error = ex.Message };
            this.Context.Response.Write(ser.Serialize(err));
        }
    }

    // Response class
    public class updatestatus
    {
        public string status;
    }
}