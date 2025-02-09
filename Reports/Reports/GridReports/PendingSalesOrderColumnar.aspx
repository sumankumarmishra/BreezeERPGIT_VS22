﻿<%--======================================Revision History=========================================================================
1.0   V2.0.35     Debashis    07/02/2023      Party Order No & Party Order Date columns required in Pending Sales Order Register - Columnar.
                                              To be done in Pending Sales Order Register also.
                                              Refer: 0025616
2.0   V2.0.36     Pallab      23/02/2023      Report pages design modification
                                              Refer: 0025575
===================================End of Revision History=====================================================================--%>
<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PendingSalesOrderColumnar.aspx.cs" Inherits="Reports.Reports.GridReports.PendingSalesOrderColumnar" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }
        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
            }
        
        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        #ListBoxProjects{
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>
    
   
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'CustomerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustModel').modal('hide');
                    ctxtCustName.SetText(Name);
                    GetObjectID('hdnCustomerId').value = key;
                }
                else {
                    ctxtCustName.SetText('');
                    GetObjectID('hdnCustomerId').value = '';
                }
            }
            else if (ArrName == 'SalesmanSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#SalesmanModel').modal('hide');
                    ctxtSalesman.SetText(Name);
                    GetObjectID('hdnSalesmanId').value = key;
                }
                else {
                    ctxtSalesman.SetText('');
                    GetObjectID('hdnSalesmanId').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
   <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#CustModel').on('shown.bs.modal', function () {
                 $('#txtCustSearch').focus();
             })
         })
         var CustArr = new Array();
         $(document).ready(function () {
             var CustObj = new Object();
             CustObj.Name = "CustomerSource";
             CustObj.ArraySource = CustArr;
             arrMultiPopup.push(CustObj);
         })
         function CustomerButnClick(s, e) {
             $('#CustModel').modal('show');
         }

         function Customer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#CustModel').modal('show');
             }
         }

         function Customerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtCustSearch").val();
             // OtherDetails.BranchID = $('#ddl_Branch').val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Customer Name");
                 HeaderCaption.push("Unique ID");
                 HeaderCaption.push("Alternate No.");
                 HeaderCaption.push("Address");

                 if ($("#txtCustSearch").val() != "") {
                     //callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues");
                     //callonServerM("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerSource");
                     callonServerM("Services/Master.asmx/GetOnlyCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtCustSearch').focus();
             else
                 $('#txtCustSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <%-- For Salesman multiselection--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#SalesmanModel').on('shown.bs.modal', function () {
                $('#txtSalesmanSearch').focus();
            })
        })

        var SalesmanArr = new Array();
        $(document).ready(function () {
            var SalesmanObj = new Object();
            SalesmanObj.Name = "SalesmanSource";
            SalesmanObj.ArraySource = SalesmanArr;
            arrMultiPopup.push(SalesmanObj);
        })
        function SalesmanButnClick(s, e) {
            $('#SalesmanModel').modal('show');
        }

        function Salesman_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#SalesmanModel').modal('show');
            }
        }

        function Salesmankeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtSalesmanSearch").val()) == "" || $.trim($("#txtSalesmanSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtSalesmanSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Salesman Name");

                if ($("#txtSalesmanSearch").val() != "") {
                    callonServerM("Services/Master.asmx/GetSalesman", OtherDetails, "SalesmanTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "SalesmanSource");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }

        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtSalesmanSearch').focus();
            else
                $('#txtSalesmanSearch').focus();
        }
    </script>
    <%-- For Salesman multiselection--%>
    <script type="text/javascript">
        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {
                }
            }

        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })
        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }
        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }

        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && popupdetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            popupdetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function fn_OpenDetails(keyValue) {
            Grid.PerformCallback('Edit~' + keyValue);
        }

        function popupPendSalesAfterHide(s, e) {
            popupPendSales.Hide();
        }
    </script>


    <script type="text/javascript">
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsPendSaleOrdColumnarFilter").val("Y");
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback();
                }
            }
            else {
                cCallbackPanel.PerformCallback();
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            GridList.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }
        function OpenPendSaleOrdDetails(invoice, type) {
            if (type == 'SO') {
                url = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + invoice + '&IsTagged=1&req=V&type=SO';
            }
            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        function CheckZeroBal(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkCloseOrder.SetCheckState('UnChecked');
            }
            else {
                CchkCloseOrder.SetEnabled(true);
            }
        }

        function CheckCloseOrder(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkZeroBal.SetCheckState('UnChecked');
            }
            else {
                CchkZeroBal.SetEnabled(true);
            }
        }
    </script>
    <script>
        function CallbackListofMaster_BeginCallback() {
            $("#drdExport").val(0);
        }
    </script>
    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }

        /*Rev 2.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet
        {
            max-width: 98% !important;
        }

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 2.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                GridList.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                GridList.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    GridList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    GridList.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>

     <div class="panel-heading">
       

         <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--Rev Subhra 24-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>

        <div>
            
        </div>
        
    </div>

    <%--Rev 2.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
             <%--Rev 2.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 2.0--%>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
             <div class="col-md-2 branch-selection-box">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--<div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" UseSubmitBehavior="False"/>
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" UseSubmitBehavior="False"/>                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                   <%-- <ClientSideEvents ValueChanged="BranchValuewiseledger" />--%>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>


            <div class="col-md-2 col-lg-2">
             <dxe:ASPxLabel ID="lbl_Product" style="color: #b5285f;font-weight: bold;" runat="server" Text="Customer:">
                </dxe:ASPxLabel>                   
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-2">
              
                <dxe:ASPxLabel ID="ASPxLabel1" style="color: #b5285f;font-weight: bold;" runat="server" Text="Salesman:">
                </dxe:ASPxLabel>                   
                <dxe:ASPxButtonEdit ID="txtSalesman" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesman" Width="100%" TabIndex="5">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){SalesmanButnClick();}" keydown="function(s,e){Salesman_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 2.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 2.0--%>
            </div>
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"></asp:Label>
                    </div>              
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
                <%--Rev 2.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 2.0--%>
            </div>
            <div class="clear"></div>
            <div class="col-md-2" id="divProj">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>

                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>

                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>

            <div class="col-md-2" style="padding-top:20px">
                <%--<div id="ckpar">
                    <asp:CheckBox runat="server" ID="chkZeroBal" style="color: #b5285f; font-weight: bold;" Text="Show Zero Balance" />
                </div>--%>
                <dxe:ASPxCheckBox runat="server" ID="chkZeroBal" Checked="false" Text="Show Zero Balance" ClientInstanceName="CchkZeroBal">
                    <ClientSideEvents CheckedChanged="CheckZeroBal" />
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="padding-top:20px">
                <dxe:ASPxCheckBox runat="server" ID="chkCloseOrder" Checked="false" Text="Show Close Order" ClientInstanceName="CchkCloseOrder">
                    <ClientSideEvents CheckedChanged="CheckCloseOrder" />
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="padding-top: 16px; padding-bottom: 15px;">
            <table>
                <tr>
                     
                    <td >
                     <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                     <% if (rights.CanExport)
                     { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>
                    </asp:DropDownList>
                        <% } %>
                    </td>
                </tr>
            </table>
            
            </div>
            <div class="clear"></div>            
            
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="1">                  
                       <div>
                           <dxe:ASPxGridView runat="server" ID="ShowGridList" ClientInstanceName="GridList" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                               Settings-HorizontalScrollBarMode="Visible" KeyFieldName="SEQ" OnSummaryDisplayText="ShowGridList_SummaryDisplayText"
                               DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="CallbackListofMaster_BeginCallback" >
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCHDESC" Width="150px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Order Number" FieldName="ORDERNO" Width="130px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                   <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPendSaleOrdDetails('<%#Eval("ORDERID") %>','<%#Eval("REPORTTYPE") %>')" class="pad">
                                            <%#Eval("ORDERNO")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Order Date" FieldName="ORDER_DATE" Width="100px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                 <%--Rev 1.0 Mantis: 0025616--%>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="PARTYORDNO" Width="130px" Caption="Party Order No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PARTYORDDATE" Caption="Party Order Date" Width="100px" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev 1.0 Mantis: 0025616--%>

                                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CUSTVENDNAME" Width="220px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="PROJ_NAME" Width="200px" Caption="Project Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="SALESMAN_NAME" Width="200px" Caption="Salesman Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="PRODCODE" Width="200px" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="PRODNAME" Width="220px" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Caption="UOM" Width="100px" VisibleIndex="11" HeaderStyle-CssClass="colDisable">                                        
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>                                    

                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Rate" Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="Actual Quantity" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="ACTUALPCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALMETERMULTQTY" Caption="Meter" Width="100px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Mature Quantity" VisibleIndex="17" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="MATUREPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATUREMETERQTY" Caption="Meter" Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATURESTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Balance Quantity" VisibleIndex="21" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="BALPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALMETERQTY" Caption="Meter" Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>
                                 
                                <dxe:GridViewDataTextColumn FieldName="ACTUAL_VALUES" Caption="Actual Values" Width="100px" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MATURE_VALUES" Caption="Mature Values" Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BALANCE_VALUES" Caption="Balance Values" Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                               
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="PRODNAME" SummaryType="Custom" Tag="SOItem_Name"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALPCSMULTQTY" SummaryType="Custom" Tag="ASO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALMETERMULTQTY" SummaryType="Custom" Tag="ASO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALSTOCKQTY" SummaryType="Custom" Tag="ASO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREPCSQTY" SummaryType="Custom" Tag="MSO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREMETERQTY" SummaryType="Custom" Tag="MSO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="MATURESTOCKQTY" SummaryType="Custom" Tag="MSO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="BALPCSQTY" SummaryType="Custom" Tag="BSO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="BALMETERQTY" SummaryType="Custom" Tag="BSO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="BALSTOCKQTY" SummaryType="Custom" Tag="BSO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUAL_VALUES" SummaryType="Custom" Tag="ASO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="MATURE_VALUES" SummaryType="Custom" Tag="MSO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="BALANCE_VALUES" SummaryType="Custom" Tag="BSO_Value"/>
                            </TotalSummary>

                        </dxe:ASPxGridView>

                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PENDINGSALEPURCHASEORDERCOLUMNAR_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

    <div>
    </div>

   <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" width="100%" placeholder="Search By Customer Name or Unique ID" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Customer Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->
    <!--Salesman Modal -->
    <div class="modal fade" id="SalesmanModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Salesman Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Salesmankeydown(event)" id="txtSalesmanSearch" width="100%" placeholder="Search By Salesman Name" />
                    <div id="SalesmanTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Salesman Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('SalesmanSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('SalesmanSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
    <!--Salesman Modal -->
    
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
     <asp:HiddenField ID="hdnCustomerId" runat="server" />
     <asp:HiddenField ID="hdnSalesmanId" runat="server" />
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsPendSaleOrdColumnarFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupPendSales" Height="500px"
        Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupPendSalesAfterHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>