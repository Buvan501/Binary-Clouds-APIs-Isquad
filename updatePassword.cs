#region UpdatePass
    [WebMethod]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json, XmlSerializeString = false)]
    public void UpdatePassword(int userid, string o_password, string n_password)
    {
        JavaScriptSerializer ser = new JavaScriptSerializer();
        try
        {
            Database oDb;
            //JobCard objJobCard = new JobCard();
            oDb = JSSM.Entity.DBConnection.dbCon;
            JSSM.Entity.User ojtrans = new JSSM.Entity.User();

            DbCommand cmd = oDb.GetStoredProcCommand(constants.StoredProcedures.USP_CHANGEPASSWORD);
            oDb.AddInParameter(cmd, "@PK_UserID", DbType.Int32, userid);
            oDb.AddInParameter(cmd, "@OldPassword", DbType.String, CommonMethods.Security.Encrypt(o_password, true));
            oDb.AddInParameter(cmd, "@NewPassword", DbType.String, CommonMethods.Security.Encrypt(n_password, true));

            int iID = oDb.ExecuteNonQuery(cmd);

            updatestatus invDetails = new updatestatus();

            if (iID != 0)
                invDetails.status = "true";
            else
                invDetails.status = "false";

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(ser.Serialize(invDetails));

        }
        catch (Exception ex)
        {
            updatestatus invDetails = new updatestatus();
            invDetails.status = "false";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            this.Context.Response.ContentType = "application/json; charset=utf-8";
            this.Context.Response.Write(ser.Serialize(invDetails));
        }
    }
#endregion