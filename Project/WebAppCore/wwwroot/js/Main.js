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

        publicMethod.GetLoggedInUserDetails = function (url) {
           // alert(url);

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json",
                dataType: "json",
                begin: function (data) {

                },
                complete: function (data) {
                    //alert(data);
                  
                },
                success: function (data) {
                    debugger;
                    //$("#globalHTMLAppender").append(data.toString());
                    //$("#_RegisterPartialViewDiv").html(data);
                    $("#globalHTMLAppender").html(data);
                }
            });

        }

    }(window.JsMain = window.JsMain || {}, jQuery)
);
