﻿#region ==========================Revision History====================================================================================================
//1.0   v2 .0.42    Debashis    17/01/2024      Customer Code column is required in the Order to Delivery report.
//                                              Refer: 0027128
#endregion =======================End Revision History================================================================================================

using DevExpress.Web;
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
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;
using Org.BouncyCastle.Ocsp;

namespace Reports.Reports.GridReports
{
    public partial class OrderToDelivery : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/OrderToDelivery.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    //Rev 1.0 Mantis: 0027128
                    //ShowGridList.Columns[8].Visible = true;
                    ShowGridList.Columns[9].Visible = true;
                    //End of Rev 1.0 Mantis: 0027128
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    //Rev 1.0 Mantis: 0027128
                    //ShowGridList.Columns[8].Visible = false;
                    ShowGridList.Columns[9].Visible = false;
                    //End of Rev 1.0 Mantis: 0027128
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Order To Delivery";
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

                Session["IsOrdToDelvRegFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["exportval"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                BranchHoOffice();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();

                //if (chkPIQuot.Checked==false)
                //{
                //    ShowGridList.Columns[1].Visible = false;
                //    ShowGridList.Columns[2].Visible = false;
                //    ShowGridList.Columns[3].Visible = false;
                //    ShowGridList.Columns[4].Visible = false;
                //}

                //if(chkSOPrintStage.Checked==false)
                //{
                //    ShowGridList.Columns[11].Visible = false;
                //    ShowGridList.Columns[12].Visible = false;
                //}

                //if (chkReadyToInv.Checked == false)
                //{
                //    ShowGridList.Columns[13].Visible = false;
                //    ShowGridList.Columns[14].Visible = false;
                //}

                //if (chkInvoice.Checked == false)
                //{
                //    //ShowGridList.Columns[13].Visible = false;
                //    //ShowGridList.Columns[14].Visible = false;
                //    //ShowGridList.Columns[15].Visible = false;
                //    //ShowGridList.Columns[16].Visible = false;
                //    //ShowGridList.Columns[17].Visible = false;
                //    //ShowGridList.Columns[18].Visible = false;
                //    //ShowGridList.Columns[19].Visible = false;
                //    //ShowGridList.Columns[20].Visible = false;
                //    ShowGridList.Columns[18].Visible = false;
                //    ShowGridList.Columns[19].Visible = false;
                //    ShowGridList.Columns[20].Visible = false;
                //    ShowGridList.Columns[21].Visible = false;
                //    ShowGridList.Columns[22].Visible = false;
                //    ShowGridList.Columns[23].Visible = false;
                //    ShowGridList.Columns[24].Visible = false;
                //    ShowGridList.Columns[25].Visible = false;
                //}

                //if (chkChallan.Checked == false)
                //{
                //    //ShowGridList.Columns[18].Visible = false;
                //    //ShowGridList.Columns[19].Visible = false;
                //    //ShowGridList.Columns[20].Visible = false;
                //    //ShowGridList.Columns[21].Visible = false;
                //    //ShowGridList.Columns[22].Visible = false;
                //    //ShowGridList.Columns[21].Visible = false;
                //    //ShowGridList.Columns[22].Visible = false;
                //    //ShowGridList.Columns[23].Visible = false;
                //    //ShowGridList.Columns[24].Visible = false;
                //    //ShowGridList.Columns[25].Visible = false;
                //    ShowGridList.Columns[26].Visible = false;
                //    ShowGridList.Columns[27].Visible = false;
                //    ShowGridList.Columns[28].Visible = false;
                //    ShowGridList.Columns[29].Visible = false;
                //    ShowGridList.Columns[30].Visible = false;
                //}

                //if (chkApprov.Checked == false)
                //{
                //    //ShowGridList.Columns[11].Visible = false;
                //    //ShowGridList.Columns[12].Visible = false;
                //    //ShowGridList.Columns[13].Visible = false;
                //    ShowGridList.Columns[15].Visible = false;
                //    ShowGridList.Columns[16].Visible = false;
                //    ShowGridList.Columns[17].Visible = false;
                //}

                //if (chkTransport.Checked == false)
                //{
                //    //ShowGridList.Columns[23].Visible = false;
                //    //ShowGridList.Columns[24].Visible = false;
                //    //ShowGridList.Columns[25].Visible = false;
                //    //ShowGridList.Columns[26].Visible = false;
                //    //ShowGridList.Columns[27].Visible = false;
                //    //ShowGridList.Columns[26].Visible = false;
                //    //ShowGridList.Columns[27].Visible = false;
                //    //ShowGridList.Columns[28].Visible = false;
                //    //ShowGridList.Columns[29].Visible = false;
                //    //ShowGridList.Columns[30].Visible = false;
                //    //ShowGridList.Columns[31].Visible = false;
                //    //ShowGridList.Columns[32].Visible = false;
                //    //ShowGridList.Columns[33].Visible = false;
                //    //ShowGridList.Columns[34].Visible = false;
                //    ShowGridList.Columns[31].Visible = false;
                //    ShowGridList.Columns[32].Visible = false;
                //    ShowGridList.Columns[33].Visible = false;
                //    ShowGridList.Columns[34].Visible = false;
                //    ShowGridList.Columns[35].Visible = false;
                //    ShowGridList.Columns[36].Visible = false;
                //    ShowGridList.Columns[37].Visible = false;
                //    ShowGridList.Columns[38].Visible = false;
                //}
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cmbl = new CommonBL();
            DataTable dtfnyear = new DataTable();

            dtfnyear = cmbl.GetDateFinancila(Finyear);
            if (dtfnyear.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

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
            CommonBL blbranch = new CommonBL();
            DataTable dtbranch = new DataTable();
            DataTable dtBranchChild = new DataTable();
            dtbranch = blbranch.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (dtbranch.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = dtbranch;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

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

        #region Branch Populate
        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                }

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
        }
        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
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
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion
        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {

            string filename = "Order To Delivery";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Order To Delivery" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridList";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });                    
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();                    
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
                default:
                    return;
            }

        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        #region Order To Delivery grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsOrdToDelvRegFilter = Convert.ToString(hfIsOrdToDelvRegFilter.Value);
            Session["IsOrdToDelvRegFilter"] = IsOrdToDelvRegFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object branch in BranchList)
            {
                BranchComponent += "," + branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

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

            Task PopulateStockTrialDataTask = new Task(() => GetOrderToDeliveryRegdata(BRANCH_ID, FROMDATE, TODATE, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetOrderToDeliveryRegdata(string BRANCH_ID, string FROMDATE, string TODATE, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ORDERTODELIVERY_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@SHOWQUOTATION", chkPIQuot.Checked);
                cmd.Parameters.AddWithValue("@SHOWINVOICE", chkInvoice.Checked);
                cmd.Parameters.AddWithValue("@SHOWCHALLAN", chkChallan.Checked);
                cmd.Parameters.AddWithValue("@SHOWAPPROVAL", chkApprov.Checked);
                cmd.Parameters.AddWithValue("@SHOWTRANSPORTER", chkTransport.Checked);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@SHOWSOPRINTSTAGEDET", chkSOPrintStage.Checked);
                cmd.Parameters.AddWithValue("@SHOWREADYTOINVEDET", chkReadyToInv.Checked);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

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

        protected void ShowGridList_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {           
            e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsOrdToDelvRegFilter"]) == "Y")
            {
                var q = from d in dc.ORDERTODELIVERY_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.ORDERTODELIVERY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if (chkPIQuot.Checked == false)
            {
                ShowGridList.Columns[1].Visible = false;
                ShowGridList.Columns[2].Visible = false;
                ShowGridList.Columns[3].Visible = false;
                ShowGridList.Columns[4].Visible = false;
            }
            else
            {
                ShowGridList.Columns[1].Visible = true;
                ShowGridList.Columns[2].Visible = true;
                ShowGridList.Columns[3].Visible = true;
                ShowGridList.Columns[4].Visible = true;
            }

            if (chkSOPrintStage.Checked == false)
            {
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[11].Visible = false;
                //ShowGridList.Columns[12].Visible = false;
                ShowGridList.Columns[12].Visible = false;
                ShowGridList.Columns[13].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[11].Visible = true;
                //ShowGridList.Columns[12].Visible = true;
                ShowGridList.Columns[12].Visible = true;
                ShowGridList.Columns[13].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }

            if (chkReadyToInv.Checked == false)
            {
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[13].Visible = false;
                //ShowGridList.Columns[14].Visible = false;
                ShowGridList.Columns[14].Visible = false;
                ShowGridList.Columns[15].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[13].Visible = true;
                //ShowGridList.Columns[14].Visible = true;
                ShowGridList.Columns[14].Visible = true;
                ShowGridList.Columns[15].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }

            if (chkInvoice.Checked == false)
            {
                //ShowGridList.Columns[13].Visible = false;
                //ShowGridList.Columns[14].Visible = false;
                //ShowGridList.Columns[15].Visible = false;
                //ShowGridList.Columns[16].Visible = false;
                //ShowGridList.Columns[17].Visible = false;
                //ShowGridList.Columns[13].Visible = false;
                //ShowGridList.Columns[14].Visible = false;
                //ShowGridList.Columns[15].Visible = false;
                //ShowGridList.Columns[16].Visible = false;
                //ShowGridList.Columns[17].Visible = false;
                //ShowGridList.Columns[18].Visible = false;
                //ShowGridList.Columns[19].Visible = false;
                //ShowGridList.Columns[20].Visible = false;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[18].Visible = false;
                //ShowGridList.Columns[19].Visible = false;
                //ShowGridList.Columns[20].Visible = false;
                //ShowGridList.Columns[21].Visible = false;
                //ShowGridList.Columns[22].Visible = false;
                //ShowGridList.Columns[23].Visible = false;
                //ShowGridList.Columns[24].Visible = false;
                //ShowGridList.Columns[25].Visible = false;
                ShowGridList.Columns[19].Visible = false;
                ShowGridList.Columns[20].Visible = false;
                ShowGridList.Columns[21].Visible = false;
                ShowGridList.Columns[22].Visible = false;
                ShowGridList.Columns[23].Visible = false;
                ShowGridList.Columns[24].Visible = false;
                ShowGridList.Columns[25].Visible = false;
                ShowGridList.Columns[26].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //ShowGridList.Columns[13].Visible = true;
                //ShowGridList.Columns[14].Visible = true;
                //ShowGridList.Columns[15].Visible = true;
                //ShowGridList.Columns[16].Visible = true;
                //ShowGridList.Columns[17].Visible = true;
                //ShowGridList.Columns[13].Visible = true;
                //ShowGridList.Columns[14].Visible = true;
                //ShowGridList.Columns[15].Visible = true;
                //ShowGridList.Columns[16].Visible = true;
                //ShowGridList.Columns[17].Visible = true;
                //ShowGridList.Columns[18].Visible = true;
                //ShowGridList.Columns[19].Visible = true;
                //ShowGridList.Columns[20].Visible = true;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[18].Visible = true;
                //ShowGridList.Columns[19].Visible = true;
                //ShowGridList.Columns[20].Visible = true;
                //ShowGridList.Columns[21].Visible = true;
                //ShowGridList.Columns[22].Visible = true;
                //ShowGridList.Columns[23].Visible = true;
                //ShowGridList.Columns[24].Visible = true;
                //ShowGridList.Columns[25].Visible = true;
                ShowGridList.Columns[19].Visible = true;
                ShowGridList.Columns[20].Visible = true;
                ShowGridList.Columns[21].Visible = true;
                ShowGridList.Columns[22].Visible = true;
                ShowGridList.Columns[23].Visible = true;
                ShowGridList.Columns[24].Visible = true;
                ShowGridList.Columns[25].Visible = true;
                ShowGridList.Columns[26].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }

            if (chkChallan.Checked == false)
            {
                //ShowGridList.Columns[18].Visible = false;
                //ShowGridList.Columns[19].Visible = false;
                //ShowGridList.Columns[20].Visible = false;
                //ShowGridList.Columns[21].Visible = false;
                //ShowGridList.Columns[22].Visible = false;
                //ShowGridList.Columns[21].Visible = false;
                //ShowGridList.Columns[22].Visible = false;
                //ShowGridList.Columns[23].Visible = false;
                //ShowGridList.Columns[24].Visible = false;
                //ShowGridList.Columns[25].Visible = false;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[26].Visible = false;
                //ShowGridList.Columns[27].Visible = false;
                //ShowGridList.Columns[28].Visible = false;
                //ShowGridList.Columns[29].Visible = false;
                //ShowGridList.Columns[30].Visible = false;
                ShowGridList.Columns[27].Visible = false;
                ShowGridList.Columns[28].Visible = false;
                ShowGridList.Columns[29].Visible = false;
                ShowGridList.Columns[30].Visible = false;
                ShowGridList.Columns[31].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //ShowGridList.Columns[18].Visible = true;
                //ShowGridList.Columns[19].Visible = true;
                //ShowGridList.Columns[20].Visible = true;
                //ShowGridList.Columns[21].Visible = true;
                //ShowGridList.Columns[22].Visible = true;
                //ShowGridList.Columns[21].Visible = true;
                //ShowGridList.Columns[22].Visible = true;
                //ShowGridList.Columns[23].Visible = true;
                //ShowGridList.Columns[24].Visible = true;
                //ShowGridList.Columns[25].Visible = true;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[26].Visible = true;
                //ShowGridList.Columns[27].Visible = true;
                //ShowGridList.Columns[28].Visible = true;
                //ShowGridList.Columns[29].Visible = true;
                //ShowGridList.Columns[30].Visible = true;
                ShowGridList.Columns[27].Visible = true;
                ShowGridList.Columns[28].Visible = true;
                ShowGridList.Columns[29].Visible = true;
                ShowGridList.Columns[30].Visible = true;
                ShowGridList.Columns[31].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }

            if (chkApprov.Checked == false)
            {
                //ShowGridList.Columns[11].Visible = false;
                //ShowGridList.Columns[12].Visible = false;
                //ShowGridList.Columns[13].Visible = false;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[15].Visible = false;
                //ShowGridList.Columns[16].Visible = false;
                //ShowGridList.Columns[17].Visible = false;
                ShowGridList.Columns[16].Visible = false;
                ShowGridList.Columns[17].Visible = false;
                ShowGridList.Columns[18].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //ShowGridList.Columns[11].Visible = true;
                //ShowGridList.Columns[12].Visible = true;
                //ShowGridList.Columns[13].Visible = true;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[15].Visible = true;
                //ShowGridList.Columns[16].Visible = true;
                //ShowGridList.Columns[17].Visible = true;
                ShowGridList.Columns[16].Visible = true;
                ShowGridList.Columns[17].Visible = true;
                ShowGridList.Columns[18].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }

            if (chkTransport.Checked == false)
            {
                //ShowGridList.Columns[23].Visible = false;
                //ShowGridList.Columns[24].Visible = false;
                //ShowGridList.Columns[25].Visible = false;
                //ShowGridList.Columns[26].Visible = false;
                //ShowGridList.Columns[27].Visible = false;
                //ShowGridList.Columns[26].Visible = false;
                //ShowGridList.Columns[27].Visible = false;
                //ShowGridList.Columns[28].Visible = false;
                //ShowGridList.Columns[29].Visible = false;
                //ShowGridList.Columns[30].Visible = false;
                //ShowGridList.Columns[31].Visible = false;
                //ShowGridList.Columns[32].Visible = false;
                //ShowGridList.Columns[33].Visible = false;
                //ShowGridList.Columns[34].Visible = false;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[31].Visible = false;
                //ShowGridList.Columns[32].Visible = false;
                //ShowGridList.Columns[33].Visible = false;
                //ShowGridList.Columns[34].Visible = false;
                //ShowGridList.Columns[35].Visible = false;
                //ShowGridList.Columns[36].Visible = false;
                //ShowGridList.Columns[37].Visible = false;
                //ShowGridList.Columns[38].Visible = false;
                ShowGridList.Columns[32].Visible = false;
                ShowGridList.Columns[33].Visible = false;
                ShowGridList.Columns[34].Visible = false;
                ShowGridList.Columns[35].Visible = false;
                ShowGridList.Columns[36].Visible = false;
                ShowGridList.Columns[37].Visible = false;
                ShowGridList.Columns[38].Visible = false;
                ShowGridList.Columns[39].Visible = false;
                //End of Rev 1.0 Mantis: 0027128
            }
            else
            {
                //ShowGridList.Columns[23].Visible = true;
                //ShowGridList.Columns[24].Visible = true;
                //ShowGridList.Columns[25].Visible = true;
                //ShowGridList.Columns[26].Visible = true;
                //ShowGridList.Columns[27].Visible = true;
                //ShowGridList.Columns[26].Visible = true;
                //ShowGridList.Columns[27].Visible = true;
                //ShowGridList.Columns[28].Visible = true;
                //ShowGridList.Columns[29].Visible = true;
                //ShowGridList.Columns[30].Visible = true;
                //ShowGridList.Columns[31].Visible = true;
                //ShowGridList.Columns[32].Visible = true;
                //ShowGridList.Columns[33].Visible = true;
                //ShowGridList.Columns[34].Visible = true;
                //Rev 1.0 Mantis: 0027128
                //ShowGridList.Columns[31].Visible = true;
                //ShowGridList.Columns[32].Visible = true;
                //ShowGridList.Columns[33].Visible = true;
                //ShowGridList.Columns[34].Visible = true;
                //ShowGridList.Columns[35].Visible = true;
                //ShowGridList.Columns[36].Visible = true;
                //ShowGridList.Columns[37].Visible = true;
                //ShowGridList.Columns[38].Visible = true;
                ShowGridList.Columns[32].Visible = true;
                ShowGridList.Columns[33].Visible = true;
                ShowGridList.Columns[34].Visible = true;
                ShowGridList.Columns[35].Visible = true;
                ShowGridList.Columns[36].Visible = true;
                ShowGridList.Columns[37].Visible = true;
                ShowGridList.Columns[38].Visible = true;
                ShowGridList.Columns[39].Visible = true;
                //End of Rev 1.0 Mantis: 0027128
            }
        }
        #endregion

        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }

    }
}