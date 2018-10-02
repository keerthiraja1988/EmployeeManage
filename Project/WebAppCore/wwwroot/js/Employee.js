(
    function (publicMethod, $) {

        publicMethod.EmployeeDetailsPage = function (url) {
           JsMain.ShowLoaddingIndicator();
            window.location.href = url;
        }

    }(window.JsEmployee = window.JsEmployee || {}, jQuery)
);
