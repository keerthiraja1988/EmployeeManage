(
    function (publicMethod, $) {

        publicMethod.ShowLoaddingIndicator = function () {
            $('#loadingIconModal').modal('show');
        },

            publicMethod.HideLoaddingIndicator = function () {
                setTimeout(
                    function () {
                        $('#loadingIconModal').modal('hide');
                    }, 500);
            }

    }(window.JsMain = window.JsMain || {}, jQuery)
);
