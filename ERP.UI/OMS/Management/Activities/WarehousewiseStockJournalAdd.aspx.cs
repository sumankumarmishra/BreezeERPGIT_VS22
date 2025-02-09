﻿
/**********************************************************************************************************************************
 1.0    v2.0.22     Sanchita	21/04/2021		New check box "Dont Consider in Profitability Report" introduced module Warehouse wise Stock Journal .
												checkbox "NotInProfReport" added.
												Refer:  23986
 2.0	v2.0.38	    PRITI		07-04-2023      0025689:Alt Qty column & data is not showing while making Warehouse wise Stock out
 3.0	v2.0.39	    PRITI		06-07-2023      0026523: Unable to Save Warehouse wise Stock Journal
 4.0    V2.0.43     Priti       22-02-2024      Mantis: 0027218 Batchwise stock has been issued from any stock out module before receiving date which caused negative stock
***********************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections;
using System.Web.Services;
using EntityLayer.CommonELS;
using DevExpress.Web;
using DataAccessLayer;
using System.Text.RegularExpressions;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class WarehousewiseStockJournalAdd : System.Web.UI.Page
    {
       
        WarehouseStockJournalBL blLayer = new WarehouseStockJournalBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        DataTable TempTable = new DataTable();
        DataTable TempAdjustmentTableSWH = new DataTable();
        DataTable TempAdjustmentTableDWH = new DataTable();
        string adjustmentNumber = "";
        int adjustmentId = 0, ErrorCode = 0;
        //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
        protected void Page_Init(object sender, EventArgs e)
        {
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            UomSelect_Source.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect_Source.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");
            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");

            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                }
            }
            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";
                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";
                }
            }
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    hdnHierarchySelectInEntryModule.Value = "1";
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnHierarchySelectInEntryModule.Value = "0";
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }
            //Rev work start 17.05.2022  Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            //added for MUltiUOM settings start           
            string MultiUOMSelection = ComBL.GetSystemSettingsResult("MultiUOMSelection");
            if (!String.IsNullOrEmpty(MultiUOMSelection))
            {
                if (MultiUOMSelection.ToUpper().Trim() == "YES")
                {
                    hddnMultiUOMSelection.Value = "1";
                    lbl1.Visible = false;
                   
                }
                else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                {
                    hddnMultiUOMSelection.Value = "0";
                    gridDEstination.Columns[10].Width = 0;
                    // Mantis Issue 24425, 24428
                    gridDEstination.Columns[11].Width = 0;
                    gridDEstination.Columns[12].Width = 0;
                    lbl1.Visible = true;
                    
                   // End of Mantis Issue 24425, 24428
                }
            }
            string MultiUOMSelection_Source = ComBL.GetSystemSettingsResult("MultiUOMSelection");
            if (!String.IsNullOrEmpty(MultiUOMSelection_Source))
            {
                if (MultiUOMSelection_Source.ToUpper().Trim() == "YES")
                {
                    hddnMultiUOMSelection_Source.Value = "1";
                    lbl2.Visible = false;
                }
                else if (MultiUOMSelection_Source.ToUpper().Trim() == "NO")
                {
                    hddnMultiUOMSelection_Source.Value = "0";
                    grid.Columns[10].Width = 0;
                    // Mantis Issue 24425, 24428
                    grid.Columns[11].Width = 0;
                    grid.Columns[12].Width = 0;
                    lbl2.Visible = true;
                    // End of Mantis Issue 24425, 24428
                }
            }
            //End
            //Revwork close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=45");

            if (dtposTime != null && dtposTime.Rows.Count > 0)
            {
                hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
            }

            if (!IsPostBack)
            {
                string TechnicianStockAdjustment = ComBL.GetSystemSettingsResult("TechnicianStockJournalBoth");
                if (TechnicianStockAdjustment != null)
                {
                    if (TechnicianStockAdjustment == "0")
                    {
                        DivTechnician.Style.Add("display", "none");
                    }
                    else
                    {
                        DivTechnician.Style.Add("display", "!inline-block");
                    }
                }      
                ddlHierarchy.Enabled = false;
                bindHierarchy();
                Session["WHSJSour_WarehouseData"] = null;
                Session["WHSJDest_WarehouseData"] = null;
                Session["TempSourceWarehousedt"] = null;
                /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                Session["MultiUOMData"] = null;
                Session["MultiUOMData_Source"] = null;
                /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                    //hdnPageStatus.Value = "update";
                    //hdnPageStatus_Source.Value = "update";
                    Session["WHJournalID"] = AdjId;
                    /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    hdAdjustmentId.Value = AdjId;
                    //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    Session["MultiUOMData"] = GetMultiUOMData().Tables[0];
                    Session["MultiUOMData_Source"] = GetMultiUOMData().Tables[1];
                    //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    lblHeading.Text = "Modify Warehouse Wise Stock Journal";
                    Session["WHSJSour_WarehouseData"] = GetWHSJWarehouseData();
                    Session["WHSJDest_WarehouseData"] = GetWHSJWarehouseDataDistination();
                   
                }
                else
                {
                    hdAddEdit.Value = "Add";
                    /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                    //hdnPageStatus.Value = "first";
                    //hdnPageStatus_Source.Value = "first";
                    /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                    AddmodeExecuted();
                    Session["AdjustWHSTDetailTable"] = null;
                    Session["AdjustDestWHSTDetailTable"] = null;
                    Session["TempTableSWHJ"] = null;
                    //DataTable Transactiondt = CreateTempTable("Transaction");
                    //Transactiondt.Rows.Add("1", "1", "", "", "0.0000", "0.0000", "0.000", "", "0.00", "0.00", "", "I");

                    //Session["ProductOrderDetails"] = Transactiondt;
                    //grid.DataSource = GetWHStockTransferBatch(Transactiondt);
                    //grid.DataBind();

                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Warehouse Wise Stock Journal";
                    /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                   /* btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;*/
                    hdnPageMode.Value = Request.QueryString["req"].ToString();
                    /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                }

                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                DBEngine odbeng = new DBEngine();
                DataTable WatermarkDt = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='WarehouseWiseStockJournalAutoPrint'");
                if (Convert.ToString(WatermarkDt.Rows[0]["Variable_Value"]).ToUpper() == "NO")
                {
                    hdnWSTAutoPrint.Value = "0";
                }
                else
                {
                    hdnWSTAutoPrint.Value = "1";
                }
            }
        }
        /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
        public DataSet GetMultiUOMData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "MultiUOMWHStockJournalINOUT");           
            proc.AddVarcharPara("@SalesOrder_Id", 500, Convert.ToString(Session["WHJournalID"]));
            //ds = proc.GetTable();
            ds = proc.GetDataSet();
            return ds;
        }
        /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
        public DataTable CreateTempTable(string Type)
        {

            DataTable WHStockTransferdt = new DataTable();

            if (Type == "Transaction")
            {
                WHStockTransferdt.Columns.Add("SrlNo", typeof(string));
                WHStockTransferdt.Columns.Add("StkTransferDetails_Id", typeof(string));
                WHStockTransferdt.Columns.Add("ProductID", typeof(string));
                WHStockTransferdt.Columns.Add("Description", typeof(string));
                WHStockTransferdt.Columns.Add("Quantity", typeof(string));

                WHStockTransferdt.Columns.Add("AvlStkSource", typeof(string));
                WHStockTransferdt.Columns.Add("AvlStkDest", typeof(string));
                WHStockTransferdt.Columns.Add("Warehouse", typeof(string));
                WHStockTransferdt.Columns.Add("Rate", typeof(string));
                WHStockTransferdt.Columns.Add("Value", typeof(string));
                WHStockTransferdt.Columns.Add("ProductName", typeof(string));
                WHStockTransferdt.Columns.Add("Status", typeof(string));

            }
            return WHStockTransferdt;
        }
        public IEnumerable GetWHStockTransferBatch(DataTable Detailsdt)
        {
            List<WHStockTransferList> WHStockTransferList = new List<WHStockTransferList>();

            DataColumnCollection dtC = Detailsdt.Columns;
            for (int i = 0; i < Detailsdt.Rows.Count; i++)
            {
                WHStockTransferList WHStockTransferDetails = new WHStockTransferList();

                WHStockTransferDetails.SrlNo = Convert.ToString(Detailsdt.Rows[i]["SrlNo"]);
                WHStockTransferDetails.StkTransferDetails_Id = Convert.ToString(Detailsdt.Rows[i]["StkTransferDetails_Id"]);
                WHStockTransferDetails.ProductID = Convert.ToString(Detailsdt.Rows[i]["ProductID"]);
                WHStockTransferDetails.Discription = Convert.ToString(Detailsdt.Rows[i]["Description"]);
                WHStockTransferDetails.TransferQuantity = Convert.ToString(Detailsdt.Rows[i]["Quantity"]);
                WHStockTransferDetails.AvlStkSourceWH = Convert.ToString(Detailsdt.Rows[i]["AvlStkSource"]);
                WHStockTransferDetails.AvlStkDestWH = Convert.ToString(Detailsdt.Rows[i]["AvlStkDest"]);
                WHStockTransferDetails.Warehouse = "";
                WHStockTransferDetails.Rate = Convert.ToString(Detailsdt.Rows[i]["Rate"]);
                WHStockTransferDetails.Value = Convert.ToString(Detailsdt.Rows[i]["Value"]);
                WHStockTransferDetails.ProductName = Convert.ToString(Detailsdt.Rows[i]["ProductName"]);
                WHStockTransferList.Add(WHStockTransferDetails);
            }

            return WHStockTransferList;
        }
        public class WHStockTransferList
        {
            public string SrlNo { get; set; }
            public string StkTransferDetails_Id { get; set; }
            public string ProductID { get; set; }
            public string Discription { get; set; }
            public string TransferQuantity { get; set; }
            public string AvlStkSourceWH { get; set; }
            public string AvlStkDestWH { get; set; }
            public string Warehouse { get; set; }
            public string Rate { get; set; }
            public string Value { get; set; }
            public string ProductName { get; set; }


        }
        private void AddmodeExecuted()
        {

            DataSet allDetails = blLayer.PopulateStockAdjustmentDetails();
            CmbScheme.DataSource = allDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = allDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            ddlBranchTo.DataSource = allDetails.Tables[1];
            ddlBranchTo.DataValueField = "branch_id";
            ddlBranchTo.DataTextField = "branch_description";
            ddlBranchTo.DataBind();

            ccmTechnician.DataSource = allDetails.Tables[3];
            ccmTechnician.ValueField = "ID";
            ccmTechnician.TextField = "Technician_Name";
            ccmTechnician.DataBind();

            //DateTime startDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            //DateTime LastDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            //dtTDate.MaxDate = LastDate;
            //dtTDate.MinDate = startDate;

            //if (DateTime.Now > LastDate)
            //    dtTDate.Date = LastDate;
            //else
            //    dtTDate.Date = DateTime.Now;

            if (Session["schemavalWHST"] != null)
            {
                List<string> SaveNewValues = (Session["schemavalWHST"]) as List<string>;
                CmbScheme.Value = SaveNewValues[0];
                CmbScheme.Text = SaveNewValues[1];
                ddlBranch.SelectedValue = SaveNewValues[2];
                ddlBranch.SelectedItem.Text = SaveNewValues[3];
                ddlBranch.Enabled = false;
                dtTDate.Date = Convert.ToDateTime(SaveNewValues[4]);

                DataTable warehouse = blLayer.PopulateWHINADDNEW(ddlBranch.SelectedValue);
                if (warehouse.Rows.Count > 0)
                {
                    cmbSourceWarehouse.DataSource = warehouse;
                    cmbSourceWarehouse.ValueField = "WarehouseID";
                    cmbSourceWarehouse.TextField = "WarehouseName";
                    cmbSourceWarehouse.DataBind();

                    cmbDestWarehouse.DataSource = warehouse;
                    cmbDestWarehouse.ValueField = "WarehouseID";
                    cmbDestWarehouse.TextField = "WarehouseName";
                    cmbDestWarehouse.DataBind();
                }



                DateTime startDate = Convert.ToDateTime(CmbScheme.Value.ToString().Split('~')[3]);
                DateTime LastDate = Convert.ToDateTime(CmbScheme.Value.ToString().Split('~')[4]);
                dtTDate.MaxDate = LastDate;
                dtTDate.MinDate = startDate;

                if (DateTime.Now > LastDate)
                    dtTDate.Date = LastDate;
                else
                    dtTDate.Date = DateTime.Now;

            }
            else
            {
                DateTime startDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
                DateTime LastDate = Convert.ToDateTime(allDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
                dtTDate.MaxDate = LastDate;
                dtTDate.MinDate = startDate;

                if (DateTime.Now > LastDate)
                    dtTDate.Date = LastDate;
                else
                    dtTDate.Date = DateTime.Now;
            }
            if (Convert.ToString(Session["SaveModeWHST"]) == "A")
            {
                dtTDate.Focus();
                txtVoucherNo.ClientEnabled = false;
                txtVoucherNo.Text = "Auto";
            }
            else if (Convert.ToString(Session["SaveModeWHST"]) == "M")
            {
                txtVoucherNo.ClientEnabled = true;
                txtVoucherNo.Text = "";
                txtVoucherNo.Focus();

            }
        }

        public void EditModeExecute(string id)
        {
            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
            //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            //DataSet EditedDataDetails = blLayer.GetEditedWHSTData(id, multiwarehouse);
            DataSet EditedDataDetails = blLayer.GetEditedWHSTData_New(id, multiwarehouse);
            //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            
            CmbScheme.DataSource = EditedDataDetails.Tables[0];
            CmbScheme.ValueField = "Id";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();

            ddlBranch.DataSource = EditedDataDetails.Tables[1];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            ddlBranchTo.DataSource = EditedDataDetails.Tables[1];
            ddlBranchTo.DataValueField = "branch_id";
            ddlBranchTo.DataTextField = "branch_description";
            ddlBranchTo.DataBind();

            ccmTechnician.DataSource = EditedDataDetails.Tables[6];
            ccmTechnician.ValueField = "ID";
            ccmTechnician.TextField = "Technician_Name";
            ccmTechnician.DataBind();

            DateTime startDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_StartDate"]);
            DateTime LastDate = Convert.ToDateTime(EditedDataDetails.Tables[2].Rows[0]["FinYear_EndDate"]);
            dtTDate.MaxDate = LastDate;
            dtTDate.MinDate = startDate;

            DataRow HeaderRow = EditedDataDetails.Tables[3].Rows[0];
            CmbScheme.Text = Convert.ToString(HeaderRow["SchemaName"]);
            CmbScheme.ClientEnabled = false;
            txtVoucherNo.Text = Convert.ToString(HeaderRow["StockTransfer_No"]);
            dtTDate.Date = Convert.ToDateTime(HeaderRow["Stock_Date"]);
            dtTDate.ClientEnabled = false;
            ddlBranch.SelectedValue = Convert.ToString(HeaderRow["BranchFrom"]);
            ddlBranch.Enabled = false;

            ddlBranchTo.SelectedValue = Convert.ToString(HeaderRow["BranchTo"]);
            ddlBranch.Enabled = false;

            ccmTechnician.Value = Convert.ToString(HeaderRow["Technician_Id"]);
            
            txtTransportationMode.Text = Convert.ToString(HeaderRow["TransportationMode"]);
            txtVehicleNo.Text = Convert.ToString(HeaderRow["VehicleNo"]);
            txRemarks.Text = Convert.ToString(HeaderRow["Remarks"]);
            //Rev 1.0
            chkNotInProfReport.Checked = Convert.ToBoolean(HeaderRow["NotInProfReport"]);
            // End of Rev 1.0
            
            //ASPxLabel12.Text=EditedDataDetails 
            var sum_tot_stockout = EditedDataDetails.Tables[4].AsEnumerable().Sum(x => x.Field<decimal>("TransferQuantity"));
            var sum_tot_altstockout = EditedDataDetails.Tables[4].AsEnumerable().Sum(x => x.Field<decimal>("PackingQty"));
            var sum_tot_Valuestockout = EditedDataDetails.Tables[4].AsEnumerable().Sum(x => x.Field<decimal>("Value"));

            var sum_tot_stockin = EditedDataDetails.Tables[5].AsEnumerable().Sum(x => x.Field<decimal>("DestQuantity"));
            var sum_tot_altstockin = EditedDataDetails.Tables[5].AsEnumerable().Sum(x => x.Field<decimal>("DestPackingQty"));
            var sum_tot_destvaluestockin = EditedDataDetails.Tables[5].AsEnumerable().Sum(x => x.Field<decimal>("DestValue"));



            DataTable dtt = GetProjectEditData(id);
            if (dtt != null && dtt.Rows.Count > 0)
            {
                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                if (dt2.Rows.Count > 0)
                {
                    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                }

            }


            
            LblTotalQty.Text = sum_tot_stockout.ToString();
            hdnStockOutTotalQty.Value = sum_tot_stockout.ToString();

            LblAltTotalQty.Text = sum_tot_altstockout.ToString();
            hdnStockOutAltTotalQty.Value = sum_tot_altstockout.ToString();

            bnrLblTotalValue.Text = sum_tot_Valuestockout.ToString();

            LblDestTotalQty.Text = sum_tot_stockin.ToString();
            hdnStockInTotalQty.Value = sum_tot_stockin.ToString();

            LblDestAltTotalQty.Text = sum_tot_altstockin.ToString();
            hdnStockInAltTotalQty.Value = sum_tot_altstockin.ToString();

            bnrLblDestTotalValue.Text = sum_tot_destvaluestockin.ToString();


            TempTable = EditedDataDetails.Tables[4];
            HiddenRowCount.Value = TempTable.Rows.Count.ToString();
            Session["TempTableSWHJ"] = EditedDataDetails.Tables[4];
            Session["AdjustWHSTDetailTable"] = EditedDataDetails.Tables[4];
            grid.DataBind();


            TempAdjustmentTableDWH = EditedDataDetails.Tables[5];
            DestHiddenRowCount.Value = TempAdjustmentTableDWH.Rows.Count.ToString();
            Session["AdjustDestWHSTDetailTable"] = EditedDataDetails.Tables[5];
            gridDEstination.DataBind();
           

        }

        public DataTable GetProjectEditData(string DocId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@DocId", 200, DocId);
            proc.AddVarcharPara("@DocTYPE", 200, "WH_Stock_Journal");
            dt = proc.GetTable();
            return dt;
        }
        #region Set session For Packing Quantity
        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }
        [WebMethod]
        public static string SetSessionPackingDWH(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            HttpContext.Current.Session["SessionPackingDetailsDWH"] = finalResult;
            return null;

        }
        #endregion

        public DataTable GetWHSJWarehouseData()
        {
            try
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
                proc.AddVarcharPara("@Action", 500, "WHSJSourceWarehouse");
                proc.AddVarcharPara("@AdjId", 500, Convert.ToString(Request.QueryString["Key"]));
                proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["WHSTWarehouse_Id"]);

                    //if (strNewVal == strOldVal)
                    //{
                    //    drr["WarehouseName"] = "";
                    //    drr["Quantity"] = "";
                    //    drr["BatchNo"] = "";
                    //    drr["SalesUOMName"] = "";
                    //    drr["SalesQuantity"] = "";
                    //    drr["StkUOMName"] = "";
                    //    drr["MfgDate"] = "";
                    //    drr["ExpiryDate"] = "";
                    //}

                    strOldVal = strNewVal;
                }

              //  Session["LoopSalesOrderWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("WHSTWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetWHSJWarehouseDataDistination()
        {
            try
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
                proc.AddVarcharPara("@Action", 500, "WHSJDestinationWarehouseWHSJ");
                //proc.AddVarcharPara("@Action", 500, "WHSJDestinationWarehouse");
                proc.AddVarcharPara("@AdjId", 500, Convert.ToString(Request.QueryString["Key"]));
                proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                //foreach (DataRow drr in tempdt.Rows)
                //{
                //    strNewVal = Convert.ToString(drr["WHSTWarehouse_Id"]);

                //    if (strNewVal == strOldVal)
                //    {
                //        drr["WarehouseName"] = "";
                //        drr["Quantity"] = "";
                //        drr["BatchNo"] = "";
                //        drr["SalesUOMName"] = "";
                //        drr["SalesQuantity"] = "";
                //        drr["StkUOMName"] = "";
                //        drr["MfgDate"] = "";
                //        drr["ExpiryDate"] = "";
                //    }

                //    strOldVal = strNewVal;
                //}

                //Session["LoopSalesOrderWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
               // tempdt.Columns.Remove("WHSTWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }

        #region Get Available Stock 
        [WebMethod]
        public static string GetStockInHand(string ProductId, string WarehouseID, string BranchId,DateTime stkdate)
        {
            WarehousewiseStockTransfer blLayer = new WarehousewiseStockTransfer();
            string StockIN = "0.00", StockUOM = "", strRate = "";
            DataTable AllStock = blLayer.PopulateStock(ProductId, WarehouseID, BranchId);
            if (AllStock.Rows.Count > 0)
            {
                StockIN = Convert.ToString(AllStock.Rows[0]["StockInHand"]);
                StockUOM = Convert.ToString(AllStock.Rows[0]["StockUOMName"]);
                // strRate = Convert.ToString(AllStock.Rows[0]["Rate"]);
            }

            DataTable DailyStock = blLayer.PopulateDailyStock(Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyyy-MM-dd"), stkdate.Date.ToString("yyyy-MM-dd"), BranchId, ProductId, "", "P");

            string stk_dailyqty = "0.00", stk_dailyAltqty = "0.00";
            

            var sum_op1 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY_OP"));
            var sum_op2 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY"));
            var Sum_out = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OUT_QTY"));
            var daily_qty = (sum_op1 + sum_op2) - Sum_out;
            stk_dailyqty = daily_qty.ToString();

            var sum_Altqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OP_ALTQTY"));
            var sum_ReAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("RE_ALTQTY"));
            var sum_DelAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("DEL_ALTQTY"));
            var daily_alt_qty = (sum_Altqty + sum_ReAltqty) - sum_DelAltqty;
            stk_dailyAltqty = daily_alt_qty.ToString();

            return StockIN + "~" + StockUOM + "~" + strRate + "~" + stk_dailyqty + "~" + stk_dailyAltqty;
        }
       
        #endregion

        //Rev Rajdip
        #region Get Available Stock
        [WebMethod]
       // public static string GetStockInHandForWarehouseWiseStockJournal(string ProductId, string WarehouseID, string BranchId, DateTime stkdate)
        public static string GetStockInHandForWarehouseWiseStockJournal(string ProductId, string WarehouseID, string BranchId, string fromBranch, string ToBranch, string Fromdate, DateTime stkdate)
        {
            WarehousewiseStockTransfer blLayer = new WarehousewiseStockTransfer();
            string StockIN = "0.00", StockUOM = "", strRate = "";
            DataTable AllStock = blLayer.PopulateStockforWarehousewiseStockTransffer(ProductId, WarehouseID,BranchId,fromBranch,ToBranch,Fromdate);
            if (AllStock.Rows.Count > 0)
            {
                StockIN = Convert.ToString(AllStock.Rows[0]["StockInHand"]);
                StockUOM = Convert.ToString(AllStock.Rows[0]["StockUOMName"]);
                // strRate = Convert.ToString(AllStock.Rows[0]["Rate"]);
            }

            DataTable DailyStock = blLayer.PopulateDailyStock(Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyyy-MM-dd"), stkdate.Date.ToString("yyyy-MM-dd"), BranchId, ProductId, "", "P");

            string stk_dailyqty = "0.00", stk_dailyAltqty = "0.00";


            var sum_op1 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY_OP"));
            var sum_op2 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY"));
            var Sum_out = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OUT_QTY"));
            var daily_qty = (sum_op1 + sum_op2) - Sum_out;
            stk_dailyqty = daily_qty.ToString();

            var sum_Altqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OP_ALTQTY"));
            var sum_ReAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("RE_ALTQTY"));
            var sum_DelAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("DEL_ALTQTY"));
            var daily_alt_qty = (sum_Altqty + sum_ReAltqty) - sum_DelAltqty;
            stk_dailyAltqty = daily_alt_qty.ToString();

            return StockIN + "~" + StockUOM + "~" + strRate + "~" + stk_dailyqty + "~" + stk_dailyAltqty;
        }

        #endregion

        //End Rev Rajdip
        #region Daily Stock Quantity
        [WebMethod]
        public static string GetDailyStock(string ProductId,string BranchId, DateTime stkdate)
        {
            string stk_dailyqty = "0.00", stk_dailyAltqty = "0.00";
            WarehousewiseStockTransfer blLayer = new WarehousewiseStockTransfer();
            DataTable DailyStock = blLayer.PopulateDailyStock(Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyyy-MM-dd"), stkdate.Date.ToString("yyyy-MM-dd"), BranchId, ProductId, "", "P");

            string stk_dailyqty1 = "0.00", stk_dailyAltqty1 = "0.00";


            var sum_op1 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY_OP"));
            var sum_op2 = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("IN_QTY"));
            var Sum_out = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OUT_QTY"));
            var daily_qty = (sum_op1 + sum_op2) - Sum_out;
            stk_dailyqty1 = daily_qty.ToString();

            var sum_Altqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("OP_ALTQTY"));
            var sum_ReAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("RE_ALTQTY"));
            var sum_DelAltqty = DailyStock.AsEnumerable().Sum(dr => dr.Field<decimal?>("DEL_ALTQTY"));
            var daily_alt_qty = (sum_Altqty + sum_ReAltqty) - sum_DelAltqty;
            stk_dailyAltqty1 = daily_alt_qty.ToString();

            return stk_dailyqty1 + "~" + stk_dailyAltqty1;
        }
        #endregion

        #region Get Negative Stock For Product

        [WebMethod]
        public static string GetNegativeStockByProductID(string ProductId)
        {
            WarehousewiseStockTransfer blLayer = new WarehousewiseStockTransfer();
            string NegativeStockMsg = "";
            DataTable AllStock = blLayer.GetNegativeStock(ProductId);
            if (AllStock.Rows.Count > 0)
            {
                NegativeStockMsg = Convert.ToString(AllStock.Rows[0]["sProduct_NegativeStock"]);

            }
            return NegativeStockMsg;
        }
        #endregion

        #region Stock Valuation
        [WebMethod]
        public static String GetStockValuation(string ProductId)
        {

            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetProductFifoValuation(Convert.ToInt32(ProductId));
            string Stock_Valuation = "";
            if (dt.Rows.Count > 0)
            {
                Stock_Valuation = Convert.ToString(dt.Rows[0]["Stockvaluation"]);
            }

            return Stock_Valuation;
        }
        [WebMethod]
        public static String GetStockValuationAmount(string Pro_Id, string Qty, string Valuationsign, string Fromdate, string BranchId)
        {

            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetValueForProductFifoValuation(Convert.ToInt32(Pro_Id),
                                                Convert.ToDecimal(Qty), Valuationsign, Fromdate,
                                                Fromdate, BranchId);
            string ValuationAmount = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ValuationAmount = Convert.ToString(dt.Rows[0]["VALUE"]);
                }
            }

            return ValuationAmount;
        }
        
         #endregion

        #region Grid For Source Warehouse

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
           /* if (e.Column.FieldName == "TransferQuantity")
            {
                e.Editor.ReadOnly = false;
            }           
            else if (e.Column.FieldName == "Rate")*/
            if (e.Column.FieldName == "Rate")
            /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "Remarks")
            {
                e.Editor.ReadOnly = false;
            }
            
            else if (e.Column.FieldName == "Value")
            {
                e.Editor.ReadOnly = false;
            }
            /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            /*else if (e.Column.FieldName == "TransferQuantity")
            {
                e.Editor.ReadOnly = false;
            }*/
            /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            else if (e.Column.FieldName == "Source_AltQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Source_AltUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection_Source.Value == "1" && e.Column.FieldName == "TransferQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "AvlStkSourceWH")
            {
                e.Editor.Enabled = true;
            }
            /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/

            else
            {
                /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                //e.Editor.Enabled = true;
                e.Editor.ReadOnly = false;
                /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            }

        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = TempTable;
            grid.DataSource = (DataTable)Session["TempTableSWHJ"];
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();
            //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
            DataTable dtTemp = new DataTable();
            if (Session["AdjustWHSTDetailTable"] != null)
            {
                //subhabrata: foreach@deleted extra column 

                DataTable dt = new DataTable();
                dtTemp = (DataTable)Session["AdjustWHSTDetailTable"];
                dt = dtTemp.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;
                    if (dtC.Contains("StockJournalDetails_ID"))
                    {
                        dt.Columns.Remove("StockJournalDetails_ID");
                    }
                    if (dtC.Contains("PackingUOM"))
                    {
                        dt.Columns.Remove("PackingUOM");
                    }
                    if (dtC.Contains("UOM_Name"))
                    {
                        dt.Columns.Remove("UOM_Name");
                    }
                    break;
                }
                AdjustmentTable = dt;
            }
            else
            {
                //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
                AdjustmentTable.Columns.Add("SrlNo", typeof(string));
                AdjustmentTable.Columns.Add("ProductName", typeof(string));
                AdjustmentTable.Columns.Add("Discription", typeof(string));
                AdjustmentTable.Columns.Add("TransferQuantity", typeof(decimal));
                AdjustmentTable.Columns.Add("AvlStkSourceWH", typeof(decimal));
                //AdjustmentTable.Columns.Add("AvlStkDestWH", typeof(decimal));
                AdjustmentTable.Columns.Add("Rate", typeof(decimal));
                AdjustmentTable.Columns.Add("Value", typeof(decimal));
                AdjustmentTable.Columns.Add("ProductID", typeof(string));
                AdjustmentTable.Columns.Add("ActualSL", typeof(string));
                AdjustmentTable.Columns.Add("SourceWarehouse", typeof(string));
                AdjustmentTable.Columns.Add("SourceWarehouseID", typeof(string));
                //AdjustmentTable.Columns.Add("DestinationWarehouse", typeof(string));
                //AdjustmentTable.Columns.Add("DestinationWarehouseID", typeof(string));
                AdjustmentTable.Columns.Add("SaleUOM", typeof(string));
                AdjustmentTable.Columns.Add("Remarks", typeof(string));
                AdjustmentTable.Columns.Add("PackingQty", typeof(decimal));

                //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                AdjustmentTable.Columns.Add("Source_AltQuantity", typeof(string));
                AdjustmentTable.Columns.Add("Source_AltUOM", typeof(string));
                //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
            }
            //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductName"])))
                {
                    string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                    //string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    //string ProductID = ProductDetailsList[0];

                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["ProductName"] = Convert.ToString(args.NewValues["ProductName"]);
                    NewRow["Discription"] = Convert.ToString(args.NewValues["Discription"]);
                    NewRow["TransferQuantity"] = Convert.ToDecimal(args.NewValues["TransferQuantity"]);
                    NewRow["AvlStkSourceWH"] = Convert.ToDecimal(args.NewValues["AvlStkSourceWH"]);
                   // NewRow["AvlStkDestWH"] = Convert.ToDecimal(args.NewValues["AvlStkDestWH"]);
                    NewRow["Rate"] = Convert.ToDecimal(args.NewValues["Rate"]);
                    NewRow["Value"] = Convert.ToDecimal(args.NewValues["Value"]);
                    NewRow["ProductID"] = ProductDetails;
                    NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                    NewRow["SourceWarehouse"] = Convert.ToString(args.NewValues["SourceWarehouse"]);
                    NewRow["SourceWarehouseID"] = Convert.ToString(args.NewValues["SourceWarehouseID"]);
                    //NewRow["DestinationWarehouse"] = Convert.ToString(args.NewValues["DestinationWarehouse"]);
                    //NewRow["DestinationWarehouseID"] = Convert.ToString(args.NewValues["DestinationWarehouseID"]);
                    NewRow["SaleUOM"] = Convert.ToString(args.NewValues["SaleUOM"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["PackingQty"] = Convert.ToDecimal(args.NewValues["PackingQty"]);
                    //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    NewRow["Source_AltQuantity"] = Convert.ToString(args.NewValues["Source_AltQuantity"]);
                    NewRow["Source_AltUOM"] = Convert.ToString(args.NewValues["Source_AltUOM"]);
                    //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string StockJournal_ID = Convert.ToString(args.Keys[0]);
                string ProductDetailsNew = Convert.ToString(args.NewValues["ProductName"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(args.Keys[0]);
                    if (DeleteID == StockJournal_ID)
                    {
                        isDeleted = true;
                        break;
                    }
                }
                if (isDeleted == false)
                {
                    //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error. 
                    if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductName"])))
                    {
                        string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                        //string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        //string ProductID = ProductDetailsList[0];

                        NewRow = AdjustmentTable.NewRow();
                        NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                        NewRow["ProductName"] = Convert.ToString(args.NewValues["ProductName"]);
                        NewRow["Discription"] = Convert.ToString(args.NewValues["Discription"]);
                        NewRow["TransferQuantity"] = Convert.ToDecimal(args.NewValues["TransferQuantity"]);
                        NewRow["AvlStkSourceWH"] = Convert.ToDecimal(args.NewValues["AvlStkSourceWH"]);
                        //  NewRow["AvlStkDestWH"] = Convert.ToDecimal(args.NewValues["AvlStkDestWH"]);
                        NewRow["Rate"] = Convert.ToDecimal(args.NewValues["Rate"]);
                        NewRow["Value"] = Convert.ToDecimal(args.NewValues["Value"]);
                        NewRow["ProductID"] = ProductDetails;
                        NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                        NewRow["SourceWarehouse"] = Convert.ToString(args.NewValues["SourceWarehouse"]);
                        NewRow["SourceWarehouseID"] = Convert.ToString(args.NewValues["SourceWarehouseID"]);
                        //NewRow["DestinationWarehouse"] = Convert.ToString(args.NewValues["DestinationWarehouse"]);
                        //NewRow["DestinationWarehouseID"] = Convert.ToString(args.NewValues["DestinationWarehouseID"]);
                        NewRow["SaleUOM"] = Convert.ToString(args.NewValues["SaleUOM"]);
                        NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                        NewRow["PackingQty"] = Convert.ToDecimal(args.NewValues["PackingQty"]);
                        //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                        NewRow["Source_AltQuantity"] = Convert.ToString(args.NewValues["Source_AltQuantity"]);
                        NewRow["Source_AltUOM"] = Convert.ToString(args.NewValues["Source_AltUOM"]);
                        //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                        //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error. 
                        bool Isexists = false;
                        foreach (DataRow drr in AdjustmentTable.Rows)
                        {
                            //string OldStockJournal_ID = Convert.ToString(args.Keys[0]);
                            string OldStockJournal_ID = Convert.ToString(drr["SrlNo"]);
                            if (OldStockJournal_ID == StockJournal_ID)
                            {
                                Isexists = true;
                                drr["SrlNo"] = NewRow["SrlNo"].ToString();
                                drr["ProductName"] = NewRow["ProductName"].ToString();
                                drr["Discription"] = NewRow["Discription"].ToString();
                                drr["TransferQuantity"] = NewRow["TransferQuantity"].ToString();
                                drr["AvlStkSourceWH"] = NewRow["AvlStkSourceWH"].ToString();
                                drr["Rate"] = NewRow["Rate"].ToString();
                                drr["Value"] = NewRow["Value"];
                                drr["ProductID"] = NewRow["ProductID"].ToString();
                                drr["ActualSL"] = NewRow["ActualSL"].ToString();
                                drr["SourceWarehouse"] = NewRow["SourceWarehouse"].ToString();
                                drr["SourceWarehouseID"] = NewRow["SourceWarehouseID"].ToString();
                                drr["SaleUOM"] = NewRow["SaleUOM"].ToString();
                                drr["Remarks"] = NewRow["Remarks"].ToString();
                                drr["PackingQty"] = NewRow["PackingQty"].ToString();

                                drr["Source_AltQuantity"] = NewRow["Source_AltQuantity"].ToString();
                                drr["Source_AltUOM"] = NewRow["Source_AltUOM"].ToString();
                                break;
                            }
                        } 
                        //AdjustmentTable.Rows.Add(NewRow);
                        if (Isexists == false)
                        {
                            AdjustmentTable.Rows.Add(NewRow);
                        }
                        //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
                    }
                }
            }


            foreach (var args in e.DeleteValues)
            {
                //DataTable AdjTable = (DataTable)Session["AdjustWHSTDetailTable"];
                DataTable AdjTable = AdjustmentTable;
                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjustmentTable.Select("ActualSL='" + delId + "'");
                    //DataRow[] delRow = AdjustmentTable.Select("DocumentId='" + AdjTableRow[0]["DocumentId"] + "' and DocumentType='" + AdjTableRow[0]["DocumentType"] + "'");
                    foreach (DataRow dr in AdjTableRow)
                        dr.Delete();

                    AdjustmentTable.AcceptChanges();
                }
            }

            TempTable = AdjustmentTable.Copy();           

            int conut = 1;
            foreach (DataRow dr in TempTable.Rows)
            {
                dr["ActualSL"] = conut;
                conut++;
            }

            Session["TempTableSWHJ"] = TempTable.Copy();

            AdjustmentTable.Columns.Remove("ActualSL");
            AdjustmentTable.Columns.Remove("SourceWarehouse");
            //AdjustmentTable.Columns.Remove("DestinationWarehouse");
            AdjustmentTable.Columns.Remove("SaleUOM");
            AdjustmentTable.AcceptChanges();
            RefetchSrlNo();

            // Datattable of Warehouse
            DataTable tempWarehousedt = new DataTable();
            if (Session["WHSJSour_WarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "Quantity", "BatchID", "SerialID", "MfgDate", "ExpiryDate");
            }
            else
            {
                tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                tempWarehousedt.Columns.Add("BatchID", typeof(string));
                tempWarehousedt.Columns.Add("SerialID", typeof(string));
                //Rev 3.0
                tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                //Rev 3.0 End
            }

            //End
            string WHMandatory = "YES", validate = "";

            foreach (DataRow dr in AdjustmentTable.Rows)
            {
                string ProductDetails = Convert.ToString(dr["ProductID"]);
                string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                string ProductID = ProductDetailsList[0];

                string SrlNo = Convert.ToString(dr["SrlNo"]);
                DataTable sProduct = oDBEngine.GetDataTable("Master_sProducts", "isnull(Is_active_Batch,0) Is_active_Batch,isnull(Is_active_serialno,0) Is_active_serialno", "sProducts_ID=" + ProductID);
               
                int Is_active_Batch = Convert.ToInt32(sProduct.Rows[0]["Is_active_Batch"]);
                int Is_active_serialno = Convert.ToInt32(sProduct.Rows[0]["Is_active_serialno"]);
                if (Is_active_Batch == 1 || Is_active_serialno == 1)
                {
                    foreach (DataRow WHdr in tempWarehousedt.Rows)
                    {
                        string WHSrlNo = Convert.ToString(WHdr["Product_SrlNo"]);
                        if (SrlNo == WHSrlNo)
                        {
                            WHMandatory = "NO";
                        }
                    }
                    //-----Rajdip
                    if (WHMandatory == "YES")
                    {
                        validate = "checkWarehouse";
                        grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                        break;
                    }
                    //----end rev rajdip
                    string IsInventory = getProductIsInventoryExists(Convert.ToString(ProductID));
                    if (Convert.ToString(dr["ProductID"]) != "0")
                    {
                        if (IsInventory.ToUpper() != "N")
                        {
                            string strSrlNo = Convert.ToString(dr["SrlNo"]);
                            decimal strProductQuantity = Convert.ToDecimal(dr["TransferQuantity"]);
                            decimal strWarehouseQuantity = 0;
                            GetQuantityBaseOnProduct(tempWarehousedt, strSrlNo, ref strWarehouseQuantity);                            
                            if (strProductQuantity != strWarehouseQuantity)
                            {                                
                                    validate = "checkWarehouseQty";
                                    grid.JSProperties["cpProductSrlIDCheck1"] = strSrlNo;
                                    break;
                               
                            }                            
                        }
                    }

                }
            }

            DataView dvData = new DataView(AdjustmentTable);
            //dvData.RowFilter = "Status<>'D'";
            DataTable dt_tempAdjust = dvData.ToTable();
            //--Rev Rajdip
           // var duplicateRecords = dt_tempAdjust.AsEnumerable()
           //.GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
           //.Where(gr => gr.Count() > 1)
           // .Select(g => g.Key);
            //End rev Rajdip
            //Revision Rajdip
            //var duplicateRecords = dt_tempAdjust.AsEnumerable()
            // .GroupBy(r => r["ProductID"], r => r["SourceWarehouseID"])
            //    //coloumn name which has the duplicate values
            //   .Where(gr => gr.Count() > 1)
            //  .Select(g => g.Key);
            ////--Rajdip
            //foreach (var d in duplicateRecords)
            //{
            //    validate = "duplicateProduct";
            //}
            //End Revision Rajdip
         

            #region Add New Filed To Check from Table

            DataTable PackingDT = new DataTable();
            PackingDT.Columns.Add("productid", typeof(Int64));
            PackingDT.Columns.Add("slno", typeof(Int32));
            PackingDT.Columns.Add("Quantity", typeof(Decimal));
            PackingDT.Columns.Add("packing", typeof(Decimal));
            PackingDT.Columns.Add("PackingUom", typeof(Int32));
            PackingDT.Columns.Add("PackingSelectUom", typeof(Int32));

            
            #endregion


            if (validate == "checkWarehouse" || validate == "duplicateProduct" || validate == "checkWarehouseQty")
            {
                grid.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else
            {
                if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                {
                    List<ProductQuantity> obj = new List<ProductQuantity>();
                    obj = (List<ProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                    foreach (var item in obj)
                    {
                        PackingDT.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                    }
                }
                HttpContext.Current.Session["SessionPackingDetails"] = PackingDT.Copy();


                foreach (DataRow d in AdjustmentTable.Rows)
                {
                    string ProductDetails = Convert.ToString(d["ProductID"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    d["ProductID"] = ProductID;
                }
                AdjustmentTable.AcceptChanges();

                Session["AdjustmentTableSWH"] = AdjustmentTable.Copy();

                Session["TempSourceWarehousedt"] = tempWarehousedt.Copy();

               // blLayer.AddEditWarehouseStockTransfer(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
               //ddlBranch.SelectedValue
               //, Convert.ToString(Session["userid"]), ref adjustmentId,
               //ref adjustmentNumber, AdjustmentTable, tempWarehousedt, ref ErrorCode, Request.QueryString["Key"], PackingDT, txtTransportationMode.Text.Trim(), txtVehicleNo.Text.Trim(), txRemarks.Text.Trim());

               // grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
               // grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
               // grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
               // e.Handled = true;

                //#region To Show By Default Cursor after SAVE AND NEW
                //if (hdAddEdit.Value == "Add")
                //{
                //    if (HiddenSaveButton.Value != "E")
                //    {
                //        string schemavalue = CmbScheme.Value.ToString();
                //        string NumberingScheme = CmbScheme.Text;
                //        string BranchID = ddlBranch.SelectedValue;
                //        string BranchName = ddlBranch.SelectedItem.Text;
                //        string strDate = dtTDate.Date.ToString("yyyy-MM-dd");
                //        List<string> AfterAdd = new List<string> { schemavalue, NumberingScheme, BranchID, BranchName, strDate };

                //        Session["schemavalWHST"] = AfterAdd;

                //        string schematype = txtVoucherNo.Text;
                //        if (schematype == "Auto")
                //        {
                //            Session["SaveModeWHST"] = "A";
                //        }
                //        else
                //        {
                //            Session["SaveModeWHST"] = "M";
                //        }
                //    }

                //}


                //#endregion
            }


        }
        public void GetQuantityBaseOnProduct(DataTable tempWarehousedt,string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            //DataTable Warehousedt = new DataTable();
            if (tempWarehousedt != null)
            {
               // Warehousedt = (DataTable)Session["SC_WarehouseData"];
                for (int i = 0; i < tempWarehousedt.Rows.Count; i++)
                {
                    DataRow dr = tempWarehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public string getProductIsInventoryExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }
        private void RefetchSrlNo()
        {
            //TempTable.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in TempTable.Rows)
            {
                dr["SrlNo"] = conut;

                conut++;
            }
        }
        protected void grid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
        }
        #endregion

        #region Grid For Destination Warehouse

        protected void gridDEstination_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
           // Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
           /* if (e.Column.FieldName == "DestQuantity")
            {
                e.Editor.ReadOnly = false;
            }            
            else if (e.Column.FieldName == "DestRate")*/
            if (e.Column.FieldName == "DestRate")
            // Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "DestRemarks")
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "DestValue")
            {
                e.Editor.ReadOnly = false;
            }
            /* Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
           /* else if (e.Column.FieldName == "DestQuantity")
            {
                e.Editor.ReadOnly = false;
            }*/            
            else if (e.Column.FieldName == "Dest_AltQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Dest_AltUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "DestQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "AvlStkDestWH")
            {
                e.Editor.Enabled = true;
            }
            /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            else
            {
                /* Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                //e.Editor.Enabled = true;
                e.Editor.ReadOnly = false;
                /* Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            }

        }

        protected void gridDEstination_DataBinding(object sender, EventArgs e)
        {
            gridDEstination.DataSource = TempAdjustmentTableDWH;
        }
        protected void gridDEstination_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridDEstination_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridDEstination_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridDEstination_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();
            //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
            DataTable dtTemp = new DataTable();
            if (Session["AdjustDestWHSTDetailTable"] != null)
            {
                //subhabrata: foreach@deleted extra column 

                DataTable dt = new DataTable();
                dtTemp = (DataTable)Session["AdjustDestWHSTDetailTable"];
                dt = dtTemp.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;
                    if (dtC.Contains("StockJournalDetails_ID"))
                    {
                        dt.Columns.Remove("StockJournalDetails_ID"); 
                    }
                    if (dtC.Contains("DestPackingUOM"))
                    {
                        dt.Columns.Remove("DestPackingUOM");
                    }
                    if (dtC.Contains("UOM_Name"))
                    {
                        dt.Columns.Remove("UOM_Name");
                    }
                    break;
                }//End

                //SalesOrderdt = (DataTable)Session["OrderDetails"];

                AdjustmentTable = dt;
            }
            else
            {
                //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                AdjustmentTable.Columns.Add("SrlNo", typeof(string));
                AdjustmentTable.Columns.Add("ProductNameDest", typeof(string));
                AdjustmentTable.Columns.Add("DestDiscription", typeof(string));
                AdjustmentTable.Columns.Add("DestQuantity", typeof(decimal));
                // AdjustmentTable.Columns.Add("AvlStkSourceWH", typeof(decimal));
                AdjustmentTable.Columns.Add("AvlStkDestWH", typeof(decimal));
                AdjustmentTable.Columns.Add("DestRate", typeof(decimal));
                AdjustmentTable.Columns.Add("DestValue", typeof(decimal));
                AdjustmentTable.Columns.Add("DestProductID", typeof(string));
                AdjustmentTable.Columns.Add("ActualDestSL", typeof(string));
                //AdjustmentTable.Columns.Add("SourceWarehouse", typeof(string));
                //AdjustmentTable.Columns.Add("SourceWarehouseID", typeof(string));
                AdjustmentTable.Columns.Add("DestinationWarehouse", typeof(string));
                AdjustmentTable.Columns.Add("DestinationWarehouseID", typeof(string));
                AdjustmentTable.Columns.Add("DestUOM", typeof(string));
                AdjustmentTable.Columns.Add("DestRemarks", typeof(string));
                AdjustmentTable.Columns.Add("DestPackingQty", typeof(decimal));
                //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                AdjustmentTable.Columns.Add("Dest_AltQuantity", typeof(string));
                AdjustmentTable.Columns.Add("Dest_AltUOM", typeof(string));
            }
            //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductNameDest"])))
                {
                    string ProductDetails = Convert.ToString(args.NewValues["DestProductID"]);
                    //string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    //string ProductID = ProductDetailsList[0];

                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["ProductNameDest"] = Convert.ToString(args.NewValues["ProductNameDest"]);
                    NewRow["DestDiscription"] = Convert.ToString(args.NewValues["DestDiscription"]);
                    NewRow["DestQuantity"] = Convert.ToDecimal(args.NewValues["DestQuantity"]);
                   // NewRow["AvlStkSourceWH"] = Convert.ToDecimal(args.NewValues["AvlStkSourceWH"]);
                    NewRow["AvlStkDestWH"] = Convert.ToDecimal(args.NewValues["AvlStkDestWH"]);
                    NewRow["DestRate"] = Convert.ToDecimal(args.NewValues["DestRate"]);
                    NewRow["DestValue"] = Convert.ToDecimal(args.NewValues["DestValue"]);
                    NewRow["DestProductID"] = ProductDetails;
                    NewRow["ActualDestSL"] = Convert.ToString(args.NewValues["ActualDestSL"]);
                    //NewRow["SourceWarehouse"] = Convert.ToString(args.NewValues["SourceWarehouse"]);
                    //NewRow["SourceWarehouseID"] = Convert.ToString(args.NewValues["SourceWarehouseID"]);
                    NewRow["DestinationWarehouse"] = Convert.ToString(args.NewValues["DestinationWarehouse"]);
                    NewRow["DestinationWarehouseID"] = Convert.ToString(args.NewValues["DestinationWarehouseID"]);
                    NewRow["DestUOM"] = Convert.ToString(args.NewValues["DestUOM"]);
                    NewRow["DestRemarks"] = Convert.ToString(args.NewValues["DestRemarks"]);
                    NewRow["DestPackingQty"] = Convert.ToDecimal(args.NewValues["DestPackingQty"]);
                    //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    NewRow["Dest_AltQuantity"] = Convert.ToString(args.NewValues["Dest_AltQuantity"]); 
                    NewRow["Dest_AltUOM"] =Convert.ToString(args.NewValues["Dest_AltUOM"]);
                    //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }
            foreach (var args in e.UpdateValues)
            {
                //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string StockJournal_ID = Convert.ToString(args.Keys[0]);
                string ProductDetailsNew = Convert.ToString(args.NewValues["DestProductID"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(args.Keys[0]);
                    if (DeleteID == StockJournal_ID)
                    {
                        isDeleted = true;
                        break;
                    }
                }
                if (isDeleted == false)
                {
                    //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error. 
                    if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductNameDest"])))
                    {
                        string ProductDetails = Convert.ToString(args.NewValues["DestProductID"]);
                        //string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        //string ProductID = ProductDetailsList[0];

                        NewRow = AdjustmentTable.NewRow();
                        NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                        NewRow["ProductNameDest"] = Convert.ToString(args.NewValues["ProductNameDest"]);
                        NewRow["DestDiscription"] = Convert.ToString(args.NewValues["DestDiscription"]);
                        NewRow["DestQuantity"] = Convert.ToDecimal(args.NewValues["DestQuantity"]);
                        // NewRow["AvlStkSourceWH"] = Convert.ToDecimal(args.NewValues["AvlStkSourceWH"]);
                        NewRow["AvlStkDestWH"] = Convert.ToDecimal(args.NewValues["AvlStkDestWH"]);
                        NewRow["DestRate"] = Convert.ToDecimal(args.NewValues["DestRate"]);
                        NewRow["DestValue"] = Convert.ToDecimal(args.NewValues["DestValue"]);
                        NewRow["DestProductID"] = ProductDetails;
                        NewRow["ActualDestSL"] = Convert.ToString(args.NewValues["ActualDestSL"]);
                        //NewRow["SourceWarehouse"] = Convert.ToString(args.NewValues["SourceWarehouse"]);
                        //NewRow["SourceWarehouseID"] = Convert.ToString(args.NewValues["SourceWarehouseID"]);
                        NewRow["DestinationWarehouse"] = Convert.ToString(args.NewValues["DestinationWarehouse"]);
                        NewRow["DestinationWarehouseID"] = Convert.ToString(args.NewValues["DestinationWarehouseID"]);
                        NewRow["DestUOM"] = Convert.ToString(args.NewValues["DestUOM"]);
                        NewRow["DestRemarks"] = Convert.ToString(args.NewValues["DestRemarks"]);
                        NewRow["DestPackingQty"] = Convert.ToDecimal(args.NewValues["DestPackingQty"]);
                        //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
                        NewRow["Dest_AltQuantity"] = Convert.ToString(args.NewValues["Dest_AltQuantity"]);
                        NewRow["Dest_AltUOM"] = Convert.ToString(args.NewValues["Dest_AltUOM"]);
                        //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal

                        //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error. 
                        bool Isexists = false;
                        foreach (DataRow drr in AdjustmentTable.Rows)
                        {
                            //string OldStockJournal_ID = Convert.ToString(args.Keys[0]);
                            string OldStockJournal_ID = Convert.ToString(drr["SrlNo"]);
                            if (OldStockJournal_ID == StockJournal_ID)
                            {
                                Isexists = true;
                                drr["SrlNo"] = NewRow["SrlNo"].ToString();
                                drr["ProductNameDest"] = NewRow["ProductNameDest"].ToString();
                                drr["DestDiscription"] = NewRow["DestDiscription"].ToString();
                                drr["DestQuantity"] = NewRow["DestQuantity"].ToString();
                                drr["AvlStkDestWH"] = NewRow["AvlStkDestWH"].ToString();
                                drr["DestRate"] = NewRow["DestRate"].ToString();
                                drr["DestValue"] = NewRow["DestValue"];
                                drr["DestProductID"] = NewRow["DestProductID"].ToString();
                                drr["ActualDestSL"] = NewRow["ActualDestSL"].ToString();
                                drr["DestinationWarehouse"] = NewRow["DestinationWarehouse"].ToString();
                                drr["DestinationWarehouseID"] = NewRow["DestinationWarehouseID"].ToString();
                                drr["DestUOM"] = NewRow["DestUOM"].ToString();
                                drr["DestRemarks"] = NewRow["DestRemarks"].ToString();
                                drr["DestPackingQty"] = NewRow["DestPackingQty"].ToString();

                                drr["Dest_AltQuantity"] = NewRow["Dest_AltQuantity"].ToString();
                                drr["Dest_AltUOM"] = NewRow["Dest_AltUOM"].ToString();                                                            
                                break;
                            }
                        }                        
                        //AdjustmentTable.Rows.Add(NewRow);
                        if (Isexists == false)
                        {
                            AdjustmentTable.Rows.Add(NewRow); 
                        }
                        //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error. 
                    }
                //Rev work start 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
                }
                //Rev work close 25.05.2022 Mantise no:24908: In Modify mode adding new product with existing product in Warehouse Wise Stock Journal geeting error.
            }
            foreach (var args in e.DeleteValues)
            {
               // DataTable AdjTable = (DataTable)Session["AdjustDestWHSTDetailTable"];
                DataTable AdjTable = AdjustmentTable;

                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjustmentTable.Select("ActualDestSL='" + delId + "'");
                    //DataRow[] delRow = AdjustmentTable.Select("DocumentId='" + AdjTableRow[0]["DocumentId"] + "' and DocumentType='" + AdjTableRow[0]["DocumentType"] + "'");
                    foreach (DataRow dr in AdjTableRow)
                        dr.Delete();

                    AdjustmentTable.AcceptChanges();
                }
            }
            
            TempAdjustmentTableDWH = AdjustmentTable.Copy();

            int conut = 1;
            foreach (DataRow dr in TempAdjustmentTableDWH.Rows)
            {
                dr["ActualDestSL"] = conut;
                conut++;
            }
            AdjustmentTable.Columns.Remove("ActualDestSL");
            //AdjustmentTable.Columns.Remove("SourceWarehouse");
            AdjustmentTable.Columns.Remove("DestinationWarehouse");
            AdjustmentTable.Columns.Remove("DestUOM");
            AdjustmentTable.AcceptChanges();
            RefetchSrlNoDest();            
            // Datattable of Warehouse
            DataTable tempWarehousedt = new DataTable();
            if (Session["WHSJDest_WarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "Quantity", "BatchID", "SerialID","MfgDate","ExpiryDate");
            }
            else
            {
                tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                tempWarehousedt.Columns.Add("BatchID", typeof(string));
                tempWarehousedt.Columns.Add("SerialID", typeof(string));
                //Revision Rajdip MfgDate, ExpiryDate
                tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                //Revision Rajdip
            }

            //End
            string WHMandatory = "YES", validate = "";

            foreach (DataRow dr in AdjustmentTable.Rows)
            {
                string ProductDetails = Convert.ToString(dr["DestProductID"]);
                string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                string DestProductID = ProductDetailsList[0];

                string SrlNo = Convert.ToString(dr["SrlNo"]);
                DataTable sProduct = oDBEngine.GetDataTable("Master_sProducts", "Is_active_Batch,Is_active_serialno", "sProducts_ID=" + DestProductID);
                int Is_active_Batch = Convert.ToInt32(sProduct.Rows[0]["Is_active_Batch"]);
                int Is_active_serialno = Convert.ToInt32(sProduct.Rows[0]["Is_active_serialno"]);
                if (Is_active_Batch == 1 || Is_active_serialno == 1)
                {
                    foreach (DataRow WHdr in tempWarehousedt.Rows)
                    {
                        string WHSrlNo = Convert.ToString(WHdr["Product_SrlNo"]);
                        if (SrlNo == WHSrlNo)
                        {
                            WHMandatory = "NO";
                        }
                    }
                    //--Rajdip
                    if (WHMandatory == "YES")
                    {
                        validate = "checkWarehouse";
                        gridDEstination.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                        break;
                    }
                    //--End rev rajdip
                    string IsInventory = getProductIsInventoryExistsDest(Convert.ToString(DestProductID));
                    if (Convert.ToString(dr["DestProductID"]) != "0")
                    {
                        if (IsInventory.ToUpper() != "N")
                        {
                            string strSrlNo = Convert.ToString(dr["SrlNo"]);
                            decimal strProductQuantity = Convert.ToDecimal(dr["DestQuantity"]);

                            decimal strWarehouseQuantity = 0;
                            GetQuantityBaseOnProductDest(tempWarehousedt, strSrlNo, ref strWarehouseQuantity);

                            //if (strWarehouseQuantity != 0)
                            //{
                            if (strProductQuantity != strWarehouseQuantity)
                            {
                                //if (WHMandatory == "YES")
                                //{
                                validate = "checkWarehouseQty";
                                gridDEstination.JSProperties["cpProductSrlIDCheck1"] = strSrlNo;
                                break;
                                //}
                            }
                            //}
                        }
                    }

                }
            }

            DataView dvData = new DataView(AdjustmentTable);
            //dvData.RowFilter = "Status<>'D'";
            DataTable dt_tempAdjust = dvData.ToTable();
            //----Rajdip
           // var duplicateRecords = dt_tempAdjust.AsEnumerable()
           //.GroupBy(r => r["DestProductID"])
           ////coloumn name which has the duplicate values
           //.Where(gr => gr.Count() > 1)
           // .Select(g => g.Key);
            //--End rev Rajdip
            //var duplicateRecords = dt_tempAdjust.AsEnumerable()
            // .GroupBy(r => r["DestProductID"],r=>r["DestinationWarehouseID"])
            //    //coloumn name which has the duplicate values
            // .Where(gr => gr.Count() > 1)
            //    // .Select(g => g["DestinationWarehouseID"]);
            //.Select(grp => new
            //{
            //    Key = grp.Key,
            //    Count = grp.Count()
            //})
            //.ToList();

            //var abc = dt_tempAdjust.AsEnumerable()
            //       .GroupBy(r => new { Col1 = r["DestProductID"], Col2 = r["DestinationWarehouseID"] })
            //       .Select(g => g.OrderBy(r => r["DestProductID"]).First())
            //       .CopyToDataTable();



            //foreach (var d in duplicateRecords)
            //{
            //    validate = "duplicateProduct";
            //}

           



            DataTable PackingDTWSH = (DataTable)Session["SessionPackingDetails"];
            HttpContext.Current.Session["SessionPackingDetails"] = null;

            if (validate == "checkWarehouse" || validate == "duplicateProduct" || validate == "checkWarehouseQty")
            {
                gridDEstination.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else
            {
                #region Add New Filed To Check from Table

                DataTable PackingDT = new DataTable();
                PackingDT.Columns.Add("productid", typeof(Int64));
                PackingDT.Columns.Add("slno", typeof(Int32));
                PackingDT.Columns.Add("Quantity", typeof(Decimal));
                PackingDT.Columns.Add("packing", typeof(Decimal));
                PackingDT.Columns.Add("PackingUom", typeof(Int32));
                PackingDT.Columns.Add("PackingSelectUom", typeof(Int32));

                if (HttpContext.Current.Session["SessionPackingDetailsDWH"] != null)
                {
                    List<ProductQuantity> obj = new List<ProductQuantity>();
                    obj = (List<ProductQuantity>)HttpContext.Current.Session["SessionPackingDetailsDWH"];
                    foreach (var item in obj)
                    {
                        PackingDT.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                    }
                }
                HttpContext.Current.Session["SessionPackingDetailsDWH"] = null;
                #endregion
                foreach (DataRow d in AdjustmentTable.Rows)
                {
                    string ProductDetails = Convert.ToString(d["DestProductID"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string DestProductID = ProductDetailsList[0];
                    d["DestProductID"] = DestProductID;
                }
                AdjustmentTable.AcceptChanges();

                TempAdjustmentTableSWH = (DataTable)Session["AdjustmentTableSWH"];
                HttpContext.Current.Session["AdjustmentTableSWH"] = null;

                DataTable tempSourceWarehousedt = new DataTable();
                tempSourceWarehousedt = (DataTable)Session["TempSourceWarehousedt"];
                HttpContext.Current.Session["TempSourceWarehousedt"] = null;

                //REV 2.0
                foreach (DataRow wtrow in tempWarehousedt.Rows)
                {
                    string strMfgDate = Convert.ToString(wtrow["MfgDate"]).Trim();
                    string strExpiryDate = Convert.ToString(wtrow["ExpiryDate"]).Trim();

                    if (strMfgDate != "")
                    {
                        string DD = strMfgDate.Substring(0, 2);
                        string MM = strMfgDate.Substring(3, 2);
                        string YYYY = strMfgDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["MfgDate"] = Date;
                    }

                    if (strExpiryDate != "")
                    {
                        string DD = strExpiryDate.Substring(0, 2);
                        string MM = strExpiryDate.Substring(3, 2);
                        string YYYY = strExpiryDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["ExpiryDate"] = Date;
                    }
                }
                tempWarehousedt.AcceptChanges();

                foreach (DataRow wtrow in tempSourceWarehousedt.Rows)
                {
                    string strMfgDate = Convert.ToString(wtrow["MfgDate"]).Trim();
                    string strExpiryDate = Convert.ToString(wtrow["ExpiryDate"]).Trim();

                    if (strMfgDate != "")
                    {
                        string DD = strMfgDate.Substring(0, 2);
                        string MM = strMfgDate.Substring(3, 2);
                        string YYYY = strMfgDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["MfgDate"] = Date;
                    }

                    if (strExpiryDate != "")
                    {
                        string DD = strExpiryDate.Substring(0, 2);
                        string MM = strExpiryDate.Substring(3, 2);
                        string YYYY = strExpiryDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["ExpiryDate"] = Date;
                    }
                }
                tempSourceWarehousedt.AcceptChanges();
                //REV 2.0 END



                Int64 Proj_id = 0;
                if (lookup_Project.Text != "")
                {
                    Proj_id = Convert.ToInt64(lookup_Project.Value);
                }
                /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                
                //datatable for MultiUOm start 
                DataTable MultiUOMDetails = new DataTable();
                if (Session["MultiUOMData"] != null)
                {
                    DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                    // Mantis Issue 24397
                    //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                    // End of Mantis Issue 24397
                }
                else
                {
                    MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                    // Mantis Issue 24397
                    MultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24397
                }
                //End

                //datatable for MultiUOm start for Source
                DataTable SourceMultiUOMDetails = new DataTable();
                if (Session["MultiUOMData_Source"] != null)
                {
                    DataTable SourceMultiUOM = (DataTable)Session["MultiUOMData_Source"];
                    // Mantis Issue 24397
                    //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                    SourceMultiUOMDetails = SourceMultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                    // End of Mantis Issue 24397
                }
                else
                {
                    SourceMultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    SourceMultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    SourceMultiUOMDetails.Columns.Add("UOM", typeof(string));
                    SourceMultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    //SourceMultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    SourceMultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    SourceMultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    SourceMultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    SourceMultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                    // Mantis Issue 24397
                    SourceMultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                    SourceMultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    SourceMultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    SourceMultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24397
                }
                //End
                /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                /*Rev work close 17.05.2022*/

                // Rev 1.0 [ parameter "chkNotInProfReport.Checked.ToString" added],
                /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
               // blLayer.AddEditWarehouseStockJournal(hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
               //ddlBranch.SelectedValue, ddlBranchTo.SelectedValue
               //, Convert.ToString(Session["userid"]), Convert.ToString(ccmTechnician.Value), ref adjustmentId,
               //ref adjustmentNumber, AdjustmentTable, tempWarehousedt, tempSourceWarehousedt, ref ErrorCode, Request.QueryString["Key"], PackingDT, PackingDTWSH, txtTransportationMode.Text.Trim(),
                blLayer.AddEditWarehouseStockJournal
                (
                hdAddEdit.Value, CmbScheme.Value.ToString().Split('~')[0], txtVoucherNo.Text, dtTDate.Date.ToString("yyyy-MM-dd"),
                 ddlBranch.SelectedValue, ddlBranchTo.SelectedValue
                 , Convert.ToString(Session["userid"]), Convert.ToString(ccmTechnician.Value), ref adjustmentId,
                 ref adjustmentNumber, AdjustmentTable, MultiUOMDetails,SourceMultiUOMDetails,
                 tempWarehousedt, tempSourceWarehousedt, ref ErrorCode, Request.QueryString["Key"], PackingDT, PackingDTWSH, txtTransportationMode.Text.Trim(),
                 txtVehicleNo.Text.Trim(), txRemarks.Text.Trim(), TempAdjustmentTableSWH, Proj_id, "BOTH", chkNotInProfReport.Checked
                );
                /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
                if (adjustmentId == -9)
                {
                    DataTable dts = new DataTable();
                    dts = GetAddLockStatus();
                    gridDEstination.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                    gridDEstination.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                }
                else
                {
                    gridDEstination.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
                    gridDEstination.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
                    gridDEstination.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
                    e.Handled = true;

                    #region To Show By Default Cursor after SAVE AND NEW
                    if (hdAddEdit.Value == "Add")
                    {
                        if (HiddenSaveButton.Value != "E")
                        {
                            string schemavalue = CmbScheme.Value.ToString();
                            string NumberingScheme = CmbScheme.Text;
                            string BranchID = ddlBranch.SelectedValue;
                            string BranchName = ddlBranch.SelectedItem.Text;
                            string strDate = dtTDate.Date.ToString("yyyy-MM-dd");
                            List<string> AfterAdd = new List<string> { schemavalue, NumberingScheme, BranchID, BranchName, strDate };

                            Session["schemavalWHST"] = AfterAdd;

                            string schematype = txtVoucherNo.Text;
                            if (schematype == "Auto")
                            {
                                Session["SaveModeWHST"] = "A";
                            }
                            else
                            {
                                Session["SaveModeWHST"] = "M";
                            }
                        }

                    }

                }
                #endregion
            }


        }

        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForWarehouseStockJournal");

            dt = proc.GetTable();
            return dt;

        }
        public void GetQuantityBaseOnProductDest(DataTable tempWarehousedt, string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            //DataTable Warehousedt = new DataTable();
            if (tempWarehousedt != null)
            {
                // Warehousedt = (DataTable)Session["SC_WarehouseData"];
                for (int i = 0; i < tempWarehousedt.Rows.Count; i++)
                {
                    DataRow dr = tempWarehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public string getProductIsInventoryExistsDest(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }
        private void RefetchSrlNoDest()
        {
            //TempTable.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in TempAdjustmentTableDWH.Rows)
            {
                dr["SrlNo"] = conut;

                conut++;
            }
        }
        protected void gridDEstination_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
        }
        #endregion

        #region Stock Details  For Source  Warehouse
        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindWarehouse")
            {
                string SourceWarehouseID = e.Parameter.Split('~')[1];
                DataTable dt = GetWarehouseData(SourceWarehouseID);

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
            }
        }
        public DataTable GetWarehouseData(string SourceWarehouseID)
        {

            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            //DataTable dt = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            //proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            //proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            //proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranch.SelectedValue));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            //proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            //dt = proc.GetTable();
            //return dt;


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlBySourceWHId");

            proc.AddIntegerPara("@SourceWahehouse", Convert.ToInt32(SourceWarehouseID));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            dt = proc.GetTable();
            return dt;
        }
        protected void CmbBatch_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindBatch")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                //Rev 4.0
                string PostingDate = Convert.ToString(e.Parameter.Split('~')[2]);
                //DataTable dt = GetBatchData(WarehouseID);
                DataTable dt = GetBatchData(WarehouseID, PostingDate);
                //Rev 4.0 End

                CmbBatch.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbBatch.Items.Add(Convert.ToString(dt.Rows[i]["BatchName"]), Convert.ToString(dt.Rows[i]["BatchID"]));
                }
            }
        }
        public DataTable GetBatchData(string WarehouseID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        //Rev 4.0
        public DataTable GetBatchData(string WarehouseID, string PostingDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GETBATCHBYPRODUCTIDWAREHOUSEPOSTINGDATE");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddDateTimePara("@PostingDate", Convert.ToDateTime(PostingDate));
            dt = proc.GetTable();
            return dt;
        }
        //Rev 4.0 End
        protected void CmbSerial_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                DataTable dt = GetSerialata(WarehouseID, BatchID);

                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID<>'0'");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
            else if (WhichCall == "EditSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                DataTable dt = GetSerialata(WarehouseID, BatchID);

                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID not in ('0','" + editSerialID + "')");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
        }
        public DataTable GetSerialata(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WHSJSour_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WHSJSour_WarehouseData"] = Warehousedt;
            }
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WHSJSour_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WHSJSour_WarehouseData"] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["WHSJSour_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
            }

            DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
            foreach (DataRow row in result)
            {
                strLoopID = row["LoopID"].ToString();
            }

            if (Warehousedt != null && Warehousedt.Rows.Count > 0)
            {
                int count = 0;
                bool IsFirst = false, IsAssign = false;
                string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    string delLoopID = Convert.ToString(dr["LoopID"]);

                    if (strPreLoopID != delLoopID)
                    {
                        count = 0;
                    }

                    if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                    {
                        IsFirst = true;

                        WarehouseName = Convert.ToString(dr["WarehouseName"]);
                        Quantity = Convert.ToString(dr["Quantity"]);
                        BatchNo = Convert.ToString(dr["BatchNo"]);
                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                        SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                        StkUOMName = Convert.ToString(dr["StkUOMName"]);
                        StkQuantity = Convert.ToString(dr["StkQuantity"]);
                        ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                        AvailableQty = Convert.ToString(dr["AvailableQty"]);
                        BalancrStk = Convert.ToString(dr["BalancrStk"]);
                        MfgDate = Convert.ToString(dr["MfgDate"]);
                        ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                        //dr.Delete();
                    }
                    else
                    {
                        if (delLoopID == strLoopID)
                        {
                            if (strKey == delSrlID)
                            {
                                //dr.Delete();
                            }
                            else
                            {
                                int S_Quantity = Convert.ToInt32(dr["TotalQuantity"]);
                                dr["Quantity"] = S_Quantity - 1;
                                dr["TotalQuantity"] = S_Quantity - 1;

                                if (IsFirst == true && IsAssign == false)
                                {
                                    IsAssign = true;

                                    dr["WarehouseName"] = WarehouseName;
                                    dr["BatchNo"] = BatchNo;
                                    dr["SalesUOMName"] = SalesUOMName;
                                    dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    dr["StkUOMName"] = StkUOMName;
                                    dr["StkQuantity"] = StkQuantity;
                                    dr["ConversionMultiplier"] = ConversionMultiplier;
                                    dr["AvailableQty"] = AvailableQty;
                                    dr["BalancrStk"] = BalancrStk;
                                    dr["MfgDate"] = MfgDate;
                                    dr["ExpiryDate"] = ExpiryDate;
                                }
                                else
                                {
                                    if (IsAssign == false)
                                    {
                                        IsAssign = true;
                                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    }
                                }
                            }
                        }
                    }

                    strPreLoopID = delLoopID;
                    count++;
                }
                Warehousedt.AcceptChanges();


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    if (strKey == delSrlID)
                    {
                        dr.Delete();
                    }
                }
                Warehousedt.AcceptChanges();
            }

            return Warehousedt;
        }
        public void GetProductType(ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(hdfProductID.Value));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }
        public void changeGridOrder()
        {
            string Type = "";
            GetProductType(ref Type);
            if (Type == "W")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
        }
        public void GetTotalStock(ref string Trans_Stock, string WarehouseID)
        {
            string ProductID = Convert.ToString(hdfProductID.Value);

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            proc.AddVarcharPara("@WarehouseID", 100, Convert.ToString(WarehouseID));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Trans_Stock = Convert.ToString(dt.Rows[0]["Trans_Stock"]);
            }
        }
        public void GetProductUOM(ref string Sales_UOM_Name, ref string Sales_UOM_Code, ref string Stk_UOM_Name, ref string Stk_UOM_Code, ref string Conversion_Multiplier, string ProductID)
        {
            DataTable Productdt = GetProductDetailsData(ProductID);
            if (Productdt != null && Productdt.Rows.Count > 0)
            {
                Sales_UOM_Name = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Name"]);
                Sales_UOM_Code = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Code"]);
                Stk_UOM_Name = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Name"]);
                Stk_UOM_Code = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Code"]);
                Conversion_Multiplier = Convert.ToString(Productdt.Rows[0]["Conversion_Multiplier"]);
            }
        }
        public DataTable GetProductDetailsData(string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsSearch");
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            dt = proc.GetTable();
            return dt;
        }
        public void GetBatchDetails(ref string MfgDate, ref string ExpiryDate, string BatchID)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@BatchID", 100, Convert.ToString(BatchID));
            DataTable Batchdt = proc.GetTable();

            if (Batchdt != null && Batchdt.Rows.Count > 0)
            {
                MfgDate = Convert.ToString(Batchdt.Rows[0]["MfgDate"]);
                ExpiryDate = Convert.ToString(Batchdt.Rows[0]["ExpiryDate"]);
            }
        }

        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["WHSJSour_WarehouseData"] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }
            }
        }
        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdWarehouse.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "";

            if (strSplitCommand == "Display")
            {
                GetProductType(ref Type);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }
                changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["SI_LoopWarehouse"]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchID = Convert.ToString(e.Parameters.Split('~')[3]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[4]);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialName = Convert.ToString(e.Parameters.Split('~')[6]);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string ProductSerialID = Convert.ToString(hdfProductSerialID.Value);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);

                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductType(ref Type);

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                }

                bool IsDelete = false;

                if (Type == "WBS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", strLoopID, SerialIDList.Length, repute);
                            }
                        }
                    }
                }
                else if (Type == "W")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    BatchID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");

                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");

                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "WB")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";


                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    //Qty = Convert.ToString(SerialIDList.Length);
                    Qty = "1";
                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";
                    BatchID = "0";

                    if (editWarehouseID != "0")
                    {
                        DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        bool isfirstRow = false;
                        var updateDeleterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                        if (updateDeleterows.Length > 0)
                        {
                            foreach (var row in updateDeleterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                row["Quantity"] = (oldQuantity - Convert.ToDecimal(1));
                                row["TotalQuantity"] = (oldQuantity - Convert.ToDecimal(1));
                                if (Convert.ToString(row["SalesQuantity"]) != "")
                                {
                                    isfirstRow = true;
                                    row["SalesQuantity"] = (oldQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                }
                            }

                            if (isfirstRow == false)
                            {
                                foreach (var row in updateDeleterows)
                                {
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["SalesQuantity"] = oldQuantity + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < SerialIDList.Length; i++)
                    {
                        string strSrlID = SerialIDList[i];
                        string strSrlName = SerialNameList[i];

                        if (editWarehouseID == "0")
                        {
                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            decimal oldQuantity = 0;
                            string whID = "1";

                            if (updaterows.Length > 0)
                            {
                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    whID = Convert.ToString(row["LoopID"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    if (Convert.ToString(row["SalesQuantity"]) != "")
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D");
                            }
                            else
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                            }
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                //string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                //Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");

                                var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                decimal oldQuantity = 0;
                                string whID = "1";

                                if (updaterows.Length > 0)
                                {
                                    foreach (var row in updaterows)
                                    {
                                        oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                        whID = Convert.ToString(row["LoopID"]);

                                        row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        if (Convert.ToString(row["SalesQuantity"]) != "")
                                        {
                                            row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                        }
                                    }

                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D");
                                }
                                else
                                {
                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                                }
                            }
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
                            }
                        }
                    }
                }
                else if (Type == "BS")
                {
                    // GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
                            }
                        }
                    }
                }

                //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, "1", BatchID, BatchName, SerialID, SerialName);
                //string sortExpression = string.Format("{0} {1}", colName, direction);
                //dt.DefaultView.Sort = sortExpression;
                //Warehousedt.DefaultView.Sort = "SrlNo Asc";
                //Warehousedt = Warehousedt.DefaultView.ToTable(true);

                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["WHSJSour_WarehouseData"] = Warehousedt;
                changeGridOrder();

                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();

                Session["SI_LoopWarehouse"] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                //Rev Rajdip
                //CmbBatch.SelectedIndex = -1;
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                }

                DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
                foreach (DataRow row in result)
                {
                    strLoopID = row["LoopID"].ToString();
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    int count = 0;
                    bool IsFirst = false, IsAssign = false;
                    string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        string delLoopID = Convert.ToString(dr["LoopID"]);

                        if (strPreLoopID != delLoopID)
                        {
                            count = 0;
                        }

                        if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                        {
                            IsFirst = true;

                            WarehouseName = Convert.ToString(dr["WarehouseName"]);
                            Quantity = Convert.ToString(dr["Quantity"]);
                            BatchNo = Convert.ToString(dr["BatchNo"]);
                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                            SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                            StkUOMName = Convert.ToString(dr["StkUOMName"]);
                            StkQuantity = Convert.ToString(dr["StkQuantity"]);
                            ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                            AvailableQty = Convert.ToString(dr["AvailableQty"]);
                            BalancrStk = Convert.ToString(dr["BalancrStk"]);
                            MfgDate = Convert.ToString(dr["MfgDate"]);
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                            //dr.Delete();
                        }
                        else
                        {
                            if (delLoopID == strLoopID)
                            {
                                if (strKey == delSrlID)
                                {
                                    //dr.Delete();
                                }
                                else
                                {
                                    decimal S_Quantity = Convert.ToDecimal(dr["TotalQuantity"]);
                                    dr["Quantity"] = S_Quantity - 1;
                                    dr["TotalQuantity"] = S_Quantity - 1;

                                    if (IsFirst == true && IsAssign == false)
                                    {
                                        IsAssign = true;

                                        dr["WarehouseName"] = WarehouseName;
                                        dr["BatchNo"] = BatchNo;
                                        dr["SalesUOMName"] = SalesUOMName;
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        dr["StkUOMName"] = StkUOMName;
                                        dr["StkQuantity"] = StkQuantity;
                                        dr["ConversionMultiplier"] = ConversionMultiplier;
                                        dr["AvailableQty"] = AvailableQty;
                                        dr["BalancrStk"] = BalancrStk;
                                        dr["MfgDate"] = MfgDate;
                                        dr["ExpiryDate"] = ExpiryDate;
                                    }
                                    else
                                    {
                                        if (IsAssign == false)
                                        {
                                            IsAssign = true;
                                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                            dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        }
                                    }
                                }
                            }
                        }

                        strPreLoopID = delLoopID;
                        count++;
                    }
                    Warehousedt.AcceptChanges();


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        if (strKey == delSrlID)
                        {
                            dr.Delete();
                        }
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["WHSJSour_WarehouseData"] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSerialID.Value);
                DeleteUnsaveWarehouse(ProductID);
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];
                    string ProductID = Convert.ToString(hdfProductSerialID.Value);
                    string strPreLoopID = "";
                    decimal sum = 0;

                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);
                            //string resultString = Regex.Match(strQuantity, @"[^0-9\.]+").Value;

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantity.Value))
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < Warehousedt.Rows.Count; i++)
                        {
                            DataRow dr = Warehousedt.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        Warehousedt.AcceptChanges();
                    }
                    else
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "N";
                    }

                    Session["WHSJSour_WarehouseData"] = Warehousedt;
                }
            }
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["WHSJSour_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJSour_WarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                    }

                    //CmbWarehouse.DataSource = GetWarehouseData();
                    //Rev Rajdip
                    //CmbBatch.DataSource = GetBatchData(strWarehouse);
                    //CmbBatch.DataBind();

                    NCallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity;
                }
            }
        }

        #endregion

        #region Stock Details  For Destination  Warehouse
        protected void CmbWarehouseDest_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindWarehouse")
            {
                string SourceWarehouseID = e.Parameter.Split('~')[1];
                DataTable dt = GetWarehouseDataDest(SourceWarehouseID);

                CmbWarehouseDest.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouseDest.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
            }
        }
        public DataTable GetWarehouseDataDest(string SourceWarehouseID)
        {

            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            //DataTable dt = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            //proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            //proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            //proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranch.SelectedValue));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            //proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            //dt = proc.GetTable();
            //return dt;


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlBySourceWHId");

            proc.AddIntegerPara("@SourceWahehouse", Convert.ToInt32(SourceWarehouseID));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            dt = proc.GetTable();
            return dt;
        }

        protected void CmbBatchDest_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindBatch")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt = GetBatchDataDest(WarehouseID);
                //Rev Rajdip
                //CmbBatchDest.Items.Clear();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    CmbBatchDest.Items.Add(Convert.ToString(dt.Rows[i]["BatchName"]), Convert.ToString(dt.Rows[i]["BatchID"]));
                //}
            }
        }
        public DataTable GetBatchDataDest(string WarehouseID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductIDdest.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranchTo.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        protected void CmbSerialDest_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                DataTable dt = GetSerialataDest(WarehouseID, BatchID);

                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID<>'0'");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
            else if (WhichCall == "EditSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                DataTable dt = GetSerialataDest(WarehouseID, BatchID);

                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID not in ('0','" + editSerialID + "')");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
        }
        public DataTable GetSerialataDest(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductIDdest.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddlBranchTo.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        public void DeleteWarehouseDest(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WHSJDest_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WHSJDest_WarehouseData"] = Warehousedt;
            }
        }
        public void DeleteUnsaveWarehouseDest(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WHSJDest_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WHSJDest_WarehouseData"] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrlDest(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["WHSJDest_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
            }

            DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
            foreach (DataRow row in result)
            {
                strLoopID = row["LoopID"].ToString();
            }

            if (Warehousedt != null && Warehousedt.Rows.Count > 0)
            {
                int count = 0;
                bool IsFirst = false, IsAssign = false;
                string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    string delLoopID = Convert.ToString(dr["LoopID"]);

                    if (strPreLoopID != delLoopID)
                    {
                        count = 0;
                    }

                    if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                    {
                        IsFirst = true;

                        WarehouseName = Convert.ToString(dr["WarehouseName"]);
                        Quantity = Convert.ToString(dr["Quantity"]);
                        BatchNo = Convert.ToString(dr["BatchNo"]);
                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                        SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                        StkUOMName = Convert.ToString(dr["StkUOMName"]);
                        StkQuantity = Convert.ToString(dr["StkQuantity"]);
                        ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                        AvailableQty = Convert.ToString(dr["AvailableQty"]);
                        BalancrStk = Convert.ToString(dr["BalancrStk"]);
                        MfgDate = Convert.ToString(dr["MfgDate"]);
                        ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                        //dr.Delete();
                    }
                    else
                    {
                        if (delLoopID == strLoopID)
                        {
                            if (strKey == delSrlID)
                            {
                                //dr.Delete();
                            }
                            else
                            {
                                int S_Quantity = Convert.ToInt32(dr["TotalQuantity"]);
                                dr["Quantity"] = S_Quantity - 1;
                                dr["TotalQuantity"] = S_Quantity - 1;

                                if (IsFirst == true && IsAssign == false)
                                {
                                    IsAssign = true;

                                    dr["WarehouseName"] = WarehouseName;
                                    dr["BatchNo"] = BatchNo;
                                    dr["SalesUOMName"] = SalesUOMName;
                                    dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    dr["StkUOMName"] = StkUOMName;
                                    dr["StkQuantity"] = StkQuantity;
                                    dr["ConversionMultiplier"] = ConversionMultiplier;
                                    dr["AvailableQty"] = AvailableQty;
                                    dr["BalancrStk"] = BalancrStk;
                                    dr["MfgDate"] = MfgDate;
                                    dr["ExpiryDate"] = ExpiryDate;
                                }
                                else
                                {
                                    if (IsAssign == false)
                                    {
                                        IsAssign = true;
                                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    }
                                }
                            }
                        }
                    }

                    strPreLoopID = delLoopID;
                    count++;
                }
                Warehousedt.AcceptChanges();


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    if (strKey == delSrlID)
                    {
                        dr.Delete();
                    }
                }
                Warehousedt.AcceptChanges();
            }

            return Warehousedt;
        }
        public void GetProductTypeDest(ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(hdfProductIDdest.Value));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }
        public void changeGridOrderDest()
        {
            string Type = "";
            GetProductTypeDest(ref Type);
            if (Type == "W")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = true;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = false;
                GrdWarehouseDest.Columns["MfgDate"].Visible = false;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = false;
                GrdWarehouseDest.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = true;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = true;
                GrdWarehouseDest.Columns["MfgDate"].Visible = true;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = true;
                GrdWarehouseDest.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = true;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = true;
                GrdWarehouseDest.Columns["MfgDate"].Visible = true;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = true;
                GrdWarehouseDest.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = false;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = true;
                GrdWarehouseDest.Columns["MfgDate"].Visible = true;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = true;
                GrdWarehouseDest.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = false;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = false;
                GrdWarehouseDest.Columns["MfgDate"].Visible = false;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = false;
                GrdWarehouseDest.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = true;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = false;
                GrdWarehouseDest.Columns["MfgDate"].Visible = false;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = false;
                GrdWarehouseDest.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouseDest.Columns["WarehouseName"].Visible = false;
                GrdWarehouseDest.Columns["AvailableQty"].Visible = false;
                GrdWarehouseDest.Columns["BatchNo"].Visible = true;
                GrdWarehouseDest.Columns["MfgDate"].Visible = true;
                GrdWarehouseDest.Columns["ExpiryDate"].Visible = true;
                GrdWarehouseDest.Columns["SerialNo"].Visible = true;
            }
        }
        public void GetTotalStockDest(ref string Trans_Stock, string WarehouseID)
        {
            string ProductID = Convert.ToString(hdfProductIDdest.Value);

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            proc.AddVarcharPara("@WarehouseID", 100, Convert.ToString(WarehouseID));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Trans_Stock = Convert.ToString(dt.Rows[0]["Trans_Stock"]);
            }
        }
        public void GetProductUOMDest(ref string Sales_UOM_Name, ref string Sales_UOM_Code, ref string Stk_UOM_Name, ref string Stk_UOM_Code, ref string Conversion_Multiplier, string ProductID)
        {
            DataTable Productdt = GetProductDetailsDataDest(ProductID);
            if (Productdt != null && Productdt.Rows.Count > 0)
            {
                Sales_UOM_Name = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Name"]);
                Sales_UOM_Code = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Code"]);
                Stk_UOM_Name = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Name"]);
                Stk_UOM_Code = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Code"]);
                Conversion_Multiplier = Convert.ToString(Productdt.Rows[0]["Conversion_Multiplier"]);
            }
        }
        public DataTable GetProductDetailsDataDest(string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsSearch");
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            dt = proc.GetTable();
            return dt;
        }
        public void GetBatchDetailsDest(ref string MfgDate, ref string ExpiryDate, string BatchID)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@BatchID", 100, Convert.ToString(BatchID));
            DataTable Batchdt = proc.GetTable();

            if (Batchdt != null && Batchdt.Rows.Count > 0)
            {
                MfgDate = Convert.ToString(Batchdt.Rows[0]["MfgDate"]);
                ExpiryDate = Convert.ToString(Batchdt.Rows[0]["ExpiryDate"]);
            }
        }

        protected void GrdWarehouseDest_DataBinding(object sender, EventArgs e)
        {
            if (Session["WHSJDest_WarehouseData"] != null)
            {
                string Type = "";
                GetProductTypeDest(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialIDDest.Value);
                DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouseDest.DataSource = dvData;
                }
                else
                {
                    GrdWarehouseDest.DataSource = Warehousedt.DefaultView;
                }
            }
        }
        protected void GrdWarehouseDest_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdWarehouseDest.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "";

            if (strSplitCommand == "Display")
            {
                GetProductTypeDest(ref Type);
                string ProductID = Convert.ToString(hdfProductIDdest.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouseDest.DataSource = dvData;
                    GrdWarehouseDest.DataBind();
                }
                else
                {
                    GrdWarehouseDest.DataSource = Warehousedt.DefaultView;
                    GrdWarehouseDest.DataBind();
                }
                changeGridOrderDest();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["SI_LoopWarehouse"]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchID = Convert.ToString(e.Parameters.Split('~')[3]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[4]);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialName = Convert.ToString(e.Parameters.Split('~')[6]);
                string ProductID = Convert.ToString(hdfProductIDdest.Value);
                string ProductSerialID = Convert.ToString(hdfProductSerialIDDest.Value);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);

                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductTypeDest(ref Type);

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                }

                bool IsDelete = false;

                if (Type == "WBS")
                {
                    GetTotalStockDest(ref Trans_Stock, WarehouseID);
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetailsDest(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", strLoopID, SerialIDList.Length, repute);
                            }
                        }
                    }
                }
                else if (Type == "W")
                {
                    GetTotalStockDest(ref Trans_Stock, WarehouseID);
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    BatchID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");

                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");

                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "WB")
                {
                    GetTotalStockDest(ref Trans_Stock, WarehouseID);
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetailsDest(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    Int32 Conversion_MultiplierROUND = Convert.ToInt32(ConversionMultiplier);
                    MfgDate = txtmfgdate.Text.ToString();
                    ExpiryDate = txtexpdate.Text.ToString();
                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        //REV RAJDIP
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                        //var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND SrlNo='" + ProductSerialID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                        //END REV RAJDIP
                        if (updaterows.Length == 0)
                        {
                            //Rev Rajdip
                            //Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name,Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_MultiplierROUND, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            //End Rev Rajdip
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                //Rev Rajdip
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                //row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                //row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                //row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;

                                //row["Quantity"] = (Convert.ToDecimal(Qty));
                                //row["TotalQuantity"] = (Convert.ToDecimal(Qty));
                                //row["SalesQuantity"] = (Convert.ToDecimal(Qty) + " " + Sales_UOM_Name);

                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;


                                row["MfgDate"] = MfgDate;
                                row["ExpiryDate"] = ExpiryDate;
                                row["WarehouseID"] = WarehouseID;
                                row["WarehouseName"] = WarehouseName;
                                row["BatchNo"] = BatchName;
                                //End Rev Rajdip
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name,Convert.ToInt32(Conversion_Multiplier), Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetailsDest(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";


                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    //Qty = Convert.ToString(SerialIDList.Length);
                    Qty = "1";
                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";
                    BatchID = "0";

                    if (editWarehouseID != "0")
                    {
                        DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        bool isfirstRow = false;
                        var updateDeleterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                        if (updateDeleterows.Length > 0)
                        {
                            foreach (var row in updateDeleterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                row["Quantity"] = (oldQuantity - Convert.ToDecimal(1));
                                row["TotalQuantity"] = (oldQuantity - Convert.ToDecimal(1));
                                if (Convert.ToString(row["SalesQuantity"]) != "")
                                {
                                    isfirstRow = true;
                                    row["SalesQuantity"] = (oldQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                }
                            }

                            if (isfirstRow == false)
                            {
                                foreach (var row in updateDeleterows)
                                {
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["SalesQuantity"] = oldQuantity + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < SerialIDList.Length; i++)
                    {
                        string strSrlID = SerialIDList[i];
                        string strSrlName = SerialNameList[i];

                        if (editWarehouseID == "0")
                        {
                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            decimal oldQuantity = 0;
                            string whID = "1";

                            if (updaterows.Length > 0)
                            {
                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    whID = Convert.ToString(row["LoopID"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    if (Convert.ToString(row["SalesQuantity"]) != "")
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D");
                            }
                            else
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                            }
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                //string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                //Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");

                                var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                decimal oldQuantity = 0;
                                string whID = "1";

                                if (updaterows.Length > 0)
                                {
                                    foreach (var row in updaterows)
                                    {
                                        oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                        whID = Convert.ToString(row["LoopID"]);

                                        row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        if (Convert.ToString(row["SalesQuantity"]) != "")
                                        {
                                            row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                        }
                                    }

                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D");
                                }
                                else
                                {
                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                                }
                            }
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetTotalStockDest(ref Trans_Stock, WarehouseID);
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
                            }
                        }
                    }
                }
                else if (Type == "BS")
                {
                    // GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOMDest(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetailsDest(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
                            }
                        }
                    }
                }

                //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, "1", BatchID, BatchName, SerialID, SerialName);
                //string sortExpression = string.Format("{0} {1}", colName, direction);
                //dt.DefaultView.Sort = sortExpression;
                //Warehousedt.DefaultView.Sort = "SrlNo Asc";
                //Warehousedt = Warehousedt.DefaultView.ToTable(true);

                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["WHSJDest_WarehouseData"] = Warehousedt;
                changeGridOrderDest();

                GrdWarehouseDest.DataSource = Warehousedt.DefaultView;
                GrdWarehouseDest.DataBind();

                Session["SI_LoopWarehouse"] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                //Rev rajdip
                //CmbBatch.SelectedIndex = -1;
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                DataTable Warehousedt = new DataTable();
                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                }

                DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
                foreach (DataRow row in result)
                {
                    strLoopID = row["LoopID"].ToString();
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    int count = 0;
                    bool IsFirst = false, IsAssign = false;
                    string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        string delLoopID = Convert.ToString(dr["LoopID"]);

                        if (strPreLoopID != delLoopID)
                        {
                            count = 0;
                        }

                        if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                        {
                            IsFirst = true;

                            WarehouseName = Convert.ToString(dr["WarehouseName"]);
                            Quantity = Convert.ToString(dr["Quantity"]);
                            BatchNo = Convert.ToString(dr["BatchNo"]);
                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                            SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                            StkUOMName = Convert.ToString(dr["StkUOMName"]);
                            StkQuantity = Convert.ToString(dr["StkQuantity"]);
                            ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                            AvailableQty = Convert.ToString(dr["AvailableQty"]);
                            BalancrStk = Convert.ToString(dr["BalancrStk"]);
                            MfgDate = Convert.ToString(dr["MfgDate"]);
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                            //dr.Delete();
                        }
                        else
                        {
                            if (delLoopID == strLoopID)
                            {
                                if (strKey == delSrlID)
                                {
                                    //dr.Delete();
                                }
                                else
                                {
                                    decimal S_Quantity = Convert.ToDecimal(dr["TotalQuantity"]);
                                    dr["Quantity"] = S_Quantity - 1;
                                    dr["TotalQuantity"] = S_Quantity - 1;

                                    if (IsFirst == true && IsAssign == false)
                                    {
                                        IsAssign = true;

                                        dr["WarehouseName"] = WarehouseName;
                                        dr["BatchNo"] = BatchNo;
                                        dr["SalesUOMName"] = SalesUOMName;
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        dr["StkUOMName"] = StkUOMName;
                                        dr["StkQuantity"] = StkQuantity;
                                        dr["ConversionMultiplier"] = ConversionMultiplier;
                                        dr["AvailableQty"] = AvailableQty;
                                        dr["BalancrStk"] = BalancrStk;
                                        dr["MfgDate"] = MfgDate;
                                        dr["ExpiryDate"] = ExpiryDate;
                                    }
                                    else
                                    {
                                        if (IsAssign == false)
                                        {
                                            IsAssign = true;
                                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                            dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        }
                                    }
                                }
                            }
                        }

                        strPreLoopID = delLoopID;
                        count++;
                    }
                    Warehousedt.AcceptChanges();


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        if (strKey == delSrlID)
                        {
                            dr.Delete();
                        }
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["WHSJDest_WarehouseData"] = Warehousedt;
                GrdWarehouseDest.DataSource = Warehousedt.DefaultView;
                GrdWarehouseDest.DataBind();
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSerialIDDest.Value);
                DeleteUnsaveWarehouseDest(ProductID);
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];
                    string ProductID = Convert.ToString(hdfProductSerialIDDest.Value);
                    string strPreLoopID = "";
                    decimal sum = 0;

                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);
                            //string resultString = Regex.Match(strQuantity, @"[^0-9\.]+").Value;

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantityDest.Value))
                    {
                        GrdWarehouseDest.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < Warehousedt.Rows.Count; i++)
                        {
                            DataRow dr = Warehousedt.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        Warehousedt.AcceptChanges();
                    }
                    else
                    {
                        GrdWarehouseDest.JSProperties["cpIsSave"] = "N";
                    }

                    Session["WHSJDest_WarehouseData"] = Warehousedt;
                }
            }
        }

        protected void CallbackPanelDest_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["WHSJDest_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["WHSJDest_WarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", BatchNo = "0";//, MfgDate = "", ExpiryDate="";
                    DateTime?MfgDate=null;
                    DateTime? ExpiryDate = null;
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                        //Rev Rajdip
                        BatchNo = (Convert.ToString(dr["BatchNo"]) != "") ? Convert.ToString(dr["BatchNo"]) : "0";
                        //MfgDate = (Convert.ToDateTime(dr["MfgDate"]) != null) ? Convert.ToDateTime(dr["MfgDate"]) : "0";
                        //ExpiryDate = (Convert.ToDateTime(dr["ExpiryDate"]) != null) ? Convert.ToDateTime(dr["ExpiryDate"]) : "0";                 
                        if (dr["MfgDate"] != DBNull.Value && dr["MfgDate"] != null && dr["MfgDate"] != "")
                        {
                            MfgDate = DateTime.ParseExact(dr["MfgDate"].ToString(), "MM-dd-yyyy", provider);
                        }
                        if (dr["ExpiryDate"] != DBNull.Value && dr["ExpiryDate"] != null && dr["ExpiryDate"] != "")
                        {
                            //ExpiryDate = Convert.ToDateTime(dr["ExpiryDate"]);
                            ExpiryDate = DateTime.ParseExact(dr["ExpiryDate"].ToString(), "MM-dd-yyyy", provider);
                            //End Rev Rajdip
                        }
                    }
                    //CmbWarehouse.DataSource = GetWarehouseData();
                    //Rev Rajdip
                    //CmbBatchDest.DataSource = GetBatchDataDest(strWarehouse);
                    //CmbBatchDest.DataBind();
                    //Rev Rajdip
                    //NCallbackPanelDest.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity;
                    NCallbackPanelDest.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity + "~" + BatchNo + "~" + MfgDate + "~" + ExpiryDate;
                    //End Rev Rajdip
                }
            }
        }

        #endregion


        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }

        protected void EntityServerModeDataStock_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddlBranch.SelectedValue)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";

            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");

            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }
        //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];

            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";
            // Rev Sanchita
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Sanchita

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMData"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData.Columns.Add("ProductId", typeof(Int64));
                    // Mantis Issue 24425, 24428
                    MultiUOMData.Columns.Add("DetailsId", typeof(string));
                    MultiUOMData.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("UpdateRow", typeof(string));
                    MultiUOMData.Columns.Add("MultiUOMSR", typeof(string));
                    // End of Mantis Issue 24425, 24428

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    grid_MultiUOM.DataBind();
                }
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";
            }

            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();
                // Mantis Issue 24425, 24428
                int MultiUOMSR = 1;
                // End of Mantis Issue 24425, 24428

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                // Mantis Issue 24397
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                // End of Mantis Issue 24397


                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData"];



                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Mantis Issue 24397 [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24397
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                    }
                    else
                    {
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(Int64));
                        // Mantis Issue 24397
                        MultiUOMSaveData.Columns.Add("DetailsId", typeof(string));
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData.Columns.Add("MultiUOMSR", typeof(string));
                        // End of Mantis Issue 24397
                    }

                    // Mantis Issue 24397
                    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);

                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        // Rev Sanchita
                        //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, (Convert.ToInt16(thisRow["MultiUOMSR"]) + 1));
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                        // End of Rev Sanchita
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // End of Mantis Issue 24397
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMData"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMData"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }
                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMData"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = null;
                    grid_MultiUOM.DataBind();
                }
            }

            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData"] = dt;
            }

            // Mantis Issue 24397
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);
                    // Mantis Issue 24425, 24428
                    Decimal AltQuantity = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    String AltUOM = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    // End of Mantis Issue 24425, 24428

                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;
                    // Mantis Issue 24425, 24428
                    grid_MultiUOM.JSProperties["cpAltQuantity"] = AltQuantity;
                    grid_MultiUOM.JSProperties["cpAltUOM"] = AltUOM;
                    // End of Mantis Issue 24425, 24428

                    //if (Session["OrderDetails"] != null)
                    //{
                    //    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];

                    //    DataRow[] drSalesOrder = SalesOrderdt.Select("SrlNo ='" + SelNo + "'");
                    //    if (drSalesOrder.Length > 0)
                    //    {
                    //        drSalesOrder[0]["Quantity"] = BaseQty;
                    //        drSalesOrder[0]["SalePrice"] = BaseRate;
                    //    }

                    //}

                }
            }
            // End of Mantis Issue 24397
            // Mantis Issue 24425, 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMData"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {


                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[12];
                // Rev Sanchita
                string SrlNo = "0";
                string Validcheck = "";
                // End of Rev Sanchita

                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData"];
                // Rev Sanchita
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        // Rev SAnchita
                        SrlNo = Convert.ToString(item["SrlNo"]);
                        // End of Rev Sanchita
                        //item.Table.Rows.Remove(item);
                        //break;

                    }
                }
                // End of Rev Sanchita

                // Rev Sanchita
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                // End of Rev Sanchita
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);

                // Rev Sanchita
                //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId,"", BaseRate, AltRate, UpdateRow, muid);

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
                        }
                    }
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            //// Rev SAnchita
                            //SrlNo = Convert.ToString(item["SrlNo"]);
                            //// End of Rev Sanchita
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    /*Rev work start 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                    //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, muid);
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, 0, BaseRate, AltRate, UpdateRow, muid);
                    /*Rev work close 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                }
                //End Rev Sanchita

                Session["MultiUOMData"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMData"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Rev Sanchita
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Rev Sanchita

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
            }
            // End of Mantis Issue 24425, 24428
        }
        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }
        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                // Mantis Issue 24397
                //DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                // End of Mantis Issue 24397

                SLVal = MultiUoMresult.Length;
            }
            return SLVal;
        }
        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMData"];
            //if(dt !=null && dt.Rows.Count >0 )
            //{
            //    DataView dvData = new DataView(dt);
            //    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

            //    grid_MultiUOM.DataSource = dvData;
            //    grid_MultiUOM.DataBind();
            //}
            //else
            //{
            //    grid_MultiUOM.DataSource = null;
            //    grid_MultiUOM.DataBind();
            //}
        }
        protected void MultiUOM_CustomCallback_Source(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];

            grid_MultiUOM_Source.JSProperties["cpDuplicateAltUOM"] = "";
            // Rev Sanchita
            grid_MultiUOM_Source.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Sanchita

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                grid_MultiUOM_Source.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData_Source = new DataTable();

                if (Session["MultiUOMData_Source"] != null)
                {
                    MultiUOMData_Source = (DataTable)Session["MultiUOMData_Source"];
                }
                else
                {
                    MultiUOMData_Source.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData_Source.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData_Source.Columns.Add("UOM", typeof(string));
                    MultiUOMData_Source.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData_Source.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData_Source.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData_Source.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData_Source.Columns.Add("ProductId", typeof(Int64));
                    // Mantis Issue 24425, 24428
                    MultiUOMData_Source.Columns.Add("DetailsId", typeof(string));
                    MultiUOMData_Source.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMData_Source.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMData_Source.Columns.Add("UpdateRow", typeof(string));
                    MultiUOMData_Source.Columns.Add("MultiUOMSR", typeof(string));
                    // End of Mantis Issue 24425, 24428

                }
                if (MultiUOMData_Source != null && MultiUOMData_Source.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    DataView dvData = new DataView(MultiUOMData_Source);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM_Source.DataSource = dvData;
                    grid_MultiUOM_Source.DataBind();
                }
                else
                {
                    grid_MultiUOM_Source.DataSource = MultiUOMData_Source.DefaultView;
                    grid_MultiUOM_Source.DataBind();
                }
                grid_MultiUOM_Source.JSProperties["cpOpenFocus"] = "OpenFocus";
            }

            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData_Source = new DataTable();
                // Mantis Issue 24425, 24428
                int MultiUOMSR = 1;
                // End of Mantis Issue 24425, 24428

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                // Mantis Issue 24397
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                // End of Mantis Issue 24397


                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData_Source"];



                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Mantis Issue 24397 [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24397
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData_Source"] != null)
                    {

                        MultiUOMSaveData_Source = (DataTable)Session["MultiUOMData_Source"];

                    }
                    else
                    {
                        MultiUOMSaveData_Source.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData_Source.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData_Source.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData_Source.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData_Source.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData_Source.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData_Source.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData_Source.Columns.Add("ProductId", typeof(Int64));
                        // Mantis Issue 24397
                        MultiUOMSaveData_Source.Columns.Add("DetailsId", typeof(string));
                        MultiUOMSaveData_Source.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData_Source.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData_Source.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData_Source.Columns.Add("MultiUOMSR", typeof(string));
                        // End of Mantis Issue 24397
                    }

                    // Mantis Issue 24397
                    //MultiUOMSaveData_Source.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);

                    DataRow thisRow;
                    if (MultiUOMSaveData_Source.Rows.Count > 0)
                    {
                        // Rev Sanchita
                        //thisRow = (DataRow)MultiUOMSaveData_Source.Rows[MultiUOMSaveData_Source.Rows.Count - 1];
                        //MultiUOMSaveData_Source.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, (Convert.ToInt16(thisRow["MultiUOMSR"]) + 1));
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData_Source.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData_Source.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                        // End of Rev Sanchita
                    }
                    else
                    {
                        MultiUOMSaveData_Source.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // End of Mantis Issue 24397
                    MultiUOMSaveData_Source.AcceptChanges();
                    Session["MultiUOMData_Source"] = MultiUOMSaveData_Source;

                    if (MultiUOMSaveData_Source != null && MultiUOMSaveData_Source.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData_Source);
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                        grid_MultiUOM_Source.DataSource = dvData;
                        grid_MultiUOM_Source.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMData"] = MultiUOMSaveData;
                        grid_MultiUOM_Source.DataSource = MultiUOMSaveData_Source.DefaultView;
                        grid_MultiUOM_Source.DataBind();
                    }
                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMData_Source"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM_Source.DataSource = dvData;
                    grid_MultiUOM_Source.DataBind();
                }
                else
                {
                    grid_MultiUOM_Source.DataSource = null;
                    grid_MultiUOM_Source.DataBind();
                }
            }

            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData_Source"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData_Source"] = dt;
            }

            // Mantis Issue 24397
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData_Source"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData_Source"];
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);
                    // Mantis Issue 24425, 24428
                    Decimal AltQuantity = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    String AltUOM = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    // End of Mantis Issue 24425, 24428

                    grid_MultiUOM_Source.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM_Source.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM_Source.JSProperties["cpBaseRate"] = BaseRate;
                    // Mantis Issue 24425, 24428
                    grid_MultiUOM_Source.JSProperties["cpAltQuantity"] = AltQuantity;
                    grid_MultiUOM_Source.JSProperties["cpAltUOM"] = AltUOM;
                    // End of Mantis Issue 24425, 24428

                    //if (Session["OrderDetails"] != null)
                    //{
                    //    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];

                    //    DataRow[] drSalesOrder = SalesOrderdt.Select("SrlNo ='" + SelNo + "'");
                    //    if (drSalesOrder.Length > 0)
                    //    {
                    //        drSalesOrder[0]["Quantity"] = BaseQty;
                    //        drSalesOrder[0]["SalePrice"] = BaseRate;
                    //    }

                    //}

                }
            }
            // End of Mantis Issue 24397
            // Mantis Issue 24425, 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData_Source"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM_Source.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM_Source.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM_Source.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM_Source.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM_Source.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM_Source.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM_Source.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM_Source.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMData_Source"] = dt;
            }
            else if (SpltCmmd == "UpdateRow")
            {


                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[12];
                // Rev Sanchita
                string SrlNo = "0";
                string Validcheck = "";
                // End of Rev Sanchita

                DataTable MultiUOMSaveData_Source = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData_Source"];
                // Rev Sanchita
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        // Rev SAnchita
                        SrlNo = Convert.ToString(item["SrlNo"]);
                        // End of Rev Sanchita
                        //item.Table.Rows.Remove(item);
                        //break;

                    }
                }
                // End of Rev Sanchita

                // Rev Sanchita
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                // End of Rev Sanchita
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);

                // Rev Sanchita
                //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId,"", BaseRate, AltRate, UpdateRow, muid);

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM_Source.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
                        }
                    }
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            //// Rev SAnchita
                            //SrlNo = Convert.ToString(item["SrlNo"]);
                            //// End of Rev Sanchita
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    /*Rev work start 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                    //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, muid);
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, 0, BaseRate, AltRate, UpdateRow, muid);
                    /*Rev work close 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                }
                //End Rev Sanchita

                Session["MultiUOMData_Source"] = dt;
                MultiUOMSaveData_Source = (DataTable)Session["MultiUOMData_Source"];

                MultiUOMSaveData_Source.AcceptChanges();
                Session["MultiUOMData_Source"] = MultiUOMSaveData_Source;

                if (MultiUOMSaveData_Source != null && MultiUOMSaveData_Source.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData_Source);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Rev Sanchita
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Rev Sanchita

                    grid_MultiUOM_Source.DataSource = dvData;
                    grid_MultiUOM_Source.DataBind();
                }
            }
            // End of Mantis Issue 24425, 24428
        }
        [WebMethod]
        public static Int32 GetQuantityfromSL_Source(string SLNo)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData_Source"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData_Source"];
                // Mantis Issue 24397
                //DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                // End of Mantis Issue 24397

                SLVal = MultiUoMresult.Length;
            }
            return SLVal;
        }
        protected void MultiUOM_DataBinding_Source(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMData"];
            //if(dt !=null && dt.Rows.Count >0 )
            //{
            //    DataView dvData = new DataView(dt);
            //    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

            //    grid_MultiUOM.DataSource = dvData;
            //    grid_MultiUOM.DataBind();
            //}
            //else
            //{
            //    grid_MultiUOM.DataSource = null;
            //    grid_MultiUOM.DataBind();
            //}
        }

        //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal

        public IFormatProvider provider { get; set; }
    }
}