﻿#region =======================Revision History=========================================================================================================
//1.0   v2 .0.42    Debashis    26/03/2024  Customer code column is required in various reports.Refer: 0027273
#endregion=======================End Revision History====================================================================================================
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class CustomerAnalysis : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        //Rev Debashis
        ExcelFile objExcel = new ExcelFile();
        DataTable CompanyInfo = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        //End of Rev Debashis

        DataTable dtPartyTotal = null;
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/CustomerAnalysis.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGridCustAnalysis.Columns[4].Visible = true;
                    ShowGridCustAnalysis.Columns[5].Visible = true;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    //Rev 1.0 Mantis: 0027273
                    //ShowGridCustAnalysis.Columns[4].Visible = false;
                    ShowGridCustAnalysis.Columns[5].Visible = false;
                    //End of Rev 1.0 Mantis: 0027273
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Analysis - Detail";
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
                Session["IsCustAnalysisDetFilter"] = null;
                //Session["exportval"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //chkallcust.Attributes.Add("OnClick", "CustAll('allcust')");
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            }


            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                //Rev 1.0 Mantis: 0027273
                //string strduedatechk = (chkduedate.Checked) ? "1" : "0";
                //string strprintdatechk = (chkprintdays.Checked) ? "1" : "0";
                //string strsalesman = (chksalesman.Checked) ? "1" : "0";
                //string strpartyordnodt = (chkPartyOrdNoDt.Checked) ? "1" : "0";
                //if (Convert.ToString(strduedatechk) == "0")
                //{
                //    //ShowGridCustAnalysis.Columns[6].Visible = false;
                //    ShowGridCustAnalysis.Columns[7].Visible = false;
                //}
                //else
                //{
                //    //ShowGridCustAnalysis.Columns[6].Visible = true;
                //    ShowGridCustAnalysis.Columns[7].Visible = true;
                //}

                //ShowGridCustAnalysis.Columns[8].Visible = false;
                //ShowGridCustAnalysis.Columns[9].Visible = false;

                //if (Convert.ToString(strpartyordnodt) == "0")
                //{
                //    //ShowGridCustAnalysis.Columns[7].Visible = false;
                //    //ShowGridCustAnalysis.Columns[8].Visible = false;
                //    //ShowGridCustAnalysis.Columns[8].Visible = false;
                //    //ShowGridCustAnalysis.Columns[9].Visible = false;
                //    ShowGridCustAnalysis.Columns[10].Visible = false;
                //    ShowGridCustAnalysis.Columns[11].Visible = false;
                //}
                //else
                //{
                //    //ShowGridCustAnalysis.Columns[7].Visible = true;
                //    //ShowGridCustAnalysis.Columns[8].Visible = true;
                //    //ShowGridCustAnalysis.Columns[8].Visible = true;
                //    //ShowGridCustAnalysis.Columns[9].Visible = true;
                //    ShowGridCustAnalysis.Columns[10].Visible = true;
                //    ShowGridCustAnalysis.Columns[11].Visible = true;
                //}

                //if (Convert.ToString(strsalesman) == "0")
                //{
                //    //ShowGridCustAnalysis.Columns[9].Visible = false;
                //    //ShowGridCustAnalysis.Columns[10].Visible = false;
                //    ShowGridCustAnalysis.Columns[12].Visible = false;
                //}
                //else
                //{
                //    //ShowGridCustAnalysis.Columns[9].Visible = true;
                //    //ShowGridCustAnalysis.Columns[10].Visible = true;
                //    ShowGridCustAnalysis.Columns[12].Visible = true;
                //}

                //if (Convert.ToString(strprintdatechk) == "0")
                //{
                //    //ShowGridCustAnalysis.Columns[16].Visible = false;
                //    //ShowGridCustAnalysis.Columns[17].Visible = false;
                //    ShowGridCustAnalysis.Columns[19].Visible = false;
                //}
                //else
                //{
                //    //ShowGridCustAnalysis.Columns[16].Visible = true;
                //    //ShowGridCustAnalysis.Columns[17].Visible = true;
                //    ShowGridCustAnalysis.Columns[19].Visible = true;
                //}
                //End of Rev 1.0 Mantis: 0027273
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {

                ASPxAsOnDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }

            }

        }
        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Rev Debashis
            //if (Filter != 0)
            //{

            //    BranchHoOffice();
            //    if (Session["exportval"] == null)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //}
            if (Convert.ToString(Session["IsCustAnalysisDetFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
            else { 
                BranchHoOffice(); 
            }
            //End of Rev Debashis
        }
        public void bindexport(int Filter)
        {
            string filename = "Customer Analysis-Detail";
            exporter.FileName = filename;
            //Rev Debashis
            //string FileHeader = "";

            //BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Customer Analysis-Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
            //FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //exporter.PageHeader.Left = FileHeader;
            //exporter.PageHeader.Font.Size = 10;
            //exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.GridViewID = "ShowGridCustAnalysis";
            //exporter.RenderBrick += exporter_RenderBrick;
            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                //Rev 1.0 Mantis: 0027273
                //string selectQuery = "SELECT PARTYNAME,BRANCH_DESCRIPTION,DOC_TYPE,ISOPENING,PROJ_NAME,DOC_NO,DOC_DATE,DUE_DATE,SONO,SODATE,PARTYORD_NO,PARTYORD_DATE,SALESMAN,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DOC_AMOUNT,'(',CASE WHEN SUBSTRING(DOC_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS DOC_AMOUNT,ADJ_DOC_TYPE,ADJ_DOC_NO,ADJ_DOC_DATE,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(ADJ_AMOUNT,'(',CASE WHEN SUBSTRING(ADJ_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS ADJ_AMOUNT,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(BAL_AMOUNT,'(',CASE WHEN SUBSTRING(BAL_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS BAL_AMOUNT,DAYS FROM PARTYWISEANALYSISDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO<>999999 AND DOC_TYPE<>'Gross Total :' AND PARTYTYPE='C' order by SEQ";
                string selectQuery = "SELECT PARTYCODE,PARTYNAME,BRANCH_DESCRIPTION,DOC_TYPE,ISOPENING,PROJ_NAME,DOC_NO,DOC_DATE,DUE_DATE,SONO,SODATE,PARTYORD_NO,PARTYORD_DATE,SALESMAN,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(DOC_AMOUNT,'(',CASE WHEN SUBSTRING(DOC_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS DOC_AMOUNT,ADJ_DOC_TYPE,ADJ_DOC_NO,ADJ_DOC_DATE,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(ADJ_AMOUNT,'(',CASE WHEN SUBSTRING(ADJ_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS ADJ_AMOUNT,CONVERT(DECIMAL(18,2),REPLACE(REPLACE(BAL_AMOUNT,'(',CASE WHEN SUBSTRING(BAL_AMOUNT,1,1)='(' THEN '-' ELSE '' END),')','')) AS BAL_AMOUNT,DAYS FROM PARTYWISEANALYSISDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO<>999999 AND DOC_TYPE<>'Gross Total :' AND PARTYTYPE='C' order by SEQ";
                //End of Rev 1.0 Mantis: 0027273
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select DOC_TYPE,TOT_BAL_AMOUNT AS BAL_AMOUNT from PARTYWISEANALYSISDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999 AND DOC_TYPE='Gross Total :' AND PARTYTYPE='C'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportcustanadetdataset"] = ds;

                string strduedatechk = (chkduedate.Checked) ? "1" : "0";
                string strprintdatechk = (chkprintdays.Checked) ? "1" : "0";
                string strsalesman = (chksalesman.Checked) ? "1" : "0";
                string strpartyordnodt = (chkPartyOrdNoDt.Checked) ? "1" : "0";
                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                //Rev 1.0 Mantis: 0027273
                dtExport.Columns.Add(new DataColumn("Code", typeof(string)));
                //End of Rev 1.0 Mantis: 0027273
                dtExport.Columns.Add(new DataColumn("Customer Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Opening?", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Due Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Party Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Salesman", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Doc. Amt.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Adj. Doc. Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Adj. Doc. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Adj. Doc. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Adj. Amt.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Days", typeof(Int32)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    //Rev 1.0 Mantis: 0027273
                    row2["Code"] = dr1["PARTYCODE"];
                    //End of Rev 1.0 Mantis: 0027273
                    row2["Customer Name"] = dr1["PARTYNAME"];
                    row2["Unit"] = dr1["BRANCH_DESCRIPTION"];
                    row2["Doc. Type"] = dr1["DOC_TYPE"];
                    row2["Opening?"] = dr1["ISOPENING"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["Doc. No."] = dr1["DOC_NO"];
                    row2["Doc. Date"] = dr1["DOC_DATE"];
                    row2["Due Date"] = dr1["DUE_DATE"];
                    row2["Order No."] = dr1["SONO"];
                    row2["Order Date"] = dr1["SODATE"];
                    row2["Party Order No."] = dr1["PARTYORD_NO"];
                    row2["Party Order Date"] = dr1["PARTYORD_DATE"];
                    row2["Salesman"] = dr1["SALESMAN"];
                    row2["Doc. Amt."] = dr1["DOC_AMOUNT"];
                    row2["Adj. Doc. Type"] = dr1["ADJ_DOC_TYPE"];
                    row2["Adj. Doc. No."] = dr1["ADJ_DOC_NO"];
                    row2["Adj. Doc. Date"] = dr1["ADJ_DOC_DATE"];
                    row2["Adj. Amt."] = dr1["ADJ_AMOUNT"];
                    row2["Balance Amount"] = dr1["BAL_AMOUNT"];
                    row2["Days"] = dr1["DAYS"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");
                if (Convert.ToString(strduedatechk) == "0")
                    dtExport.Columns.Remove("Due Date");
                if(chkSoNoDt.Checked==false)
                {
                    dtExport.Columns.Remove("Order No.");
                    dtExport.Columns.Remove("Order Date");
                }
                if (Convert.ToString(strpartyordnodt) == "0")
                {
                    dtExport.Columns.Remove("Party Order No.");
                    dtExport.Columns.Remove("Party Order Date");
                }
                if (Convert.ToString(strsalesman) == "0")
                    dtExport.Columns.Remove("Salesman");
                if (Convert.ToString(strprintdatechk) == "0")
                    dtExport.Columns.Remove("Days");

                //Rev 1.0 Mantis: 0027273
                dtExport.Columns.Remove("PARTYCODE");
                //End of Rev 1.0 Mantis: 0027273
                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("BRANCH_DESCRIPTION");
                dtExport.Columns.Remove("DOC_TYPE");
                dtExport.Columns.Remove("ISOPENING");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("DOC_NO");
                dtExport.Columns.Remove("DOC_DATE");
                dtExport.Columns.Remove("DUE_DATE");
                dtExport.Columns.Remove("SONO");
                dtExport.Columns.Remove("SODATE");
                dtExport.Columns.Remove("PARTYORD_NO");
                dtExport.Columns.Remove("PARTYORD_DATE");
                dtExport.Columns.Remove("SALESMAN");
                dtExport.Columns.Remove("DOC_AMOUNT");
                dtExport.Columns.Remove("ADJ_DOC_TYPE");
                dtExport.Columns.Remove("ADJ_DOC_NO");
                dtExport.Columns.Remove("ADJ_DOC_DATE");
                dtExport.Columns.Remove("ADJ_AMOUNT");
                dtExport.Columns.Remove("BAL_AMOUNT");
                dtExport.Columns.Remove("DAYS");

                DataRow row3 = dtExport.NewRow();
                row3["Doc. Type"] = ds.Tables[1].Rows[0]["DOC_TYPE"].ToString();
                row3["Balance Amount"] = ds.Tables[1].Rows[0]["BAL_AMOUNT"].ToString();
                dtExport.Rows.Add(row3);

                //For Excel/PDF Header
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String)));

                string GridHeader = "";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                HeaderRow1[0] = Convert.ToString(Session["BranchNames"]);
                dtReportHeader.Rows.Add(HeaderRow1);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                HeaderRow2[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow2);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                DataRow HeaderRow3 = dtReportHeader.NewRow();
                HeaderRow3[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow3);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                DataRow HeaderRow4 = dtReportHeader.NewRow();
                HeaderRow4[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow4);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                DataRow HeaderRow5 = dtReportHeader.NewRow();
                HeaderRow5[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow5);
                DataRow HeaderRow6 = dtReportHeader.NewRow();
                HeaderRow6[0] = "Customer Analysis-Detail";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "As On: " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);

                //For Excel/PDF Footer
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
            }
            else
            {
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.GridViewID = "ShowGridCustAnalysis";
            }
            //End of Rev Debashis

            switch (Filter)
            {
                case 1:
                    //Rev Debashis
                    //exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    objExcel.ExportToExcelforExcel(dtExport, "Customer Analysis-Detail", "Total of ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    //End of Rev Debashis
                    break;
                case 2:
                    //Rev Debashis
                    //exporter.WritePdfToResponse();
                    objExcel.ExportToPDF(dtExport, "Customer Analysis-Detail", "Total of ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    //End of Rev Debashis
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                //Rev Debashis
                //case 4:
                //    exporter.WriteRtfToResponse();
                //    break;
                //End of Rev Debashis
                default:
                    return;
            }

        }
        //Rev Debashis
        //public string ReplaceFirst(string text, string search, string replace)
        //{
        //    int pos = text.IndexOf(search);
        //    if (pos < 0)
        //    {
        //        return text;
        //    }
        //    return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        //}

        //protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        //{
        //    e.BrickStyle.BackColor = Color.White;
        //    e.BrickStyle.ForeColor = Color.Black;
        //}
        //End of Rev Debashis
        #endregion

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
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

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
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

        #region Customer Analysis grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsCustAnalysisDetFilter = Convert.ToString(hfIsCustAnalysisDetFilter.Value);
            Session["IsCustAnalysisDetFilter"] = IsCustAnalysisDetFilter;

            DateTime dtFrom;

            dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

            string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Brnch in BranchList)
            {
                BranchComponent += "," + Brnch;
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

            string PARTY_TYPE = "C";
            string DUEDATE = (chkduedate.Checked) ? "1" : "0";
            string ALLPARTY = (chkallcust.Checked) ? "1" : "0";
            string PRINT_DAYS = (chkprintdays.Checked) ? "1" : "0";
            string SALESMAN = (chksalesman.Checked) ? "1" : "0";
            string CBVOUCHER = (chkcb.Checked) ? "1" : "0";
            string JVOUCHER = (chkjv.Checked) ? "1" : "0";
            string DNCNNOTE = (chkdncn.Checked) ? "1" : "0";

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count==0)
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

            Task PopulateStockTrialDataTask = new Task(() => GetCustAnalysisDetdata(ASONDATE, BRANCH_ID, HEAD_BRANCH, PARTY_TYPE, DUEDATE, ALLPARTY, PRINT_DAYS, SALESMAN, CBVOUCHER, JVOUCHER, DNCNNOTE, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();

        }
        public void GetCustAnalysisDetdata(string ASONDATE, string BRANCH_ID, string HEAD_BRANCH, string PARTY_TYPE, string DUEDATE, string ALLPARTY, string PRINT_DAYS, string SALESMAN, string CBVOUCHER, string JVOUCHER, string DNCNNOTE, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYWISEANALYSISDET_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@ONDATE", ddlondate.SelectedValue);
                cmd.Parameters.AddWithValue("@DUEDATE", DUEDATE);
                cmd.Parameters.AddWithValue("@ALLPARTY", ALLPARTY);
                cmd.Parameters.AddWithValue("@PRINTDAYS", PRINT_DAYS);
                cmd.Parameters.AddWithValue("@SHOWSALESMAN", SALESMAN);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@PARTY_TYPE", PARTY_TYPE);
                cmd.Parameters.AddWithValue("@INCLUDECB", CBVOUCHER);
                cmd.Parameters.AddWithValue("@INCLUDEJV", JVOUCHER);
                cmd.Parameters.AddWithValue("@EXCLUDEDNCN", DNCNNOTE);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@SHOWPARTYORDNODATE", (chkPartyOrdNoDt.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWSOORDNODATE", (chkSoNoDt.Checked) ? "1" : "0");
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

        protected void ShowGridCustAnalysis_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsCustAnalysisDetFilter"]) == "Y")
            {
                //dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,BAL_AMOUNT from PARTYWISEANALYSISDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999 AND DOC_TYPE='Gross Total :' AND PARTYTYPE='C'");
                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,TOT_BAL_AMOUNT from PARTYWISEANALYSISDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999 AND DOC_TYPE='Gross Total :' AND PARTYTYPE='C'");
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalBalAmt = dtPartyTotal.Rows[0][1].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_BalAmt":
                        e.Text = PartyTotalBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCustAnalysisDetFilter"]) == "Y")
            {
                var q = from d in dc.PARTYWISEANALYSISDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999" && Convert.ToString(d.PARTYTYPE) == "C"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYWISEANALYSISDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strduedatechk = (chkduedate.Checked) ? "1" : "0";
            string strprintdatechk = (chkprintdays.Checked) ? "1" : "0";
            string strsalesman = (chksalesman.Checked) ? "1" : "0";
            string strpartyordnodt = (chkPartyOrdNoDt.Checked) ? "1" : "0";

            if (Convert.ToString(strduedatechk) == "0")
            {
                //ShowGridCustAnalysis.Columns[6].Visible = false;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[7].Visible = false;
                ShowGridCustAnalysis.Columns[8].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else
            {
                //ShowGridCustAnalysis.Columns[6].Visible = true;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[7].Visible = true;
                ShowGridCustAnalysis.Columns[8].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            if (chkSoNoDt.Checked==false)
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[8].Visible = false;
                //ShowGridCustAnalysis.Columns[9].Visible = false;
                ShowGridCustAnalysis.Columns[9].Visible = false;
                ShowGridCustAnalysis.Columns[10].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else
            {
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[8].Visible = true;
                //ShowGridCustAnalysis.Columns[9].Visible = true;
                ShowGridCustAnalysis.Columns[9].Visible = true;
                ShowGridCustAnalysis.Columns[10].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            if (Convert.ToString(strpartyordnodt) == "0")
            {
                //ShowGridCustAnalysis.Columns[7].Visible = false;
                //ShowGridCustAnalysis.Columns[8].Visible = false;
                //ShowGridCustAnalysis.Columns[8].Visible = false;
                //ShowGridCustAnalysis.Columns[9].Visible = false;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[10].Visible = false;
                //ShowGridCustAnalysis.Columns[11].Visible = false;
                ShowGridCustAnalysis.Columns[11].Visible = false;
                ShowGridCustAnalysis.Columns[12].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else
            {
                //ShowGridCustAnalysis.Columns[7].Visible = true;
                //ShowGridCustAnalysis.Columns[8].Visible = true;
                //ShowGridCustAnalysis.Columns[8].Visible = true;
                //ShowGridCustAnalysis.Columns[9].Visible = true;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[10].Visible = true;
                //ShowGridCustAnalysis.Columns[11].Visible = true;
                ShowGridCustAnalysis.Columns[11].Visible = true;
                ShowGridCustAnalysis.Columns[12].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            if (Convert.ToString(strsalesman) == "0")
            {
                //ShowGridCustAnalysis.Columns[9].Visible = false;
                //ShowGridCustAnalysis.Columns[10].Visible = false;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[12].Visible = false;
                ShowGridCustAnalysis.Columns[13].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else
            {
                //ShowGridCustAnalysis.Columns[9].Visible = true;
                //ShowGridCustAnalysis.Columns[10].Visible = true;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[12].Visible = true;
                ShowGridCustAnalysis.Columns[13].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            if (Convert.ToString(strprintdatechk) == "0")
            {
                //ShowGridCustAnalysis.Columns[16].Visible = false;
                //ShowGridCustAnalysis.Columns[17].Visible = false;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[19].Visible = false;
                ShowGridCustAnalysis.Columns[20].Visible = false;
                //End of Rev 1.0 Mantis: 0027273
            }
            else
            {
                //ShowGridCustAnalysis.Columns[16].Visible = true;
                //ShowGridCustAnalysis.Columns[17].Visible = true;
                //Rev 1.0 Mantis: 0027273
                //ShowGridCustAnalysis.Columns[19].Visible = true;
                ShowGridCustAnalysis.Columns[20].Visible = true;
                //End of Rev 1.0 Mantis: 0027273
            }

            ShowGridCustAnalysis.ExpandAll();
        }
        #endregion
        protected void ShowGridCustAnalysis_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Doc. Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }
        protected void ShowGridCustAnalysis_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue).Contains("Total of"))
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
        }

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