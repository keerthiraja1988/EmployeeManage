(
    function (publicMethod, $) {

        publicMethod.EmployeeDetailsPage = function (url) {
            setTimeout(
                function () {
                    JsMain.ShowLoaddingIndicator();
                }, 800);

            setTimeout(
                function () {
                    window.location.href = url;
                }, 1300);
        },

            publicMethod.GetEmployeeSearchDataForRead = function () {
                var dateOfBirthStart =
                    kendo.toString($("#inputDateOfBirthFrom").data("kendoDatePicker").value(), 'd');
                var dateOfBirthEnd =
                    kendo.toString($("#inputDateOfBirthTo").data("kendoDatePicker").value(), 'd');
                var dateOfJoiningStart =
                    kendo.toString($("#inputDateOfJoiningFrom").data("kendoDatePicker").value(), 'd');
                var dateOfJoiningStartEnd =
                    kendo.toString($("#inputDateOfJoiningTo").data("kendoDatePicker").value(), 'd');

                var searchAllEmployee = $("#hdnSeachAllEmployee").val();
                $("#hdnSeachAllEmployee").val('0');

                return {
                    EmployeeId: $("#inputEmployeeId").val(),
                    FullName: $("#inputFullName").val(),
                    Email: $("#inputEmail").val(),
                    TIN: $("#inputTIN").val(),
                    Passport: $("#inputPassport").val(),
                    DateOfBirthStart: dateOfBirthStart,
                    DateOfBirthEnd: dateOfBirthEnd,
                    DateOfJoiningStart: dateOfJoiningStart,
                    DateOfJoiningEnd: dateOfJoiningStartEnd,
                    SearchAllEmployee: searchAllEmployee,
                }
            },

            publicMethod.EditEmployeeDetails = function (url, data) {

                $.ajax({
                    async: true,
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data),
                    contentType: 'application/json;',
                    dataType: 'json',
                    begin: function (data) {
                        JsMain.ShowLoaddingIndicator();
                    },
                    complete: function (data) {
                        JsMain.HideLoaddingIndicator();
                    },
                    success: function (data) {
                        $("#appendEmployeeManagePartialViews").html(data);
                    },
                    error: function (data) {
                        //JsMain.HideLoaddingIndicator();
                        JsMain.Response404Error(data);
                    }
                });
            },

            publicMethod.DeleteEmployee = function (url, data, dataGrid) {
           
                $.ajax({
                    async: true,
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data),
                    contentType: 'application/json;',
                    dataType: 'json',
                    begin: function (data) {
                        JsMain.ShowLoaddingIndicator();
                    },
                    complete: function (data) {
                        JsMain.HideLoaddingIndicator();
                    },
                    success: function (returnData) {
                      
                        var dataArray = returnData.split("|");
                        //Request Failed
                        if (dataArray[1] == '1') {
                            JsMain.ShowMessageShowReloadPopUp(dataArray[2], dataArray[3] );
                        }
                        //Request Passed
                        if (dataArray[1] == '0') {
                            var oldConfirm = window.confirm;
                            window.confirm = function () { return true; };
                            var grid = $("#grdEmployeeSearch").data("kendoGrid");
                            grid.removeRow($(dataGrid.currentTarget).closest("tr"));
                            window.confirm = oldConfirm;
                            JsMain.ShowMessageShowPopUp(dataArray[2], dataArray[3]);
                        }
                    },
                    error: function (data) {
                        //JsMain.HideLoaddingIndicator();
                        JsMain.Response404Error(data);
                    }
                });
            },

            publicMethod.ValidateEmployeeDetailsOnSearch = function (url) {
                JsMain.ShowLoaddingIndicator();
                var data = JSON.stringify(JsEmployee.GetEmployeeSearchDataForRead());

                $.ajax({
                    async: true,
                    type: "POST",
                    url: url,
                    data: data,
                    contentType: 'application/json;',
                    dataType: 'json',

                    begin: function (data) {

                    },
                    complete: function (data) {
                        JsMain.HideLoaddingIndicator();
                    },
                    success: function (data) {
                        if (data.startsWith("RequestFailed")) {
                            JsMain.ShowMessageShowPopUp1(data);
                        }
                        else {
                            $("#grdEmployeeSearch").data("kendoGrid").dataSource.read();
                        }
                    },
                    error: function (data) {
                        //JsMain.HideLoaddingIndicator();
                        JsMain.Response404Error(data);
                    }
                });
            }


    }(window.JsEmployee = window.JsEmployee || {}, jQuery)
);
