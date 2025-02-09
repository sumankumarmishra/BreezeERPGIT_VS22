﻿//====================================================Revision History=========================================================================
// 1.0  Priti   V2.0.36  18-01-2023  0025318: Views to be converted to Procedures in the Listing Page of Transaction / Return-Sales / Rate Difference Entry Customer
//====================================================End Revision History=====================================================================


using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.IO;
using ERP.Models;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace ERP.OMS.Management.Activities
{
    public partial class RateDifferenceEntryCustomerList : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SalesReturnBL objSalesReturnBL = new SalesReturnBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            MasterSettings objmaster = new MasterSettings();
            hdnActiveEInvoice.Value = objmaster.GetSettings("ActiveEInvoice");
            if (hdnActiveEInvoice.Value == "0")
            {
                GrdSalesReturn.Columns[10].Visible = false;
                GrdSalesReturn.Columns[11].Visible = false;
                GrdSalesReturn.Columns[12].Visible = false;
                GrdSalesReturn.Columns[13].Visible = false;
            }
            else if (hdnActiveEInvoice.Value == "1")
            {
                GrdSalesReturn.Columns[10].Visible = true;
                GrdSalesReturn.Columns[11].Visible = true;
                GrdSalesReturn.Columns[12].Visible = true;
                GrdSalesReturn.Columns[13].Visible = true;
            }
            String finyear = "";
            finyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/RateDifferenceEntryCustomerList.aspx");

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");


            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {

                    GrdSalesReturn.Columns[6].Width = 160;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {

                    GrdSalesReturn.Columns[6].Width = 0;
                }
            }


            if (!IsPostBack)
            {
               


                //Session.Remove("UndeliveryReturnSalesListingDetails");
                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    // string ubranch = Convert.ToString(HttpContext.Current.Session["UserSiblingBranchHierarchy"]);
                    PopulateBranchByHierchy(userbranch);

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    //   GetFinacialYearBasedQouteDate();
                    string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
                    string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);
                    //   GetSalesReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);

                    GrdSalesReturn.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesReturnDifferenceListDetailsGrid";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesReturnDifferenceListDetailsGrid');</script>");

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                    {
                        string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                        ddlBranch.SelectedValue = strdefaultBranch;
                        //BindCashBankAccount(strdefaultBranch);
                    }
                    Session["RDEC_schemavalue"] = null;

                }
            }
            BindBranch();
        }
        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }


        public void BindBranch()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
        }
        //public void GetFinacialYearBasedQouteDate()
        //{
        //    String finyear = "";
        //    if (Session["LastFinYear"] != null)
        //    {
        //        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        //        finyear = Convert.ToString(Session["LastFinYear"]).Trim();
        //        DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
        //        if (dtFinYear != null && dtFinYear.Rows.Count > 0)
        //        {
        //            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
        //            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

        //        }
        //    }
        //    //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        //}
        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
                //if (Session["exportval"] == null)
                //{
                //    Session["exportval"] = Filter;
                //    bindexport(Filter);
                //}
                //else if (Convert.ToInt32(Session["exportval"]) != Filter)
                //{
                //    Session["exportval"] = Filter;
                //    bindexport(Filter);
                //}
            }
        }
        public void bindexport(int Filter)
        {
            // GrdSalesReturn.Columns[6].Visible = false;
            GrdSalesReturn.Columns[11].Visible = false;
            string filename = "Rate Difference Entry Customer";
            exporter.FileName = filename;
            //    exporter.FileName = "GrdUndeliveryReturn";
            exporter.Landscape = true;
            exporter.PaperKind = System.Drawing.Printing.PaperKind.A4;
            exporter.MaxColumnWidth = 200;
            exporter.PageHeader.Left = "Rate Difference Entry Customer";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }

        #region Export Grid Section Start

        protected void GrdSalesReturn_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            if (Command == "Delete")
            {
                string SalesReturnID = Convert.ToString(e.Parameters).Split('~')[1];
                int deletecnt = 0;
                deletecnt = objSalesReturnBL.DeleteSalesReturn(SalesReturnID, Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), "RDEC", Convert.ToString(Session["userbranchID"]));
                if (deletecnt == 1)
                {

                    string BranchID = Convert.ToString(cmbBranchfilter.Value);


                    string FromDate = Convert.ToString(FormDate.Value);
                    string ToDate = Convert.ToString(toDate.Value);

                    DataTable dtdata = new DataTable();
                    dtdata = objSalesReturnBL.GetUndeliveryReturnListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), BranchID, FromDate, ToDate);


                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        ///Session["UndeliveryReturnSalesListingDetails"] = dtdata;
                        GrdSalesReturn.DataBind();
                    }

                    GrdSalesReturn.JSProperties["cpDelete"] = "Deleted successfully";

                }
                else
                {
                    GrdSalesReturn.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }
                //if (deletecnt>0)
                //{ GrdSalesReturn.JSProperties["cpDelete"] = "Deleted successfully."; }
                //else { GrdSalesReturn.JSProperties["cpDelete"] = "Please try again."; }


            }

            else if (Command == "FilterGridByDate")
            {

                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                // FillAllSearchGrid(true);

                DataTable dtdata = new DataTable();
                // dtdata = objSalesReturnBL.GetCustomerReturnListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "SC");

                dtdata = objSalesReturnBL.GetUndeliveryReturnListGridData(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate);

                //  Session["UndeliveryReturnSalesListingDetails"] = dtdata;
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    GrdSalesReturn.DataSource = dtdata;
                    GrdSalesReturn.DataBind();
                }
                else
                {
                    GrdSalesReturn.DataSource = null;
                    GrdSalesReturn.DataBind();
                }

            }
        }
        [WebMethod]
        public static string GetReturnIdIsExistPosAdjustment(string keyValue)
        {
            SalesReturnBL objCRMSalesReturnBL = new SalesReturnBL();

            int ispermission = 0;
            ispermission = objCRMSalesReturnBL.IsPoSAdjustment(keyValue);

            //}
            return Convert.ToString(ispermission);

        }

        public void GetSalesReturnListGridData(string userbranch, string lastCompany, string FinyearStartDate, string FinYearEndDate)
        {
            //DataTable dtdata = new DataTable();
            //dtdata = objSalesReturnBL.GetUndeliveryReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    GrdSalesReturn.DataSource = dtdata;
            //    GrdSalesReturn.DataBind();
            //}
            //else
            //{
            //    GrdSalesReturn.DataSource = null;
            //    GrdSalesReturn.DataBind();
            //}
        }
        #endregion

        //   protected void GrdSalesReturn_DataBinding(object sender, EventArgs e)
        //{
        //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);


        //string FinyearStartDate = Convert.ToString(Session["FinYearStartDate"]);
        //string FinYearEndDate = Convert.ToString(Session["FinYearEndDate"]);

        //DataTable dtdata = new DataTable();

        //dtdata = objSalesReturnBL.GetUndeliveryReturnListGridData(userbranch, lastCompany, FinyearStartDate, FinYearEndDate);
        //if (dtdata != null && dtdata.Rows.Count > 0)
        //{
        //    GrdSalesReturn.DataSource = dtdata;

        //}
        //else
        //{
        //    GrdSalesReturn.DataSource = null;

        //}

        //   DataTable dsdata = (DataTable)Session["UndeliveryReturnSalesListingDetails"];
        // GrdSalesReturn.DataSource = dsdata;
        //   }



        //warehouse kaushik


        //warehouse kaushik


        protected void BranchTransferCallBackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "Edit")
            {
                string ReturnID = e.Parameter.Split('~')[1];
                BranchTransferCallBackPanel.JSProperties["cpEdit"] = ReturnID;
                DataTable CRPOrderEditdt = GetReturnEditData(ReturnID);
                if (CRPOrderEditdt != null && CRPOrderEditdt.Rows.Count > 0)
                {
                    // hdnEditID.Value = ReceiptPaymentID;
                    string BranchID = Convert.ToString(CRPOrderEditdt.Rows[0]["Return_TransferToBranch"]);
                    if (BranchID != "")
                    {
                        ddlBranch.SelectedValue = BranchID;
                    }

                    string Narration = Convert.ToString(CRPOrderEditdt.Rows[0]["Return_Narration"]);

                    // ddlCashBank.Value = CashBankID;
                    txtNarration.Text = Narration.Trim();
                    //if (IsCRTTransactionExist(ReturnID))
                    //{
                    //    BranchTransferCallBackPanel.JSProperties["cpBtnVisible"] = "false";
                    //}
                }
            }
            if (WhichCall == "Save")
            {
                //string strCashBankBranchID = ddlBranch.SelectedValue;
                string strCashBankBranchID = e.Parameter.Split('~')[2];
                //string strCashBankID =Convert.ToString(ddlCashBank.Value);
                string strNarration = txtNarration.Text;

                try
                {
                    DataSet dsInst = new DataSet();


                   // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("prc_CRMSalesReturn_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    string ID = e.Parameter.Split('~')[1];
                    cmd.Parameters.AddWithValue("@Action", "EditBranch");
                    cmd.Parameters.AddWithValue("@SalesReturnID", ID);
                    cmd.Parameters.AddWithValue("@BranchID", strCashBankBranchID);
                    //cmd.Parameters.AddWithValue("@ReceiptPaymentCashBankID", strCashBankID);
                    cmd.Parameters.AddWithValue("@Narration", strNarration);

                    cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();
                    BranchTransferCallBackPanel.JSProperties["cpTransfer"] = "YES";
                }
                catch (Exception ex)
                {
                    BranchTransferCallBackPanel.JSProperties["cpTransfer"] = "NO";
                }
            }


        }


        public DataTable GetReturnEditData(string SalesReturnID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "SalesReturnBranchEditDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, SalesReturnID);
            dt = proc.GetTable();
            return dt;
        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\RateDiffCustomer\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\RateDiffCustomer\DocDesign\Normal";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
                //SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            // e.KeyExpression = "SrlNo";
            e.KeyExpression = "SrlNo";
          //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                    //----REV 1.0
                    //var q = from d in dc.v_SalesReturnLists
                    //        where DateTime.ParseExact(d.Return_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) >=
                    //        DateTime.ParseExact(strFromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        && DateTime.ParseExact(d.Rn_Date.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        <= DateTime.ParseExact(strToDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                    //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                    //       && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.Rn_Date descending
                    //        select d;   
                    //e.QueryableSource = q;

                    //var q = from d in dc.v_RateDifferenceEntryCustomerLists
                    //        where d.Rn_Date >= Convert.ToDateTime(strFromDate) && d.Rn_Date <= Convert.ToDateTime(strToDate)
                    //        && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                    //       && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.Rn_Date descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.RateDifferenceEntryCustomerLists
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0


                }
                else
                {
                    //----REV 1.0
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    //var q = from d in dc.v_RateDifferenceEntryCustomerLists
                    //        where
                    //        d.Rn_Date >= Convert.ToDateTime(strFromDate) && d.Rn_Date <= Convert.ToDateTime(strToDate) &&
                    //        branchidlist.Contains(Convert.ToInt32(d.BranchID))
                    //        && Convert.ToString(d.Return_FinYear) == Convert.ToString(FinYear)
                    //       && Convert.ToString(d.Return_CompanyID) == Convert.ToString(strCompanyID)
                    //        orderby d.Rn_Date descending
                    //        select d;
                    //e.QueryableSource = q;
                    var q = from d in dc.RateDifferenceEntryCustomerLists
                            where d.USERID == userid
                            orderby d.SEQ descending
                            select d;
                    e.QueryableSource = q;
                    //----END REV 1.0
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //----REV 1.0
                //var q = from d in dc.v_RateDifferenceEntryCustomerLists
                //        where d.BranchID == '0'
                //        orderby d.Rn_Date descending
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.RateDifferenceEntryCustomerLists
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
                //----END REV 1.0
            }
        }

        [WebMethod]
        public static string Prc_EInvoiceChecking_details(string returnid, string mode)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            int i;
            String rtrnvalue = "";
            ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceSalesReturnChecking_details");
            proc.AddVarcharPara("@Action", 500, mode);
            proc.AddPara("@Module", "RateDifferenceEntryCustomer");
            proc.AddPara("@DocumentId", returnid);
            proc.AddPara("@cmp_internalid", Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return Convert.ToString(rtrnvalue);
        }

        //REV 1.0
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(FormDate.Date);
            dtTo = Convert.ToDateTime(toDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Task PopulateStockTrialDataTask = new Task(() => GetRateDifferenceEntryCustomerdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetRateDifferenceEntryCustomerdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_RateDifferenceEntryCustomerList_List", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                if (BRANCH_ID == "0")
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", Convert.ToString(Session["userbranchHierarchy"]));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                }
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        //END REV 1.0
    }
}