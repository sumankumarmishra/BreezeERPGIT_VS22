﻿<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SrvTechMast.aspx.cs" Inherits="ERP.OMS.Management.Master.SrvTechMast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <%-- changes by subhra 13-02-2017--%>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -6px;
            top: 4px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstAssignTo {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }

        .floatedBtnArea {
            top: 2px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function changeFunc() {
            if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                ChangeselectedMainActvalue();
            } else {
                var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
                document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
            }
        }


        function ChangeselectedMainActvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                    if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                        lstTaxRates_MainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }

        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }

        function ChangeSourceMainAccount() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "SrvTechMast.aspx/GetEmployeeList",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));

                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        ChangeselectedMainActvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }




        function ListAssignTo() {

            $('#lstAssignTo').chosen();
            $('#lstAssignTo').fadeIn();
        }
        $(function () {
            // debugger;
            //  BindAssign(branchid);
            if ($('#<%=hdbBranch.ClientID %>').val() != "") {

                BindAssign($('#<%=hdbBranch.ClientID %>').val());
                $('#<%=hdbBranch.ClientID %>').val('');
            }
            //BindAssign();
        })

        function BindAssign(branchid) {

            var lAssignTo = $('select[id$=lstAssignTo]');
            //  var lSupervisor = $('select[id$=lstSupervisor]');

            lAssignTo.empty();
            //   lSupervisor.empty();

            var lAddEdit = document.getElementById("hdnstorequrystring").value;
            $.ajax({
                type: "POST",
                data: JSON.stringify({ id: lAddEdit, branchid: branchid }),
                url: 'SrvTechMast.aspx/GetAllUserListBeforeSelect',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lAssignTo).append(listItems.join(''));
                        //   $(lSupervisor).append(listItems.join(''));


                        //   ListSupervisor();
                        ListAssignTo();
                        setWithFromAssign();


                        $('#lstAssignTo').trigger("chosen:updated");
                    }
                    else {
                        //$('#lstSupervisor').trigger("chosen:updated");
                        //$('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });


        }
        function setWithFromAssign() {
            // debugger;
            var lstAssignTo = document.getElementById("lstAssignTo");
            var listValue = document.getElementById("hdnEditAssignTo").value;
            var str_array = listValue.split(',');

            for (var i = 0; i < lstAssignTo.options.length; i++) {
                var selectedValue = lstAssignTo.options[i].value;

                if (str_array.indexOf(selectedValue) > -1) {
                    lstAssignTo.options[i].selected = true;
                }
                else {
                    lstAssignTo.options[i].selected = false;
                }
            }
            $("#lstAssignTo").chosen().change();
            //if (str_array != "") {

            //    $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
            //}
        }

        $(document).ready(function () {
            ListBind();
            ListAssignTo();
            ChangeSourceMainAccount();

            debugger;
            $("#lstAssignTo").chosen().change(function () {
                var assignId = $(this).val();
                <%-- $('#<%=hdnAssign.ClientID %>').val(assignId);--%>
                $('#MandatoryAssign').attr('style', 'display:none');
            })



            //$('#lblSalesActivity').text('Assign Sales Activity');
        });

        function ClientSaveClick() {
            debugger;
            var flag = true;
            if (Page_ClientValidate()) {
                if (isValid()) {
                    var assignID = $("#lstAssignTo").find("option:selected").text();
                    //if (assignID == "") {
                    //    flag = false;
                    //    $('#MandatoryAssign').attr('style', 'display:block');
                    //}
                    //else { $('#MandatoryAssign').attr('style', 'display:none'); flag = true; }

                    return flag;
                }
            }
            return false;
        }
    </script>
    <script language="javascript" type="text/javascript">

        function isValidChar(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            if (theEvent.key == "|" || theEvent.key == "~") {
                theEvent.returnValue = false;
            } else {
                theEvent.returnValue = true;
            }

        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=TM';
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        function popUpRedirect(obj) {
            alert('Saved successfully');
            window.location.href = obj;
            //function (message, title, callback) {
            //    $.alerts.alert(message, title, callback);
            //}
            //jAlert('Saved successfully', 'Saved', doAlert());


        }

        //function doAlert() {
        //    window.location.href = obj;
        //}
        $(document).ready(function () {
            //  loadExecutiveNameFromField();

            $('#cmbBranch').change(function () {

                //  alert();

                $('#lstAssignTo').val('').trigger('chosen:updated');
                $('#lstAssignTo').val('');
                $('#lstAssignTo').val('').trigger('chosen:updated');
                $('#lstAssignTo').val('').trigger('chosen:updated');

                BindAssign($(this).val());


            });
        });


        function loadExecutiveNameFromField() {
            var table = document.getElementById("executiveTable");
            var exeName = document.getElementById('executiveName_hidden').value;
            for (var i = 0 ; i < exeName.split('~').length; i++) {
                if (exeName.split('~')[i].trim() != '') {
                    if (table.rows[0].cells[0].children[0].value.trim() != '') {
                        var row = table.insertRow(0);
                        var cell0 = row.insertCell(0);
                        var cell1 = row.insertCell(1);
                        var cell2 = row.insertCell(2);
                        var cell3 = row.insertCell(3);
                        var cell4 = row.insertCell(4);

                        cell0.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)'  maxlength='50' value='" + exeName.split('~')[i].split('|')[0] + "'/>";
                        //check Duplicate or not
                        var indx = document.getElementById('ErrorExecutive').value.indexOf(exeName.split('~')[i].split('|')[1]);
                        if (indx != -1) {
                            cell1.setAttribute("Class", "relative");
                            cell1.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' class='gsajdfgvasd'  maxlength='50' value='" + exeName.split('~')[i].split('|')[1] + "'/> <span class='flMan fa fa-exclamation-circle iconRed' title='Already Exist'></span>";
                        }
                        else
                            cell1.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' maxlength='50' value='" + exeName.split('~')[i].split('|')[1] + "'/>";

                        cell2.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' maxlength='50' value='" + exeName.split('~')[i].split('|')[2] + "'/>";
                        if (exeName.split('~')[i].split('|')[3] == '1')
                            cell3.innerHTML = "<input type='checkbox'  checked='true'/>";
                        else
                            cell3.innerHTML = "<input type='checkbox'  />";
                        cell4.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                    }
                    else {
                        table.rows[0].cells[0].children[0].value = exeName.split('~')[i].split('|')[0];
                        table.rows[0].cells[1].children[0].value = exeName.split('~')[i].split('|')[1];
                        var indx = document.getElementById('ErrorExecutive').value.indexOf(exeName.split('~')[i].split('|')[1]);
                        if (indx != -1)
                            table.rows[0].cells[1].children[1].setAttribute("Class", "clsExists");


                        table.rows[0].cells[2].children[0].value = exeName.split('~')[i].split('|')[2];
                        if (exeName.split('~')[i].split('|')[3] == '1')
                            table.rows[0].cells[3].children[0].checked = true;
                        else
                            table.rows[0].cells[3].children[0].checked = false;

                    }
                }
            }

        }


        function AddNewexecutive() {
            var table = document.getElementById("executiveTable");

            var row = table.insertRow(0);
            var cell0 = row.insertCell(0);
            var cell1 = row.insertCell(1);
            var cell2 = row.insertCell(2);
            var cell3 = row.insertCell(3);
            var cell4 = row.insertCell(4);
            cell0.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell1.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell2.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell3.innerHTML = "<input type='checkbox'/>";
            cell4.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
        }
        function removeExecutive(obj) {
            var rowIndex = obj.rowIndex;
            var table = document.getElementById("executiveTable");
            if (table.rows.length > 1) {
                table.deleteRow(rowIndex);
            } else {
                jAlert('Cannot delete all Executive.');
            }
        }
        function isValid() {
            if (document.getElementById('txtTechnicianId').value.trim() == '') {
                return false;
            }
            if (document.getElementById('txtTechnicianName').value.trim() == '') {
                return false;
            }
            if ($("#lstTaxRates_MainAccount").val() == null) {
                jAlert("Please select Employee Name.");
                $("#lstTaxRates_MainAccount").focus();
                return false;
            }
            return true;
        }


        function OnEndCallback(s, e) {

        }

        function disp_prompt(name) {
            if (name == "tab1") {

                document.location.href = "SrvTechMast_Correspondence.aspx";
            }
            if (name == "tab2") {

                document.location.href = "SrvTechMast_document.aspx";
            }
        }

        function onTechnicianChange() {

            var finCd = 0;
            if (GetObjectID('hiddenedit').value != '') {
                finCd = GetObjectID('hiddenedit').value;
            }
            var ShortName = document.getElementById("txtTechnicianId").value.trim();
            $.ajax({
                type: "POST",
                url: "SrvTechMast.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName, FinCode: finCd }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique Technician ID");
                        document.getElementById("txtTechnicianId").value = '';
                        document.getElementById("txtTechnicianId").focus();
                        return false;
                    }
                }
            });


        }

        function branchGridEndCallBack() {
            if (cbranchGrid.cpReceviedString) {
                if (cbranchGrid.cpReceviedString == 'SetAllRecordToDataTable') {
                    cBranchSelectPopup.Hide();
                }
            }

            if (cbranchGrid.cpBrselected) {

                if (cbranchGrid.cpBrselected == '1') {
                    jAlert("Individual branch selection not allowed when all branch option is checked.");
                    cbranchGrid.cpBrselected = null;
                    cBranchSelectPopup.Show();
                }
                else { cbranchGrid.cpBrselected = null; }
            }

            if (cbranchGrid.cpBrChecked) {
                if (cbranchGrid.cpBrChecked == '1') {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', true)
                    cbranchGrid.cpBrChecked = null;
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                }
                else {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', false)
                    cbranchGrid.cpBrChecked = null;
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                }
            }
        }

        function ClearSelectedBranch() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
        }

        function SelectAllBranches(e) {

            if (e.checked == true) {

                ClearSelectedBranch();
                $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');
            }
            else {
                $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');
            }
        }

        function CmbBranchChanged() {
            var branchCode = CmbBranch.GetValue();
            if (branchCode == 0) {
                $('#MultiBranchButton').show();
            }
            else {
                $('#MultiBranchButton').hide();
            }
        }

        function MultiBranchClick() {
            cbranchGrid.PerformCallback('SetAllSelectedRecord');
            cBranchSelectPopup.Show();
        }

        function SaveSelectedBranch() {
            cbranchGrid.PerformCallback('SetAllRecordToDataTable');
        }

        function selectAll() {

            cbranchGrid.PerformCallback('SelectAllBranchesFromList');
            cBranchSelectPopup.Show();
        }
        function unselectAll() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
            cBranchSelectPopup.Show();
        }
    </script>

    <style>
        .pdBotm > tbody > tr > td {
            padding-top: 10px;
        }

        #lstAssignTo {
            width: 200px;
        }

        .nbackBtn button {
            background: transparent !important;
            border: none !important;
            font-size: 21px;
        }

            .nbackBtn button.green {
                color: #2db52d;
            }

            .nbackBtn button.red {
                color: #f53434;
            }

        .nbackBtn tr td:last-child {
            position: absolute;
        }

        .abs {
            position: absolute;
            top: 8px;
            right: -20px;
        }

        .clsExists {
            border-color: red !important;
        }

        .padR10 {
            padding-right: 10px;
        }

        .clsExists {
            border-color: red;
        }
        /*.clsExists:before {
            content:'Error';
            position:absolute;
            right:0;
        }*/
        .flMan {
            position: absolute;
            right: 15px;
            top: 7px;
        }

        #executiveTable tr td:nth-child(2),
        #executiveTable tr td:nth-child(3),
        #executiveTable tr td:nth-child(1) {
            padding-right: 10px;
        }

        /*Rev 1.0*/
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
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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
            top: 6px;
            right: -2px;
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        #B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM
        {
            max-width: 98% !important;
        }*/

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

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

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .chosen-container-single .chosen-single div::after
        {
            font-size: 17px !important;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
    opacity: 0.4 !important;
    color: #ffffff !important;
}*/
        /*.padTopbutton {
    padding-top: 27px;
}*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
            <div class="panel-title">
                <h3 id="headingName" runat="server">Add/Edit Technician</h3>
                <div class="crossBtn"><a href="SrvTechMastList.aspx"><i class="fa fa-times"></i></a></div>
            </div>
        </div>

        <div class="form_main">
        <dxe:ASPxCallbackPanel runat="server" ID="ExecutiveCallbackPanel" ClientInstanceName="CallbackPanel" RenderMode="Table" OnCallback="ExecutiveCallbackPanel_Callback">
            <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
            <PanelCollection>
                <dxe:PanelContent ID="PanelContent3" runat="server">
                </dxe:PanelContent>
            </PanelCollection>
        </dxe:ASPxCallbackPanel>
        <asp:HiddenField runat="server" ID="hiddenedit" />

        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />

        <asp:HiddenField ID="hdnstorequrystring" runat="server" />

        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="PageControl1" runat="server" Width="100%" ActiveTabIndex="0"
                        ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                            <table class="pdBotm">
                                                <tr>
                                                    <td width="150px">
                                                        <label>Technician ID <span style="color: red">*</span></label></td>
                                                    <td width="300px" style="position: relative;">
                                                        <asp:TextBox ID="txtTechnicianId" runat="server" Width="100%" onblur="onTechnicianChange()"
                                                            MaxLength="20" />

                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtTechnicianId"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup">                                                        
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <label>Technician Name <span style="color: red">*</span></label></td>
                                                    <td style="position: relative;">
                                                        <asp:TextBox ID="txtTechnicianName" runat="server" Width="100%"
                                                            MaxLength="100" />
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtTechnicianName"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup">                                                        
                                                        </asp:RequiredFieldValidator>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <label>Branch  <span style="color: red">*</span></label>
                                                    </td>
                                                    <td>
                                                        <%-- <asp:DropDownList ID="cmbBranch" runat="server" Width="100%">
                                                        </asp:DropDownList>--%>
                                                        <div>
                                                            <asp:Label ID="lblSelectedBranch" runat="server"></asp:Label>
                                                        </div>

                                                        <dxe:ASPxComboBox ID="cmbMultiBranches" ClientInstanceName="CmbBranch" runat="server" Visible="false"
                                                            ValueType="System.String" DataSourceID="branchdtl" ValueField="branch_id"
                                                            TextField="branch_description" EnableIncrementalFiltering="true"
                                                            Width="90%" AutoPostBack="false">
                                                            <ClientSideEvents SelectedIndexChanged="CmbBranchChanged" Init="CmbBranchChanged" />
                                                        </dxe:ASPxComboBox>
                                                        <input type="button" onclick="MultiBranchClick()" class="btn btn-success btn-xs " value="Select Branch(s)" id="MultiBranchButton" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="padBot5" style="display: block;">
                                                            <span>Employee Name  <span style="color: red">*</span></span>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="Left_Content" style="margin-bottom: 5px;">
                                                            <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." TabIndex="10" onchange="changeFunc();"></asp:ListBox>
                                                            <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                        </div>
                                                    </td>


                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <label>Contact No. <span style="color: red">*</span></label></td>
                                                    <td style="position: relative;">
                                                        <asp:TextBox ID="txtContactNo" runat="server" Width="100%" MaxLength="15" />
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtContactNo"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup">                                                        
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <label>Is Active? </label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top: 10px; padding-left: 151px;">

                                                <%--<asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()"/>--%>
                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()" />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click" />
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Correspondence" Text="Correspondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <%--details goes here--%>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <%--details goes here--%>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                           
	                                            
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            }"></ClientSideEvents>
                    </dxe:ASPxPageControl>


                </td>
            </tr>
        </table>
         <asp:SqlDataSource ID="branchdtl" runat="server" 
            SelectCommand="select '0' as branch_id ,  'Select' as branch_description union all   select branch_id,branch_description from tbl_master_branch order by branch_description"></asp:SqlDataSource>

        <asp:SqlDataSource ID="BranchdataSource" runat="server" SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>
    </div>
    </div>
    <dxe:ASPxPopupControl ID="BranchSelectPopup" runat="server" Width="700"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cBranchSelectPopup"
        HeaderText="Select Branch" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div style="margin-bottom: 10px; margin-top: 10px;" class="hide">
                    Apply for All Branch &nbsp; 
                     <asp:CheckBox ID="chkAllBranch" runat="server" OnClick="SelectAllBranches(this);" />
                    <asp:Label ID="lblBranch" runat="server" Text="All Branch Selected, No need to select individual Branch" CssClass="vehiclecls"></asp:Label>
                </div>

                <dxe:ASPxGridView ID="branchGrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False" DataSourceID="BranchdataSource"
                    Width="100%" ClientInstanceName="cbranchGrid" OnCustomCallback="branchGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />
                        <dxe:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Branch Description" FieldName="branch_description"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <ClientSideEvents EndCallback="branchGridEndCallBack" />
                </dxe:ASPxGridView>
                <br />
                <input type="button" value="Ok" class="btn btn-primary" onclick="SaveSelectedBranch()" />
                <div style="float: right;">
                    <input type="button" runat="server" value="Select All" onclick="selectAll()" />
                    <input type="button" runat="server" value="Deselect All" onclick="unselectAll()" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdbBranch" runat="server" />
    <asp:HiddenField ID="hdnBranchAllSelected" runat="server" />
    <asp:HiddenField ID="hdnflag" runat="server" />
</asp:Content>
