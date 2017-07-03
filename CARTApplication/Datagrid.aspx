<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Datagrid.aspx.cs" Inherits="PPLPicker.Test" %>

<html>
<head runat="server">
    <title>AD People Picker</title>
    <link rel="STYLESHEET" type="text/css" href="codebase/dhtmlxgrid.css" />
    <link rel="stylesheet" type="text/css" href="codebase/skins/dhtmlxgrid_dhx_skyblue.css" />
    <link rel="STYLESHEET" type="text/css" href="CSS/style.css" />

   <script src="codebase/dhtmlxcommon.js"></script>

    <script src="codebase/dhtmlxgrid.js"></script>

    <script src="codebase/ext/dhtmlxgrid_srnd.js"></script>
    <script src="codebase/dhtmlxgridcell.js"></script>

    <script type="text/javascript" language="javascript">
        var mygrid;
        var timeoutHnd;
        var flAuto = false;
        var numItems = 0;  // manual list counter

        function doOnLoad() {
           // alert('onLoad1');
            if (document.entryForm.txtbx_hdn.value == "Single," || document.entryForm.txtbx_hdn.value == "Single") {
             //   alert('onLoad2');
                document.getElementById("div1").style.display = 'none';
            }

            document.entryForm.txtbx_Lname.focus();
            mygrid = new dhtmlXGridObject('gridbox');
            mygrid.setImagePath("codebase/imgs/");
            mygrid.setHeader("First Name, Initials, Last Name, Email, Title, Department, Phone No., DisplayName, Network ID");
            mygrid.setInitWidths("100,50,100,250,250,180,80,100,80");
            mygrid.setColAlign("left,left,left,left,left,left,left,left,left,left");
            //mygrid.setColSorting("str,na,str,na,na,na,na,na,na");
            mygrid.setColTypes("ro,ro,ro,ro,ro,ro,ro,ro,ro,ro");
            mygrid.attachEvent("onRowSelect", doOnRowSelected);
            mygrid.setSkin("dhx_skyblue");
            mygrid.init();
            mygrid.enableSmartRendering(true, 100, 100);
            mygrid.attachEvent("onXLE", setCounter);
            //available in Pro Edition only;
            if (mygrid.setColspan) {
                mygrid.attachEvent("onXLE", setCounter);
            }
            else {
                //code below written to support standard edtiton;
                //it written especially for current sample and may not work;
                //in other cases, DON'T USE it if you have pro version;
                mygrid.sortField_old = mygrid.sortField;
                mygrid.sortField = function() {
                    mygrid.setColSorting("str,na,str,na,na,na,na,na,na");
                    if (customColumnSort(arguments[0])) {
                        mygrid.sortField_old.apply(this, arguments);
                    }
                }
                mygrid.sortRows = function(col, type, order) { }
            }

        }

        function setCounter() {
            var span = document.getElementById("recfound");
            span.style.color = "";
            span.innerHTML = mygrid.getRowsNum();
            //            document.getElementById("div1").style.display = 'none';

        }

        function showLoading() {
            var span = document.getElementById("recfound");
            span.style.color = "red";
            span.innerHTML = "loading...";
        }

        function doSearch(ev) {
            var elem = ev.target || ev.srcElement;
            if (timeoutHnd) {
                clearTimeout(timeoutHnd);
                timeoutHnd = setTimeout(reloadGrid, 500);
            }
        }
        function reloadGrid() {
            if (document.entryForm.txtbx_Lname.value.length > 2 || document.entryForm.txtbx_Fname.value.length > 2) {
                var varFname = document.getElementById("txtbx_Fname").value;
                var varLname = document.getElementById("txtbx_Lname").value;
                mygrid.clearAll();
                mygrid.loadXML("FetchData.aspx?ppl_Fname=" + varFname + "&ppl_Lname=" + varLname + "&orderBy=" + window.s_col + "&direction=" + window.a_direction);
                if (window.a_direction) {
                    mygrid.setSortImgState(true, window.s_col, window.a_direction);

                    showLoading();
                }
            }
        }
        function customColumnSort(ind) {
            if ((ind == 1) || (ind == 3) || (ind == 4) || (ind == 5) || (ind == 6) || (ind == 7) || (ind == 8)) {
                alert("Records cannot be sorted by this column.");
                if (window.s_col);
                mygrid.setSortImgState(true, window.s_col, window.a_direction);
                return false;
            }

            var a_state = mygrid.getSortingState();
            window.s_col = ind;
            window.a_direction = ((a_state[1] == "desc") ? "asc" : "desc");
            reloadGrid();
            return false;

        }
        function doOnRowSelected(rowID, celInd) {

            
            var frm = document.entryForm.entryList;
            var len = frm.length;
           
            if (document.entryForm.txtbx_hdn.value == "Multiple") {
               
                var flag = 0;
                if (len > 0 && len < 11) {
                    for (i = 0; i < len; i++) {
                        if (frm.options[i].value == mygrid.cells(rowID, 8).cell.innerHTML) {
                            flag = 1;
                        }
                    }
                }
                if (flag == 1) {
                    alert("You have already selected this user.");
                }
                if ((len == 0 || flag == 0) && (len < 10)) {
                    addOption = new Option(mygrid.cells(rowID, 8).cell.innerHTML, mygrid.cells(rowID, 8).cell.innerHTML);
                    document.entryForm.entryList.options[numItems++] = addOption;
                    return true;
                }
                if (len == 10) {
                    alert("You can select a maximum of 10 people.");
                }
            }
            else if (document.entryForm.txtbx_hdn.value == "Single," || document.entryForm.txtbx_hdn.value == "Single") {
            //alert('151');
            var varid = document.entryForm.tbxt1.value;
            var finalADid = varid + "hdnfldADID";
            var finalDispname = varid + "txtbxDispName";
            window.opener.document.forms[0].elements[finalADid].value = "";
            window.opener.document.forms[0].elements[finalDispname].value = "";
            window.opener.document.forms[0].elements[finalADid].value += mygrid.cells(rowID, 8).cell.innerHTML;
            window.opener.document.forms[0].elements[finalDispname].value += mygrid.cells(rowID, 7).cell.innerHTML;
            if (document.entryForm.txtbxpostbox.value == "Yes") {
                window.opener.CallPostback();
            }
            window.close();
            return true;
            }

        }
        function deleteIt() {
            var contSearch = 1;
            var selIndex;

            // check to see if there is at least one item selected
            if (document.entryForm.entryList.selectedIndex == -1) {
                alert("No entries selected for deletion");
                return true;
            }

            // loop through all selected items and delete them
            while (contSearch > 0) {
                selIndex = document.entryForm.entryList.selectedIndex;
                if (selIndex >= 0) {
                    document.entryForm.entryList.options[selIndex] = null;
                    --numItems;
                }
                else
                    contSearch = 0;
            }

            return true;
        }
        function submitvalues() {
            if (confirm('You have selected ' + numItems + ' people. Click OK to continue..')) {
                if (window.opener && !window.opener.closed) {
                    var frm = document.entryForm.entryList;
                    var varid = document.entryForm.tbxt1.value;
                    var finalADid = varid + "hdnfldADID";
                    var finalDispname = varid + "txtbxDispName";
                    var len = frm.length;
                    window.opener.document.forms[0].elements[finalADid].value = "";
                    window.opener.document.forms[0].elements[finalDispname].value = "";
                    for (i = 0; i < len; i++) {

                        window.opener.document.forms[0].elements[finalADid].value += frm[i].value + ";";
                        window.opener.document.forms[0].elements[finalDispname].value += frm[i].text + ";";
                    }
                    window.close();
                }
            }
        }
    </script>

</head>
<body onload="doOnLoad()">
    <form id="entryForm" runat="server">
    <table width="700" border="0">
        <tr style="display: block">
            <td>
                <asp:HiddenField ID="txtbx_hdn" runat="server" />
                    <asp:HiddenField ID="tbxt1" runat="server" />
                    <asp:HiddenField ID="txtbxpostbox" runat="server" />
                    
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="txtbxCntrl" runat="server" />
                <asp:HiddenField ID="hdnisupdate" runat="server" />
                <asp:HiddenField ID="hdnvinreq" runat="server" />
                    
                    
            </td>
        </tr>
        <tr>
            <td class="style1">
                Last Name
            </td>
            <td>
                <asp:TextBox ID="txtbx_Lname" runat="server" AutoPostBack="False" CssClass="style1"></asp:TextBox>
            </td>
            <td class="style1">
                First Name
            </td>
            <td>
                <asp:TextBox ID="txtbx_Fname" runat="server" AutoPostBack="False" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="style1">
        Records found: <span id="recfound"></span>
    </div>
    <div id="gridbox" style="width: 100%; height: 300px; margin-top: 20px; margin-bottom: 10px;">
    </div>
    <div id="div1">
        <table>
            <tr>
                <td>
                    <asp:ListBox ID="entryList" runat="server" CssClass="style1" Width="200"></asp:ListBox>
                   </td><td valign="top"> <input id="btndel" type="button" value="Remove User" onclick="deleteIt();" class="style1" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input id="btnSubmit" type="button" value="Submit" onclick="submitvalues();" class="style1" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>


