﻿using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class JournalRegister_Details : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Journal Voucher Register - Detail";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();

                Session["exportval"] = null;
                Session["exportval2"] = null;
                //Session["dt_SalesRegisterRpt"] = null;
                //Session["dt_SalesRegisterRpt2"] = null;
                Session["IsJournalRegDet"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }


            if (!IsPostBack)
            {
                //Session.Remove("dt_SalesRegisterRpt2");
                //ShowGrid.JSProperties["cpSave"] = null;
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    bindexport(Filter);
                }
            }
        }

        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");

            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "Journal Voucher Register - Detail";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Journal Voucher Register - Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 20-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev 
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        //Rev Subhra 20-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dtLedger"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}
        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string returnPara = Convert.ToString(e.Parameters);            
            //Session.Remove("dt_SalesRegisterRpt2");
            //ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsJournalRegDet = Convert.ToString(hfIsJournalRegDet.Value);
            Session["IsJournalRegDet"] = IsJournalRegDet;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchID = "";
            List<object> BranchIDList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchIDList)
            {
                BranchID += "," + Branch;
            }
            BRANCH_ID = BranchID.TrimStart(',');
            //Rev Subhra 20-12-2018   0017670
            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);
            //End of Rev
            Task PopulateStockTrialDataTask = new Task(() => GetJournalRegisterdata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetJournalRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_JOURNALREGISTERDETAIL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dt_SalesRegisterRpt2"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsJournalRegDet"]) == "Y")
            {
                var q = from d in dc.JOURNALREGISTERDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.JOURNALREGISTERDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        //[WebMethod]
        //public static List<string> GetBranchesList(String NoteId, string Hoid)
        //{
        //    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    //StringBuilder filter = new StringBuilder();
        //    //StringBuilder Supervisorfilter = new StringBuilder();
        //    //BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    //DataTable dtbl = new DataTable();
        //    //if (NoteId.Trim() == "")
        //    //{
        //    //    dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
        //    //}

        //    //List<string> obj = new List<string>();

        //    //foreach (DataRow dr in dtbl.Rows)
        //    //{

        //    //    obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    //}
        //    //return obj;


        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}

        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_SalesRegisterRpt2"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_SalesRegisterRpt2"];
        //    }
        //}

        public void Date_finyearwise(string Finyear)
        {
            ////CommonBL bll1 = new CommonBL();
            ////DataTable stbill = new DataTable();
            ////stbill = bll1.GetDateFinancila(Finyear);
            ////if (stbill.Rows.Count > 0)
            ////{
            ////    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            ////    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            ////}

            CommonBL journal = new CommonBL();
            DataTable dtjournal = new DataTable();
            //DateTime MinDate, MaxDate;

            dtjournal = journal.GetDateFinancila(Finyear);
            if (dtjournal.Rows.Count > 0)
            {
                ////ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ////ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
                //FromDate.Value = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                //ToDate.Value = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");

                ASPxFromDate.MaxDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtjournal.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }

        }

        public void BranchHoOffice()
        {
            CommonBL branch = new CommonBL();
            DataTable dtbranch = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //dtbranch = branch.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            dtbranch = branch.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (dtbranch.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = dtbranch;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }
        //End of Rev Debashis

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        //protected void grid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        //{

        //    e.Text = string.Format("{0}", e.Value);
        //}
        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    //if (Session["userbranchHierarchy"] != null)
                    //{
                    //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
                    //}
                    ///  ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and  branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }


        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        } 
    }
}