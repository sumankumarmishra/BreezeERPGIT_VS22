﻿<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   09-05-2023      26068: Add Rate Difference Entry Vendor module design modification & check in small device
    2.0   Priti     V2.0.43   11-03-2024      0027304: While clicking on the charges Taxable value and GST amount getting changed.
=========================================================End Revision History===================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateDifferenceEntryVendor.aspx.cs" EnableEventValidation="false" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Activities.RateDifferenceEntryVendor" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <%-- <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js?var=1.0"></script>
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .pullleftClass.PODate {
            position: absolute;
            right: -3px;
            top: 16px;
        }

        .pullleftClass.POVendor {
            position: absolute;
            right: -3px;
            top: 16px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }


        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
            }

        #grid_DXMainTable > tbody > tr > td:last-child,
        #grid_DXMainTable > tbody > tr > td:last-child > div,
        #grid_DXMainTable > tbody > tr > td:nth-child(3),
        #grid_DXMainTable > tbody > tr > td:nth-child(20),
        #grid_DXMainTable > tbody > tr > td:nth-child(21) {
            display: none !important;
        }

        .classout {
            text-transform: none !important;
        }
    </style>
    <style type="text/css">
        #grid_DXStatus {
            display: none !important;
        }

        #aspxGridTax_DXStatus {
            display: none !important;
        }

        #gridTax_DXStatus {
            display: none !important;
        }

        #grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 18px;
        }

        .horizontallblHolder {
            height: auto;
            border: 1px solid #12a79b;
            border-radius: 3px;
            overflow: hidden;
        }

            .horizontallblHolder > table > tbody > tr > td {
                padding: 8px 10px;
                background: #ffffff;
                background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
                background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
            }

                .horizontallblHolder > table > tbody > tr > td:first-child {
                    background: #12a79b;
                    color: #fff;
                }

                .horizontallblHolder > table > tbody > tr > td:last-child {
                    font-weight: 500;
                    text-transform: uppercase;
                    color: #121212;
                }

        .dynamicPopupTbl > thead > tr > th, .dynamicPopupTbl > tbody > tr > td {
            padding: 5px 8px !important;
        }

        .dynamicPopupTbl > tbody > tr > td {
            cursor: pointer;
        }

        .dynamicPopupTbl.back > thead > tr > th {
            background: #4e64a6;
            color: #fff;
        }

        .dynamicPopupTbl.back > tbody > tr > td {
            background: #fff;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
        }

        .focusrow {
            background-color: #0000ff3d;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .gridStatic {
            margin-top: 25px;
        }

            .gridStatic .dynamicPopupTbl {
                width: 100%;
            }

        .bodBot {
            border-bottom: 1px solid #ccc;
            padding-bottom: 10px;
        }

        table.scroll {
            /* width: 100%; */ /* Optional */
            /* border-collapse: collapse; */
            border-spacing: 0;
            width: 100%;
        }

            table.scroll tbody,
            table.scroll thead {
                display: block;
            }

        thead tr th {
            height: 30px;
            line-height: 30px;
            /* text-align: left; */
        }

        table.scroll tbody {
            height: 250px;
            overflow-y: auto;
            overflow-x: hidden;
        }
    </style>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        function Project_gotFocus() {
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
            clookup_Project.ShowDropDown();
        }
        function clookup_Project_LostFocus() {

            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }
        //Hierarchy Start 
        function ProjectValueChange(s, e) {
            var projID = clookup_Project.GetValue();
            $.ajax({
                type: "POST",
                url: 'RateDifferenceEntryVendor.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }
        //Hierarchy End 

    </script>
    <script type="text/javascript">


        function SetLabelValue() {

            var totQtylbl = 0;
            var totPurPriceLbl = 0;
            var totAmtlbl = 0;
            var totCharlbl = 0;
            var totnetAmtlbl = 0;
            //grid.batchEditApi.EndEdit();
            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {

                if (globalRowIndex != i) {
                    totQtylbl = totQtylbl + parseFloat(grid.GetRow(i).children[5].innerText);
                    totPurPriceLbl = totPurPriceLbl + parseFloat(grid.GetRow(i).children[7].innerText);
                    totAmtlbl = totAmtlbl + parseFloat(grid.GetRow(i).children[9].innerText);
                    totCharlbl = totCharlbl + parseFloat(grid.GetRow(i).children[10].innerText);
                    totnetAmtlbl = totnetAmtlbl + parseFloat(grid.GetRow(i).children[11].innerText);
                }
                else {
                    totQtylbl = totQtylbl + parseFloat(grid.GetEditor("Quantity").GetValue());
                    totPurPriceLbl = totPurPriceLbl + parseFloat(grid.GetEditor("SalePrice").GetValue());
                    totAmtlbl = totAmtlbl + parseFloat(grid.GetEditor("Amount").GetValue());
                    totCharlbl = totCharlbl + parseFloat(grid.GetEditor("TaxAmount").GetValue());
                    totnetAmtlbl = totnetAmtlbl + parseFloat(grid.GetEditor("TotalAmount").GetValue());
                }
            }

            cTotalQty.SetText(totQtylbl.toFixed(2));
            cTotalNPrice.SetText(totPurPriceLbl.toFixed(2));
            cTotalNAmount.SetText(totAmtlbl.toFixed(2));
            cTotalNCharges.SetText(totCharlbl.toFixed(2));
            cTotalNAmt.SetText(totnetAmtlbl.toFixed(2));
        }

        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }
        }


        function GlobalBillingShippingEndCallBack() {

            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                var branchid = $('#ddl_Branch').val();
                var type = "PI";
                if (gridquotationLookup.GetValue() != null) {
                    var key = GetObjectID('hdnCustomerId').value;
                    // var key = cCustomerComboBox.GetValue();                   
                    if (key != null && key != '') {
                        cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);

                        //grid.PerformCallback('GridBlank');
                        deleteAllRows();
                        grid.AddNewRow();
                        grid.GetEditor('SrlNo').SetValue('1');


                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "", "");
                    }
                }
                else {
                    // var key = cCustomerComboBox.GetValue();
                    var key = GetObjectID('hdnCustomerId').value;
                    if (key != null && key != '') {
                        cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                        //   cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);

                    }
                }
            }
        }

        function deleteTax(Action, srl, productid) {
            var OtherDetail = {};
            OtherDetail.Action = Action;
            OtherDetail.srl = srl;
            OtherDetail.prodid = productid;


            $.ajax({
                type: "POST",
                url: "RateDifferenceEntryVendor.aspx/taxUpdatePanel_Callback",
                data: JSON.stringify(OtherDetail),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var Code = msg.d;
                    if (Code != null) {
                    }
                    if (productid != "") {
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 5);
                        }, 600)
                    }
                }
            });
        }





    </script>
    <script>


        var ProductGetQuantity = "0";
        var ProductGetTotalAmount = "0";
        var ProductSaleprice = "0";
        var globalNetAmount = 0;
        var ProductDiscount = "0";
        function AmtGotFocus() {
            globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
            ProductGetTotalAmount = globalNetAmount;


        }
        function AmtTextChange(s, e) {


            var grossamt = grid.GetEditor('Amount').GetValue();
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));
            var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

            if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                //grid.GetEditor('TaxAmount').SetValue(0);
                ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());



            }

            //TDSDetail();
        }
        //contactperson phone
        function acpContactPersonPhoneEndCall(s, e) {
            if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
                cacpContactPersonPhone.cpPhone = null;

            }
        }

        //contactperson phones

        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }
        var SimilarProjectStatus = "0";
        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();


            var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

            debugger;
            if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {

                var quote_Id = "";
                // otherDets.quote_Id = quote_Id;
                for (var i = 0; i < quotetag_Id.length; i++) {
                    if (quote_Id == "") {
                        quote_Id = quotetag_Id[i];
                    }
                    else {
                        quote_Id += ',' + quotetag_Id[i];
                    }
                }
                var Doctype = $("#rdl_PurchaseInvoice").find(":checked").val();
                debugger;
                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryVendor.aspx/DocWiseSimilarProjectCheck",
                    data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        SimilarProjectStatus = msg.d;
                        debugger;
                        if (SimilarProjectStatus != "1") {
                            $("#txt_InvoiceDate").val("");
                            jAlert("Unable to procceed. Project are for the selected Document(s) are different.");

                            return false;

                        }
                    }
                });
            }


        }

        function componentEndCallBack(s, e) {
            gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                //  OnAddNewClick();  kaushik 3/7/2017  avoid extra rows
            }

            if (cQuotationComponentPanel.cpstockoutbranchId != null) {
                document.getElementById('ddl_StockOutBranch').value = cQuotationComponentPanel.cpstockoutbranchId;

                cQuotationComponentPanel.cpstockoutbranchId = null;
            }
            if (cQuotationComponentPanel.cpDate != null) {


                $('#<%=txt_InvoiceDate.ClientID %>').val(cQuotationComponentPanel.cpDate);
                cQuotationComponentPanel.cpDate = null;
            }

            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;

                var SpliteDetails = details.split("~");
                var Reference = SpliteDetails[0];
                var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
                var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];
                var CurrencyRate = SpliteDetails[4];
                var Contact_person_id = SpliteDetails[5];
                var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];
                var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];

                $('#<%=txt_Refference.ClientID %>').val(Reference);
                ctxt_Rate.SetValue(CurrencyRate);
                cddl_AmountAre.SetValue(Tax_option);
                if (Tax_option == 1) {

                    grid.GetEditor('TaxAmount').SetEnabled(true);
                    cddlVatGstCst.SetEnabled(false);
                    cddlVatGstCst.SetSelectedIndex(0);
                    cbtn_SaveRecords.SetVisible(true);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }

                }
                else if (Tax_option == 2) {
                    grid.GetEditor('TaxAmount').SetEnabled(true);

                    cddlVatGstCst.SetEnabled(true);
                    cddlVatGstCst.PerformCallback('2');
                    cddlVatGstCst.Focus();
                    cbtn_SaveRecords.SetVisible(true);
                }
                else if (Tax_option == 3) {

                    grid.GetEditor('TaxAmount').SetEnabled(false);


                    cddlVatGstCst.SetSelectedIndex(0);
                    cddlVatGstCst.SetEnabled(false);
                    cbtn_SaveRecords.SetVisible(false);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }


                }

                cddlVatGstCst.PerformCallback('Tax-code' + '~' + Tax_Code)
                document.getElementById('ddl_Currency').value = Currency_Id;
                document.getElementById('ddl_SalesAgent').value = SalesmanId;
                if (Contact_person_id != "0" && Contact_person_id != "")
                { cContactPerson.SetValue(Contact_person_id); }

            }
        }

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }

        function BindOrderProjectdata(OrderId, TagDocType) {
            debugger;
            clookup_Project.SetEnabled(false);
            var OtherDetail = {};

            OtherDetail.OrderId = OrderId;
            OtherDetail.TagDocType = TagDocType;


            if ((OrderId != null) && (OrderId != "")) {

                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryVendor.aspx/SetProjectCode",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var Code = msg.d;

                        clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                        clookup_Project.SetEnabled(false);
                    }
                });

                //Hierarchy Start Tanmoy
                var projID = clookup_Project.GetValue();

                $.ajax({
                    type: "POST",
                    url: 'RateDifferenceEntryVendor.aspx/getHierarchyID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ ProjID: projID }),
                    success: function (msg) {
                        var data = msg.d;
                        $("#ddlHierarchy").val(data);
                    }
                });
                //Hierarchy End Tanmoy
            }
        }
        function PerformCallToGridBind() {

            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

            grid.PerformCallback('BindGridOnQuotation' + '~' + '@' + '~' + type);
            cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Invoiceupdate');
            cProductsPopup.Hide();
            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if (quote_Id.length > 0) {
                BindOrderProjectdata(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
            }

            return false;
        }
        function PurchaseInvoiceNumberChanged() {
            // ;
            var quote_Id = gridquotationLookup.GetValue();

            if (SimilarProjectStatus != "-1") {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

                if (quote_Id != null) {
                    var arr = quote_Id.split(',');
                    if (arr.length > 1) {
                        $('#<%=txt_InvoiceDate.ClientID %>').val('Multiple Dates');

                    }
                    else {
                        if (arr.length == 1) {
                            cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id);
                        }
                        else {
                            $('#<%=txt_InvoiceDate.ClientID %>').val('');
                        }
                    }
                }
                else {
                    $('#<%=txt_InvoiceDate.ClientID %>').val('');

                }

                if (quote_Id != null) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + type);
                    cProductsPopup.Show();
                }
                else {
                    grid.PerformCallback('RemoveDisplay');
                }
            }
        }
        //.............Available Stock Div Show............................



        function ProductsGotFocus(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);



            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
            }
        }


        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";

        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);
                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
            }
        }

        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                divAvailableStk.style.display = "block";
                divpopupAvailableStock.style.display = "block";
                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;

                cCmbWarehouse.cpstock = null;
            }
        }
        //................Available Stock Div Show....................


        (function (global) {

            if (typeof (global) === "undefined") {
                throw new Error("window is undefined");
            }

            var _hash = "!";
            var noBackPlease = function () {
                global.location.href += "#";

                // making sure we have the fruit available for juice (^__^)
                global.setTimeout(function () {
                    global.location.href += "!";
                }, 50);
            };

            global.onhashchange = function () {
                if (global.location.hash !== _hash) {
                    global.location.hash = _hash;
                }
            };

            global.onload = function () {
                noBackPlease();

                // disables backspace on page except on input fields and textarea..
                document.body.onkeydown = function (e) {
                    var elm = e.target.nodeName.toLowerCase();
                    if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                        e.preventDefault();
                    }
                    // stopping event bubbling up the DOM tree..
                    e.stopPropagation();
                };
            }

        })(window);

        var isCtrl = false;

        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }

        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") {
                //  alert('kkk'); //run code for Alt + n -- ie, Save & New
                StopDefaultAction(e);
                Save_ButtonClick();
            }
            else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!   

                StopDefaultAction(e);
                SaveExit_ButtonClick();
            }
            else if (event.keyCode == 85 && event.altKey == true) { //run code for alt+U -- ie, Save & Exit!   

                StopDefaultAction(e);
                OpenUdf();
            }
            else if (event.keyCode == 84 && event.altKey == true && getUrlVars().req != "V") { //run code for alt+T -- ie, Save & Exit!   

                StopDefaultAction(e);
                Save_TaxesClick();
            }
            else if (event.keyCode == 79 && event.altKey == true) { //run code for alt + O -- ie, For billing shipping Samrat!   
                StopDefaultAction(e);
                if (page.GetActiveTabIndex() == 1) {
                    fnSaveBillingShipping();
                }
            }
        }
        document.onkeyup = function (e) {

            if (event.altKey == true) {
                switch (event.keyCode) {
                    case 83:
                        if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                            if (getUrlVars().req != "V") {
                                SaveVehicleControlData();
                            }
                        }
                        break;
                    case 67:
                        modalShowHide(0);
                        break;
                    case 82:
                        modalShowHide(1);
                        $('body').on('shown.bs.modal', '#exampleModal', function () {
                            $('input:visible:enabled:first', this).focus();
                        })
                        break;
                    case 78:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            Save_ButtonClick();
                        }
                        break;
                    case 88:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            SaveExit_ButtonClick();
                        }
                        break;
                    case 120:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            SaveExit_ButtonClick();
                        }
                        break;
                    case 84:
                        StopDefaultAction(e);
                        if (getUrlVars().req != "V") {
                            Save_TaxesClick();
                        }
                        break;
                    case 85:
                        OpenUdf();
                        break;
                }
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
    </script>

    <%--Debu Section--%>
    <script type="text/javascript">

        function ShowTaxPopUp(type) {
            if (type == "IY") {
                $('#ContentErrorMsg').hide();
                $('#content-6').show();


                if (ccmbGstCstVat.GetItemCount() <= 1) {
                    $('.InlineTaxClass').hide();
                } else {
                    $('.InlineTaxClass').show();
                }
                if (cgridTax.GetVisibleRowsOnPage() < 1) {
                    $('.cgridTaxClass').hide();

                } else {
                    $('.cgridTaxClass').show();
                }

                if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
                    $('#ContentErrorMsg').show();
                    $('#content-6').hide();
                }
            }
            if (type == "IN") {
                $('#ErrorMsgCharges').hide();
                $('#content-5').show();

                if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
                    $('.chargesDDownTaxClass').hide();
                } else {
                    $('.chargesDDownTaxClass').show();
                }
                if (gridTax.GetVisibleRowsOnPage() < 1) {
                    $('.gridTaxClass').hide();

                } else {
                    $('.gridTaxClass').show();
                }

                if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
                    $('#ErrorMsgCharges').show();
                    $('#content-5').hide();
                }
            }
        }

        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;


        }

        function OnBatchEditEndEditing(s, e) {
            var ProductIDColumn = s.GetColumnByField("ProductID");
            if (!e.rowValues.hasOwnProperty(ProductIDColumn.index))
                return;
            var cellInfo = e.rowValues[ProductIDColumn.index];
            if (cCmbProduct.GetSelectedIndex() > -1 || cellInfo.text != cCmbProduct.GetText()) {
                cellInfo.value = cCmbProduct.GetValue();
                cellInfo.text = cCmbProduct.GetText();
                cCmbProduct.SetValue(null);
            }
        }

        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
        }
        function taxAmountLostFocus(s, e) {
            var finalTaxAmt = parseFloat(s.GetValue());
            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
            } else {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
            }


            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            //Set Running Total
            SetRunningTotal();

            RecalCulateTaxTotalAmountInline();
        }

        function cmbGstCstVatChange(s, e) {


            SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);
            $('.RecalculateInline').hide();
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            if (s.GetValue().split('~')[2] == 'G') {
                ProdAmt = parseFloat(clblTaxProdGrossAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'N') {
                ProdAmt = parseFloat(clblProdNetAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'O') {
                //Check for Other Dependecy
                $('.RecalculateInline').show();
                ProdAmt = 0;
                var taxdependentName = s.GetValue().split('~')[3];
                for (var i = 0; i < taxJson.length; i++) {
                    cgridTax.batchEditApi.StartEdit(i, 3);
                    var gridTaxName = cgridTax.GetEditor("Taxes_Name").GetText();
                    gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                    if (gridTaxName == taxdependentName) {
                        ProdAmt = cgridTax.GetEditor("Amount").GetValue();
                    }
                }
            }
            else if (s.GetValue().split('~')[2] == 'R') {
                ProdAmt = GetTotalRunningAmount();
                $('.RecalculateInline').show();
            }

            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
            ctxtGstCstVat.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));

            //tax others
            SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
            gstcstvatGlobalName = ccmbGstCstVat.GetText();
        }


        //for tax and charges
        var GlobalCurChargeTaxAmt;
        var ChargegstcstvatGlobalName;
        function ChargecmbGstCstVatChange(s, e) {

            SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
            $('.RecalculateCharge').hide();
            var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

            //Set ProductAmount
            if (s.GetValue().split('~')[2] == 'G') {
                ProdAmt = parseFloat(ctxtProductAmount.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'N') {
                ProdAmt = parseFloat(clblProdNetAmt.GetValue());
            }
            else if (s.GetValue().split('~')[2] == 'O') {
                //Check for Other Dependecy
                $('.RecalculateCharge').show();
                ProdAmt = 0;
                var taxdependentName = s.GetValue().split('~')[3];
                for (var i = 0; i < taxJson.length; i++) {
                    gridTax.batchEditApi.StartEdit(i, 3);
                    var gridTaxName = gridTax.GetEditor("TaxName").GetText();
                    gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                    if (gridTaxName == taxdependentName) {
                        ProdAmt = gridTax.GetEditor("Amount").GetValue();
                    }
                }
            }
            else if (s.GetValue().split('~')[2] == 'R') {
                $('.RecalculateCharge').show();
                ProdAmt = GetChargesTotalRunningAmount();
            }


            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

            var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
            ctxtGstCstVatCharge.SetValue(calculatedValue);

            var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
            ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

            //tax others
            SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

            //set Total Amount
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
        }




        function GetChargesTotalRunningAmount() {
            var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
            for (var i = 0; i < chargejsonTax.length; i++) {
                gridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }

        function chargeCmbtaxClick(s, e) {
            GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
            ChargegstcstvatGlobalName = s.GetText();
        }


        function acbpCrpUdfEndCall(s, e) {
            debugger;
            //   LoadingPanel.Hide();
            if (cacbpCrpUdf.cpUDF) {
                if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true") {
                    // OnAddNewClick();
                    grid.AddNewRow();
                    grid.UpdateEdit();
                    //  LoadingPanel.Hide();
                    // OnAddNewClick();
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cprefCreditNoteNo = null;
                }
                else if (cacbpCrpUdf.cpUDF == "false") {
                    LoadingPanel.Hide();
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cprefCreditNoteNo = null;
                }

                else if (cacbpCrpUdf.cprefCreditNoteNo == "false") {
                    LoadingPanel.Hide();
                    jAlert("Ref. Credit Note No. already exist for the selected vendor.", "Alert", function () { });


                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cprefCreditNoteNo = null;
                }
                else {
                    LoadingPanel.Hide();
                    jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                    cacbpCrpUdf.cprefCreditNoteNo = null;
                }
            }



        }


        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
        }
        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt));
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt));
                    GlobalCurTaxAmt = 0;
                }
                else {
                    s.SetText("");
                }

            } else {
                var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                if (s.GetValue() == null) {
                    s.SetValue(0);
                }

                if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

        }

        function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
            for (var i = 0; i < taxJson.length; i++) {
                if (taxJson[i].applicableBy == name) {
                    cgridTax.batchEditApi.StartEdit(i, 3);
                    cgridTax.GetEditor('calCulatedOn').SetValue(amt);

                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var s = cgridTax.GetEditor("TaxField");
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            cgridTax.batchEditApi.EndEdit();

        }



        function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
            name = name.substring(0, name.length - 3).trim();
            for (var i = 0; i < chargejsonTax.length; i++) {
                if (chargejsonTax[i].applicableBy == name) {
                    gridTax.batchEditApi.StartEdit(i, 3);
                    gridTax.GetEditor('calCulatedOn').SetValue(amt);

                    var totLength = gridTax.GetEditor("TaxName").GetText().length;
                    var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
                    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
                    var s = gridTax.GetEditor("Percentage");
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            gridTax.batchEditApi.EndEdit();
        }

        function RecalCulateTaxTotalAmountInline() {

            var totalInlineTaxAmount = 0;
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                if (sign == '(+)') {
                    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                } else {
                    totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
                }

                cgridTax.batchEditApi.EndEdit();
            }

            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
        }

        function txtPercentageLostFocus(s, e) {
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            if (s.GetText().trim() != '') {

                if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
                    //Checking Add or less
                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

                    //Call for Running Total
                    SetRunningTotal();

                } else {
                    s.SetText("");
                }
            }

            RecalCulateTaxTotalAmountInline();
        }

        function SetRunningTotal() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                if (taxJson[i].applicableOn == "R") {
                    cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var thisRunningAmt = 0;
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                      //  ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);



                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                }
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }
        }

        function GetTotalRunningAmount() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }

            return runningTot;
        }



        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
        }


        function txtTax_TextChanged(s, i, e) {
            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        }
        var pchange = 'N';
        function taxAmtButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

                    if (ProductID.trim() != "") {
                        //   ;
                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();


                        var comid = grid.GetRowKey(globalRowIndex);

                        // alert(comid);
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();
                        var StockQuantity = strMultiplier * QuantityValue;

                        var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                        //REV 2.0
                        //clblTaxProdGrossAmt.SetText(DecimalRoundoff(strSalePrice, 2) * DecimalRoundoff(QuantityValue, 2));
                        clblTaxProdGrossAmt.SetText(DecimalRoundoff(Amount, 2));
                        //REV 2.0 END
                        //chinmopy edited Net Amount start
                        var GridDis = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                        var DiscountForNet = DecimalRoundoff(DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) * DecimalRoundoff((GridDis)/100,2), 2);
                        var DiscountValue = DecimalRoundoff(DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) - DecimalRoundoff(DiscountForNet, 2), 2);
                        //debugger;
                        //clblProdNetAmt.SetText(parseFloat((Math.round(Amount * 100)) / 100).toFixed(2));
                        clblProdNetAmt.SetValue(DiscountValue);
                        //End
                        //document.getElementById('HdProdGrossAmt').value = DecimalRoundoff(grid.GetEditor('Amount').GetValue(),2);
                        //document.getElementById('HdProdNetAmt').value = DecimalRoundoff(Amount,2);
                        document.getElementById('HdProdGrossAmt').value = DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2);
                        document.getElementById('HdProdNetAmt').value = DecimalRoundoff(clblProdNetAmt.GetValue(), 2);

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            // var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            var GridDis = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                            var DiscountForNet = DecimalRoundoff(DecimalRoundoff(clblTaxProdGrossAmt.GetValue(), 2) * DecimalRoundoff((GridDis) / 100, 2), 2);
                            clblTaxDiscount.SetText(DiscountForNet);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            $('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            $('.gstNetAmount').show();
                            //Set Gross Amount with GstValue
                            //Get The rate of Gst
                            var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                            if (gstRate) {
                                if (gstRate != 0) {
                                    var gstDis = (gstRate / 100) + 1;
                                    if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                        $('.gstNetAmount').hide();
                                        clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                        document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                        clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();
                                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                        clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                        clblTaxableGross.SetText("");
                                    }
                                }


                            } else {
                                $('.gstGrossAmount').hide();
                                $('.gstNetAmount').hide();
                                clblTaxableGross.SetText("");
                                clblTaxableNet.SetText("");
                            }
                        }
                        else if (cddl_AmountAre.GetValue() == "1") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                            ////###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';

                            var shippingStCode = '';
                            shippingStCode = cbsSCmbState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();


                            //Debjyoti 09032017
                            if (shippingStCode.trim() != '') {
                                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                                    //Check if gstin is blank then delete all tax
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                            //if its state is union territories then only UTGST will apply
                                            if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                                    cmbCount--;
                                                }
                                            }
                                            else {
                                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                                    cmbCount--;
                                                }
                                            }
                                        } else {
                                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                                ccmbGstCstVat.RemoveItem(cmbCount);
                                                cmbCount--;
                                            }
                                        }
                                    } else {
                                        //remove tax because GSTIN is not define
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            }




                        }
                        //End here


                        var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";


                        //alert(purchasepricegotfocus);
                        //alert(purchasepricelostfocus);

                        if (parseFloat(purchasepricegotfocus) != parseFloat(purchasepricelostfocus)) {

                            pchange = 'Y';
                            //if (ProductSaleprice == '0')
                            //{ pchange = 'N'; }
                            //else
                            //{ pchange = 'Y'; }

                        }
                        else { pchange = 'N'; }


                        if (globalRowIndex > -1) {

                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue() + '~' + comid + '~' + pchange);
                        } else {

                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
        }
        function taxAmtButnClick1(s, e) {
            console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }

        function BatchUpdate() {


            if (cgridTax.GetVisibleRowsOnPage() > 0) {
                cgridTax.UpdateEdit();
            }
            else {
                cgridTax.PerformCallback('SaveGST');
            }
            return false;
        }

        var taxJson;
        function cgridTax_EndCallBack(s, e) {

            $('.cgridTaxClass').show();

            cgridTax.StartEditRow(0);


            //check Json data
            if (cgridTax.cpJsonData) {
                if (cgridTax.cpJsonData != "") {
                    taxJson = JSON.parse(cgridTax.cpJsonData);
                    cgridTax.cpJsonData = null;
                }
            }
            //End Here

            if (cgridTax.cpComboCode) {
                if (cgridTax.cpComboCode != "") {
                    if (cddl_AmountAre.GetValue() == "1") {
                        var selectedIndex;
                        for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                            if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                                selectedIndex = i;
                            }
                        }
                        if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                            ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                        }
                        cmbGstCstVatChange(ccmbGstCstVat);
                        cgridTax.cpComboCode = null;
                    }
                }
            }


            if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
                ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
                var ddValue = parseFloat(ctxtGstCstVat.GetValue());
                ctxtTaxTotAmt.SetValue(gridValue + ddValue);
                cgridTax.cpUpdated = "";
            }
            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 16);
                grid.GetEditor("TaxAmount").SetValue(totAmt);
                grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));

            }

            if (cgridTax.GetVisibleRowsOnPage() == 0) {
                $('.cgridTaxClass').hide();
                ccmbGstCstVat.Focus();
            }
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section

            SetRunningTotal();
            ShowTaxPopUp("IY");
        }

        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }

    </script>
    <%--Debu Section End--%>

    <%--Sam Section Start--%>
    <script type="text/javascript">


        function refCreditNoteDtMandatorycheck() {

            var PoRefNote = $('#<%=txt_refCreditNoteNo.ClientID %>').val();
            var Podt = cdt_refCreditNoteDt.GetValue();
            if (Podt != null) {


                if (PoRefNote != null && PoRefNote != '') {
                    $('#MandatorysRefCreditNoteno').attr('style', 'display:none');
                    var sdate = cdt_refCreditNoteDt.GetValue();
                    var edate = tstartdate.GetValue();

                    var startDate = new Date(sdate);
                    var endDate = new Date(edate);
                    //if (startDate > endDate) {
                    //    //LoadingPanel.Hide();
                    //    //flag = false;
                    //    $('#MandatoryREgSDate').attr('style', 'display:block');
                    //}
                    //else { $('#MandatoryREgSDate').attr('style', 'display:none'); }
                }
                else {
                    $('#MandatorysRefCreditNoteno').attr('style', 'display:block');


                    var sdate = cdt_refCreditNoteDt.GetValue();
                    var edate = tstartdate.GetValue();

                    var startDate = new Date(sdate);
                    var endDate = new Date(edate);
                    //if (startDate > endDate) {
                    //    //LoadingPanel.Hide();
                    //    //flag = false;
                    //    $('#MandatoryREgSDate').attr('style', 'display:block');
                    //}
                    //else { $('#MandatoryREgSDate').attr('style', 'display:none'); }
                }

            }


        }

        $(document).ready(function () {
            if ($("#hdnADDEditMode").val() == 'Edit') {
                if ($("#hdnCustomerId").val() != "") {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }
            }
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }
            $('#ApprovalCross').click(function () {
                //  ;
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh()();
            })
        })

             <%--kaushik 24-2-2017--%>
        function UniqueCodeCheck() {

            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                $('#<%=txt_PLQuoteNo.ClientID %>').focus();
            }
            else {
                var ReturnNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
                if (ReturnNo != '') {

                    var SchemaLength = GetObjectID('hdnSchemaLength').value;
                    var x = parseInt(SchemaLength);
                    var y = parseInt(ReturnNo.length);

                    if (y > x) {
                        alert('Rate Difference Entry Vendor No length cannot be more than ' + x);
                        $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                        $('#<%=txt_PLQuoteNo.ClientID %>').focus();

                    }
                    else {
                        var CheckUniqueCode = false;
                        $.ajax({
                            type: "POST",
                            url: "RateDifferenceEntryVendor.aspx/CheckUniqueCode",
                            data: JSON.stringify({ ReturnNo: ReturnNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                CheckUniqueCode = msg.d;
                                if (CheckUniqueCode == true) {
                                    alert('Please enter unique Rate Difference Entry Vendor  No');
                                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                                    $('#<%=txt_PLQuoteNo.ClientID %>').focus();
                                }
                                else {
                                    $('#MandatorysQuoteno').attr('style', 'display:none');
                                }
                            }

                        });
                    }
                }
            }
        }


        function GetContactPersonPhone(e) {
            var key = cContactPerson.GetValue();
            cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
        }


        function cmbRequestTextChange() {
            var type = "PI";
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var key = GetObjectID('hdnCustomerId').value;
            // var key = cCustomerComboBox.GetValue();
            if (key != null && key != '') {
                //  cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

            }
            $('#<%=txt_InvoiceDate.ClientID %>').val('');
            //grid.PerformCallback('GridBlank');    //kaushik 30_6_2017 becaues two rows bond in grid
        }

        function GetContactPerson(e) {
            var startDate = new Date();
            var branchid = $('#ddl_Branch').val();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            $('#<%=txt_refCreditNoteNo.ClientID %>').val('');
            cdt_refCreditNoteDt.SetText('');

            var type = "PI";
            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();
                        // var key = cCustomerComboBox.GetValue();

                        var key = GetObjectID('hdnCustomerId').value;
                        if (key != null && key != '') {
                            LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PR');
                            GetObjectID('hdnCustomerId').value = key;
                            page.tabs[0].SetEnabled(true);
                            page.tabs[1].SetEnabled(true);

                            $('.dxeErrorCellSys').addClass('abc');

                            if (key != null && key != '') {
                                // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

                            }


                            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                clearTransporter();
                            }

                            $('#<%=txt_InvoiceDate.ClientID %>').val('');

                        }

                    } else {

                        ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                        var Vendorid = $('#<%=hdnCustomerId.ClientID %>').val();
                        ctxtCustName.PerformCallback(Vendorid);

                    }
                });
            }
            else {
                // var key = cCustomerComboBox.GetValue();  
                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    $('#<%=txt_InvoiceDate.ClientID %>').val('');

                    page.SetActiveTabIndex(0);
                    $('.dxeErrorCellSys').addClass('abc');

                    //###### Added By : Samrat Roy ##########

                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PR');
                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);

                    //###### END : Samrat Roy : END ########## 

                    GetObjectID('hdnAddressDtl').value = '0';
                }

            }

        }
        $(document).ready(function () {

            if ($("#hdnADDEditMode").val() != "ADD")
            {
                tstartdate.SetEnabled(false);
            }

            $('#<%=txt_PLQuoteNo.ClientID %>').on('change', function () {
                UniqueCodeCheck();
            });

            $('#<%=txt_refCreditNoteNo.ClientID %>').on('change', function () {
                DuplicateCreditNoteNo();
            });

            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != null) {
                if (schemaid == '') {
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
            }
        }
            $('#ddl_numberingScheme').change(function () {
                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];
                var branchID = NoSchemeTypedtl.toString().split('~')[3];
                document.getElementById('ddl_Branch').value = branchID;

                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];
                var dt = new Date();
                tstartdate.SetDate(dt);
                if (dt < new Date(fromdate)) {
                    tstartdate.SetDate(new Date(fromdate));
                }
                if (dt > new Date(todate)) {
                    tstartdate.SetDate(new Date(todate));
                }
                tstartdate.SetMinDate(new Date(fromdate));
                tstartdate.SetMaxDate(new Date(todate));
                document.getElementById('ddl_StockOutBranch').value = branchID;
                //  ccmbRequest.PerformCallback(branchID);
                if (NoSchemeType == '1') {
                    $('#<%=txt_PLQuoteNo.ClientID %>').val('Auto');
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                    
                    tstartdate.Focus();

                    if($("#HdnBackDatedEntryPurchaseGRN").val()=="0")
                    {
                        tstartdate.SetEnabled(false);
                    }
                    else
                    {
                        tstartdate.SetEnabled(true);
                    }

                }
                else if (NoSchemeType == '0') {
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = false;
                    tstartdate.SetEnabled(true);
                    $('#<%=txt_PLQuoteNo.ClientID %>').maxLength = quotelength;
                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                    $('#<%=txt_PLQuoteNo.ClientID %>').focus();

                }
                else {

                    $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                    document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                    tstartdate.SetEnabled(true);

                }

                //clookup_Project.gridView.Refresh();
            });


            $('#ddl_Currency').change(function () {

                var CurrencyId = $(this).val();
                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~")[0];
                if ($("#ddl_Currency").val() == basedCurrency) {
                    ctxt_Rate.SetValue("");
                    ctxt_Rate.SetEnabled(false);
                }
                else {
                    if (basedCurrency != CurrencyId) {
                        if (LocalCurrency != null) {
                            if (CurrencyId != '0') {
                                $.ajax({
                                    type: "POST",
                                    url: "SalesInvoice.aspx/GetCurrentConvertedRate",
                                    data: "{'CurrencyId':'" + CurrencyId + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var currentRate = msg.d;
                                        if (currentRate != null) {

                                            ctxt_Rate.SetValue(currentRate);
                                        }
                                        else {
                                            ctxt_Rate.SetValue('1');
                                        }
                                        ReBindGrid_Currency();
                                    }
                                });
                            }
                            else {
                                ctxt_Rate.SetValue("1");
                                ReBindGrid_Currency();
                            }
                        }
                    }
                    else {
                        ctxt_Rate.SetValue("1");
                        ReBindGrid_Currency();
                    }
                    ctxt_Rate.SetEnabled(true);
                }



            });
        });

        function SetFocusonDemand(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == '1' || key == '3') {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            }
            else if (key == '2') {
                cddlVatGstCst.Focus();
            }

        }

        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.SetSelectedIndex(0);
                cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('ProductID').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('TaxAmount').SetEnabled(false);

                cddlVatGstCst.SetSelectedIndex(0);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

        }

        //Date Function Start

        function Startdate(s, e) {
            grid.batchEditApi.EndEdit();
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }


            var t = s.GetDate();
            ccmbGstCstVat.PerformCallback(t);
            ccmbGstCstVatcharge.PerformCallback(t);
            //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            deleteTax('DeleteAllTax', "", "", "");
            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                $('#<%=HdUpdateMainGrid.ClientID %>').val('True');
                cacbpCrpUdf.PerformCallback();
                //kaushik
            }

            if (t == "")
            { $('#MandatorysDate').attr('style', 'display:block'); }
            else { $('#MandatorysDate').attr('style', 'display:none'); }
        }
        function Enddate(s, e) {

            var t = s.GetDate();
            if (t == "")
            { $('#MandatoryEDate').attr('style', 'display:block'); }
            else { $('#MandatoryEDate').attr('style', 'display:none'); }



            var sdate = tstartdate.GetValue();
            var edate = tenddate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);

            if (startDate > endDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

        //Date Function End

        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>
    <%--Sam Section End--%>
    <%--Sudip--%>
    <script>
        var IsProduct = "";
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;

        function GridCallBack() {

            $('#ddl_numberingScheme').focus();
            //grid.PerformCallback('Display');
        }

        function ReBindGrid_Currency() {
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                cacbpCrpUdf.PerformCallback();
                grid.PerformCallback('CurrencyChangeDisplay');
            }
        }

        function ProductsCombo_SelectedIndexChanged(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");

            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            strProductName = strDescription;

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);

            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            //Debjyoti
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());

        }
        function cmbContactPersonEndCall(s, e) {
            LoadingPanel.Hide();
            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                cdt_SaleInvoiceDue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpReferredBy != null) {
                var ReferredBy = cContactPerson.cpReferredBy;
                $('#<%=txt_Refference.ClientID %>').val(ReferredBy);
                cContactPerson.cpReferredBy = null;
            }


            if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {


                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');

                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = cContactPerson.cpOutstanding;

                cContactPerson.cpOutstanding = null;
            }
            else if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {
                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
                $("#<%=divGSTN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }
            else {
                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:none');
                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = '';
            }
    }

    function OnEndCallback(s, e) {

        var value = document.getElementById('hdnRefreshType').value;

        //Debjyoti Check grid needs to be refreshed or not
        if ($('#<%=HdUpdateMainGrid.ClientID %>').val() == 'True') {
            $('#<%=HdUpdateMainGrid.ClientID %>').val('False');
            grid.PerformCallback('DateChangeDisplay');
        }



        if (grid.cpInvoiceDetails != null) {
            var details = grid.cpInvoiceDetails;
            grid.cpInvoiceDetails = null;

            var SpliteDetails = details.split("~");
            var Reference = SpliteDetails[0];
            var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
            var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];
            var CurrencyRate = SpliteDetails[4];
            var Contact_person_id = SpliteDetails[5];
            var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];
            var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];

            $('#<%=txt_Refference.ClientID %>').val(Reference);
            ctxt_Rate.SetValue(CurrencyRate);
            cddl_AmountAre.SetValue(Tax_option);
            cddl_AmountAre.SetEnabled(false);
            if (Tax_option == 1) {

                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.SetSelectedIndex(0);
                cbtn_SaveRecords.SetVisible(true);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (Tax_option == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (Tax_option == 3) {

                grid.GetEditor('TaxAmount').SetEnabled(false);


                cddlVatGstCst.SetSelectedIndex(0);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

            cddlVatGstCst.PerformCallback('Tax-code' + '~' + Tax_Code)
            document.getElementById('ddl_Currency').value = Currency_Id;
            document.getElementById('ddl_SalesAgent').value = SalesmanId;
            if (Contact_person_id != "0" && Contact_person_id != "")
            { cContactPerson.SetValue(Contact_person_id); }

        }

        if (grid.cpInvoiceRunningBalance) {
            var RunningBalance = grid.cpInvoiceRunningBalance;
            var RunningSpliteDetails = RunningBalance.split("~");
            grid.cpInvoiceRunningBalance = null;

            var SUM_Price = RunningSpliteDetails[0];
            var SUM_Amount = RunningSpliteDetails[1];
            var SUM_TotalAmount = RunningSpliteDetails[2];
            var SUM_TaxAmount = RunningSpliteDetails[3];
            var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[4]).toFixed(2);


            cTotalNPrice.SetValue(SUM_Price);
            cTotalNAmount.SetValue(SUM_TotalAmount);
            cTotalNAmt.SetValue(SUM_Amount);
            cTotalNCharges.SetValue(SUM_TaxAmount);
            cTotalQty.SetValue(SUM_ProductQuantity);



        }


        if (grid.cpRemoveProductInvoice) {
            if (grid.cpRemoveProductInvoice == "valid") {
                OnAddNewClick();
                grid.cpRemoveProductInvoice = null;
            }
        }
        else { grid.GetEditor('Product').SetEnabled(true); }  //when purchase invoice is not select

        if (grid.cpSaveSuccessOrFail == "outrange") {
            LoadingPanel.Hide();
            jAlert('Can Not Add More Rate Difference Entry Vendor Number as Rate Difference Entry Vendor Scheme Exausted.<br />Update The Scheme and Try Again');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
            LoadingPanel.Hide();
            OnAddNewClick();
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
            grid.cpSaveSuccessOrFail = null;
        }
        else if (grid.cpSaveSuccessOrFail == "duplicate") {
            LoadingPanel.Hide();
            jAlert('Can Not Save as Duplicate Rate Difference Entry Vendor Number No. Found');
            OnAddNewClick();

        }
        else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
            LoadingPanel.Hide();
            jAlert('Quantity of selected products cannot be less than Ordered Quantity.');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
            LoadingPanel.Hide();
            jAlert('Please select Project.');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "errorInsert") {
            LoadingPanel.Hide();
            jAlert('Please try again later.');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "nullAmount") {
            LoadingPanel.Hide();
            jAlert('total amount cant not be zero(0).');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
            LoadingPanel.Hide();
            jAlert('Please fill Quantity');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
            LoadingPanel.Hide();
            jAlert('Can not Duplicate Product in the Return With Stock & Account List.');
            OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
            LoadingPanel.Hide();
            var SrlNo = grid.cpProductSrlIDCheck;
            var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
            jAlert(msg);
            OnAddNewClick();

        }
        else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
            OnAddNewClick();
            LoadingPanel.Hide();
            grid.cpSaveSuccessOrFail = null;
            jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
            grid.cpSaveSuccessOrFail = '';
            grid.cpSerialNo = '';
            grid.cpProductName = '';
        }

        else if (grid.cpSaveSuccessOrFail == "checkAddress") {
            LoadingPanel.Hide();
            jAlert('Enter Billing Shipping address to save this document.');
            //OnAddNewClick();
            grid.AddNewRow();
            grid.cpSaveSuccessOrFail = null;
        }
        else {
            var SalesReturn_Number = grid.cpQuotationNo;
            var SalesReturn_ID = grid.cpRateDiffVendID;

            var SalesReturn_Msg = "Rate Difference Entry Vendor No. " + SalesReturn_Number + " saved.";
            var AutoPrint = document.getElementById('hdnAutoPrint').value;

            if (value == "E") {
                if (grid.cpApproverStatus == "approve") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                else if (grid.cpApproverStatus == "rejected") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                else {
                    if (SalesReturn_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'RateDifferenceVendor-Branch~D'
                            var module = 'RateDiff_Entry_Vend'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SalesReturn_ID, '_blank')
                        }
                        jAlert(SalesReturn_Msg, 'Alert Dialog: [RateDifferenceEntryVendor]', function (r) {
                            LoadingPanel.Hide();
                            grid.cpQuotationNo = null;
                            if (r == true) {
                                window.location.assign("RateDifferenceEntryVendorList.aspx");
                            }
                        });


                    }
                    else {

                        window.location.assign("RateDifferenceEntryVendorList.aspx");
                    }
                }

            }
            else if (value == "N") {
                if (grid.cpApproverStatus == "approve") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                else {
                    if (SalesReturn_Number != "") {
                        if (AutoPrint == "Yes") {
                            var reportName = 'RateDifferenceVendor-Branch~D'
                            var module = 'RateDiff_Entry_Vend'
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SalesReturn_ID, '_blank')
                        }
                        jAlert(SalesReturn_Msg, 'Alert Dialog: [RateDifferenceEntryVendor]', function (r) {
                            LoadingPanel.Hide();
                            grid.cpQuotationNo = null;
                            if (r == true) {

                                window.location.assign("RateDifferenceEntryVendor.aspx?key=ADD");
                            }
                        });

                    }
                    else {

                        window.location.assign("RateDifferenceEntryVendor.aspx?key=ADD");
                    }
                }
            }
            else {
                var pageStatus = document.getElementById('hdnPageStatus').value;
                if (pageStatus == "first") {
                    OnAddNewClick();
                    grid.batchEditApi.EndEdit();

                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                        ctxt_Rate.SetEnabled(false);
                    }
                }
                else if (pageStatus == "update") {
                    OnAddNewClick();
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                        ctxt_Rate.SetEnabled(false);
                    }
                }
                else if (pageStatus == "Invoiceupdate") {
                    grid.StartEditRow(0);
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                }
                else if (pageStatus == "delete") {
                    grid.StartEditRow(0);
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                }
                else {
                    grid.StartEditRow(0);

                }
}
}
    if (grid.cpGridBlank == "1") {
        grid.GetEditor('Product').SetEnabled(true);
        cddl_AmountAre.SetEnabled(true);
        OnAddNewClick();
        gridquotationLookup.gridView.Refresh();
        grid.cpGridBlank = null;
    }
    else {
        if (gridquotationLookup.GetValue() != null) {

            grid.GetEditor('Product').SetEnabled(false);


        }
        else {

            grid.GetEditor('Product').SetEnabled(true);
        }
    }
    cProductsPopup.Hide();

}

function Save_ButtonClick() {

    LoadingPanel.Show();
    flag = true;
    grid.batchEditApi.EndEdit();

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }


    var QuoteNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End

    var quote_Id = gridquotationLookup.GetValue();

    if (quote_Id == null) {
        LoadingPanel.Hide();
        $('#MandatorysPCno').attr('style', 'display:block');
        flag = false;

    }
    else {
        $('#MandatorysPCno').attr('style', 'display:none');
    }
    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);

    var RefCreditNotedate = cdt_refCreditNoteDt.GetValue();


    var RefCreditNoteNo = $('#<%=txt_refCreditNoteNo.ClientID %>').val();

    var rDate = new Date(RefCreditNotedate);



    if (RefCreditNoteNo != null && RefCreditNoteNo != '') {

        if (RefCreditNotedate == null || RefCreditNotedate == '') {
            LoadingPanel.Hide();

            $("#MandatoryRefDate").show();

            flag = false;
        }
        else { $("#MandatoryRefDate").hide(); }

        if (rDate != null && rDate != '') {
            //if (rDate > startDate) {
            //    LoadingPanel.Hide();
            //    flag = false;
            //    $('#MandatoryREgSDate').attr('style', 'display:block');
            //}
            //else {

            //    $('#MandatoryREgSDate').attr('style', 'display:none');
            //}
        }
        else {
            $('#MandatoryRefDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            flag = false;

        }


    }
    else {

        if (RefCreditNotedate != null && RefCreditNotedate != '') {
            LoadingPanel.Hide();
            flag = false;
            $('#MandatorysRefCreditNoteno').attr('style', 'display:block');
        }
        else {

            $('#MandatorysRefCreditNoteno').attr('style', 'display:none');


        }

        if (RefCreditNotedate != null && RefCreditNotedate != '') {
            $("#MandatoryRefDate").hide();

        }

    }


    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End
    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        var taxcodeid = cddlVatGstCst.GetValue();
        if (taxcodeid == '' || taxcodeid == null) {
            $('#Mandatorytaxcode').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#Mandatorytaxcode').attr('style', 'display:none');
        }
    }

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {
            var customerval = ($('#<%=hdnCustomerId.ClientID %>').val() != null) ? $('#<%=hdnCustomerId.ClientID %>').val() : "";
            // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : "";           
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
            // Custom Control Data Bind

            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $('#<%=hdnRefreshType.ClientID %>').val('N');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }
}

function SaveExit_ButtonClick() {

    LoadingPanel.Show();
    flag = true;
    grid.batchEditApi.EndEdit();
    // Quote no validation Start
    //var ProjectCode = clookup_Project.GetText();
    //if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
    //    LoadingPanel.Hide();
    //    jAlert("Please Select Project.");
    //    flag = false;
    //}


    var QuoteNo = $('#<%=txt_PLQuoteNo.ClientID %>').val();
    QuoteNo = QuoteNo.trim();
    if (QuoteNo == '' || QuoteNo == null) {
        $('#MandatorysQuoteno').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysQuoteno').attr('style', 'display:none');
    }
    // Quote no validation End


    var quote_Id = gridquotationLookup.GetValue();

    if (quote_Id == null) {
        $('#MandatorysPCno').attr('style', 'display:block');
        flag = false;

    }
    else {
        $('#MandatorysPCno').attr('style', 'display:none');
    }
    // Quote Date validation Start
    var sdate = tstartdate.GetValue();
    var edate = tenddate.GetValue();

    var startDate = new Date(sdate);
    var endDate = new Date(edate);



    var RefCreditNotedate = cdt_refCreditNoteDt.GetValue();
    var RefCreditNoteNo = $('#<%=txt_refCreditNoteNo.ClientID %>').val();


    var rDate = new Date(RefCreditNotedate);



    if (RefCreditNoteNo != null && RefCreditNoteNo != '') {

        if (RefCreditNotedate == null || RefCreditNotedate == '') {
            LoadingPanel.Hide();

            $("#MandatoryRefDate").show();

            flag = false;
        }
        else { $("#MandatoryRefDate").hide(); }

        if (rDate != null && rDate != '') {
            //if (rDate > startDate) {
            //    LoadingPanel.Hide();
            //    flag = false;
            //    $('#MandatoryREgSDate').attr('style', 'display:block');
            //}
            //else {

            //    // LoadingPanel.Hide();
            //    $('#MandatoryREgSDate').attr('style', 'display:none');
            //}
        }
        else {
            $('#MandatoryRefDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            flag = false;

        }


    }
    else {

        if (RefCreditNotedate != null && RefCreditNotedate != '') {
            LoadingPanel.Hide();
            flag = false;
            $('#MandatorysRefCreditNoteno').attr('style', 'display:block');
        }
        else {

            $('#MandatorysRefCreditNoteno').attr('style', 'display:none');


        }

        if (RefCreditNotedate != null && RefCreditNotedate != '') {
            $("#MandatoryRefDate").hide();

        }

    }


    // Quote Customer validation Start
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    // Quote Customer validation End

    var amtare = cddl_AmountAre.GetValue();
    if (amtare == '2') {
        var taxcodeid = cddlVatGstCst.GetValue();
        if (taxcodeid == '' || taxcodeid == null) {
            $('#Mandatorytaxcode').attr('style', 'display:block');
            flag = false;
        }
        else {
            $('#Mandatorytaxcode').attr('style', 'display:none');
        }
    }

    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (IsProduct == "Y") {
            // var customerval = (cCustomerComboBox.GetValue() != null) ? cCustomerComboBox.GetValue() : ""; 
            var customerval = ($('#<%=hdnCustomerId.ClientID %>').val() != null) ? $('#<%=hdnCustomerId.ClientID %>').val() : "";
            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
            // Custom Control Data Bind

            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            grid.batchEditApi.EndEdit();
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            LoadingPanel.Hide();
        }
    }
    else { LoadingPanel.Hide(); }
}


function SalePriceGotFocus() {
    ProductSaleprice = grid.GetEditor("SalePrice").GetValue();
    purchasepricegotfocus = grid.GetEditor("SalePrice").GetValue();

    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}
function QuantityGotFocus(s, e) {

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());

}
var fromColumn = '';
function QuantityTextChange(s, e) {



    $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        //var key = cCustomerComboBox.GetValue();
        var key = GetObjectID('hdnCustomerId').value;
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";

            if (key != null && key != '') {
                var IsComponentProduct = SpliteDetails[15];
                var ComponentProduct = SpliteDetails[16];
                var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
                var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
                var CurrQty = 0;



                BalanceQty = parseFloat(BalanceQty);
                TotalQty = parseFloat(TotalQty);
                QuantityValue = parseFloat(QuantityValue);


                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";


                if (type == "PI") {

                    if (TotalQty > QuantityValue) {
                        CurrQty = BalanceQty + (TotalQty - QuantityValue);
                    }
                    else {
                        CurrQty = TotalQty > QuantityValue - (QuantityValue - TotalQty);
                    }
                    // alert(BalanceQty +'+'+ '('+TotalQty+' -'+ QuantityValue+')')
                    //  CurrQty = (TotalQty - QuantityValue);
                    if (QuantityValue > TotalQty) {

                        grid.GetEditor("TotalQty").SetValue(TotalQty);
                        grid.GetEditor("Quantity").SetValue(TotalQty);
                        var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                        grid.batchEditApi.EndEdit();
                        jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                            grid.batchEditApi.StartEdit(globalRowIndex, 7);
                        });
                        return false;
                    }
                    else {

                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                        grid.GetEditor("BalanceQty").SetValue(BalanceQty);


                    }
                }
                else if (type == "TPI") {

                    CurrQty = (TotalQty - QuantityValue);


                    if (CurrQty < 0) {
                        grid.GetEditor("TotalQty").SetValue(TotalQty);
                        grid.GetEditor("Quantity").SetValue(TotalQty);
                        var OrdeMsg = 'Cannot enter quantity more than balance quantity.';
                        grid.batchEditApi.EndEdit();
                        jAlert(OrdeMsg, 'Alert Dialog: [Balance Quantity ]', function (r) {
                            grid.batchEditApi.StartEdit(globalRowIndex, 7);
                        });
                        return false;
                    }
                    else {
                        grid.GetEditor("TotalQty").SetValue(QuantityValue);
                        grid.GetEditor("BalanceQty").SetValue(BalanceQty);


                    }


                }

            }
            else {
                grid.GetEditor("TotalQty").SetValue(QuantityValue);
                grid.GetEditor("BalanceQty").SetValue(BalanceQty);


            }
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);



            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);

        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}


/// Code Added By Sam on 23022017 after make editable of sale price field Start


var purchasepricegotfocus = 0;
var purchasepricelostfocus = 0;

function SalePriceTextChange(s, e) {
    purchasepricelostfocus = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";


    if (ProductSaleprice != parseFloat(Saleprice)) {
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            var strStkUOM = SpliteDetails[4];

            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            if (strRate == 0) {
                strRate = 1;
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(amountAfterDiscount);
            grid.GetEditor('TaxAmount').SetValue("0");
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            DiscountTextChange(s, e);
            //  ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            // DiscountTextChange(s, e);

        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('SalePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}


/// Code Above Added By Sam on 23022017 after make editable of sale price field End



function DiscountGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;

    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductSaleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
}

function DiscountTextChange(s, e) {
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var SalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(SalePrice) != parseFloat(ProductSaleprice)) {



            var strFactor = SpliteDetails[8];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                        divPacking.style.display = "block";
                    } else {
                        divPacking.style.display = "none";
                    }

                    grid.GetEditor('TaxAmount').SetValue(0);
                    var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
                    var tbTotalAmount = grid.GetEditor("TotalAmount");

                    tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));



                    var incluexclutype = "N";
                    if (cddl_AmountAre.GetValue() == "1")
                        incluexclutype = "E";
                    else if (cddl_AmountAre.GetValue() == "2")
                        incluexclutype = "I";


                    var schemabranch = $('#ddl_Branch').val();
                    var CompareStateCode = "";
                    if (incluexclutype != "N")
                        caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[17], DecimalRoundoff(Amount, 2), DecimalRoundoff(amountAfterDiscount,2), incluexclutype, CompareStateCode, schemabranch, $("#hdnEntityType").val(), tstartdate.GetDate(), QuantityValue, 'P')

                    SetLabelValue();
                }
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Discount').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
            //Debjyoti 

            var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                // grid.GetEditor('TaxAmount').SetValue(0);

                ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
            }



        }
        function AddBatchNew(s, e) {
            var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";

            var globalRow_Index = 0;
            if (globalRowIndex > 0) {
                globalRow_Index = globalRowIndex + 1;
            }
            else {
                globalRow_Index = globalRowIndex - 1;
            }


            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            if (keyCode === 13) {
                if (ProductIDValue != "") {

                    grid.batchEditApi.EndEdit();

                    grid.AddNewRow();
                    grid.SetFocusedRowIndex();
                    var noofvisiblerows = grid.GetVisibleRowsOnPage();

                    var tbQuotation = grid.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);

                    grid.batchEditApi.StartEdit(globalRow_Index, 2);

                }
            }
        }
        function OnAddNewClick() {


            if (gridquotationLookup.GetValue() == null) {
                //   grid.AddNewRow();

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                //kaushik  8_2_2018 to avoid create more then 1 rows
                if (noofvisiblerows < 1)
                { grid.AddNewRow(); }
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
            }
            else {

                PurchaseInvoiceNumberChanged();
                grid.StartEditRow(0);
            }
        }

        function Save_TaxClick() {
            if (gridTax.GetVisibleRowsOnPage() > 0) {
                gridTax.UpdateEdit();
            }
            else {
                gridTax.PerformCallback('SaveGst');
            }
            cPopup_Taxes.Hide();
        }

        var Warehouseindex;
        function OnCustomButtonClick(s, e) {
            if (e.buttonID == 'CustomDelete') {
                var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
                grid.batchEditApi.EndEdit();

                $('#<%=hdnDeleteSrlNo.ClientID %>').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (gridquotationLookup.GetValue() != null) {
            var messege = "";
            messege = "Cannot Delete using this button as the Purchase Invoice is linked with this Return With Stock & Account.<br /> Click on Plus(+) sign to Add or Delete Product from last column !";
            jAlert(messege, 'Alert Dialog: [Delete Purchase Invoice Products]', function (r) {
            });

        }
        else {
            if (noofvisiblerows != "1") {
                grid.DeleteRow(e.visibleIndex);
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                grid.PerformCallback('Display');
                $('#<%=hdnPageStatus.ClientID %>').val('delete');

            }
        }
    }
    else if (e.buttonID == 'AddNew') {
        //
        if (gridquotationLookup.GetValue() == null) {
            var ProductIDValue = (grid.GetEditor('ProductDisID').GetText() != null) ? grid.GetEditor('ProductDisID').GetText() : "0";
            if (ProductIDValue != "") {
                OnAddNewClick();
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);

                return false;
            }
            else {

                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
        else {
            PurchaseInvoiceNumberChanged();
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {

        debugger;
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)

        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ComponentID = (grid.GetEditor('ComponentID').GetValue() != null) ? grid.GetEditor('ComponentID').GetValue() : "0";

        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();

        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strMultiplier = SpliteDetails[7];
            var strProductName = strDescription;
            var StkQuantityValue = QuantityValue * strMultiplier;
            var Ptype = SpliteDetails[16];
            $('#<%=hdfProductType.ClientID %>').val(Ptype);
            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
            document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
            document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
            document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
            document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;
            $('#<%=hdfProductID.ClientID %>').val(strProductID);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
            $('#<%=hdfComponentID.ClientID %>').val(ComponentID);

            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "B") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "S") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WB") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WBS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "BS") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else {

                jAlert("No Warehouse or Batch or Serial is actived !");
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {

            jAlert("Please enter Quantity !");
        }


    }

}


function DateCheck() {
    var type = "PI";
    if (gridquotationLookup.GetValue() != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
            if (r == true) {

                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');

                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                // var key = cCustomerComboBox.GetValue();
                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

                }
                // grid.PerformCallback('GridBlank');
                deleteAllRows();
                grid.AddNewRow();
                grid.GetEditor('SrlNo').SetValue('1');
                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    clearTransporter();
                }
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                deleteTax('DeleteAllTax', "", "", "");
                $('#<%=txt_InvoiceDate.ClientID %>').val('');

            }
        });
    }
    else {

        var startDate = new Date();
        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
        page.SetActiveTabIndex(0);
        $('.dxeErrorCellSys').addClass('abc');
        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

        ccmbGstCstVat.PerformCallback();
        ccmbGstCstVatcharge.PerformCallback();
        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        deleteTax('DeleteAllTax', "", "", "");
        page.SetActiveTabIndex(0);

    }
}
function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}

function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 5);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (IsPostBack == "N") {
                checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B") {
                cCmbBatch.Focus();
            }
            else {
                ctxtserial.Focus();
            }
        }
        else {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouse.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
}

var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";




function ctaxUpdatePanelEndCall(s, e) {
    console.log(ctaxUpdatePanel.cpstock);
    if (ctaxUpdatePanel.cpstock != null) {

        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
        document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
        ctaxUpdatePanel.cpstock = null;

    }

    if (fromColumn == 'product') {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
        fromColumn = '';
    }


    return;
}


function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        cCmbWarehouse.SetEnabled(true);
    }

    // Changes
    if (cCmbWarehouse.cpwarehouseid == "Y") {
        cCmbWarehouse.cpwarehouseid = null;

        var WarehouseID = cCmbWarehouse.GetValue();
        var type = document.getElementById('hdfProductType').value;

        if (type == "WBS" || type == "WB") {
            cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
        }
        else if (type == "WS") {
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
        }
    }
}

function CmbBatchEndCall(s, e) {
    if (SelectBatch != "0") {
        cCmbBatch.SetValue(SelectBatch);
        SelectBatch = "0";
    }
    else {
        cCmbBatch.SetEnabled(true);
    }
}

function listBoxEndCall(s, e) {
    if (SelectSerial != "0") {
        var values = [SelectSerial];
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText();
        SelectSerial = "0";
        cCmbBatch.SetEnabled(false);
        cCmbWarehouse.SetEnabled(false);
    }
}

function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }

    if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
        cnt = 1;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);

            cnt++;
        }
    }

    //Debjyoti 
    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    //End Here

    //kaushik 29-7-2017
    ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

        //Set Gross Amount with GstValue
        //Get The rate of Gst
        var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
        if (gstRate) {
            if (gstRate != 0) {
                var gstDis = (gstRate / 100) + 1;
                if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                    $('.lblChargesGSTforNet').hide();
                    ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");

                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                    clblChargesTaxableNet.SetText("(Taxable)");
                }
            }

        } else {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();

        //Debjyoti 09032017
        for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            } else {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            }
        }






    }
    //End here





    //Set Total amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    gridTax.PerformCallback('Display');
    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {
        $('.chargeGstCstvatClass').hide();
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.chargeGstCstvatClass').show();
    }
    //End here
    $('.RecalculateCharge').hide();
    cPopup_Taxes.Show();
    gridTax.StartEditRow(0);
}

var chargejsonTax;
function OnTaxEndCallback(s, e) {
    GetPercentageData();
    $('.gridTaxClass').show();
    if (gridTax.GetVisibleRowsOnPage() == 0) {
        $('.gridTaxClass').hide();
        ccmbGstCstVatcharge.Focus();
    }
    else {
        gridTax.StartEditRow(0);
    }
    //check Json data
    if (gridTax.cpJsonChargeData) {
        if (gridTax.cpJsonChargeData != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            gridTax.cpJsonChargeData = null;
        }
    }

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }

    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
}

function GetPercentageData() {
    var Amount = ctxtProductAmount.GetValue();
    var GlobalTaxAmt = 0;
    var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, totalAmount = 0;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
        var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
        var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

        if (sign == '(+)') {
            sumAmount = sumAmount + parseFloat(DisAmount);
        }
        else {
            sumAmount = sumAmount - parseFloat(DisAmount);
        }

        cnt++;
    }

    totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));

}



function PercentageTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}

//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        if (chargejsonTax[i].applicableOn == "R") {
            gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
            var GlobalTaxAmt = 0;

            var Percentage = gridTax.GetEditor("Percentage").GetText();
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

            if (sign == '(+)') {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


        }
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
}

/////////////////// QuotationTaxAmountTextChange By Sam on 23022017
var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}


function QuotationTaxAmountTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();

}


function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
        } else {
            totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }

        gridTax.batchEditApi.EndEdit();
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

    ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

////////////

var AmountOldValue;
var AmountNewValue;

function AmountTextChange(s, e) {
    AmountLostFocus(s, e);
    var RecieveValue = (grid.GetEditor('Amount').GetValue() != null) ? parseFloat(grid.GetEditor('Amount').GetValue()) : "0";
}

function AmountLostFocus(s, e) {
    AmountNewValue = s.GetText();
    var indx = AmountNewValue.indexOf(',');

    if (indx != -1) {
        AmountNewValue = AmountNewValue.replace(/,/g, '');
    }
    if (AmountOldValue != AmountNewValue) {
        changeReciptTotalSummary();
    }
}

function AmountGotFocus(s, e) {
    AmountOldValue = s.GetText();
    var indx = AmountOldValue.indexOf(',');
    if (indx != -1) {
        AmountOldValue = AmountOldValue.replace(/,/g, '');
    }
}

function changeReciptTotalSummary() {
    var newDif = AmountOldValue - AmountNewValue;
    var CurrentSum = ctxtSumTotal.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }

    ctxtSumTotal.SetValue(parseFloat(CurrentSum - newDif));
}
function CmbWarehouse_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
    }
    else if (type == "WS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
}
function CmbBatch_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var BatchID = cCmbBatch.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
    }
    else if (type == "BS") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
    }
}
function SaveWarehouse() {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();

    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    for (var i = 0; i < items.length; i++) {
        if (items[i].index != 0) {
            if (i == 0) {
                SerialID = items[i].value;
                SerialName = items[i].text;
            }
            else {
                if (SerialID == "" && SerialID == "") {
                    SerialID = items[i].value;
                    SerialName = items[i].text;
                }
                else {
                    SerialID = SerialID + '||@||' + items[i].value;
                    SerialName = SerialName + '||@||' + items[i].text;
                }
            }

        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    var Ptype = document.getElementById('hdfProductType').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBox").show();
    }
    else {
        if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
            }
            else {
                IsPostBack = "N";
                PBWarehouseID = WarehouseID;
                PBBatchID = BatchID;
            }
        }
        else {
            cCmbWarehouse.PerformCallback('BindWarehouse');
            cCmbBatch.PerformCallback('BindBatch~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}

var IsPostBack = "";
var PBWarehouseID = "";
var PBBatchID = "";


$(document).ready(function () {
    $('#ddl_VatGstCst_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })
    $('#ddl_AmountAre').blur(function () {
        var id = cddl_AmountAre.GetValue();
        if (id == '1' || id == '3') {
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 2);
            }
        }
    })


});

function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }
    OnAddNewClick();
}
function txtserialTextChanged() {
    checkListBox.UnselectAll();
    var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserial.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTexts(texts);

        if (values.length > 0) {
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
            SaveWarehouse();
        }
        else {
            jAlert("This Serial Number does not exists.");
        }
    }
}


function AutoCalculateMandateOnChange(element) {
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    if (document.getElementById("myCheck").checked == true) {
        divSingleCombo.style.display = "block";
        divMultipleCombo.style.display = "none";

        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";

        ctxtserial.Focus();
    }
}

function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}
function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}
    </script>
    <script type="text/javascript">
        // <![CDATA[
        var textSeparator = ";";
        var selectedChkValue = "";

        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();

            //Added Subhabrata
            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);
            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {

            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);

            var ItemCount = GetSelectedItemsCount(selectedItems);
            checkComboBox.SetText(ItemCount + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);

        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = selectedChkValue.split(textSeparator);

            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        } function GetSelectedItemsCount(items) {

            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.length;
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
        $(function () {
            $('[data-toggle="popover"]').popover();
        })
        // ]]>
    </script>
    <script>
        function ProductsGotFocus(s, e) {
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

        }




        function PsGotFocusFromID(s, e) {

            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            divAvailableStk.style.display = "block";

            var ProductID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];


            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
            }
        }
        function ProductsGotFocusFromID(s, e) {

            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";
            var strProductName = (grid.GetEditor('ProductID').GetText() != null && grid.GetEditor('ProductID').GetText() != "") ? grid.GetEditor('ProductID').GetText() : "0";

            var ProductdisID = (grid.GetEditor('ProductDisID').GetText() != null && grid.GetEditor('ProductDisID').GetText() != "") ? grid.GetEditor('ProductDisID').GetText() : "0";

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strProductShortCode = SpliteDetails[14];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            strProductName = strDescription;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = $("[id*=ddl_Branch]").find("option:selected").val();
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID + "~" + strBranch);
            }
            else { cacpAvailableStock.PerformCallback(ProductdisID + "~" + strBranch); }
        }
    </script>
    <style>
        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .popover {
            z-index: 999999;
            max-width: 350px;
        }

            .popover .popover-title {
                margin-top: 0 !important;
                background: #465b9d;
                color: #fff;
            }

        .pdLeft15 {
            padding-left: 15px;
        }

        .mTop {
            margin-top: 10px;
        }

        .mLeft {
            margin-left: 15px;
        }

        .popover .popover-content {
            min-height: 60px;
        }


        #grid_DXStatus {
            display: none;
        }

        #aspxGridTax_DXStatus {
            display: none;
        }

        #gridTax_DXStatus {
            display: none;
        }

        .hideCell {
            display: none;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 24px;
        }

        #myCheck {
            transform: translateY(2px);
            -webkit-transform: translateY(2px);
            -moz-transform: translateY(2px);
            margin-right: 5px;
        }
    </style>
    <%--End Sudip--%>

    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .dxeButtonEditClearButton_PlasticBlue {
            display: none;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .validclass {
            position: absolute;
            right: -4px;
            top: 20px;
        }

        .validvendorclass {
            position: absolute;
            right: -15px;
            top: 20px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        #txtProductAmount, #txtProductTaxAmount, #txtProductDiscount {
            font-weight: bold;
        }


        .crossBtn {
            cursor: pointer;
        }

        #txtTaxTotAmt input, #txtprodBasicAmt input, #txtGstCstVat input {
            text-align: right;
        }

        #grid .dxgvHSDC > div, #grid .dxgvCSD {
            width: 100% !important;
        }
    </style>


    <%--Batch Product Popup Start--%>

    <script>
        function ProductKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var CID = GetObjectID('hdnCustomerId').value;
                if (CID != null && CID != "") {

                    setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                    $('#txtProdSearch').val('');
                    $('#ProductModel').modal('show');
                }
                else {
                    jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
                }
            }
        }



        function ProductDisKeyDown(s, e) {
            console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function ProductDisButnClick(s, e) {
            if (e.buttonIndex == 0) {
                var CID = GetObjectID('hdnCustomerId').value;
                if (CID != null && CID != "") {

                    setTimeout(function () { $("#txtProdDisSearch").focus(); }, 500);

                    $('#txtProdDisSearch').val('');
                    $('#ProductDisModel').modal('show');
                }
                else {
                    jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
                }
            }
        }
        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');

            var LookUpData = Id;
            var ProductCode = Name;

            if (!ProductCode) {
                LookUpData = null;
            }
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);
            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            fromColumn = 'product';
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "");

            grid.batchEditApi.StartEdit(globalRowIndex, 7);
        }



        function SetDisProduct(Id, Name) {
            $('#ProductDisModel').modal('hide');

            var LookUpData = Id;
            var ProductCode = Name;

            if (!ProductCode) {
                LookUpData = null;
            }

            grid.batchEditApi.StartEdit(globalRowIndex, 3);
            var productall = LookUpData.split('||')
            cddl_AmountAre.SetEnabled(false);
            var productdsc = productall[0];
            grid.GetEditor("ProductDisID").SetText(productdsc);
            grid.GetEditor("Product").SetText(ProductCode);


            grid.batchEditApi.StartEdit(globalRowIndex, 3);

        }

        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');
            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);
            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            fromColumn = 'product';
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), "");
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
        }



        function SetDisProduct(Id, Name) {
            $('#ProductDisModel').modal('hide');

            var LookUpData = Id;
            var ProductCode = Name;

            if (!ProductCode) {
                LookUpData = null;
            }

            grid.batchEditApi.StartEdit(globalRowIndex, 3);
            var productall = LookUpData.split('||')
            cddl_AmountAre.SetEnabled(false);
            var productdsc = productall[0];
            grid.GetEditor("ProductDisID").SetText(productdsc);
            grid.GetEditor("Product").SetText(ProductCode);


            grid.batchEditApi.StartEdit(globalRowIndex, 3);

        }

        function ProductDisSelected(s, e) {
            var LookUpData = cproductDisLookUp.GetValue();
            if (LookUpData == null)
                return;
            var ProductCode = cproductDisLookUp.GetText();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUpdis.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 3);
            var productall = LookUpData.split('||')

            var productdsc = productall[0];
            grid.GetEditor("ProductDisID").SetText(productdsc);
            grid.GetEditor("Product").SetText(ProductCode);

            grid.batchEditApi.StartEdit(globalRowIndex, 3);
        }

        function ProductSelected(s, e) {

            if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
                jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
                return;
            }

            var LookUpData = cproductLookUp.GetValue();
            var ProductCode = cproductLookUp.GetText();

            var quote_Id = gridquotationLookup.GetValue();





            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            console.log(LookUpData);
            $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            // tbSalePrice.SetValue(strSalePrice);
            if (quote_Id == null) {
                tbSalePrice.SetValue(strSalePrice);
                grid.GetEditor("Quantity").SetValue("0.00");
                grid.GetEditor("Discount").SetValue("0.00");
                grid.GetEditor("Amount").SetValue("0.00");
                grid.GetEditor("TaxAmount").SetValue("0.00");
                grid.GetEditor("TotalAmount").SetValue("0.00");
            }
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
             $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                 divPacking.style.display = "block";
             } else {
                 divPacking.style.display = "none";
             }

             fromColumn = 'product';
             ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());
             grid.batchEditApi.StartEdit(globalRowIndex, 7);
         }

         function onBranchItems() {

             grid.batchEditApi.StartEdit(-1, 1);
             var accountingDataMin = grid.GetEditor('ProductName').GetValue();
             grid.batchEditApi.EndEdit();

             grid.batchEditApi.StartEdit(0, 1);
             var accountingDataplus = grid.GetEditor('ProductName').GetValue();
             grid.batchEditApi.EndEdit();
             var type = "PI";
             if (accountingDataMin != null || accountingDataplus != null) {
                 jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

                     if (r == true) {

                         page.SetActiveTabIndex(0);
                         $('.dxeErrorCellSys').addClass('abc');
                         var startDate = new Date();
                         startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                         cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                         var key = GetObjectID('hdnCustomerId').value;
                         //  var key = cCustomerComboBox.GetValue();
                         if (key != null && key != '') {
                             cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

                         }
                         //grid.PerformCallback('GridBlank');
                         deleteAllRows();
                         grid.AddNewRow();
                         grid.GetEditor('SrlNo').SetValue('1');
                         if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }

                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "", "");
                        //   ctxt_InvoiceDate.SetText('');
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');

                     } else {

                     }
                });
             }
             else {


                 var startDate = new Date();
                 startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                 page.SetActiveTabIndex(0);
                 $('.dxeErrorCellSys').addClass('abc');
                 cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                //  var key = cCustomerComboBox.GetValue();
                 var key = GetObjectID('hdnCustomerId').value;
                 if (key != null && key != '') {
                     cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);

                 }
                 ccmbGstCstVat.PerformCallback();
                 ccmbGstCstVatcharge.PerformCallback();
                // ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                 deleteTax('DeleteAllTax', "", "", "");
                 page.SetActiveTabIndex(0);

             }
         }


         function ProductlookUpdisKeyDown(s, e) {
             if (e.htmlEvent.key == "Escape") {
                 cProductpopUpdis.Hide();
                 grid.batchEditApi.StartEdit(globalRowIndex, 3);
             }
         }


         function ProductlookUpKeyDown(s, e) {
             if (e.htmlEvent.key == "Escape") {
                 cProductpopUp.Hide();
                 grid.batchEditApi.StartEdit(globalRowIndex, 5);
             }
         }





    </script>

    <script>
        function prodkeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                HeaderCaption.push("Installation Reqd.");

                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseReturnProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }


        function prodDiskeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdDisSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdDisSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseReturnProduct", OtherDetails, "ProductDisTable", HeaderCaption, "ProdDisIndex", "SetDisProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdDisIndex=0]"))
                    $("input[ProdDisIndex=0]").focus();
            }
        }
    </script>
    <%--Added By : kaushik --Reference credit Note Section--%>
    <script>

        function DuplicateCreditNoteNo() {

            var pID = '';

            var creditNoteno = $('#<%=txt_refCreditNoteNo.ClientID %>').val();
            if (creditNoteno != null && creditNoteno != '') {
                $("#MandatorysRefCreditNoteno").hide();

            }
            else {

                var rDate = cdt_refCreditNoteDt.GetValue('');
                if (rDate != null && rDate != '') {

                    $("#MandatorysRefCreditNoteno").show();
                }
                else { $("#MandatorysRefCreditNoteno").hide(); }
            }

            var vendorid = ($('#<%=hdnCustomerId.ClientID %>').val() != null) ? $('#<%=hdnCustomerId.ClientID %>').val() : "";

            if (vendorid != null && vendorid != '') {

                if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    mode = 'A'
                }
                else {
                    mode = 'E'
                    pID = "<%=Convert.ToString(Session["PRWS_ReturnID"])%>";
                }
                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryVendor.aspx/CheckUniqueRefCreditNoteNo",
                    data: JSON.stringify({ vendorid: vendorid, creditNoteno: creditNoteno, mode: mode, pID: pID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var data = msg.d;

                        if (data == true) {
                            $("#DuplicateRefCreditNoteno").show();

                            $('#<%=txt_refCreditNoteNo.ClientID %>').val('');
                            $('#<%=txt_refCreditNoteNo.ClientID %>').focus();

                        }
                        else {
                            $("#DuplicateRefCreditNoteno").hide();
                        }
                    }
                });

            }
        }


    </script>

    <%--Added By : Samrat Roy -- New Billing/Shipping Section--%>
    <script>
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        function disp_prompt(name) {

            if (name == "tab0") {
            }
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);
                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                }
            }
        }
        var canCallBack = true;

        function AllControlInitilize() {

            if (canCallBack) {
                //--   if (grid.GetVisibleRowsOnPage() == 0) {

                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.batchEditApi.EndEdit();
                $('#ddl_numberingScheme').focus();
                canCallBack = false;
                var NoSchemeTypedtl = $("#ddl_numberingScheme").val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];
                var branchID = NoSchemeTypedtl.toString().split('~')[3];

                $("#hfVendorGSTIN").val($("#hdnRDVendGSTIN").val());


                if (NoSchemeTypedtl) {
                    document.getElementById('ddl_Branch').value = branchID;
                    if (NoSchemeType == '1') {

                        $('#<%=txt_PLQuoteNo.ClientID %>').val('Auto');
                        document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                        tstartdate.SetEnabled(true);

                        tstartdate.Focus();
                    }
                    else if (NoSchemeType == '0') {

                        document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = false;
                        tstartdate.SetEnabled(true);
                        $('#<%=txt_PLQuoteNo.ClientID %>').maxLength = quotelength;
                        $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                        $('#<%=txt_PLQuoteNo.ClientID %>').focus();

                    }
                    else {

                        $('#<%=txt_PLQuoteNo.ClientID %>').val('');
                        document.getElementById('<%= txt_PLQuoteNo.ClientID %>').disabled = true;
                        tstartdate.SetEnabled(true);

                    }
            }
                //}
        }
    }
    </script>
    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>

    <style>
        .col-md-2 > label, .col-md-2 > span,
        .col-md-1 > label, .col-md-1 > span {
            margin-top: 8px;
            display: inline-block;
        }
    </style>



    <script>
        function VendorButnClick(s, e) {
            document.getElementById("txtCustSearch").value = "";
            var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
            document.getElementById("CustomerTable").innerHTML = txt;
            $('#CustModel').modal('show');
            $('#txtCustSearch').focus();
        }

        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                document.getElementById("txtCustSearch").value = "";
                var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
                document.getElementById("CustomerTable").innerHTML = txt;
                $('#CustModel').modal('show');
                $('#txtCustSearch').focus();
            }
        }

        function Customerkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");
                HeaderCaption.push("Unique Id");

                callonServer("Services/Master.asmx/GetVendorWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {

            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                SetEntityType(Id);
                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdfLookupCustomer').value = Id;
                $('.crossBtn').hide();
                $('#CustModel').modal('hide');
            }
            var startDate = new Date();
            var branchid = $('#ddl_Branch').val();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            $('#<%=txt_refCreditNoteNo.ClientID %>').val('');
            cdt_refCreditNoteDt.SetText('');
            var type = "PI";
            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();
                        var key = GetObjectID('hdnCustomerId').value;
                        if (key != null && key != '') {
                            LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PR');
                            GetObjectID('hdnCustomerId').value = key;
                            page.tabs[0].SetEnabled(true);
                            page.tabs[1].SetEnabled(true);
                            $('.dxeErrorCellSys').addClass('abc');
                            if (key != null && key != '') {
                                // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type);
                            }
                            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                clearTransporter();
                            }
                            $('#<%=txt_InvoiceDate.ClientID %>').val('');
                        }

                    } else {
                        ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                        var Vendorid = $('#<%=hdnCustomerId.ClientID %>').val();
                        ctxtCustName.PerformCallback(Vendorid);
                    }
                });
            }
            else {

                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    $('#<%=txt_InvoiceDate.ClientID %>').val('');
                    page.SetActiveTabIndex(0);
                    $('.dxeErrorCellSys').addClass('abc');
                    //###### Added By : Samrat Roy ##########
                    LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PR');
                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);
                    //###### END : Samrat Roy : END ########## 

                    GetObjectID('hdnAddressDtl').value = '0';
                }
            }

            cContactPerson.Focus();
        }

        function ValueSelected(e, indexName) {
            // debugger;
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var Name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {

                    if (indexName == "ProdIndex") {
                        SetProduct(Id, Name);
                    }
                    else if (indexName == "ProdDisIndex") {
                        SetDisProduct(Id, Name);
                    }
                    else if (indexName == "customerIndex") {
                        SetCustomer(Id, Name);
                    }

                }

            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "ProdIndex") {
                        $('#txtProdSearch').focus();
                    }
                    else if (indexName == "ProdDisIndex") {
                        $('#txtProdDisSearch').focus();
                    }
                    else if (indexName == "customerIndex") {
                        $('#txtCustSearch').focus();
                    }

                }
            }

        }
    </script>

    <script>


        function selectValue() {

            //  debugger;
            gridquotationLookup.gridView.UnselectRows();
            var checked = $('#rdl_PurchaseInvoice').attr('checked', true);
            if (checked) {
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', true);
            }
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');

            var key = $('#<%=hdnCustomerId.ClientID %>').val();

            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";

            if (key == null || key == "") {
                jAlert("Vendor required !", 'Alert Dialog: [Purchase Invoice]', function (r) {
                    if (r == true) {
                        ctxtCustName.Focus();
                    }
                });

                return;

            }


            if (key != null && key != '' && type != "") {

                cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%' + '~' + type);

            }


            var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                //grid.PerformCallback('GridBlank');
                deleteAllRows();
                grid.AddNewRow();
                grid.GetEditor('SrlNo').SetValue('1');
                $('#<%=txt_InvoiceDate.ClientID %>').val('');
                cTotalNPrice.SetValue('0.00');
                cTotalNAmount.SetValue('0.00');
                cTotalNAmt.SetValue('0.00');
                cTotalNCharges.SetValue('0.00');
                cTotalQty.SetValue('0.00');
            }

        }

        function SetEntityType(Id) {
            $.ajax({
                type: "POST",
                url: "SalesQuotation.aspx/GetEntityType",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("#hdnEntityType").val(r.d);
                }

            });
        }
    </script>
    <%--Batch Product Popup End--%>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate , #dt_refCreditNoteDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1 ,
        #dt_refCreditNoteDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img ,
        #dt_refCreditNoteDt_B-1 #dt_refCreditNoteDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 26px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 4px;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 26px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>

        </h3>

        <div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="display: none;" runat="server">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Contact Person's Phone</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                </tr>
                            </table>
                        </div>

                    </li>
                    <li>
                        <div class="lblHolder" id="divOutstanding" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Receivable</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOutstanding" runat="server" Text="0.0" CssClass="classout"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>

                    <li>
                        <div class="lblHolder" id="divAvailableStk" style="display: none;">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAvailableStkPro" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divPacking" style="display: none;">
                            <table>
                                <tr>
                                    <td>Packing Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>

                    <li>
                        <div class="lblHolder" id="divGSTN" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>GST Registed?</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Unit</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbranchName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Product</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Stock Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                        <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="RateDifferenceEntryVendorList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
        <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style=" padding: 4px 0; margin-bottom: 0px; border-radius: 4px;" class="clearfix col-md-12">
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="col-md-2 simple-select" id="divScheme" runat="server">

                                                <asp:Label ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme"></asp:Label>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_SaleInvoiceNo" runat="server" Text="Document No."></asp:Label>

                                                <asp:TextBox ID="txt_PLQuoteNo" runat="server" TabIndex="2" Width="100%"></asp:TextBox>
                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                <span id="duplicateQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date"></asp:Label>

                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%">

                                                    <ClientSideEvents DateChanged="function(s, e) {DateCheck();}" />
                                                    <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}"></ClientSideEvents>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <%--Rev 1.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 1.0--%>
                                            </div>
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="col-md-2 simple-select">
                                                <asp:Label ID="lbl_Branch" runat="server" Text="Unit"></asp:Label>

                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="onBranchItems()">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_Customer" runat="server" Text="Vendor"></asp:Label>

                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                                <span id="MandatorysCustomer" style="display: none" class="validvendorclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor21_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_ContactPerson" runat="server" Text="Contact Person"></asp:Label>

                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" GotFocus="function(s,e){cContactPerson.ShowDropDown();}"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-2" style="display: none">

                                                <asp:Label ID="ASPxLabel4" runat="server" Text="Request"></asp:Label>
                                                <dxe:ASPxComboBox ID="cmbRequest" DropDownWidth="400px" DropDownHeight="70px" runat="server" OnCallback="cmbRequest_Callback" TabIndex="6" Width="100%" ClientInstanceName="ccmbRequest" Font-Size="12px">
                                                    <ClientSideEvents TextChanged="function(s, e) { cmbRequestTextChange()}"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="col-md-2" style="display: none">

                                                <asp:Label ID="ASPxLabel8" runat="server" Text="Stock Out Unit"></asp:Label>
                                                <asp:DropDownList ID="ddl_StockOutBranch" runat="server" Width="100%" TabIndex="4">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_Refference" runat="server" Text="Reference"></asp:Label>

                                                <asp:TextBox ID="txt_Refference" runat="server" TabIndex="7" Width="100%"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2" style="display: none;">
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="8">
                                                </asp:DropDownList>GST Registed
                                            </div>

                                            <div class="col-md-4">
                                                <asp:RadioButtonList ID="rdl_PurchaseInvoice" runat="server" RepeatDirection="Horizontal" TabIndex="9" onchange="return selectValue();" Width="360px">
                                                    <asp:ListItem Text="Purchase Invoice" Value="PI"></asp:ListItem>
                                                    <asp:ListItem Text="Transit Purchase Invoice" Value="TPI"></asp:ListItem>
                                                </asp:RadioButtonList>


                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="10" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Document Number" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="PartyInvoiceNo" Visible="true" VisibleIndex="4" Caption="Party Invoice No." Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="PartyInvoiceDate" Visible="true" VisibleIndex="5" Caption="Party Invoice Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="6" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="7" Caption="Reference" Width="80" Settings-AutoFilterCondition="Contains" />

                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowAllRecords">
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClientSideEvents ValueChanged="function(s, e) { PurchaseInvoiceNumberChanged();}" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                                <span id="MandatorysPCno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor21_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>

                                                <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                                                    Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                                                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                                                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                                                    <HeaderTemplate>
                                                        <strong><span style="color: #fff">Select Products</span></strong>
                                                        <%-- <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                                            <ClientSideEvents Click="function(s, e){ 
                                                                                        cProductsPopup.Hide();
                                                                                    }" />
                                                        </dxe:ASPxImage>--%>
                                                    </HeaderTemplate>
                                                    <ContentCollection>
                                                        <dxe:PopupControlContentControl runat="server">
                                                            <div style="padding: 7px 0;">
                                                                <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                                <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                                <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                                            </div>
                                                            <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                                                                OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">

                                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                                <SettingsPager Visible="false"></SettingsPager>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="gvColProduct" ReadOnly="true" Caption="Product" Width="0">
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="gvColDiscription" Width="200" ReadOnly="true" Caption="Product Description">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Purchase Invoice No">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="70" VisibleIndex="6" Visible="false">
                                                                        <PropertiesTextEdit>
                                                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>

                                                                <SettingsDataSecurity AllowEdit="true" />

                                                            </dxe:ASPxGridView>
                                                            <div class="text-center" style="padding-top: 8px;">

                                                                <dxe:ASPxButton ID="Button13" ClientInstanceName="cbtn_Button13" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">


                                                                    <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                                                                </dxe:ASPxButton>

                                                            </div>
                                                        </dxe:PopupControlContentControl>
                                                    </ContentCollection>
                                                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                </dxe:ASPxPopupControl>
                                            </div>


                                            <div class="col-md-2 lblmTop8" style="padding-top: 7px;">
                                                <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Close">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxCheckBox ID="chk_Isclosed" ClientInstanceName="cchk_Isclosed" runat="server">
                                                </dxe:ASPxCheckBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_InvoiceNO" runat="server" Text="Invoice Date"></asp:Label>
                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <asp:TextBox ID="txt_InvoiceDate" runat="server" TabIndex="8" Width="100%" Enabled="false"></asp:TextBox>
                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                    </dxe:ASPxCallbackPanel>
                                                </div>

                                            </div>

                                            <div class="col-md-2" style="display: none">
                                                <asp:Label ID="lbl_DueDate" runat="server" Text="Due Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" TabIndex="12" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="col-md-1 simple-select">
                                                <asp:Label ID="lbl_Currency" runat="server" Text="Currency"></asp:Label>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="13">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label ID="lbl_Rate" runat="server" Text="Exch Rate"></asp:Label>
                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="14" Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="function(s,e){ctxt_Rate.ShowDropDown();}" />
                                                </dxe:ASPxTextBox>
                                            </div>



                                            <div class="clear"></div>

                                            <div class="col-md-2">


                                                <asp:Label ID="lbl_refCreditNoteNo" runat="server" Text="Ref. Credit Note No."></asp:Label>
                                                <asp:TextBox ID="txt_refCreditNoteNo" runat="server" TabIndex="16" Width="100%"></asp:TextBox>

                                                <span id="MandatorysRefCreditNoteno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <span id="DuplicateRefCreditNoteno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Ref.  Credit  Note No. already exist for the selected vendor."></span>


                                            </div>

                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_refCreditNoteDt" runat="server" Text="Ref. Credit Note Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_refCreditNoteDt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_refCreditNoteDt"
                                                    TabIndex="17" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents LostFocus="refCreditNoteDtMandatorycheck" GotFocus="function(s,e){cdt_refCreditNoteDt.ShowDropDown();}" />
                                                </dxe:ASPxDateEdit>
                                                <span id="MandatoryRefDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <span id="MandatoryREgSDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Ref. Credit Note Date can not be greater than Rate Difference Entry vendor Date"></span>
                                                <%--                                            <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                                            </div>

                                            <div class="col-md-6">
                                                <asp:Label ID="ASPxLabel9" runat="server" Text="Reason For Return"></asp:Label>
                                                <asp:TextBox ID="txtReasonforChange" runat="server" TabIndex="18" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_AmountAre" runat="server" Text="Amounts are"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="19" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="col-md-2 hide">

                                                <asp:Label ID="lblVatGstCst" runat="server" Text="Select GST"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="16" Width="100%">
                                                    <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                                <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project"></dxe:ASPxLabel>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesOrder"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />
                                                    <ClearButton DisplayMode="Always">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="">
                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>
                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID" OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="14%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductDisButnClick" KeyDown="ProductDisKeyDown" GotFocus="PsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Returned" VisibleIndex="3" Width="14%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="23" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductDisID" Caption="hidden Field Id" VisibleIndex="21" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup End--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="1" ReadOnly="True" Width="0" CellStyle-CssClass="hide">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                        <%--<PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="ProductsGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>--%>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="4" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="ProductsGotFocusFromID" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Purc)" VisibleIndex="5" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn VisibleIndex="6" Caption="Stk Details" Width="6%" Visible="false">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="7" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="8" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Purc Price" VisibleIndex="9" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalePriceGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="6%">
                                                    <PropertiesTextEdit DisplayFormatString="0.00" MaxLength="6"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="<0..999>.<00..99>" AllowMouseWheel="false"  />
                                                        <ClientSideEvents LostFocus="DiscountTextChange" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                        </PropertiesSpinEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="11" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="12" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="13" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Invoice ID" VisibleIndex="16" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="18" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="ComponentNumber" ReadOnly="true" Caption="Number" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="14" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="PurchaseInvoiceDetail_ComponentDetailID" Caption="" VisibleIndex="20" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <%--BatchEditEndEditing="OnBatchEditEndEditing"--%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                            <ul style="display: none;">
                                                <li class="clsbnrLblTaxableAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTotalQty" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="txt_TotalQty" runat="server" Text="0.00" ClientInstanceName="cTotalQty" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxableAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTotalAmt" runat="server" Text="Total Purchase Price" ClientInstanceName="cbnrLblTotalAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="txt_TotalNPrice" runat="server" Text="0.00" ClientInstanceName="cTotalNPrice" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblTNAmt" runat="server" Text="Total Amount" ClientInstanceName="cbnrLblTNAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="txt_TotalNAmount" runat="server" Text="0.00" ClientInstanceName="cTotalNAmount" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblTaxAmt">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblChargesAmt" runat="server" Text="Total Charges" ClientInstanceName="cbnrLblChargesAmt" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="txt_TotalNCharges" runat="server" Text="0.00" ClientInstanceName="cTotalNCharges" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblInvVal">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Total  Net Amount" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="txt_TotalNAmt" runat="server" Text="0.00" ClientInstanceName="cTotalNAmt" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                        <div>
                                            <br />
                                        </div>

                                        <div class="col-md-12" id="divSubmitButton" runat="server">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" UseSubmitBehavior="False" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" UseSubmitBehavior="False" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--   <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton3" Visible="false" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>

                                            <%-- <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" Visible="false" />--%>
                                            <asp:HiddenField ID="hfControlData" runat="server" />

                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>

                        <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">



                                    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                        Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                        <ContentCollection>
                                            <dxe:PopupControlContentControl runat="server">
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                    </dxe:ASPxPopupControl>
                                    <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PR" />
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>

                    </TabPages>
                    <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

                </dxe:ASPxPageControl>
            </div>
            <%--SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">--%>

            <asp:SqlDataSource ID="CountrySelect" runat="server"
                SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
            <asp:SqlDataSource ID="StateSelect" runat="server"
                SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">

                <SelectParameters>
                    <asp:Parameter Name="State" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectCity" runat="server"
                SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SelectArea" runat="server"
                SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
                <SelectParameters>
                    <asp:Parameter Name="Area" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectPin" runat="server"
                SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqltaxDataSource" runat="server"
                SelectCommand="select Taxes_ID,Taxes_Name from dbo.Master_Taxes"></asp:SqlDataSource>

            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <%--Sudip--%>
            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Taxes" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforGross">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Discount</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Charges</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforNet">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <%--Error Msg--%>

                                <div class="col-md-8" id="ErrorMsgCharges" style="display: none;">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Tax Code/Charges Not Defined.
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>

                                <div class="clear">
                                </div>
                                <div class="col-md-12 gridTaxClass" style="">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                        OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="col-md-12">
                                    <table style="" class="chargesDDownTaxClass">
                                        <tr class="chargeGstCstvatClass">
                                            <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; width: 200px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0" TabIndex="2"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    OnCallback="cmbGstCstVatcharge_Callback">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>
                                                    <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                        GotFocus="chargeCmbtaxClick" />

                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px">
                                                <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-3">
                                    <div>
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>

                                <div class="col-sm-9">
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>

                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>

                <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                    Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Selected Product</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Entered Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Available Stock</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                                <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li style="display: none;">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Stock Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>

                                    </ul>
                                </div>

                                <div class="clear">
                                    <br />
                                </div>
                                <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div>
                                        <div class="col-md-3" id="div_Warehouse">
                                            <div style="margin-bottom: 5px;">
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Batch">
                                            <div style="margin-bottom: 5px;">
                                                Batch/Lot
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                    TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-4" id="div_Serial">
                                            <div style="margin-bottom: 5px;">
                                                Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                            </div>
                                            <div class="" id="divMultipleCombo">
                                                <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                            runat="server">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton6" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                                </dxe:ASPxDropDownEdit>
                                                <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <div class="pull-left">
                                                    <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                                </div>
                                            </div>
                                            <div class="" id="divSingleCombo" style="display: none;">
                                                <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                    <ClientSideEvents LostFocus="txtserialTextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Quantity">
                                            <div style="margin-bottom: 2px;">
                                                Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                                </dxe:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                    <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                        Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                                        <Columns>
                                            <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                                VisibleIndex="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                                VisibleIndex="1">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                                VisibleIndex="2">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                                VisibleIndex="3">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                                VisibleIndex="4">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                                VisibleIndex="5">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                                VisibleIndex="6">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                                VisibleIndex="7">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                                VisibleIndex="8">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                                VisibleIndex="9">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                                <DataItemTemplate>
                                                    <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Edit">
                                                        <img src="../../../assests/images/Edit.png" /></a>
                                                    &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                                </DataItemTemplate>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <SettingsLoadingPanel Text="Please Wait..." />
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="clearfix">
                                    <br />
                                    <div style="align-content: center">
                                        <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>


            </div>
            <div>
                <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
                </dxe:ASPxCallbackPanel>
                <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="acpContactPersonPhoneEndCall" />
                </dxe:ASPxCallbackPanel>
                <%----hidden --%>

                <asp:HiddenField ID="hdfProductIDPC" runat="server" />
                <asp:HiddenField ID="hdfstockidPC" runat="server" />
                <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
                <asp:HiddenField ID="hdbranchIDPC" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />

                <asp:HiddenField ID="hdfComponentID" runat="server" />


                <asp:HiddenField ID="hdniswarehouse" runat="server" />
                <asp:HiddenField ID="hdnisbatch" runat="server" />
                <asp:HiddenField ID="hdnisserial" runat="server" />
                <asp:HiddenField ID="hdndefaultID" runat="server" />

                <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />

                <asp:HiddenField ID="hdntotalqntyPC" runat="server" Value="0" />

                <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
                <asp:HiddenField ID="hdnoldbatchno" runat="server" />
                <asp:HiddenField ID="hidencountforserial" runat="server" />
                <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />

                <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
                <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />

                <asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />

                <asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />
                <asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />
                <asp:HiddenField ID="hdnstrUOM" runat="server" />
                <asp:HiddenField ID="hdnenterdopenqnty" runat="server" />
                <asp:HiddenField ID="hdnnewenterqntity" runat="server" />

                <asp:HiddenField ID="hdnisoldupdate" runat="server" />
                <asp:HiddenField ID="hdncurrentslno" runat="server" />
                <asp:HiddenField ID="oldopeningqntity" runat="server" Value="0" />
                <asp:HiddenField ID="hdnisedited" runat="server" />

                <asp:HiddenField ID="hdnisnewupdate" runat="server" />

                <asp:HiddenField ID="hdnisviewqntityhas" runat="server" />
                <asp:HiddenField ID="hdndeleteqnity" runat="server" Value="0" />
                <asp:HiddenField ID="hdnisolddeleted" runat="server" Value="false" />

                <asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />
                <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />

                <asp:HiddenField ID="hdnpcslno" runat="server" Value="0" />

                <asp:HiddenField ID="LastCompany" runat="server" />
                <asp:HiddenField ID="LastFinancialYear" runat="server" />
                <%---- hidden--%>






                <asp:HiddenField ID="HdUpdateMainGrid" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                <%--Subhra--%>
                <asp:HiddenField ID="hdnInnumber" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />

                <asp:HiddenField ID="hdnADDEditMode" runat="server" />
                <asp:HiddenField ID="hdnRDVendGSTIN" runat="server" />
            </div>
            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>



            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <%--Debu Section--%>

            <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductSelected" KeyDown="ProductlookUpKeyDown" GotFocus="function(s,e){cproductLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
                        <%-- <label><strong>Search By product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="ProductpopUpdis" runat="server" ClientInstanceName="cProductpopUpdis"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productDisLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductDisLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productDisLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productDisLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductDisSelected" KeyDown="ProductlookUpdisKeyDown" GotFocus="function(s,e){cproductDisLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>
                        <%-- <label><strong>Search By product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productDisLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductDisLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductDisSelected" ClientSideEvents-KeyDown="ProductlookUpdisKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <%--  <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    <asp:SessionParameter Name="campany_Id" SessionField="LastCompanyPRWS" Type="String" />
                    <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYearPRWS" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%--Batch Product Popup End--%>


            <%--InlineTax--%>

            <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
                Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Select Tax</strong></span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                        <asp:HiddenField runat="server" ID="HdSerialNo" />
                        <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                        <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                        <div id="content-6">
                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-3 gstGrossAmount">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>GST</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Discount</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-2 gstNetAmount">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>GST</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>

                        <%--Error Message--%>
                        <div id="ContentErrorMsg" style="display: none;">
                            <div class="col-sm-8">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Status
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tax Code/Charges Not defined.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>





                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2"></td>
                            </tr>

                            <tr>
                                <td colspan="2"></td>
                            </tr>


                            <tr style="display: none">
                                <td><span><strong>Product Basic Amount</strong></span></td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" TabIndex="1" ReadOnly="true"
                                        runat="server" Width="50%">
                                        <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>







                            <tr class="cgridTaxClass">
                                <td colspan="3">
                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                        Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
                                        OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                    <%--<MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                    <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>
                                        <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

                                    </dxe:ASPxGridView>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table class="InlineTaxClass">
                                        <tr class="GstCstvatClass" style="">
                                            <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; padding-bottom: 15px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1" TabIndex="2"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">

                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>

                                                    <ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                        GotFocus="CmbtaxClick" />
                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                                <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" TabIndex="3" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>


                                            </td>
                                            <td>
                                                <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="pull-left">
                                        <asp:Button ID="Button1" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>

                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
                                                </dxe:ASPxTextBox>

                                            </td>
                                        </tr>
                                    </table>


                                    <div class="clear"></div>
                                </td>
                            </tr>

                        </table>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <%--debjyoti 22-12-2016--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <%--End debjyoti 22-12-2016--%>
            <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <%--Debu Section End--%>
        </asp:Panel>
    </div>
    </div>

    <script type="text/javascript">

        function Keypressevt() {

            if (event.keyCode == 13) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                SaveWarehouse();
                return false;
            }
        }




        function Setenterfocuse(s) {

        }



        function changedqnty(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();

            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);

        }


        function changedqntybatch(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();
            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);


        }


        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PR&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }



    </script>

    <script>
        function prodkeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseReturnProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }


        function prodDiskeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdDisSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");

                if ($("#txtProdDisSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetPurchaseReturnProduct", OtherDetails, "ProductDisTable", HeaderCaption, "ProdDisIndex", "SetDisProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdDisIndex=0]"))
                    $("input[ProdDisIndex=0]").focus();
            }
        }
    </script>
    <div style="display: none">
        <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate" TabIndex="4">
            <ClientSideEvents DateChanged="Enddate" />
        </dxe:ASPxDateEdit>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:SqlDataSource runat="server" ID="dsCustomer"
        SelectCommand="prc_PurchaseReturn_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
        </SelectParameters>
    </asp:SqlDataSource>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmit1Button"
        Modal="True">
    </dxe:ASPxLoadingPanel>


    <%-- TaxDetails HiddenField Field --%>
    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <%-- TaxDetails HiddenField Field --%>

    <%-- <asp:SqlDataSource ID="VendorDataSource" runat="server" />--%>

    <!--Vendor Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Vendor Name</th>
                                <th>Unique Id</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->
    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->



    <!--Product Modal Dis-->
    <div class="modal fade" id="ProductDisModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodDiskeydown(event)" id="txtProdDisSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductDisTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal Dis-->
    <asp:HiddenField ID="hdnAutoPrint" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnEntityType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />
     <asp:HiddenField ID="HdnBackDatedEntryPurchaseGRN" runat="server" />

</asp:Content>
