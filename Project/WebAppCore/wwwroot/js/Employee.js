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
        }

    }(window.JsEmployee = window.JsEmployee || {}, jQuery)
);
